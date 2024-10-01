using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RentHall.Application.Services.Jwt;
using RentHall.Core.Models.AggregateUser;
using System.Text;

namespace RentHall.API.Extations
{
    public static class ApiExtations
    {
        public static void AddApiAuthentication(this IServiceCollection services, IOptions<JwtOptions> jwtOptions)
        {
            // задаём сценарий аутентификации 
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.TokenValidationParameters = new()
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Value.SecretKey))
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            context.Token = context.Request.Cookies["ttt"];

                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminPolisy", policy =>
                {
                    policy.RequireClaim("UserRole", "admin");
                });

                options.AddPolicy("UserPolisy", policy =>
                {
                    policy.RequireClaim("UserRole", "user", "admin");
                });

                options.AddPolicy("AnalyticsPolisy", policy =>
                {
                    policy.RequireClaim("UserRole", "analytic", "admin");
                });
            });
        }
    }
}
