using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retr0OS.Commands
{
    public class ViewCommand : Command
    {
        public ViewCommand() : base("view", "Views file content.")
        {
        }

        public override void Execute(string[] args)
        {
            if (args.Length > 0)
            {
                try
                {
                    Console.WriteLine(File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), args[0])));
                }
                catch (IOException e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error: " + e.Message);
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Usage: view [fileName]");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
    }
}
