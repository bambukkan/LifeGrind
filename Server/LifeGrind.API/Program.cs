using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddDbContext<LifeGrindDbContext>(options =>
{
    options.UseNpgsql(configuration.GetConnectionString("LifeGrindDbContext"));
});

var app = builder.Build();

app.Run();
