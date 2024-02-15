using Cosmos.System.FileSystem;
using Retr0OS.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Sys = Cosmos.System;

namespace Retr0OS
{
    public class Kernel : Sys.Kernel
    {
        public List<Command> commands;

        protected override void BeforeRun()
        {
            CosmosVFS fs = new CosmosVFS();
            Sys.FileSystem.VFS.VFSManager.RegisterVFS(fs);
            Directory.SetCurrentDirectory(@"0:\");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Retr0OS Booted Successfully!");
            Console.ForegroundColor = ConsoleColor.White;

            commands = new List<Command>() { new InfoCommand(fs), new CDCommand(), new HelpCommand(commands) };
        }

        protected override void Run()
        {
            Console.Write(Directory.GetCurrentDirectory());
            var input = Console.ReadLine();

            if (commands.Exists(cmd => cmd.name == input.Split(" ")[0]))
            {
                string[] args = input.Split(" ").Skip(1).ToArray();

                commands.Find(cmd => cmd.name == input).Execute(args);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("The command \"" + input + "\" does not exist!");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
    }
}
