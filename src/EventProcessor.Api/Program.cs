using EventProcessor.Api.Middlewares;
using EventProcessor.Application.DependencyInjection;
using EventProcessor.Consumer.DependencyInjection;
using EventProcessor.DAL.DependencyInjection;
using EventProcessor.Producer.DependencyInjection;
using Serilog;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddDataAccessLayer(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddProducer();
builder.Services.AddConsumer();

builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(x =>
{
    x.WithHeaders().AllowAnyHeader();
    x.WithOrigins("http://localhost:5113");
    x.WithMethods().AllowAnyMethod();
});

app.UseHttpsRedirection();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.MapControllers();
app.Run();
