using System;
using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;

namespace Producer
{
    class Producer
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var client = new AmazonSQSClient(RegionEndpoint.USEast1);
            var request = new SendMessageRequest
            {
                QueueUrl = "https://sqs.us-east-1.amazonaws.com/",
                MessageBody = "teste 123"
            };

            await client.SendMessageAsync(request);
        }
    }
}
