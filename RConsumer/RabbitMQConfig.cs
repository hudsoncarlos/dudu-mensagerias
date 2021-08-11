using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace RConsumer
{
    public class RabbitMQConfig
    {
        private const string HOSTNAME = "dudu-ubuntu";
        private const string USERNAME = "devgordinho";
        private const string PASSWORD = "Rabbitmq@12345";

        public static IConnection FabricaConexao(ILogger log)
        {
            try
            {
                log.LogInformation(@$"Fabricando conexão rabbitmq. 
                Hostname: {HOSTNAME} 
                Username: {USERNAME}
                Password: {PASSWORD}");

                return new ConnectionFactory
                {
                    HostName = HOSTNAME,
                    UserName = USERNAME,
                    Password = PASSWORD
                }.CreateConnection();
            }
            catch(Exception ex)
            {
                log.LogError(ex, "Ocorreu um erro ao FabricaConexao");
                return new ConnectionFactory() { HostName = HOSTNAME }.CreateConnection();
            }
        }

        public class Fila
        {
            public string NomeFila { get; set; }
            public bool Duravel { get; set; }
            public bool Exclusive { get; set; }
            public bool AutoDelete { get; set; }
            public object Arguments { get; set; }

            public Fila(string nomeFila, bool durable, bool exclusive, bool autoDelete, object arguments = null)
            {
                NomeFila = nomeFila;
                Duravel = durable;
                Exclusive = exclusive;
                AutoDelete = autoDelete;
                Arguments = arguments;
            }
        }
    }
}
