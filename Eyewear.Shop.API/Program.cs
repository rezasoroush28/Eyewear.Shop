using Eyewear.Shop.Application.Interfaces.Repository;
using Eyewear.Shop.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<AppDbContext>());
builder.Services.AddScoped<IOtpRepository>(provider => provider.GetRequiredService<AppDbContext>());
builder.Services.AddScoped<IUserRepository>(provider => provider.GetRequiredService<AppDbContext>());

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseAuthorization();

app.MapControllers();

app.Run();
