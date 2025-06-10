using CloudPhotoStorage.DataBase;
using CloudPhotoStorage.DataBase.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

IConfiguration configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

// Регистрация DbContext 
builder.Services.AddDbContext<ApplicationContext>(
    options => options.UseNpgsql(configuration.GetConnectionString("CloudPhotoStorageDataBase")));

// Add services to the container.
builder.Services.AddScoped<CategoryRepo>();
builder.Services.AddScoped<ImageRepo>();
builder.Services.AddScoped<UserRepo>();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
