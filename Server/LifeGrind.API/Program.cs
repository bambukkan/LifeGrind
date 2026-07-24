using System.Text;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.Configure<JwtOptions>(
    configuration.GetSection("JwtOptions")
);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwtKey = configuration["JwtOptions:SecretKey"];

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey!)
            ),
            ValidateIssuer = true,
            ValidIssuer = configuration["JwtOptions:Issuer"],

            ValidateAudience = true,
            ValidAudience = configuration["JwtOptions:Audience"],

            ValidateLifetime = true,
        };
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                context.Token = context.Request.Cookies["Access-cookies"];
                return Task.CompletedTask;
            }
        };
    }
);

builder.Services.AddValidatorsFromAssemblyContaining<CreateQuestRequestValidator>();

builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ITransactionManager, TransactionManager>();

builder.Services.AddScoped<IJwtProvider, JwtProvider>();

builder.Services.AddScoped<IUserRepository,UserRepository>();
builder.Services.AddScoped<IUserService,UserService>();

builder.Services.AddScoped<ISkillRepository,SkillRepository>();
builder.Services.AddScoped<ISkillService,SkillService>();

builder.Services.AddScoped<IQuestRepository,QuestRepository>();
builder.Services.AddScoped<IQuestService,QuestService>();

builder.Services.AddScoped<IPersonalRewardRepository,PersonalRewardRepository>();
builder.Services.AddScoped<IPersonalRewardService,PersonalRewardService>();

builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();

builder.Services.AddDbContext<LifeGrindDbContext>(options =>
{
    options.UseNpgsql(configuration.GetConnectionString("LifeGrindDbContext"));
});

builder.Services.AddControllers(options =>
    {
        options.Filters.Add<ValidationFilter>();
    })
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
    });
    
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<GlobalExceptionMiddlware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();

/*
cd Client
npm run dev
Stop-Process -Id 21132
cd C:\Users\gk\Desktop\LearningBackend\LifeGrind\Server\LifeGrind.API
dotnet run
*/