using System.Text;
using dotnet_echo_microservice.Models;
using dotnet_echo_microservice.Utility;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// RabbitMQ
var client = new AMQPConsumer("bbsway", "th&bbsw4y", "",
                        new List<string>() { "echo-service" },
                        new List<string>() { "" },
                        new List<EventingBasicConsumer>());

client.Listen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

