using CommandLine;
using MySql.Data.MySqlClient;
using SPOCSimulator.Statistics;
using SPOCSimulator.Utils;
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

        [Option('d', "db", HelpText = "database to use", Required = false, Default = "spocsim")]
        public string Db { get; set; }

        [Option("tableprefix", HelpText = "table prefix", Required = false, Default = "")]
        public string TablePrefix { get; set; }

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
            Print("Connecting...");
            conn = new MySql.Data.MySqlClient.MySqlConnection(GetConnectionString());
            conn.Open();
            Print("Connected!");
        }

        protected void TruncateTable(string table)
        {
            var command = conn.CreateCommand();
            command.CommandText = "TRUNCATE TABLE " + table +";";
            var res = command.ExecuteNonQuery();
            Print("Table truncated {0}", res);
        }

        protected void DropTable(string table, bool ifExists = false)
        {
            var command = conn.CreateCommand();
            command.CommandText = "DROP TABLE " + (ifExists ? "IF EXISTS " : "") + table + "; ";
            var res = command.ExecuteNonQuery();
            Print("Table dropped {0}", res);
        }

        protected void CreateTable(string table, Type type)
        {
            var command = conn.CreateCommand();
            command.CommandText = InternalMySqlHelper.GetCreateTable(table, type);
            var res = command.ExecuteNonQuery();
            Print("Table created {0}", res);
        }

        protected void CreateTables()
        {
            CreateTable(TablePrefix + Statics.TableDatapoints, typeof(SimulationDatapoint));
            CreateTable(TablePrefix + Statics.TableSummaries, typeof(SimulationSummary));
        }

        protected void TruncateTables()
        {
            TruncateTable(TablePrefix + Statics.TableDatapoints);
            TruncateTable(TablePrefix + Statics.TableSummaries);
        }

        protected void DropTables(bool ifExists = false)
        {
            DropTable(TablePrefix + Statics.TableDatapoints, ifExists);
            DropTable(TablePrefix + Statics.TableSummaries, ifExists);
        }

        private int DeleteForMarker(string marker, string table)
        {
            var deleteCommand = new MySqlCommand(string.Format("DELETE FROM {0} WHERE Marker=@Marker", table), conn);
            deleteCommand.Parameters.Add("@Marker", MySqlDbType.VarChar).Value = marker;
            return deleteCommand.ExecuteNonQuery();
        }

        protected int DeleteDatapointsForMarker(string marker) => DeleteForMarker(marker, Statics.TableDatapoints);
        
        protected int DeleteSummariesForMarker(string marker) => DeleteForMarker(marker, Statics.TableSummaries);

    }
}
