using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace RProducer
{
    public class RabbitMQConfig
    {
        private const string HOSTNAME = "localhost";
        private const string USERNAME = "devgordinho";
        private const string PASSWORD = "Rabbitmq@12345";
        
        //public static IConnection FabricaConexao()
        //{
        //    try
        //    {
        //        return new ConnectionFactory
        //        {
        //            HostName = HOSTNAME,
        //            UserName = USERNAME,
        //            Password = PASSWORD
        //        }.CreateConnection();
        //    }
        //    catch(Exception ex)
        //    {
        //        Console.WriteLine("");   
        //    }
        //}
    }
}
