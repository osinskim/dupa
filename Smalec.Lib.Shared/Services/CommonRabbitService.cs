using System;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Smalec.Lib.Shared.Abstraction.Interfaces;
using Smalec.Lib.Shared.Helpers;
using Smalec.Lib.Shared.Commands;

namespace Smalec.Lib.Shared.Services
{
    public class CommonRabbitService : ICommonRabbitService
    {
        private readonly IConnection _connection;

        public CommonRabbitService()
        {
            var factory = new ConnectionFactory() { HostName = "rabbitmq" };
            _connection = factory.CreateConnection();
        }

        public async Task PerformActionAfterFileUploaded(SynchronizedRequest actionRequest, Func<object, object, Task> actionAfterAck)
        {
            var channel = _connection.CreateModel();

            channel.ExchangeDeclare(exchange: "dupa", type: ExchangeType.Direct);
            channel.QueueDeclare(queue: actionRequest.RequestId, exclusive: false, autoDelete: true);
            channel.QueueBind(actionRequest.RequestId, exchange: "dupa", routingKey: actionRequest.RequestId);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var uploadRequestResult = JsonConvert.DeserializeObject<UploadSuccessCommand>(Encoding.UTF8.GetString(body));
                await actionAfterAck(uploadRequestResult, actionRequest);
            };

            await Task.Run(() => channel.BasicConsume(queue: actionRequest.RequestId, autoAck: true, consumer: consumer));
        }
    }
}