using GenerativeAI.Models;
using OasisAPI.Configurations;
using OasisAPI.Extensions;
using OasisAPI.Interfaces;
using OasisAPI.Services;

var builder = WebApplication.CreateBuilder(args);

//Configuradores de Serviços
builder.Services.Configure<ChatGptConfig>(builder.Configuration.GetSection("ChatGPT"));
builder.Services.Configure<GeminiConfig>(builder.Configuration.GetSection("Gemini"));

//Serviços dos Chatbots
builder.Services.AddScoped<IChatbotService, ChatGptService>();
builder.Services.AddScoped<IChatbotService, GeminiService>();

//Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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