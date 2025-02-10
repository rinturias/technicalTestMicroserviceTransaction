using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Yape.Transactions.Domain.Interfaces.Repository;
using Yape.Transactions.Infrastructure.Context;
using Yape.Transactions.Infrastructure.Messaging;
using Yape.Transactions.Infrastructure.Middleware;
using Yape.Transactions.Infrastructure.Repositories;
using Yape.Transactions.Infrastructure.UnitOfWorks;

namespace Yape.Transactions.Infrastructure
{
    public static class Extensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<ContextDatabase>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
         
            services.AddKafkaProducer(configuration);
            services.AddHostedService<KafkaConsumer>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<IContextDatabase, ContextDatabase>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }
    }
}
