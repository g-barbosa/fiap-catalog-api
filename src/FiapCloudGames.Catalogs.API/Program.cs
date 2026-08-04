using FiapCloudGames.Catalogs.Application.Avaliacoes.Interfaces;
using FiapCloudGames.Catalogs.API.Middleware;
using FiapCloudGames.Catalogs.Application.Bibliotecas.Interfaces;
using FiapCloudGames.Catalogs.Application.Bibliotecas.Services;
using FiapCloudGames.Catalogs.Application.Avaliacoes.Services;
using FiapCloudGames.Catalogs.Application.Jogos.Interfaces;
using FiapCloudGames.Catalogs.Application.Jogos.Services;
using FiapCloudGames.Catalogs.Application.Pedidos;
using FiapCloudGames.Catalogs.Application.Pedidos.Interfaces;
using FiapCloudGames.Catalogs.Application.Pedidos.Services;
using FiapCloudGames.Catalogs.Domain.Avaliacoes.Interfaces;
using FiapCloudGames.Catalogs.Domain.Bibliotecas.Interfaces;
using FiapCloudGames.Catalogs.Domain.Jogos.Interfaces;
using FiapCloudGames.Catalogs.Domain.Pedidos.Interfaces;
using FiapCloudGames.Catalogs.Domain.Pedidos.Interfaces.Messaging;
using FiapCloudGames.Catalogs.Domain.Pedidos.Services;
using FiapCloudGames.Catalogs.Infrastructure.Caching;
using FiapCloudGames.Catalogs.Infrastructure.Data.Mongo;
using FiapCloudGames.Catalogs.Infrastructure.Data.Persistence;
using FiapCloudGames.Catalogs.Infrastructure.Data.Persistence.Repositories;
using FiapCloudGames.Catalogs.Infrastructure.Messaging.Consumers;
using FiapCloudGames.Catalogs.Infrastructure.Messaging.Publishers;
using Microsoft.EntityFrameworkCore;
using Serilog;
using MongoDB.Driver;
using Prometheus;

namespace FiapCloudGames.Catalogs.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .Enrich.FromLogContext()
                .CreateLogger();

            builder.Host.UseSerilog((context, services, loggerConfiguration) =>
            {
                loggerConfiguration
                    .ReadFrom.Configuration(context.Configuration)
                    .Enrich.FromLogContext();
            });

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Version = "v1",
                    Title = "FIAP Cloud Games API - API de Catalogos"
                });
            });

            var rabbitMqHost = builder.Configuration["RabbitMq:Host"] ?? "rabbitmq";
            var rabbitMqPort = int.Parse(builder.Configuration["RabbitMq:Port"] ?? "5672");
            var rabbitMqUri = new Uri($"amqp://admin:rabbitmq123@{rabbitMqHost}:{rabbitMqPort}/");

            builder.Services.AddHealthChecks()
                .AddSqlServer(builder.Configuration.GetConnectionString("DefaultConnection") ?? "", name: "SqlServer");

            builder.Services.AddDbContext<CatalogsDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            var redisConnection = builder.Configuration.GetConnectionString("Redis");
            if (!string.IsNullOrWhiteSpace(redisConnection))
            {
                builder.Services.AddStackExchangeRedisCache(options =>
                {
                    options.Configuration = redisConnection;
                    options.InstanceName = "fiap-catalog:";
                });
            }
            else
            {
                builder.Services.AddDistributedMemoryCache();
            }

            builder.Services.AddSingleton<IMongoClient>(_ =>
            {
                var connectionString = builder.Configuration.GetConnectionString("MongoDb")
                    ?? throw new InvalidOperationException("ConnectionStrings:MongoDb não configurada.");
                return new MongoClient(connectionString);
            });

            builder.Services.AddSingleton(sp =>
            {
                var client = sp.GetRequiredService<IMongoClient>();
                var databaseName = builder.Configuration["MongoDb:Database"] ?? "FiapCloudGamesCatalog";
                return client.GetDatabase(databaseName);
            });

            builder.Services.AddScoped<IJogoRepository, JogoRepository>();
            builder.Services.AddScoped<IJogoCache, RedisJogoCache>();
            builder.Services.AddScoped<IJogoService, JogoService>();

            builder.Services.AddScoped<IAvaliacaoRepository, AvaliacaoRepository>();
            builder.Services.AddScoped<IAvaliacaoService, AvaliacaoService>();

            builder.Services.AddScoped<IBibliotecaRepository, BibliotecaRepository>();
            builder.Services.AddScoped<IBibliotecaService, BibliotecaService>();

            builder.Services.AddScoped<IPedidoRepository, PedidoRepository>();
            builder.Services.AddScoped<IPedidoDomainService, PedidoDomainService>();
            builder.Services.AddScoped<IPedidoService, PedidoService>();

            builder.Services.AddScoped<IPedidoCriadoPublisher, RabbitMqPedidoEventPublisher>();
            builder.Services.AddHostedService<PagamentoProcessadoConsumer>();
            builder.Services.AddScoped<ProcessaPagamentoService>();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<CatalogsDbContext>();
                dbContext.Database.Migrate();
            }

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseMiddleware<CorrelationIdMiddleware>();
            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.UseHttpMetrics();
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.MapHealthChecks("/health");
            app.MapMetrics();

            app.Run();
        }
    }
}