using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retr0OS.Commands
{
    public class InfoCommand : Command
    {
        public InfoCommand() : base("info", "Shows system information.")
        {
        }

        public override void Execute(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Retr0OS - Operating System");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Version: 1.0");
            Console.WriteLine("Interface: Console");
            Console.WriteLine("Author: Alexander Buchkov (Retr0A)");
        }
    }
}
