using Microsoft.EntityFrameworkCore;
using Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<FabrikaContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("FabrikaCS"));
});

// BITNO: dodaj kontrolere
builder.Services.AddControllers();

//dodato
builder.Services.AddCors(options =>
{
    options.AddPolicy("ClientPolicy", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection(); //zakomentarisano

//dodato
app.UseRouting();
app.UseCors("ClientPolicy");
app.UseAuthorization();

// BITNO: mapiraj kontrolere
app.MapControllers();

app.Run();
