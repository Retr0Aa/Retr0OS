using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retr0OS.Commands
{
    public class ViewCommand : Command
    {
        public ViewCommand() : base("view", "Views file's contents.")
        {
        }

        public override void Execute(string[] args)
        {
            string realPath = "";

            for (int i = 0; i < args.Length; i++)
            {
                if (i > 0)
                    realPath += " " + args[i];
                else
                    realPath += args[i];
            }

            if (File.Exists(realPath))
            {
                Console.WriteLine(File.ReadAllText(realPath));
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("The file " + realPath + " doesnt exist!");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
    }
}
