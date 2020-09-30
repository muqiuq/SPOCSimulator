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

        [Option("testconn", HelpText = "Test connection")]
        public bool DoTestConnection { get; set; }

        [Option("createtables", HelpText = "Create datapoints and summaries table")]
        public bool DoCreateTable { get; set; }

        [Option("droptables", HelpText = "Drop datapoints and summaries tables")]
        public bool DoDropTable { get; set; }

        [Option("truncatetables", HelpText = "Drop datapoints and summaries tables")]
        public bool DoTruncateTable { get; set; }

        [Option("dropcreate", HelpText = "Drop first (if exists) and create datapoints and summaries tables")]
        public bool DoDropCreate { get; set; }

        [Option("clearmarker", HelpText = "clear tables using a specific marker")]
        public bool ClearTablesUsingMarker { get; set; }

        [Option("marker", HelpText = "Marker used for clearmarker function", Default = null)]
        public string Marker { get; set; }

        public int Run()
        {
            if (Helper.DiffersFromThreshold(1, DoTestConnection, DoCreateTable, DoDropTable, DoTruncateTable, DoDropCreate, ClearTablesUsingMarker))
            {
                Print("You may only select one function! (Use --help for more information)");
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
            if(ClearTablesUsingMarker)
            {
                if(Marker == null)
                {
                    Print("Marker must be set!");
                    return 1;
                }
                Print("Deleted datapoints (if exists) ({0})", DeleteDatapointsForMarker(Marker));
                Print("Deleted summary (if exists) ({0})", DeleteSummariesForMarker(Marker));
            }

            return 0;
        }
    }
}
