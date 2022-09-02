using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// RabbitMQ
/*
ConnectionFactory factory = new ConnectionFactory();
factory.UserName = "bbsway";
factory.Password = "th&bbsw4y";
factory.VirtualHost = "/";
factory.HostName = "localhost:5672";

IConnection conn = factory.CreateConnection();

IModel channel = conn.CreateModel();
channel.QueueDeclare("echo-service", false, false, false );
var consumer = new EventingBasicConsumer(channel);
consumer.Received += (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine(" [x] Received {0}", message );
};
channel.BasicConsume("echo-service", true, consumer);
*/ 

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