using System.Collections.Concurrent;
using System.Text;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMQConsumer
{
    public class RabbitMQConsumerService : BackgroundService
    {
        private readonly ILogger<RabbitMQConsumerService> _logger;
        private IConnection _connection;
        private IModel _channel;
        private readonly BlockingCollection<string> _messageQueue;

        public RabbitMQConsumerService(ILogger<RabbitMQConsumerService> logger)
        {
            _logger = logger;
            _messageQueue = new BlockingCollection<string>();
            InitializeRabbitMQListener();
        }

        private void InitializeRabbitMQListener()
        {
            var factory = new ConnectionFactory() { HostName = "host.docker.internal" , Port = 5672,
                UserName = "guest",
                Password = "guest",
            };
            _connection = factory.CreateConnection();
            _channel= _connection.CreateModel();
            _channel.QueueDeclare("myQueue",false,false,false,arguments:null);
            _logger.LogInformation("Rabbit MQ Connection and Channel Initialized.");
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
           stoppingToken.ThrowIfCancellationRequested();
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                _messageQueue.Add(content);
                _channel.BasicAck(ea.DeliveryTag, false);
            };
            _channel.BasicConsume("myQueue", false, consumer);
            return Task.CompletedTask;
        }
        public string GetMessage()
        {
            _messageQueue.TryTake(out var message,TimeSpan.FromSeconds(1));
            return message;
        }
        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }


    }
}
