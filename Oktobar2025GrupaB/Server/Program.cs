using Microsoft.EntityFrameworkCore;
using Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<HamburgerContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("HamburgerCS")
    )
);


// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
// builder.Services.AddOpenApi();

// BITNO: dodaj kontrolere
builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("ClientPolicy", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});



builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

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

app.UseRouting();
app.UseCors("ClientPolicy");
app.UseAuthorization();


//app.UseHttpsRedirection();

// BITNO: mapiraj kontrolere
app.MapControllers();


app.Run();
