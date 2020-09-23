using CommandLine;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace SPOCSimulator.Commands
{
    public abstract class DbBaseCommand : BaseCommand
    {
        protected MySqlConnection conn;

        [Option('h', "server", HelpText = "server host", Required = false, Default = "localhost")]
        public string Host { get; set; }

        [Option('d', "db", HelpText = "db", Required = false, Default = "spocsim")]
        public string Db { get; set; }

        [Option('u', "username", HelpText = "mysql username", Required = false, Default = "root")]
        public string Username { get; set; }

        [Option('p', "password", HelpText = "mysql password", Required = false, Default = "root")]
        public string Password { get; set; }

        [Option('P', "port", HelpText = "server host", Required = false, Default = 3306)]
        public int Port { get; set; }

        protected string GetConnectionString()
        {
            return string.Format("Server={0};Port={1};Uid={2};Pwd={3};Database={4}",
                Host,
                Port,
                Username,
                Password,
                Db
                );
        }

        protected void ConnectDb()
        {
            conn = new MySql.Data.MySqlClient.MySqlConnection(GetConnectionString());
            conn.Open();
            Print("Connected!");
        }

    }
}
