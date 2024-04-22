using Microsoft.EntityFrameworkCore;
using OasisAPI.Config;
using OasisAPI.Context;
using OasisAPI.Dto;
using OasisAPI.Extensions;
using OasisAPI.Interfaces;
using OasisAPI.Repositories;
using OasisAPI.Services;
using OasisAPI.Utils;

var builder = WebApplication.CreateBuilder(args);

//MySql
var mysqlConnection = builder.Configuration.GetConnectionString("DefaultConnection");

//Serviço EF
builder.Services.AddDbContext<OasisDbContext>(options =>
    options.UseMySql(mysqlConnection, ServerVersion.AutoDetect(mysqlConnection))
);

//Repositórios
builder.Services.AddScoped<IUserRepository, UserRepository>();

//Configuradores de Serviços
builder.Services.Configure<ChatGptConfig>(builder.Configuration.GetSection("ChatGPT"));
builder.Services.Configure<GeminiConfig>(builder.Configuration.GetSection("Gemini"));

//Serviços
builder.Services.AddScoped<IChatbotsService, ChatbotsService>();
builder.Services.AddScoped<IUserService, UserService>();

//AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

//Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

var app = builder.Build();

DatabaseConnectionTester.TestConnection(app.Services);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.ConfigureExceptionHandler(app.Environment);
app.UseHttpsRedirection();
app.MapControllers();
app.MapGet("/", () => "Oasis API!");
app.Run();