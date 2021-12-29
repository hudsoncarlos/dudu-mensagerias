using System;
using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;

namespace Consumer
{
    class Consumer
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Iniciando Fornecedor.");

            var client = new AmazonSQSClient(RegionEndpoint.SAEast1);
            var request = new ReceiveMessageRequest
            {
                QueueUrl = ""
            };

            var response = await client.ReceiveMessageAsync(request);

            foreach (var item in response.Messages)
            {
                Console.WriteLine(item.Body);
                await client.DeleteMessageAsync("", item.ReceiptHandle);
            }
        }
    }
}
