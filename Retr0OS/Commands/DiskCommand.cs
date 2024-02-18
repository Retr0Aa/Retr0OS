using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retr0OS.Commands
{
    public class DiskCommand : Command
    {
        public DiskCommand() : base("disk", "Performs actions with file system." +
            "\n\tType \"disk help\" to see description of all disk commands.")
        {
        }

        public override void Execute(string[] args)
        {
            string entryToEdit = "";

            for (int i = 1; i < args.Length; i++)
            {
                if (i > 1)
                    entryToEdit += " " + args[i];
                else
                    entryToEdit += args[i];
            }

            switch (args[0])
            {
                case "crfile":
                    if (!File.Exists(entryToEdit))
                    {
                        try
                        {
                            Cosmos.System.FileSystem.VFS.VFSManager.CreateFile(entryToEdit);

                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("The file " + entryToEdit + " was created successfully!");
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        catch (Exception e)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Error creating file " + entryToEdit + ". Error:\n" + e.Message);
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("The file " + entryToEdit + " already exists!");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    break;
                case "delfile":
                    if (File.Exists(entryToEdit))
                    {
                        try
                        {
                            File.Delete(entryToEdit);

                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("The file " + entryToEdit + " was deleted successfully!");
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        catch (Exception e)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Error deleting file " + entryToEdit + ". Error:\n" + e.Message);
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("The file " + entryToEdit + " doesn't exist!");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    break;
                case "crdir":
                    if (!Directory.Exists(entryToEdit))
                    {
                        try
                        {
                            Cosmos.System.FileSystem.VFS.VFSManager.CreateDirectory(entryToEdit);

                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("The directory " + entryToEdit + " was created successfully!");
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        catch (Exception e)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Error creating directory " + entryToEdit + ". Error:\n" + e.Message);
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("The directory " + entryToEdit + " already exists!");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    break;
                case "deldir":

                    if (Directory.Exists(entryToEdit))
                    {
                        try
                        {
                            Directory.Delete(entryToEdit, true);

                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("The directory " + entryToEdit + " was deleted successfully!");
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        catch (Exception e)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Error deleting directory " + entryToEdit + ". Error:\n" + e.Message);
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("The directory " + entryToEdit + " Doesn't exist!");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    break;
                case "help":
                    Console.WriteLine("disk crfile [path]: Creates empty file in path.");
                    Console.WriteLine("disk delfile [path]: deletes file from path.");
                    Console.WriteLine("disk crdir [path]: Creates empty directory from path.");
                    Console.WriteLine("disk deldir [path]: Deletes directory in path.");
                    Console.WriteLine("disk list: Shows all files and folders in path or current directory.");
                    Console.WriteLine("disk help: Shows this help screen.");
                    break;
                case "list":
                    Console.WriteLine("Name,\tType");
                    Console.WriteLine("---------------");
                    
                    foreach (var item in Directory.GetFiles(Directory.GetCurrentDirectory()))
                    {
                        FileInfo fi = new FileInfo(item);
                        
                        Console.WriteLine(fi.Name + "\t" + fi.Extension);
                    }
                    foreach (var item in Directory.GetDirectories(Directory.GetCurrentDirectory()))
                    {
                        DirectoryInfo di = new DirectoryInfo(item);
                        
                        Console.WriteLine(di.Name + "\t" + "Folder");
                    }
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Usage: disk [Action]. \nType \"disk help\" to see description of all disk commands.");
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
            }
        }
    }
}
