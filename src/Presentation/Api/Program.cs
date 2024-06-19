using Application;
using Application.Models;
using Infra;

var builder = WebApplication.CreateBuilder(args);

builder
    .Services
    .AddInfra(builder.Environment.EnvironmentName.ParseEnvironment(), builder.Configuration)
    .AddApplication();

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.UseHttpsRedirection();

app.Run();