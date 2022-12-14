using HealthChecks.UI.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace AwesomeChat.API.Extensions
{
    public static class AddHealthCheckExtension
    {
        public static void AddHealthCheckDependencies(this IServiceCollection services,IConfiguration configuration)
        {
   
            var _msssqlDbConn = configuration.GetConnectionString("DefaultConnection");


            services.AddHealthChecks()
                   .AddSqlServer(connectionString: _msssqlDbConn,
                    healthQuery: "SELECT 1",
                    name: "MS SQL Server Check",
                    failureStatus: HealthStatus.Unhealthy | HealthStatus.Degraded,
                    tags: new string[] { "db", "sql", "sqlserver" })
                    .AddMongoDb(
                    mongodbConnectionString: "mongodb://localhost:27017",
                    name: "Mongo Db Check",
                    failureStatus: HealthStatus.Unhealthy | HealthStatus.Degraded,
                    tags: new string[] { "mongodb" })
                    .AddRabbitMQ(
                    rabbitConnectionString: "amqp://guest:guest@localhost",
                    name: "RabbitMQ Check",
                    failureStatus: HealthStatus.Unhealthy | HealthStatus.Degraded,
                    tags: new string[] { "rabbitmq" })
                    .AddElasticsearch(
                     elasticsearchUri: configuration.GetValue<string>("ElasticSearch:URL"),
                     "Elastic Search",
                     failureStatus: HealthStatus.Unhealthy | HealthStatus.Degraded,
                     tags: new string[] { "elastic-search" })
                    .AddSeqPublisher(options =>
                    {
                        options.ApiKey = configuration.GetValue<string>("Seq:ApiKey");
                        options.Endpoint = configuration.GetValue<string>("Seq:ServerUrl");
                        options.DefaultInputLevel = HealthChecks.Publisher.Seq.SeqInputLevel.Information;
                    }, "Chat Service");


        }
    }
}
