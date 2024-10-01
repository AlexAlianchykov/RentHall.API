using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RentHall.API.Extations;
using RentHall.Application.AbstractionsServices;
using RentHall.Application.Services;
using RentHall.Application.Services.Jwt;
using RentHall.Core.AbstractionsRepositories.HallAbstractions;
using RentHall.Core.AbstractionsRepositories.UserAbstractions;
using RentHall.DataAccess;
using RentHall.DataAccess.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString(nameof(ApplicationDbContext)));
});

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IHallRepository, HallRepository>(); 
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IAnalyticsRepositoty, AnalyticsRepositoty>();
builder.Services.AddScoped<IAdditionalServicesRepository, AdditionalServicesRepository>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IHallService, HallService>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<IAnalyticsServices, AnalyticsService>();
builder.Services.AddScoped<IAdditionalServicesServices, AdditionalServicesService>();
builder.Services.AddScoped<IPasswordHash, PasswordHash>();
builder.Services.AddScoped<IJwtProvider, JwtProvider>();
builder.Services.AddScoped<AuthenticationService>();

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)));
builder.Services.AddApiAuthentication(builder.Services.BuildServiceProvider().GetRequiredService<IOptions<JwtOptions>>());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
