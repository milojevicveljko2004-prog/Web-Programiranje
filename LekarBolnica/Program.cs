using Microsoft.EntityFrameworkCore;
using Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BolnicaContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("BolnicaCS"));
});

// BITNO: dodaj kontrolere
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// BITNO: mapiraj kontrolere
app.MapControllers();

app.Run();

