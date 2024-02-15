using Cosmos.System.FileSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retr0OS.Commands
{
    public class CommandManager
    {
        private List<Command> commands;

        public CommandManager(CosmosVFS fs)
        {
            commands = new List<Command>(1);
            commands.Add(new InfoCommand(fs));
            commands.Add(new CDCommand());
            commands.Add(new HelpCommand(commands));
        }

        public void ProcessInput(string input)
        {
            string[] split = input.Split(' ');

            string label = split[0];

            List<string> args = new List<string>();

            int ctr = 0;
            foreach (var s in split)
            {
                if (ctr != 0)
                    args.Add(s);
                ++ctr;
            }

            foreach (Command cmd in commands)
            {
                if (cmd.name == label)
                {
                    cmd.Execute(args.ToArray());

                    return;
                }
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("The command \"" + label + "\" does not exist!");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
