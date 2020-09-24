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

        [Option("testconn", HelpText = "Test Connection")]
        public bool DoTestConnection { get; set; }

        [Option("createtables", HelpText = "Create Datapoint table")]
        public bool DoCreateTable { get; set; }

        [Option("droptables", HelpText = "Drop Datapoint table")]
        public bool DoDropTable { get; set; }

        [Option("truncatetables", HelpText = "Drop Datapoint table")]
        public bool DoTruncateTable { get; set; }

        [Option("dropcreate", HelpText = "Drop first (if exists) and create tables")]
        public bool DoDropCreate { get; set; }

        public int Run()
        {
            if (Helper.DiffersFromThreshold(1, DoTestConnection, DoCreateTable, DoDropTable, DoTruncateTable, DoDropCreate))
            {
                Print("You may only select one function!");
                return 1;
            }
            
            ConnectDb();

            if(DoCreateTable)
            {
                CreateTables();
            }
            if(DoDropTable)
            {
                DropTables();
            }
            if(DoTruncateTable)
            {
                TruncateTables();
            }
            if(DoDropCreate)
            {
                DropTables(true);
                CreateTables();
            }

            return 0;
        }
    }
}
