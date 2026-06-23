using FiapCloudGames.Catalogs.Application.Jogos.Interfaces;
using FiapCloudGames.Catalogs.Application.Jogos.Services;
using FiapCloudGames.Catalogs.Domain.Jogos.Interfaces;
using FiapCloudGames.Catalogs.Domain.Pedidos.Interfaces.Messaging;
using FiapCloudGames.Catalogs.Infrastructure.Data.Persistence;
using FiapCloudGames.Catalogs.Infrastructure.Data.Persistence.Repositories;
using FiapCloudGames.Catalogs.Infrastructure.Messaging.Consumers;
using FiapCloudGames.Catalogs.Infrastructure.Messaging.Publishers;
using Microsoft.EntityFrameworkCore;

namespace FiapCloudGames.Catalogs.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

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

            builder.Services.AddHealthChecks();

            builder.Services.AddDbContext<CatalogsDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Jogos
            builder.Services.AddScoped<IJogoRepository, JogoRepository>();
            builder.Services.AddScoped<IJogoService, JogoService>();

            builder.Services.AddScoped<IPedidoCriadoPublisher, RabbitMqUsuarioEventPublisher>();
            builder.Services.AddHostedService<PagamentoProcessadoConsumer>();

            var app = builder.Build();


            app.UseSwagger();
            app.UseSwaggerUI();


            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.MapHealthChecks("/health");

            app.Run();
        }
    }
}
