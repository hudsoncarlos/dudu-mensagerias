using System;
using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;

namespace Producer
{
    class Producer
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Iniciando Fornecedor.");

            var client = new AmazonSQSClient(RegionEndpoint.SAEast1);
            var request = new SendMessageRequest
            {
                QueueUrl = "",
                MessageBody = ""
            };

            client.SendMessageAsync(request);
        }
    }
}
