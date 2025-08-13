using Application.IEventBus;
using Application.IRepositories;
using Application.IServices;

using Infastructure.Services;
using Infrastructure.Messages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infastructure.Persistence
{
    public static class DenpendenceInjection
    {
        #region repository registation
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {

            services.AddScoped<ICommandUnitOfWork, CommandUnitOfWork>();
            services.AddScoped<IQueryUnitOfWork, QueryUnitOfWork>();
            return services;
        }
        #endregion
        #region Services registation
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IClassService, ClassService>();
            services.AddScoped<ISystemAccountService, SystemAccountService>();
            services.AddScoped<IServiceAggregator, ServiceAggregator>();
            var kafkaBootstrapServers = configuration.GetConnectionString("BootstrapServers")
                                     ?? configuration["Kafka:BootstrapServers"]
                                     ?? "localhost:9092";
            services.AddScoped<IMessagePublisher>(sp =>
                 new KafkaMessagePublisher(kafkaBootstrapServers));

            services.AddScoped<IMessageConsumer>(sp =>
                new KafkaMessageConsumer(kafkaBootstrapServers));
            return services;
        }
        #endregion
        public static IServiceCollection AddDatabaseAndConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<SqlContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultSQLConnection"));
            }
           );

            services.AddSingleton<IConfiguration>(configuration);
            return services;
        }

    }
}
