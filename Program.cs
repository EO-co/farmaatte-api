using farmaatte_api.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<FarmaatteDbContext>(options => options.UseNpgsql("Host=http://postgres:5432;Database=farmaatte_db;username=myuser;Password=mypassword"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// We don't use this, since this is handled by nginx
// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
