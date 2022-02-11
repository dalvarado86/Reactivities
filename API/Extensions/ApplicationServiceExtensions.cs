using Application;
using Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddInfrastructure(configuration);
            services.AddApplication();

            services.AddCors(options => 
            {
                options.AddPolicy(name: "CorsPolicy", policy => 
                {
                    policy.AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithOrigins(configuration["Tokens:Audience"]);
                });
            });
             
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Reactivities API", Version = "v1" });
            }); 

            return services;
        }
    }
}