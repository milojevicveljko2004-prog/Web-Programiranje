using Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//ODKOMENTARISI KAD NAPRAVIS CONTEXT KLASU!
builder.Services.AddDbContext<TakmicenjeContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Takmicenje"));
});


// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();

// BITNO: dodaj kontrolere
builder.Services.AddControllers();


//Dodato za swagger:
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

//dodato za swagger:
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
