using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using RabbitMQ.Client;
using RestSharp;
using System.IO;
using RestSharp.Authenticators;

namespace ST.IoT.Spikes.DeleteAllQueuesOnRabbitVirtualHost
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            new Program().deleteAllQueuesOnRabbitMQVirtualHost("localhost", "iot", "iot", virtualHost: "IoT");
        }

        private void deleteAllQueuesOnRabbitMQVirtualHost(
            string hostname, 
            string username, string password,
            int port = 15672, 
            string virtualHost = "/")
        {

            var baseUrl = "http://" + hostname + ":" + port;
            var client = new RestClient(baseUrl);
            client.Authenticator = new HttpBasicAuthenticator(username, password);

            var patchedVHost = virtualHost == "/" ? "%2f" : "/" + virtualHost;
            var requestPath = "api/queues/" + patchedVHost;

            var request = new RestRequest(requestPath,  Method.GET);
            var response = client.Execute(request);

            var queues = JArray.Parse(response.Content);

            foreach (var queue in queues)
            {
                var queueName = queue["name"].ToString();

                var deletePath = "api/queues" + patchedVHost + "/" + queueName;
                var deleteRequest = new RestRequest(deletePath, Method.DELETE);
                var deleteResult = client.Execute(deleteRequest);

                if (deleteResult.ResponseStatus != ResponseStatus.Completed)
                {
                    Console.WriteLine("Could not delete queue: {0} code: {1} reason: {2}", queueName, deleteResult.ResponseStatus, response.Content);
                }
                else
                {
                    Console.WriteLine("Deleted: " + queueName);
                }
            }
        }
    }
}
