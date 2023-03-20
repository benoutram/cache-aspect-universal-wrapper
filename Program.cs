using CacheAspectUniversalWrapper.Cache;
using CacheAspectUniversalWrapper.Cache.Attributes;
using CacheAspectUniversalWrapper.Cache.Interfaces;
using CacheAspectUniversalWrapper.Services;
using CacheAspectUniversalWrapper.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ICacheService, NoOpCacheService>();
builder.Services.AddScoped<IWeatherForecastService, WeatherForecastService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var serviceScope = app.Services.CreateScope())
{
    TestCacheAttribute.CacheService = serviceScope.ServiceProvider.GetRequiredService<ICacheService>();
}

app.Run();
