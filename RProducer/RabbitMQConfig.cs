using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace RProducer
{
    public class RabbitMQConfig
    {
        private const string HOSTNAME = "localhost";
        private const string USERNAME = "Dudu";
        private const string PASSWORD = "radeon";

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
