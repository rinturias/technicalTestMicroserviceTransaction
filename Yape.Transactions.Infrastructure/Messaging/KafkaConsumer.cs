using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Yape.Transactions.Application.DTO.Messaging;
using Yape.Transactions.Application.Interfaces;
using Yape.Transactions.Domain.Interfaces;

namespace Yape.Transactions.Infrastructure.Messaging
{
    public class KafkaConsumer : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IConfiguration _configuration;
        private readonly string _inputTopic;
        private readonly string _bootstrapServers;
        private readonly string _groupId;
        private readonly AutoOffsetReset _autoOffsetReset;
        private readonly bool _enableAutoCommit;

        public KafkaConsumer(IServiceScopeFactory scopeFactory, IConfiguration configuration)
        {
            _scopeFactory = scopeFactory;
            _configuration = configuration;
            _bootstrapServers = _configuration["Kafka:BootstrapServers"] ?? "localhost:9092";
            _groupId = _configuration["Kafka:GroupId"] ?? "default-group";
            _inputTopic = _configuration["Kafka:InputTopic"] ?? "default-topic";
            _autoOffsetReset = Enum.TryParse(_configuration["Kafka:AutoOffsetReset"], out AutoOffsetReset offsetReset) ? offsetReset : AutoOffsetReset.Latest;
            _enableAutoCommit = bool.TryParse(_configuration["Kafka:EnableAutoCommit"], out bool autoCommit) && autoCommit;
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = _bootstrapServers,
                GroupId = _groupId,
                AutoOffsetReset = _autoOffsetReset,
                EnableAutoCommit = _enableAutoCommit
            };
            using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
            consumer.Subscribe(_inputTopic);
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var consumeResult = consumer.Consume(stoppingToken);
                    var transactionValidation = JsonSerializer.Deserialize<TransactionValidationEvent>(consumeResult.Value);
                    if (transactionValidation != null)
                    {
                        using (var scope = _scopeFactory.CreateScope())
                        {
                            var transactionService = scope.ServiceProvider.GetRequiredService<ITransactionProcessor>();
                            await transactionService.UpdateTransactionStatusAsync(transactionValidation);
                            consumer.Commit(consumeResult);
                        }
                    }
                }


                catch (ConsumeException ex)
                {
                    Console.WriteLine($"Error al consumir el mensaje de Kafka: {ex.Error.Reason}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error inesperado en KafkaConsumer: {ex.Message}");
                }
                await Task.Yield();
            }
        }
    }
}
