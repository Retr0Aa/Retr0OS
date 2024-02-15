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
        public CDCommand() : base("cd", "Changes Directory.")
        {
        }

        public override void Execute(string[] args)
        {
            Directory.SetCurrentDirectory(args[0]);
        }
    }
}
