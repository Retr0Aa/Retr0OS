using Cosmos.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Retr0OS.Commands
{
    public class EditCommand : Command
    {
        private List<string> lines;
        private int cursorX;
        private int cursorY;
        private string filePath;

        public EditCommand() : base("edit", "Built-in basic text editor. Usage: edit [fileName]") { }

        public override void Execute(string[] args)
        {
            filePath = args[0];
            lines = new List<string>();

            if (File.Exists(filePath))
                lines.AddRange(File.ReadAllLines(filePath));

            if (lines.Count == 0)
                lines.Add("");

            cursorX = 0;
            cursorY = 0;

            Console.Clear();
            Render();

            while (true)
            {
                var key = Console.ReadKey(true);

                if ((key.Modifiers & ConsoleModifiers.Alt) != 0 && key.Key == ConsoleKey.C)
                {
                    if (ConfirmExit()) break;
                    Console.Clear();
                    Render();
                    continue;
                }

                switch (key.Key)
                {
                    case ConsoleKey.LeftArrow:
                        if (cursorX > 0) cursorX--;
                        else if (cursorY > 0) { cursorY--; cursorX = lines[cursorY].Length; }
                        break;

                    case ConsoleKey.RightArrow:
                        if (cursorX < lines[cursorY].Length) cursorX++;
                        else if (cursorY < lines.Count - 1) { cursorY++; cursorX = 0; }
                        break;

                    case ConsoleKey.UpArrow:
                        if (cursorY > 0) { cursorY--; cursorX = Math.Min(cursorX, lines[cursorY].Length); }
                        break;

                    case ConsoleKey.DownArrow:
                        if (cursorY < lines.Count - 1) { cursorY++; cursorX = Math.Min(cursorX, lines[cursorY].Length); }
                        break;

                    case ConsoleKey.Backspace:
                        if (cursorX > 0)
                        {
                            lines[cursorY] = lines[cursorY].Remove(cursorX - 1, 1);
                            cursorX--;
                        }
                        else if (cursorY > 0)
                        {
                            int prevLen = lines[cursorY - 1].Length;
                            lines[cursorY - 1] += lines[cursorY];
                            lines.RemoveAt(cursorY);
                            cursorY--;
                            cursorX = prevLen;
                        }
                        break;

                    case ConsoleKey.Enter:
                        string currentLine = lines[cursorY];
                        string newLine = currentLine.Substring(cursorX);
                        lines[cursorY] = currentLine.Substring(0, cursorX);
                        lines.Insert(cursorY + 1, newLine);
                        cursorY++;
                        cursorX = 0;
                        break;

                    default:
                        if (key.KeyChar >= ' ')
                        {
                            lines[cursorY] = lines[cursorY].Insert(cursorX, key.KeyChar.ToString());
                            cursorX++;
                        }
                        break;
                }

                Console.Clear();
                Render();
            }
        }

        private void Render()
        {
            for (int i = 0; i < lines.Count; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write(lines[i].PadRight(Console.WindowWidth));
            }

            Console.SetCursorPosition(cursorX, cursorY);
        }

        private bool ConfirmExit()
        {
            Console.Clear();
            Console.WriteLine("Save changes before exit?");
            Console.WriteLine("[Y]es  [N]o  [C]ancel");

            while (true)
            {
                var key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.Y)
                {
                    try
                    {
                        StringBuilder sb = new StringBuilder();
                        for (int i = 0; i < lines.Count; i++)
                        {
                            var line = lines[i];

                            if (i == lines.Count - 1)
                                sb.Append(line);
                            else
                                sb.AppendLine(line);
                        }

                        File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), filePath), sb.ToString());
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                    }
                    return true;
                }
                if (key == ConsoleKey.N)
                    return true;
                if (key == ConsoleKey.C)
                    return false;
            }
        }
    }
}
