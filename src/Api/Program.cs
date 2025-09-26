using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using VaccinationCard.Application.Common.Behaviors;
using VaccinationCard.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// MediatR (varre Application assembly)
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(
    Assembly.Load("Application")));

// FluentValidation (varre Application)
builder.Services.AddValidatorsFromAssembly(Assembly.Load("Application"));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

// Infra (EF Core + SQLite)
var conn = builder.Configuration.GetConnectionString("Default") ?? "Data Source=vaccination.db";
builder.Services.AddInfrastructure(conn);

// Controllers
builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        // deixa o FluentValidation controlar os erros
        options.InvalidModelStateResponseFactory = context =>
            new BadRequestObjectResult(context.ModelState);
    });

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
