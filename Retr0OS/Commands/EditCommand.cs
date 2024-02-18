using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retr0OS.Commands
{
    public class EditCommand : Command
    {
        public string filePath;
        public string currentText;

        public EditCommand() : base("edit", "Opens and edits file.")
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

            if (args.Length <= 0)
            {
                Console.WriteLine("Usage: edit [file path]");
                return;
            }

            if (File.Exists(realPath))
            {
                filePath = realPath;
                currentText = File.ReadAllText(realPath);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("The file " + realPath + " doesnt exist!");
                Console.ForegroundColor = ConsoleColor.White;

                return;
            }
            
            Console.Clear();

            Console.WriteLine(currentText);

            WriteChar();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nFile contents saved successfully.");
            Console.ForegroundColor = ConsoleColor.White;
        }

        public void WriteChar()
        {
            ConsoleKeyInfo keyPressed = Console.ReadKey(false);

            if (keyPressed.Key != ConsoleKey.Escape)
            {
                switch (keyPressed.Key)
                {
                    case ConsoleKey.Backspace:
                        currentText = currentText.Remove(currentText.Length - 1, 1);
                        break;
                    case ConsoleKey.Enter:
                        currentText += "\n";
                        break;
                    default:
                        currentText += keyPressed.KeyChar;
                        break;
                }

                Console.Clear();
                Console.Write(currentText);
                
                WriteChar();
            }
            else // Attempt closing editor
            {
                File.WriteAllText(filePath, currentText);

                return;
            }
        }
    }
}
