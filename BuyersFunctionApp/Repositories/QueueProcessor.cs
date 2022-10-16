using Azure.Messaging.ServiceBus;
using BuyersFunctionApp.Model;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using JsonConvert = Newtonsoft.Json.JsonConvert;

namespace BuyersFunctionApp.Repositories
{
    public class QueueProcessor : IQueueProcessor
    {
        private readonly IBuyerRepository _buyerRepository;
        private readonly string ConnectionString = "Endpoint=sb://eauctionasb.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=B2/0fZbSEawJvA7pgzz1kr/2MfVchtgK+UPipmzyEfE=";
        private readonly string QueueName = "buyers";
        static ServiceBusClient client;
        static ServiceBusSender sender;

        public QueueProcessor(IBuyerRepository buyerRepository)
        {
            _buyerRepository = buyerRepository;

        }

        public async Task<ActionResult> SendMessageAsync(BuyerProduct product)
        {
            try
            {
                var clientOptions = new ServiceBusClientOptions() { TransportType = ServiceBusTransportType.AmqpWebSockets };
                client = new ServiceBusClient(ConnectionString, clientOptions);
                sender = client.CreateSender(QueueName);
                string json = JsonConvert.SerializeObject(product);
                var msg = BinaryData.FromString(json);
                var message = new ServiceBusMessage
                {
                    MessageId = Guid.NewGuid().ToString(),
                    ContentType = "application/json",
                    CorrelationId = Guid.NewGuid().ToString(),
                    Body = msg
                };

                await sender.SendMessageAsync(message);

                // await ReceiveMessageAsync();

                return new OkObjectResult("Message successfully triggered");
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
                throw;
            }

        }

        public async Task ReceiveMessageAsync()
        {
            var clientOptions = new ServiceBusClientOptions() { TransportType = ServiceBusTransportType.AmqpWebSockets };
            client = new ServiceBusClient(ConnectionString, clientOptions);
            var receiver = client.CreateReceiver(QueueName, new ServiceBusReceiverOptions { ReceiveMode = ServiceBusReceiveMode.ReceiveAndDelete });

            var messageOfQueue = new List<ServiceBusReceivedMessage>();

            var messageBatch = await receiver.ReceiveMessagesAsync(10, maxWaitTime: TimeSpan.FromSeconds(20));

            messageOfQueue.AddRange(messageBatch);


            foreach (var message in messageOfQueue)
            {
                await InsertToDB(message);

            }
        }

        private async Task InsertToDB(ServiceBusReceivedMessage message)
        {
            var strData = Encoding.UTF8.GetString(message.Body);

            var messageResponse = BsonSerializer.Deserialize<BsonDocument>(strData);

            BuyerProduct buyerProduct = new BuyerProduct()
            {
                BidAmount = Convert.ToDouble(messageResponse.GetValue("BidAmount")),
                Address = messageResponse.GetValue("Address").ToString(),
                City = messageResponse.GetValue("City").ToString(),
                Email = messageResponse.GetValue("Email").ToString(),
                FirstName = messageResponse.GetValue("FirstName").ToString(),
                LastName = messageResponse.GetValue("LastName").ToString(),
                Phone = messageResponse.GetValue("Phone").ToString(),
                Pin = messageResponse.GetValue("Pin").ToString(),
                State = messageResponse.GetValue("State").ToString(),
            };

            await _buyerRepository.PlaceBidAsync(buyerProduct);
        }
    }
}
