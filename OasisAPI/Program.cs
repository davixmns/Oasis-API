using OasisAPI.Configurations;
using OasisAPI.Extensions;
using OasisAPI.Interfaces;
using OasisAPI.Services;

var builder = WebApplication.CreateBuilder(args);

var chatGptConfigurations = builder.Configuration.GetSection("ChatGPT");
var geminiConfigurations = builder.Configuration.GetSection("Gemini");

builder.Services.Configure<ChatGptConfig>(chatGptConfigurations);
builder.Services.Configure<GeminiConfig>(geminiConfigurations);

builder.Services.AddScoped<IChatGptService, ChatGptService>();
builder.Services.AddScoped<IGeminiService, GeminiService>();

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