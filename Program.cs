using EFCorePeliculas; // a�adido
using Microsoft.EntityFrameworkCore; // a�adido
using System.Text.Json.Serialization; // a�adido

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(opciones => 
    opciones.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(opciones =>
    {
        opciones.UseSqlServer(connectionString, sqlServer => sqlServer.UseNetTopologySuite());
        opciones.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    }); // le pasamos el conectionstring

builder.Services.AddAutoMapper(typeof(Program));  // a�adido 

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
