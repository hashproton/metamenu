using Api.Middlewares;
using Application;
using Application.Models;
using Infra;

var builder = WebApplication.CreateBuilder(args);

builder
    .Services
    .AddInfra(builder.Environment.EnvironmentName.ParseEnvironment(), builder.Configuration)
    .AddApplication();

builder.Services.AddScoped<SetAuthContextMiddleware>();

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddControllers();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

var app = builder.Build();
app.UseExceptionHandler(opt => { });
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors(pb => pb.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
}

app.UseMiddleware<SetAuthContextMiddleware>();
app.MapControllers();
app.UseHttpsRedirection();

app.Run();