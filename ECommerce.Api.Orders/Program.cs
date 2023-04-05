using ECommerce.Api.Orders.Db;
using ECommerce.Api.Orders.Interfaces;
using ECommerce.Api.Orders.Providers;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<OrdersDbContext>(option =>
{
    option.UseInMemoryDatabase("Orders");
});
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddScoped<IOrdersProvider, OrdersProvider>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
