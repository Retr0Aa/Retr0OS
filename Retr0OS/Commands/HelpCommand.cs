using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retr0OS.Commands
{
    public class HelpCommand : Command
    {
        public List<Command> commands;

        public HelpCommand(List<Command> commands) : base("help", "Shows all avaliable commands and their usage.")
        {
            this.commands = commands;
        }

        public override void Execute(string[] args)
        {
            foreach (var command in commands)
            {
                Console.WriteLine(command.name + ":\n\t" + command.description);
            }
        }
    }
}
