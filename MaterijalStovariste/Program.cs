using Microsoft.EntityFrameworkCore;
using Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<StovaristeContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Stovariste2CS"));
});


// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();

// BITNO: dodaj kontrolere
builder.Services.AddControllers();


//Za swagger:
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

//Za swagger:
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.MapOpenApi();
// }

app.UseHttpsRedirection();

// BITNO: mapiraj kontrolere
app.MapControllers();


app.Run();

