using RabbitMQ.Client;
using System.Text;

namespace Producer
{
    class Program
    {
        private static IModel Canal;

        static void Main(string[] args)
        {
            // Criando a fabrica
            var factory = new ConnectionFactory() { HostName = "localhost" };
            // Criando as conexões com o RabbitMQ
            using (var connection = factory.CreateConnection())
            // Apartir de uma conexão podemos ter vário channels
            using (Canal = connection.CreateModel())
            {
                // Criar a fila
                Canal.QueueDeclare(queue: "Hudson",
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: CriarExchangeDeadLetter());

                for (int i = 0; i < 2; i++)
                {
                    string message = $"Aprendendo RabbitMQ, publicando mensagens {i}";
                    var body = Encoding.UTF8.GetBytes(message);

                    Canal.BasicPublish(exchange: "",
                                         routingKey: "Hudson",
                                         basicProperties: null,
                                         body: body);

                    Console.WriteLine(" [x] Sent {0}", message);
                }
            }

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }

        public static Dictionary<string, object> CriarExchangeDeadLetter()
        {
            try
            {
                Canal.ExchangeDeclare("DeadLetterExchange", ExchangeType.Fanout);
                Canal.QueueDeclare(queue: "DeadLetterQueue",
                                    durable: true,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);
                Canal.QueueBind("DeadLetterQueue", "DeadLetterExchange", "");

                return new Dictionary<string, object>()
                {
                    { "x-dead-letter-exchange", "DeadLetterExchange" }
                };
            }
            catch
            {
                return null;
            }
        }
    }
}
