using System.Reflection;
using Application.Behaviors;
using FluentValidation;
using Infrastructure;
using MediatR;

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
app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();
app.Run();
