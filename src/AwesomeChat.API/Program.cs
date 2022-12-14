using AwesomeChat.API.Configuration;
using AwesomeChat.API.Extensions;
using AwesomeChat.Application;
using AwesomeChat.Persistence;
using Elastic.Apm.Api;
using Elastic.Apm.AspNetCore;
using Elastic.Apm.NetCoreAll;
using HealthChecks.UI.Client;
using HealthChecks.UI.Core.Data;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Nest;
using Newtonsoft.Json;
using Serilog.Sinks.Elasticsearch;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Configuration;
using System.Reflection;
using Microsoft.Extensions.Logging.Console;
using Serilog.Core;
using MongoDB.Driver;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Serilog.Sinks.MSSqlServer.Sinks.MSSqlServer.Options;
using Serilog.Sinks.MSSqlServer;
using Serilog.Events;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);


builder.Host.ConfigureLogging(a => { a.ClearProviders(); }).UseSerilog();
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
     .WriteTo.Seq("http://localhost:5341")
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Information)
            .Enrich.WithProperty("AppName", "Chat awesome serilog")
            .Enrich.WithProperty("Environment", "development")
            .ReadFrom.Configuration(builder.Configuration)
            .WriteTo.Console(restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information)
            .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
            .WriteTo.MSSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
             sinkOptions: new MSSqlServerSinkOptions { TableName = "logs", AutoCreateSqlTable = true }, null, null,
             LogEventLevel.Error, null, columnOptions: null, null, null)
            .Enrich.FromLogContext()
    .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://localhost:9200/"))

{
    AutoRegisterTemplate = true,
    IndexFormat=$"chat-awesome-{DateTime.UtcNow:yyyy}"
}).CreateLogger();

using var loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddSimpleConsole(i => i.ColorBehavior = LoggerColorBehavior.Disabled);
});


builder.Services.AddControllers();
builder.Services.AddMemoryCache();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddApplicationDependencies();
builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddHealthCheckDependencies(builder.Configuration);
builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
    options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
});

builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
var xmlFilePath = Path.Combine(AppContext.BaseDirectory, xmlFile);

builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.AddSeq(builder.Configuration.GetSection("Seq"));
});

builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, SwaggerConfigureOptions>();
builder.Services.AddSwaggerGen(options =>
{
    options.IncludeXmlComments(xmlFilePath);
});


builder.Host.UseSerilog();



var app = builder.Build();
app.UseSerilogRequestLogging();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.UseElasticApm(builder.Configuration);

app.UseHealthChecks("/healthcheck", new HealthCheckOptions
{
    ResponseWriter = async (c, r) =>
    {
        c.Response.ContentType = "application/json";

        var result = JsonConvert.SerializeObject(new
        {
            status = r.Status.ToString(),
            components = r.Entries.Select(e => new { key = e.Key, value = e.Value.Status.ToString() })
        });
        await c.Response.WriteAsync(result);
    }
});

app.MapControllers();

app.Run();
