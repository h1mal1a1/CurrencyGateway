using System.Text;
using CurrencyGateway.Application.Interfaces;
using CurrencyGateway.Application.Services;
using CurrencyGateway.Infrastructure.Cbr;
using CurrencyGateway.Application.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddProblemDetails();

builder.Services.AddScoped<ICurrencyService, CurrencyService>();

builder.Services.AddHttpClient<ICbrClient, CbrClient>(client =>
{
    client.BaseAddress = new Uri("https://www.cbr.ru/");
});

var app = builder.Build();

app.UseExceptionHandler(exceptionHandlerApp =>
{
    exceptionHandlerApp.Run(async context =>
    {
        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;

        if (exception is ExternalServiceException)
        {
            context.Response.StatusCode = StatusCodes.Status502BadGateway;

            await Results.Problem(statusCode: StatusCodes.Status502BadGateway, title: "External service error",
                detail: exception.Message)
                    .ExecuteAsync(context);

            return;
        }

        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

        await Results.Problem(statusCode: StatusCodes.Status500InternalServerError, title: "Internal server error")
            .ExecuteAsync(context);
    });
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();