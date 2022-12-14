using AwesomeChat.Application.RabbitMq;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeChat.Application
{
    public static class ApplicationServiceRegistrations
    {
        public static void AddApplicationDependencies(this IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddSignalR();
            services.AddScoped<IRabbitMqProcuder, RabbitMqProcuder>();
            services.AddMediatR(Assembly.GetExecutingAssembly());
        }
    }
}
