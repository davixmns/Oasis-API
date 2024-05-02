using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using OasisAPI.Config;
using OasisAPI.Context;
using OasisAPI.Dto;
using OasisAPI.Extensions;
using OasisAPI.Interfaces;
using OasisAPI.Interfaces.Repositories;
using OasisAPI.Interfaces.Services;
using OasisAPI.Middlewares;
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

//jwt
builder.Services.AddAuthentication("CustomJwtAuth")
    .AddScheme<AuthenticationSchemeOptions, JwtAuthenticationMiddleware>("CustomJwtAuth", _ => {});
//UnitOfWork
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

//Repositórios
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IChatRepository, ChatRepository>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();

//Configuradores de Serviços
builder.Services.Configure<ChatGptConfig>(builder.Configuration.GetSection("ChatGPT"));
builder.Services.Configure<GeminiConfig>(builder.Configuration.GetSection("Gemini"));
builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("Jwt"));

//Serviços
builder.Services.AddScoped<IChatbotsService, ChatbotsService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITokenService, TokenService>();

//AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

//Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.Urls.Add("http://0.0.0.0:5013");

DatabaseConnectionTester.TestConnection(app.Services);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseMiddleware<JwtMiddleware>();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.ConfigureExceptionHandler(app.Environment);
app.MapControllers();
app.MapGet("/", () => "Oasis API!");
app.Run();