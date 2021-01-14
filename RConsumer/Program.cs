using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace RConsumer
{
    class Program
    {
        private static ILogger LOG;
        private static IConnection ConexaoFabrica;
        private static IModel Canal;

        static void Main(string[] args)
        {
            LOG = Log.CriarLog();
            LOG.LogInformation("Iniciando Execução - " + DateTime.Now);

            LOG.LogInformation("Fabricando a connexão - " + DateTime.Now);
            ConexaoFabrica = RabbitMQConfig.FabricaConexao(LOG);

            LOG.LogInformation("Criando o canal - " + DateTime.Now);
            using (Canal = ConexaoFabrica.CreateModel())
            {
                LOG.LogInformation("Criando a fila Hudson - " + DateTime.Now);
                Canal.QueueDeclare("Hudson", true, false, false, CriarExchangeDeadLetter());

                LOG.LogInformation("Configurando BasicQos por consumer prefetchCount = 1  - " + DateTime.Now);
                Canal.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);// global = false por consumer | global = true por channel

                var consumer = new EventingBasicConsumer(Canal);// Criando o consumidor
                consumer.Received += (model, ea) =>
                {
                    try
                    {
                        var body = ea.Body.ToArray();
                        LOG.LogInformation($" [x] Received {Encoding.UTF8.GetString(body)}");

                        throw new Exception();

                        Canal.BasicAck(ea.DeliveryTag, multiple: false);
                    }
                    catch (Exception ex)
                    {
                        LOG.LogInformation(ex, $"Houve um erro ao processar a mensagem - " + DateTime.Now);
                        Canal.BasicNack(ea.DeliveryTag, false, false);
                    }
                };
                Canal.BasicConsume("Hudson", false, consumer);// Toda vez ele vai confimar que recebeu o item

                LOG.LogInformation(" Press [enter] to exit - " + DateTime.Now);
                Console.ReadLine();
            }
        }

        public static bool ExcluirFila(string nome, bool excluirSeNaoUsada = true, bool excluirSeVazia = true)
        {
            try
            {
                Canal.QueueDelete(nome, excluirSeNaoUsada, excluirSeVazia);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static Dictionary<string, object> CriarExchangeDeadLetter()
        {
            try
            {
                LOG.LogInformation("Criando Exchange DeadLetterExchange - " + DateTime.Now);
                Canal.ExchangeDeclare("DeadLetterExchange", ExchangeType.Fanout);

                LOG.LogInformation("Criando a fila DeadLetterQueue - " + DateTime.Now);
                Canal.QueueDeclare(queue: "DeadLetterQueue",
                                    durable: true,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);

                LOG.LogInformation("Fazendo o bind DeadLetterQueue ra DeadLetterExchange - " + DateTime.Now);
                Canal.QueueBind("DeadLetterQueue", "DeadLetterExchange", "");

                return new Dictionary<string, object>() 
                { 
                    { "x-dead-letter-exchange", "DeadLetterExchange" } 
                };
            }
            catch (Exception ex)
            {
                LOG.LogError(ex, "Criando Exchange DeadLetterExchange - " + DateTime.Now);
                return null;
            }
        }
    }
}
