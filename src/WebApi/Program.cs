using System.Reflection;
using System.Linq;                       // <-- para .Select(...)
using System.Threading.Tasks;            // (geral)
using Application.Behaviors;
using FluentValidation;
using Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;         // <-- para HttpResponse
using Microsoft.AspNetCore.Http.Json;    // <-- para WriteAsJsonAsync
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);

// Application
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.Load("Application")));
builder.Services.AddValidatorsFromAssembly(Assembly.Load("Application"));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

// Infra
builder.Services.AddInfrastructure();

// Web
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (ValidationException ex)
    {
        context.Response.StatusCode = 400;
        await context.Response.WriteAsJsonAsync(new {
            message = "Validation failed",
            errors = ex.Errors.Select(e => new { e.PropertyName, e.ErrorMessage })
        });
    }
});

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();
app.Run();
