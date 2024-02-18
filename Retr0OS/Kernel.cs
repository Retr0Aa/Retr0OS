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
        private CommandManager commandManager;

        protected override void BeforeRun()
        {
            CosmosVFS fs = new CosmosVFS();
            Sys.FileSystem.VFS.VFSManager.RegisterVFS(fs);
            Directory.SetCurrentDirectory(@"0:\");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Retr0OS Booted Successfully!");
            Console.ForegroundColor = ConsoleColor.White;

            commandManager = new CommandManager(fs);
        }

        protected override void Run()
        {
            Console.Write(new DirectoryInfo(Directory.GetCurrentDirectory()).FullName + ">");
            var input = Console.ReadLine();

            commandManager.ProcessInput(input);
        }
    }
}
