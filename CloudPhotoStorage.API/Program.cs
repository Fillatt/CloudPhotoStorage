using CloudPhotoStorage.DataBase;
using CloudPhotoStorage.DataBase.Repositories;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Регистрация DbContext 
builder.Services.AddDbContext<ApplicationContext>();

// Add services to the container.
builder.Services.AddScoped<CategoryRepo>();
builder.Services.AddScoped<ImageRepo>();
builder.Services.AddScoped<LoginHistoryRepo>();
builder.Services.AddScoped<RolesRepo>();
builder.Services.AddScoped<UserRepo>();
builder.Services.AddScoped<WasteBasketRepo>();

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
