using Api.Middlewares;
using Application;
using Application.Models;
using Infra;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder
    .Services
    .AddInfra(builder.Environment.EnvironmentName.ParseEnvironment(), builder.Configuration)
    .AddApplication();

builder.Services.AddScoped<SetAuthContextMiddleware>();

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen(op =>
    {
        op.SwaggerDoc("v1", new() { Title = "Metamenu API", Version = "v1" });
        op.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please insert JWT with Bearer into field",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "bearer"
        });
        
        op.AddSecurityDefinition("RefreshToken", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please insert Refresh Token into field",
            Name = "RefreshToken",
            Type = SecuritySchemeType.Http,
        });
        
        op.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });
    
    })
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