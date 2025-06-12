using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retr0OS.Commands
{
    public class ClearCommand : Command
    {
        public ClearCommand() : base("clear", "Clears the console.")
        {
        }

        public override void Execute(string[] args)
        {
            Console.Clear();
        }
    }
}
