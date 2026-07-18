using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IUserRepository,UserRepository>();
builder.Services.AddScoped<IUserService,UserService>();

builder.Services.AddScoped<ISkillRepository,SkillRepository>();
builder.Services.AddScoped<ISkillService,SkillService>();

builder.Services.AddScoped<IQuestRepository,QuestRepository>();
builder.Services.AddScoped<IQuestService,QuestService>();

builder.Services.AddDbContext<LifeGrindDbContext>(options =>
{
    options.UseNpgsql(configuration.GetConnectionString("LifeGrindDbContext"));
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();
