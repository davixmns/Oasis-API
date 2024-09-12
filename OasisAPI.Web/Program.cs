using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using OasisAPI.App.Commands;
using OasisAPI.App.Config;
using OasisAPI.App.Interfaces.Services;
using OasisAPI.App.Mapper;
using OasisAPI.App.Services;
using OasisAPI.App.Validators;
using OasisAPI.Infra.Clients;
using OasisAPI.Infra.Context;
using OasisAPI.Infra.Mapper;
using OasisAPI.Infra.Repositories;
using OasisAPI.Infra.Utils;
using OasisAPI.Interfaces.Services;
using OasisAPI.Middlewares;

var builder = WebApplication.CreateBuilder(args);

DotNetEnv.Env.Load();
var mysqlConnection = Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING") ?? throw new Exception("DATABASE_CONNECTION_STRING not found");
var chatGptApiKey = Environment.GetEnvironmentVariable("CHATGPT_API_KEY") ?? throw new Exception("CHATGPT_API_KEY not found");
var chatGptAssistantId = Environment.GetEnvironmentVariable("CHATGPT_ASSISTANT_ID") ?? throw new Exception("CHATGPT_ASSISTANT_ID not found");
var geminiApiKey = Environment.GetEnvironmentVariable("GEMINI_API_KEY") ?? throw new Exception("GEMINI_API_KEY not found");
var geminiModel = Environment.GetEnvironmentVariable("GEMINI_MODEL") ?? throw new Exception("GEMINI_MODEL not found");
var jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET") ?? throw new Exception("JWT_SECRET not found");
var jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? throw new Exception("JWT_ISSUER not found");
var jwtAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? throw new Exception("JWT_AUDIENCE not found");
int.TryParse(Environment.GetEnvironmentVariable("ACCESS_TOKEN_EXPIRY")!, out var accessTokenExpiry);
int.TryParse(Environment.GetEnvironmentVariable("REFRESH_TOKEN_EXPIRY")!, out var refreshTokenExpiry);

//Serviço EF
builder.Services.AddDbContext<OasisDbContext>(options =>
    options.UseMySql(mysqlConnection, ServerVersion.AutoDetect(mysqlConnection))
);

//MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<LoginCommand>());

//FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<LoginRequestDtoValidator>();
builder.Services.AddFluentValidationAutoValidation();

//jwt
builder.Services.AddAuthentication("CustomJwtAuth")
    .AddScheme<AuthenticationSchemeOptions, JwtAuthenticationMiddleware>("CustomJwtAuth", _ => {});

//UnitOfWork
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

//Repositórios
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

//Clientes
builder.Services.AddScoped<IChatGptClient, ChatGptClient>(c =>
{
    var chatGtpConfig = new ChatGptConfig(apiKey: chatGptApiKey, assistantId: chatGptAssistantId);
    return new ChatGptClient(chatGtpConfig, c.GetRequiredService<IMapper>());
});

builder.Services.AddScoped<IGeminiClient, GeminiClient>(c =>
{
    var geminiConfig = new GeminiConfig(apiKey: geminiApiKey, model: geminiModel);
    return new GeminiClient(geminiConfig, c.GetRequiredService<IMapper>());
});

//Serviços
builder.Services.AddScoped<IChatBotsClientFacade, ChatBotsClientFacade>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IChatService, ChatService>();
builder.Services.AddScoped<ITokenService, TokenService>(s =>
{
    var jwtConfig = new JwtConfig(
        secretKey: jwtSecret,
        issuer: jwtIssuer,
        audience: jwtAudience,
        accessTokenExpiry: accessTokenExpiry,
        refreshTokenExpiry: refreshTokenExpiry
    );
    return new TokenService(jwtConfig);
});

//AutoMapper
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<AutoMapperInfraProfile>();
    cfg.AddProfile<AutoMapperAppProfile>();
});

//Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

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