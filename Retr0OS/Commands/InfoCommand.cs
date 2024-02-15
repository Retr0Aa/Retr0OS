using Cosmos.System.FileSystem;
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
            Console.WriteLine("Available Free Space: " + available_space);
        }
    }
}
