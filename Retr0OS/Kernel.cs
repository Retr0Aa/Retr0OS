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
        private List<Command> commands = new()
        {
            new ExitCommand(),
            new DirectoryCommand(),
            new ClearCommand(),
            new ViewCommand(),
            new CDCommand(),
            new EditCommand(),
            new InfoCommand(),
            new Retr0IDECommand()
        };

        protected override void BeforeRun()
        {
            commands.Add(new HelpCommand(commands));

            Directory.SetCurrentDirectory(@"0:\");

            Sys.FileSystem.CosmosVFS fs = new Cosmos.System.FileSystem.CosmosVFS();
            Sys.FileSystem.VFS.VFSManager.RegisterVFS(fs);

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Retr0OS Booted successfully! Type \"help\" to show all available commands.");
            Console.ForegroundColor = ConsoleColor.White;
        }

        protected override void Run()
        {
            Console.Write(Directory.GetCurrentDirectory() + "|");
            var input = Console.ReadLine();

            try
            {
                if (commands.Exists(command => command.keyword == input.Split()[0]))
                {
                    List<string> rawArgs = input.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();
                    rawArgs.RemoveAt(0);

                    commands.Find(command => command.keyword == input.Split()[0]).Execute(rawArgs.ToArray());
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error: Command not found!");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error: {ex.Message}");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
    }
}
