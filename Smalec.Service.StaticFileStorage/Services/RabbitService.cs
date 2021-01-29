using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RabbitMQ.Client;
using Smalec.Service.StaticFileStorage.Abstraction;
using Smalec.Lib.Shared.Commands;

namespace Smalec.Service.StaticFileStorage.Services
{
    public class RabbitService : IRabbitService
    {
        private readonly IConnection _connection;

        public RabbitService()
        {
            var factory = new ConnectionFactory() { HostName = "rabbitmq" };
            _connection = factory.CreateConnection();
        }

        public async Task SendPhotoUploadSuccess(string fileName, string requestId, string userId)
        {
            var tmpChannel = _connection.CreateModel();

            tmpChannel.ExchangeDeclare(exchange: "dupa", type: ExchangeType.Direct);
            tmpChannel.QueueDeclare(queue: requestId, exclusive: false, autoDelete: true);
            tmpChannel.QueueBind(requestId, exchange: "dupa", routingKey: requestId);
            tmpChannel.ConfirmSelect();

            var message = JsonConvert.SerializeObject(new UploadSuccessCommand
            {
                RequestId = requestId,
                Resource = fileName,
                UserUuid = userId
            });

            var body = Encoding.UTF8.GetBytes(message);

            await Task.Run(() =>
            {
                MandatoryPublish(tmpChannel, requestId, body);
                tmpChannel.WaitForConfirmsOrDie();
                tmpChannel.Close();
                tmpChannel.Dispose();
            }); 
        }

        private void MandatoryPublish(IModel channel, string routingKey, byte[] body)
        {
            var messageProperties = channel.CreateBasicProperties();
            messageProperties.Persistent = true;
            channel.BasicPublish(exchange: "dupa",
                                        routingKey: routingKey,
                                        basicProperties: messageProperties,
                                        body: body);
        }
    }

}