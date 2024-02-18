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
            string realPath = "";

            for (int i = 0; i < args.Length; i++)
            {
                if (i > 0)
                    realPath += " " + args[i];
                else
                    realPath += args[i];
            }

            string chosenDir = realPath.StartsWith(@"0:\") ? realPath + @"\" : Directory.GetCurrentDirectory() + realPath + @"\";

            if (args.Length > 0)
            {
                if (Directory.Exists(chosenDir))
                {
                    Directory.SetCurrentDirectory(chosenDir);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("The directory \"" + chosenDir + "\" does not exist!");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Usage: cd [Directory Path]");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
    }
}
