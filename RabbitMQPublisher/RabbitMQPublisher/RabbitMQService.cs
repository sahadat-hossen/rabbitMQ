using System.Text;

using RabbitMQ.Client;

namespace RabbitMQPublisher
{
 
    public class RabbitMQService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMQService()
        {
            var factory = new ConnectionFactory()
            {
                HostName = "host.docker.internal",
                Port = 5672,
                UserName= "guest",
                Password= "guest",
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
        }
        public void CloseConnection()
        {
            _channel.Close();
            _connection.Close();
        }
        public void SendMessage(string queueName,string message)
        {
            _channel.QueueDeclare(queue: queueName, durable: false, false, false, null) ;
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null,body: body);
        }
    }
}
