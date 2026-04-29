using Microsoft.EntityFrameworkCore;
using Models;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BibliotekaContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("BibliotekaCS"));
});

// BITNO: dodaj kontrolere
builder.Services.AddControllers();



// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();
//Dodato za swagger:
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

//Dodato za swagger:
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

