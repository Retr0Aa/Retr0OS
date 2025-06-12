using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retr0OS.Commands
{
    public class DirectoryCommand : Command
    {
        public DirectoryCommand() : base("dir", "Provides file system operations. Type \"dir help\" to show all directory operations.")
        {
        }

        public override void Execute(string[] args)
        {
            if (args.Length > 0)
            {
                switch (args[0])
                {
                    case "help":
                        Console.WriteLine("Available directory operations:");
                        Console.WriteLine("\tlist: Shows all files/directories in current directory.");
                        Console.WriteLine("\tcrfile [filename]: Creates empty file.");
                        Console.WriteLine("\tcrdir [dirname]: Creates empty directory.");
                        Console.WriteLine("\tdelete [path]: Deletes file/directory.");
                        break;
                    case "list":
                        Console.WriteLine("Files and directories in current directory:");
                        foreach (var file in Directory.GetFiles(Directory.GetCurrentDirectory()))
                        {
                            FileInfo fi = new(file);
                            Console.WriteLine("\t" + fi.Name + " - " + fi.Extension);
                        }
                        foreach (var directory in Directory.GetDirectories(Directory.GetCurrentDirectory()))
                        {
                            DirectoryInfo di = new(directory);
                            Console.WriteLine("\t" + di.Name + " - Directory");
                        }
                        break;
                    case "crfile":
                        if (args.Length > 1)
                        {
                            File.Create(Path.Combine(Directory.GetCurrentDirectory(), args[1])).Close();

                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("File created successfully!");
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("You need third argument to execute this file operation! Type \"dir help\" to show all available operations.");
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        break;
                    case "crdir":
                        if (args.Length > 1)
                        {
                            Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), args[1]));

                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Directory created successfully!");
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("You need third argument to execute this file operation! Type \"dir help\" to show all available operations.");
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        break;
                    case "delete":
                        if (args.Length > 1)
                        {
                            if (File.Exists(args[1]))
                            {
                                File.Delete(args[1]);

                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("File deleted successfully!");
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                            else if (Directory.Exists(args[1]))
                            {
                                Directory.Delete(args[1]);

                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("Directory deleted successfully!");
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                            else
                            {
                                Console.WriteLine("Path does not exist");
                            }
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("You need third argument to execute this file operation! Type \"dir help\" to show all available operations.");
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        break;
                    default:
                        break;
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("You need second argument to provide file operation! Type \"dir help\" to show all available operations.");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
    }
}
