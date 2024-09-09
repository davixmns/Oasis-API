using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using OasisAPI.Clients;
using OasisAPI.Config;
using OasisAPI.Context;
using OasisAPI.Dto;
using OasisAPI.Interfaces;
using OasisAPI.Interfaces.Clients;
using OasisAPI.Interfaces.Repositories;
using OasisAPI.Interfaces.Services;
using OasisAPI.Middlewares;
using OasisAPI.Repositories;
using OasisAPI.Services;
using OasisAPI.Utils;

var builder = WebApplication.CreateBuilder(args);

DotNetEnv.Env.Load();
var mysqlConnection = Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING")!;
var chatGptApiKey = Environment.GetEnvironmentVariable("CHATGPT_API_KEY")!;
var chatGptAssistantId = Environment.GetEnvironmentVariable("CHATGPT_ASSISTANT_ID")!;
var geminiApiKey = Environment.GetEnvironmentVariable("GEMINI_API_KEY")!;
var geminiModel = Environment.GetEnvironmentVariable("GEMINI_MODEL")!;
var jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET")!;
var jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER")!;
var jwtAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE")!;
int.TryParse(Environment.GetEnvironmentVariable("ACCESS_TOKEN_EXPIRY")!, out var accessTokenExpiry);
int.TryParse(Environment.GetEnvironmentVariable("REFRESH_TOKEN_EXPIRY")!, out var refreshTokenExpiry);

//Serviço EF
builder.Services.AddDbContext<OasisDbContext>(options =>
    options.UseMySql(mysqlConnection, ServerVersion.AutoDetect(mysqlConnection))
);

//jwt
builder.Services.AddAuthentication("CustomJwtAuth")
    .AddScheme<AuthenticationSchemeOptions, JwtAuthenticationMiddleware>("CustomJwtAuth", _ => {});

//UnitOfWork
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

//Repositórios
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IChatRepository, ChatRepository>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();

//Configuradores de Serviços
builder.Services.Configure<ChatGptConfig>(config =>
{
    config.ApiKey = chatGptApiKey;
    config.AssistantId = chatGptAssistantId;
});

builder.Services.Configure<GeminiConfig>(config =>
{
    config.ApiKey = geminiApiKey;
    config.Model = geminiModel;
});

builder.Services.Configure<JwtConfig>(config =>
{
    config.SecretKey = jwtSecret;
    config.Issuer = jwtIssuer;
    config.Audience = jwtAudience;
    config.AccessTokenExpiry = accessTokenExpiry;
    config.RefreshTokenExpiry = refreshTokenExpiry;
});

//Serviços
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IChatService, ChatService>();
builder.Services.AddScoped<ITokenService, TokenService>();

//AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

//Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

//Clientes
builder.Services.AddScoped<IChatGptClient, ChatGptClient>();
builder.Services.AddScoped<IGeminiClient, GeminiClient>();

var app = builder.Build();

app.Urls.Add("http://0.0.0.0:5013");

DatabaseConnectionTester.TestConnection(app.Services);

app.UseMiddleware<GlobalExceptionHandler>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapGet("/", () => "Oasis API!");
app.Run();