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

        [Option("createtable", HelpText = "Create Datapoint table")]
        public bool DoCreateTable { get; set; }

        [Option("droptable", HelpText = "Drop Datapoint table")]
        public bool DoDropTable { get; set; }

        [Option("truncatetable", HelpText = "Drop Datapoint table")]
        public bool DoTruncateTable { get; set; }

        public int Run()
        {
            if (Helper.DiffersFromThreshold(1, DoTestConnection, DoCreateTable, DoDropTable, DoTruncateTable))
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

            return 0;
        }
    }
}
