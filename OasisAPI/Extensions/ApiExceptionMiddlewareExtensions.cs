using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using OasisAPI.Models;

namespace OasisAPI.Extensions; 

public static class ApiExceptionMiddlewareExtensions
{
    //tratamento de exceções globais
    public static void ConfigureExceptionHandler(this IApplicationBuilder app, IWebHostEnvironment environment)
    {
        app.UseExceptionHandler(appError =>
        {
            appError.Run(async context =>
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                if (contextFeature != null)
                {
                    var errorDetails = new ErrorDetails
                    {
                        StatusCode = context.Response.StatusCode,
                        Message = "Ocorreu um erro interno..."
                    };

                    if (environment.IsDevelopment())
                    {
                        errorDetails.Message = contextFeature.Error.Message;
                        errorDetails.Trace = contextFeature.Error.StackTrace;
                    }
                    
                    await context.Response.WriteAsync(errorDetails.ToString());
                }
            });
        });
    }
}