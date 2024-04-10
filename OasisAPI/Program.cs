using OasisAPI.Configurations;
using OasisAPI.Extensions;
using OasisAPI.Services;

var builder = WebApplication.CreateBuilder(args);

var chatGptConfigurations = builder.Configuration.GetSection("ChatGPT");

builder.Services.Configure<ChatGptConfig>(chatGptConfigurations);

builder.Services.AddScoped<IChatGptService, ChatGptService>();

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