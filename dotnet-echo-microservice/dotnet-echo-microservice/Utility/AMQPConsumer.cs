using System.Text;
using dotnet_echo_microservice.Models;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace dotnet_echo_microservice.Utility;

public class AMQPConsumer
{
    private string username = "";
    private string password = "";
    private string exchange = "";
    
    private List<string> queues = new List<string>();
    private List<string> routing_keys = new List<string>();
    private List<EventingBasicConsumer> consumers = new List<EventingBasicConsumer>();
    
    private string hostname = "";
    private int port = 5672;

    private IConnection connection = null;
    private IModel channel = null;
    
    public AMQPConsumer( string _username, string _password, string _exchange, 
                         List<string> _queues, List<string> _routing_keys, List<EventingBasicConsumer> _consumers,
                         string _hostname = "rabbitmq", int _port = 5672)
    {
        this.username = _username;
        this.password = _password;
        this.exchange = _exchange;

        this.queues = _queues;
        this.routing_keys = _routing_keys;
        this.consumers = _consumers;

        this.hostname = _hostname;
        this.port = _port;

        this.connection = null;
        this.channel = null;
    }

    private void CreateConnection()
    {
        Console.WriteLine($"Attempting to connect to {this.hostname}");
        ConnectionFactory factory = new ConnectionFactory();
        factory.UserName = this.username;
        factory.Password = this.password;
        factory.VirtualHost = "/";
        factory.HostName = this.hostname;
        factory.Port = this.port;

        this.connection = factory.CreateConnection();
    }

    // Setup canale
    private bool CreateChannel()
    {
        if (this.connection != null)
        {
            this.channel = this.connection.CreateModel();
            
            // Controllo fatto per questo commento su github: https://github.com/celery/kombu/issues/209#ref-commit-5cba634
            if (!string.IsNullOrWhiteSpace(exchange))
            {
                this.channel.ExchangeDeclare(exchange, ExchangeType.Direct);
            }

            return true;
        }

        return false;
    }
    
    // Setup queue
    private void CreateQueues()
    {
        int i = 0;
        foreach (string queue in this.queues)
        {
            if (this.channel != null)
            {
                this.channel.QueueDeclare(queue, false, false, false);
                
                // Commento per via di questo commento su github: https://github.com/celery/kombu/issues/209#issuecomment-300733350
                // this.channel.QueueBind(queue, this.exchange, this.routing_keys[i]);
                
                // TODO: this next line... BISOGNA VEDERE COSA INTENDE FABIO PER callbacks[i]
                // this.channel.BasicConsume(queue: queue, callback: this.callbacks[i] autoAck: false);
                this.consumers.Add(new EventingBasicConsumer(this.channel));
                this.channel.BasicConsume(queue, true, this.consumers.Last());

                ++i;
            }
        }
    }

    private void StartConnection()
    {
        // Start connection to RabbitMQ
        this.CreateConnection();
        bool channelCreated = this.CreateChannel();
        if (channelCreated)
        {
            this.CreateQueues();
            
            Console.WriteLine("Connection and channel are open");
        }
        else
        {
            Console.WriteLine("Channel was not created!");
        }
    }

    public void Listen()
    {
        try
        {
            this.StartConnection();
            Console.WriteLine("Listening for messages");
            foreach (EventingBasicConsumer consumer in this.consumers)
            {
                /*
                consumer.ConsumerCancelled += (model, ea) =>
                {
                    consumer.OnCancel(ea.ConsumerTags);
                };
                */
                
                consumer.Received += (model, ea) =>
                {
                    string response = null; 
    
                    Console.WriteLine($"[Received Event] Exchange: {ea.Exchange}"); // VUOTO
                    Console.WriteLine($"[Received Event] RoutingKey: {ea.RoutingKey}"); // echo-service
                    Console.WriteLine($"[Received Event] ConsumerTag: {ea.ConsumerTag}"); // amq.ctag-2WlphtvmJBXcyP0UL0E0IA
                    
                    var body = ea.Body.ToArray();

                    BasePayload<dynamic> payload = null;

                    try
                    {
                        var message = Encoding.UTF8.GetString(body);

                        payload = JsonConvert.DeserializeObject<BasePayload<dynamic>>(message);

                        // _props.CorrelationId = payload.id;

                        Console.WriteLine(" [x] Received {0}", message );
                        Console.WriteLine($"REPLY TO: \"{ea.BasicProperties.ReplyTo}\"");
                        Console.WriteLine($"ROUTING KEY: \"{ea.RoutingKey}\"");
                        response = message;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(" [.] " + e.Message);
                        response = "";
        
                        channel.Dispose();
                        connection.Dispose();
                    }
                    finally
                    {
                        var replyProps = this.channel.CreateBasicProperties();
                        replyProps.CorrelationId = payload.id;
                        // props.ReplyTo = "echo-service-reply";
                        
                        // var responseBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(responseCustom));
                        
                        channel.BasicPublish("", ea.BasicProperties.ReplyTo, replyProps, ea.Body);
                        // channel.BasicAck(ea.DeliveryTag, false);
                    }
                };
            }
            
        }
        catch (Exception /*AMQPConnectionError*/ e)
        {
            Console.WriteLine("EXCEPTION OCCURRED!");
            Console.WriteLine(e.Message);
            Console.WriteLine(e.InnerException?.Message ?? "NO INNER EXCEPTION PRESENT!");
            
            this.channel.Close();
            this.connection.Close();
            Console.WriteLine($"Retrying to connect to {this.hostname}");

            Task.Delay(5 * 1000).Wait();
            
            this.Listen();
            
            // return this.Listen();
        }
    }
}