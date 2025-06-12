using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retr0OS.Commands
{
    public class HelpCommand : Command
    {
        List<Command> commands;

        public HelpCommand(List<Command> commands) : base("help", "Shows all available commands.")
        {
            this.commands = commands;
        }

        public override void Execute(string[] args)
        {
            Console.WriteLine("Available commands:");
            foreach (var command in commands)
            {
                Console.WriteLine($"\t{command.keyword} - {command.description}");
            }
        }
    }
}
