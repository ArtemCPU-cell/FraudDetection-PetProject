using Contracts.Interfaces;
using DataAccess.Postgres;
using Microsoft.EntityFrameworkCore;
using RiskEngineCore;
using RiskEngineCore.Rules;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddHttpClient();

// IServiceCollection serviceCollection =
builder.Services.AddScoped<IRiskEngine, RiskEngine>();
builder.Services.AddScoped<IDatabase, PostgresDatabase>();
builder.Services.AddScoped<RiskContextFactory>();


builder.Services.AddSingleton<IRiskRule, NewDeviceRule>();
builder.Services.AddSingleton<IRiskRule, NewCountryRule>();
builder.Services.AddSingleton<IRiskRule, LargeAmountRule>();
builder.Services.AddSingleton<IRiskRule, NightTimeTransactionRule>();


builder.Services.AddDbContext<PostgresDatabase>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")));


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
