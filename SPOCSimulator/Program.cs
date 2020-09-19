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
            //try
            //{
                var parsed = Parser.Default.ParseArguments<
                    VoidCommand,
                    ExampleCommand,
                    GenerateCommand,
                    RunCommand
                    >(args);
                parsed.MapResult(
                    (VoidCommand c) => c.Run(),
                    (ExampleCommand c) => c.Run(),
                    (GenerateCommand c) => c.Run(),
                    (RunCommand c) => c.Run(),
                    e => 1);
            /*}catch(Exception e)
            {
                Console.WriteLine(e);
                if (Debugger.IsAttached) throw e;
            }*/

        }
    }
}
