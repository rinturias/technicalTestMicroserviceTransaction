using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Yape.Transactions.Application.Interfaces;
using Yape.Transactions.Application.UseCases.Transactions;
using Yape.Transactions.Domain.Interfaces;

namespace Yape.Transactions.Application
{
    public static class Extensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<ITransactionService, TransactionService>();

            services.AddValidatorsFromAssemblies(
                AppDomain.CurrentDomain.GetAssemblies().Where(a => a.FullName.Contains("Yape.Transactions.Application")).ToArray()
            );
            services.AddScoped<ITransactionProcessor, TransactionProcessor > ();
            return services;
        }
    }
}
