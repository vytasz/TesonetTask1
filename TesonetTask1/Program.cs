using System;
using TesonetTask1.Modules;
using TesonetTask1.Request;
using TesonetTask1.Model;
using System.Collections.Generic;
using System.Linq;

namespace TesonetTask1
{
    class Program
    {
        private const string BaseAddress = "https://playground.tesonet.lt/v1";
        static void Main(string[] args)
        {
            var correctLogin = false;
            string token = null;
            var db = new ServerTable();

            while (correctLogin == false) //checks login and token
            {
                Console.WriteLine("Input Username");
                string username = Console.ReadLine();
                Console.WriteLine("Input Password");
                string password = Console.ReadLine();

                using (var Connection =
                    new Connection<TokenRequest, TokenResponse>(new TokenRequest(username, password), BaseAddress))
                {
                    var response = Connection.Execute();
                    if (response != null)
                    {
                        token = response.token;
                    }
                    else
                    {
                        Console.WriteLine("Incorrect Credidentials");
                        Console.ReadLine();
                        continue;
                    }
                }

                List<Server> result = null;
                using (var Connection =
                    new Connection<ServerRequest, List<Server>>(new ServerRequest(token), BaseAddress))
                {
                    result = Connection.Execute(); //TODO
                }
                if (result != null)
                {
                    db.WriteServers(result);
                    correctLogin = true;
                }
                else
                {
                    Console.WriteLine("Received incorrect token");
                    Console.ReadLine();
                    continue;
                }
            }
            Console.Clear();

            var servers = db.ReadServers().OrderByDescending(x => x.distance);
            //Write out servers
            Console.WriteLine("Name:                   Distance:");
            foreach (var server in servers)
            {
                var space = "                        ";
                if (space.Length > server.name.Length)
                {
                    space = space.Remove(0, server.name.Length);
                }
                else space = " ";
                Console.WriteLine(server.name + space + server.distance);
            }
            Console.ReadLine();
        }
    }
}
