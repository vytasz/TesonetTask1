using System.Collections.Generic;
using System.Data;
using TesonetTask1.Model;

namespace TesonetTask1.Modules
{
    public class ServerTable
    {
        private Database db;
        private string tableName = "Servers";
        public ServerTable()
        {
            db = new Database();
        }

        public void WriteServers(List<Server> servers)
        {
            foreach (var server in servers)
            {
                var data = new Dictionary<string, object>();
                data.Add("name", server.name);
                data.Add("distance", server.distance);
                db.Write(tableName, data);
            }

        }

        public List<Server> ReadServers()
        {
            var result = new List<Server>();
            var table = db.Read(tableName);
            foreach (DataRow row in table.Rows)
            {
                var server = new Server();
                server.name = row["name"].ToString();
                server.distance = int.Parse(row["distance"].ToString());
                result.Add(server);
            }
            return result;
        }
    }
}
