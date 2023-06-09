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
            Console.WriteLine("Hello World!");

            var client = new AmazonSQSClient(RegionEndpoint.USEast1);
            var request = new ReceiveMessageRequest
            {
                QueueUrl = "https://sqs.us-east-1.amazonaws.com/"
            };

            while (true)
            {
                var response = await client.ReceiveMessageAsync(request);

                foreach (var mensagem in response.Messages)
                {
                    Console.WriteLine(mensagem.Body);
                    await client.DeleteMessageAsync("https://sqs.us-east-1.amazonaws.com/", mensagem.ReceiptHandle);
                }
            }
        }
    }
}
