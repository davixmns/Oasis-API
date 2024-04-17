using GenerativeAI.Models;
using Microsoft.EntityFrameworkCore;
using OasisAPI.Configurations;
using OasisAPI.Context;
using OasisAPI.Extensions;
using OasisAPI.Interfaces;
using OasisAPI.Services;

var builder = WebApplication.CreateBuilder(args);

//MySql
var mysqlConnection = builder.Configuration.GetConnectionString("DefaultConnection");

//Serviço EF
builder.Services.AddDbContext<OasisDbContext>(options =>
    options.UseMySql(mysqlConnection, ServerVersion.AutoDetect(mysqlConnection))
);

//Configuradores de Serviços
builder.Services.Configure<ChatGptConfig>(builder.Configuration.GetSection("ChatGPT"));
builder.Services.Configure<GeminiConfig>(builder.Configuration.GetSection("Gemini"));

//Serviços dos Chatbots
builder.Services.AddScoped<IChatGptService, ChatGptService>();
builder.Services.AddScoped<IGeminiService, GeminiService>();

//Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<OasisDbContext>();
    try
    {
        dbContext.Database.OpenConnection();
        dbContext.Database.CloseConnection();
    }
    catch (Exception ex)
    {
        Console.WriteLine("Erro ao conectar com o banco de dados: " + ex.Message);
    }
}

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