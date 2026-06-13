using Decoupler.Application;
using Decoupler.Infrastructure;
using Decoupler.WebApi;
using Decoupler.WebApi.Common.Endpoints;
using Decoupler.WebApi.Common.Logging;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureSerilog();

builder.Services.AddOpenApi(options => options.AddScalarTransformers());

builder.Services
    .AddPresentationLayer()
    .AddApplicationLayer()
    .AddInfrastructureLayer();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapEndpoints();
app.UseHttpsRedirection();

app.Run();