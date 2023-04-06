using ECommerce.Api.Search.Interfaces;
using ECommerce.Api.Search.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<ISearchService, SearchService>();
builder.Services.AddScoped<IOrderService, OrdersService>();
builder.Services.AddHttpClient("OrderService", config =>
{
    config.BaseAddress = new Uri(builder.Configuration["Services:Orders"]);
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
