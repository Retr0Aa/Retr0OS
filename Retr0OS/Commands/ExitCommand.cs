using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retr0OS.Commands
{
    public class ExitCommand : Command
    {
        public ExitCommand() : base("exit", "Shuts down your PC.")
        {
        }

        public override void Execute(string[] args)
        {
            Console.Clear();
            Console.WriteLine("Exiting Retr0OS...");
            Environment.Exit(0);
        }
    }
}
