using FiapCloudGames.Catalogs.Application.Pedidos;
using FiapCloudGames.Catalogs.Domain.Pedidos.Events;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace FiapCloudGames.Catalogs.Infrastructure.Messaging.Consumers
{
    public class PagamentoProcessadoConsumer : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ConnectionFactory _factory;
        public PagamentoProcessadoConsumer(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _serviceProvider = serviceProvider;

            _factory = new ConnectionFactory
            {
                HostName = configuration.GetSection("RabbitMq:Host").Value!,
                UserName = configuration.GetSection("RabbitMq:Username").Value!,
                Password = configuration.GetSection("RabbitMq:Password").Value!,
                Port = Int32.Parse(configuration.GetSection("RabbitMq:Port").Value!)
            };
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var connection = await _factory.CreateConnectionAsync();

            var channel = await connection.CreateChannelAsync();

            await channel.ExchangeDeclareAsync(
                exchange: "pagamento-processado",
                type: ExchangeType.Fanout,
                durable: true,
                autoDelete: false);

            await channel.QueueDeclareAsync(
                queue: "pagamento-processado-catalog",
                durable: true,
                exclusive: false,
                autoDelete: false);

            await channel.QueueBindAsync(
                queue: "pagamento-processado-catalog",
                exchange: "pagamento-processado",
                routingKey: "");

            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.ReceivedAsync += async (_, ea) =>
            {
                var body = ea.Body.ToArray();

                var json = Encoding.UTF8.GetString(body);

                var evento = JsonSerializer.Deserialize<PagamentoProcessadoEvent>(json);

                if (evento is null)
                    return;

                using var scope = _serviceProvider.CreateScope();

                var handler = scope.ServiceProvider.GetRequiredService<ProcessaPagamentoService>();

                await handler.ProcessarPagamento(evento);
            };

            await channel.BasicConsumeAsync(queue: "pagamento-processado-catalog", autoAck: true, consumer: consumer);
        }
    }
}
