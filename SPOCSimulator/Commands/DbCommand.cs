using CommandLine;
using CommandLine.Text;
using MySql.Data.MySqlClient;
using SPOCSimulator.Statistics;
using SPOCSimulator.Utils;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;

namespace SPOCSimulator.Commands
{
    [Verb("db", HelpText = "db helper commands")]
    public class DbCommand : DbBaseCommand, ICommandVerb
    {

        public DbCommand()
        {
        }

        [Option('t', "testconn", HelpText = "Test Connection")]
        public bool TestConnection { get; set; }

        [Option("createtable", HelpText = "Create Datapoint table")]
        public bool CreateTable { get; set; }

        [Option("droptable", HelpText = "Drop Datapoint table")]
        public bool DropTable { get; set; }

        [Option("truncatetable", HelpText = "Drop Datapoint table")]
        public bool TruncateTable { get; set; }

        public int Run()
        {
            if (Helper.DiffersFromThreshold(1, TestConnection, CreateTable, DropTable, TruncateTable))
            {
                Print("You may only select one function!");
                return 1;
            }
            
            ConnectDb();

            if(CreateTable)
            {
                var command = conn.CreateCommand();
                command.CommandText = MySQLHelper.GetCreateTable("datapoints", typeof(SimulationDatapoint));
                var res = command.ExecuteNonQuery();
                Print("Table created {0}", res);
            }
            if(DropTable)
            {
                var command = conn.CreateCommand();
                command.CommandText = "DROP TABLE datapoints;";
                var res = command.ExecuteNonQuery();
                Print("Table dropped {0}", res);
            }
            if(TruncateTable)
            {
                var command = conn.CreateCommand();
                command.CommandText = "TRUNCATE TABLE datapoints;";
                var res = command.ExecuteNonQuery();
                Print("Table truncated {0}", res);
            }

            return 0;
        }
    }
}
