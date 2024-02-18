using Cosmos.System.FileSystem;
using Cosmos.System.FileSystem.VFS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retr0OS.Commands
{
    public class InfoCommand : Command
    {
        CosmosVFS fs;

        public InfoCommand(CosmosVFS fs) : base("info", "Gets system information.")
        {
            this.fs = fs;
        }

        public override void Execute(string[] args)
        {
            var available_space = fs.GetAvailableFreeSpace(@"0:\");
            string label = VFSManager.GetFileSystemLabel(@"0:\");
            string fs_type = VFSManager.GetFileSystemType(@"0:\");
            Console.WriteLine("Retr0OS Version 0.5. Created by Alexander Buchkov (Retr0A)");
            Console.WriteLine("Available Free Space: " + available_space);
            Console.WriteLine("File System Label: " + label);
            Console.WriteLine("File System Type: " + fs_type);
        }
    }
}
