using Basket.API.GrpcServices;
using Basket.API.Repositories;
using Discount.Grpc.Protos;
using MassTransit;
using Microsoft.OpenApi.Models;

namespace Basket.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            // Redis Configuration
            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = builder.Configuration
                    .GetValue<string>("CacheSettings:ConnectionString");
            });

            // General Configuration
            builder.Services.AddScoped<IBasketRepository, BasketRepository>();
            builder.Services.AddAutoMapper(typeof(Program));

            // Grpc Configuration
            builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(
                o => o.Address = new Uri(builder.Configuration["GrpcSettings:DiscountUrl"]));
            builder.Services.AddScoped<DiscountGrpcService>();

            // MassTransit-RabbitMQ Configuration
            builder.Services.AddMassTransit(config =>
            {
                config.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(builder.Configuration["EventBusSettings:HostAddress"]);
                });
            });

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Basket.API", Version = "v1" });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Basket.API v1"));
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}