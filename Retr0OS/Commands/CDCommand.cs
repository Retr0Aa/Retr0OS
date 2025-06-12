using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retr0OS.Commands
{
    public class CDCommand : Command
    {
        public CDCommand() : base("cd", "Enters directory. usage: cd [directoryPath]")
        {
        }

        public override void Execute(string[] args)
        {
            if (args.Length > 0)
            {
                if (args.Length == 1 && args[0] == "..")
                {
                    // Go to the parent directory
                    if (Directory.GetCurrentDirectory() == Directory.GetDirectoryRoot(Directory.GetCurrentDirectory()))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("You are already at the root directory.");
                        Console.ForegroundColor = ConsoleColor.White;
                        return;
                    }

                    Directory.SetCurrentDirectory(Directory.GetParent(Directory.GetCurrentDirectory()).FullName);
                    return;
                }

                string path = string.Join(' ', args);
                if (Directory.Exists(path))
                {
                    Directory.SetCurrentDirectory(new DirectoryInfo(path).FullName);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Directory {path} does not exist.");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Usage: cd [directoryPath]");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
    }
}
