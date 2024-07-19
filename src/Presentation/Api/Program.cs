using Api.Middlewares;
using Application;
using Infra;

var builder = WebApplication.CreateBuilder(args);

builder
    .Services
    .AddInfra(builder.Configuration)
    .AddApplication();

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen(op =>
    {
        op.SwaggerDoc("v1", new() { Title = "Metamenu API", Version = "v1" });
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

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseHttpsRedirection();

app.Run();