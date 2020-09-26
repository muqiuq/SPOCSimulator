using CommandLine;
using SPOCSimulator.Commands;
using System;
using System.Diagnostics;

namespace SPOCSimulator
{
    class Program
    {
        static void Main(string[] args)
        {
#if (!DEBUG)
            try
            {
#endif
                var parsed = Parser.Default.ParseArguments<
                    VoidCommand,
                    ExampleCommand,
                    GenerateCommand,
                    RunCommand,
                    DbCommand
                    >(args);
                parsed.MapResult(
                    (VoidCommand c) => c.Run(),
                    (ExampleCommand c) => c.Run(),
                    (GenerateCommand c) => c.Run(),
                    (RunCommand c) => c.Run(),
                    (DbCommand c) => c.Run(),
                    e => 1);
#if (!DEBUG)
            }catch(Exception e)
            {
                Console.WriteLine(e);
            }
#endif

        }
    }
}
