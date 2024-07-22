using Microsoft.Extensions.Hosting;
using POS_System.Data;
using POS_System.Entities;
using POS_System.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register the DataContextEntity with the in-memory database provider.
builder.Services.AddDbContext<DataContextEntity>(options =>
    options.UseInMemoryDatabase("InMemoryDb"));

// Register the services
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<UserService>();

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

app.Run();
