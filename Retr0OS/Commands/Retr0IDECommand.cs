using Cosmos.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Retr0OS.Commands
{
    public class Retr0IDECommand : Command
    {
        private List<string> lines;
        private int cursorX, cursorY;
        private string filePath;

        private readonly string[] highlightWords = {
            "print",
            "consoleColor",
            "executeCommand",

            "if",
            "true",
            "false",

            "+",
            "-",
            "/",
            "*",
            "=",

            "number",
            "bool",
            "string"
        };
        private readonly ConsoleColor[] highlightColors = {
            ConsoleColor.Green,
            ConsoleColor.Green,
            ConsoleColor.Green,

            ConsoleColor.DarkRed,
            ConsoleColor.DarkRed,
            ConsoleColor.DarkRed,

            ConsoleColor.Yellow,
            ConsoleColor.Yellow,
            ConsoleColor.Yellow,
            ConsoleColor.Yellow,
            ConsoleColor.Yellow,

            ConsoleColor.Blue,
            ConsoleColor.Blue,
            ConsoleColor.Blue
        };

        public Retr0IDECommand()
            : base("retr0ide", "Built-in basic IDE for Retr0Script. Usage: retr0ide [fileName]") { }

        public override void Execute(string[] args)
        {
            filePath = args[0];
            lines = new List<string>();
            if (File.Exists(filePath))
                lines.AddRange(File.ReadAllLines(filePath));
            if (lines.Count == 0) lines.Add("");

            cursorX = cursorY = 0;
            Console.Clear();
            Render();

            while (true)
            {
                var key = Console.ReadKey(true);
                if ((key.Modifiers & ConsoleModifiers.Alt) != 0 && key.Key == ConsoleKey.C)
                {
                    if (ConfirmExit()) break;
                    Console.Clear();
                }
                else HandleKey(key);
                Render();
            }
        }

        private void HandleKey(ConsoleKeyInfo key)
        {
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
                        cursorY--; cursorX = prevLen;
                    }
                    break;
                case ConsoleKey.Enter:
                    var cur = lines[cursorY];
                    var rest = cur.Substring(cursorX);
                    lines[cursorY] = cur.Substring(0, cursorX);
                    lines.Insert(cursorY + 1, rest);
                    cursorY++; cursorX = 0;
                    break;
                default:
                    if (key.KeyChar >= ' ')
                    {
                        lines[cursorY] = lines[cursorY].Insert(cursorX, key.KeyChar.ToString());
                        cursorX++;
                    }
                    break;
            }
        }

        private void Render()
        {
            Console.Clear();
            const int maxW = 80, maxH = 25;
            for (int y = 0; y < lines.Count && y < maxH; y++)
            {
                Console.SetCursorPosition(0, y);
                var line = lines[y];
                int i = 0;
                while (i < line.Length)
                {
                    bool matched = false;
                    // try each highlight word
                    for (int w = 0; w < highlightWords.Length; w++)
                    {
                        var word = highlightWords[w];
                        if (i + word.Length <= line.Length)
                        {
                            bool ok = true;
                            for (int j = 0; j < word.Length; j++)
                                if (line[i + j] != word[j]) { ok = false; break; }
                            if (ok)
                            {
                                Console.ForegroundColor = highlightColors[w];
                                for (int j = 0; j < word.Length; j++)
                                    Console.Write(line[i + j]);
                                Console.ResetColor();
                                i += word.Length;
                                matched = true;
                                break;
                            }
                        }
                    }
                    if (!matched)
                    {
                        Console.ResetColor();
                        Console.Write(line[i]);
                        i++;
                    }
                }
                if (line.Length < maxW)
                    Console.Write(new string(' ', maxW - line.Length));
            }

            cursorY = Math.Min(cursorY, Math.Min(lines.Count - 1, maxH - 1));
            cursorX = Math.Min(cursorX, Math.Min(lines[cursorY].Length, maxW - 1));
            Console.SetCursorPosition(cursorX, cursorY);
        }

        private bool ConfirmExit()
        {
            Console.Clear();
            Console.WriteLine("Save changes before exit?");
            Console.WriteLine("[Y]es  [N]o  [S]tart code  [C]ancel");

            while (true)
            {
                var k = Console.ReadKey(true).Key;
                if (k == ConsoleKey.Y)
                {
                    File.WriteAllText(filePath, string.Join("\r\n", lines));
                    return true;
                }
                if (k == ConsoleKey.N) return true;
                if (k == ConsoleKey.C) return false;
                if (k == ConsoleKey.S)
                {
                    RunRetr0Script(lines);
                    Console.WriteLine("Press any key to return to editor...");
                    Console.ReadKey(true);
                    return false;
                }
            }
        }

        private void RunRetr0Script(List<string> codeLines)
        {
            var vars = new Dictionary<string, object>();
            int i = 0;
            while (i < codeLines.Count)
            {
                string line = codeLines[i].Trim();
                if (line == "" || line.StartsWith("//")) { i++; continue; }

                // if statement
                if (line.StartsWith("if "))
                {
                    string condition = line.Substring(3).Trim(':').Trim();
                    bool result = EvaluateCondition(condition, vars);
                    i++;
                    if (result)
                    {
                        while (i < codeLines.Count && codeLines[i].Trim() != "end if")
                        {
                            RunRetr0Script(new List<string> { codeLines[i] });
                            i++;
                        }
                    }
                    else
                    {
                        while (i < codeLines.Count && codeLines[i].Trim() != "end if") i++;
                    }
                    i++; // skip 'end if'
                    continue;
                }

                // variable definition
                if (line.StartsWith("number ") || line.StartsWith("bool ") || line.StartsWith("string "))
                {
                    var parts = line.Split(new[] { ' ', '=' }, 3);
                    if (parts.Length == 3)
                    {
                        string type = parts[0];
                        string name = parts[1];
                        string valueRaw = parts[2];

                        object value = type switch
                        {
                            "number" => double.Parse(valueRaw),
                            "bool" => bool.Parse(valueRaw),
                            "string" => valueRaw.Trim('"'),
                            _ => null
                        };
                        vars[name] = value;
                    }
                    i++;
                    continue;
                }

                // print statement
                if (line.StartsWith("print "))
                {
                    string expression = line.Substring(6).Trim();
                    var result = EvaluateExpression(expression, vars);
                    Console.WriteLine(result);
                    i++;
                    continue;
                }

                i++;
            }
        }
        private bool EvaluateCondition(string condition, Dictionary<string, object> vars)
        {
            if (vars.ContainsKey(condition) && vars[condition] is bool b) return b;
            if (bool.TryParse(condition, out bool val)) return val;
            return false;
        }
        private object EvaluateExpression(string expr, Dictionary<string, object> vars)
        {
            // Replace variable names with their values (safely for strings)
            foreach (var kv in vars)
            {
                if (kv.Value is string)
                    expr = expr.Replace(kv.Key, $"\"{kv.Value}\"");
                else
                    expr = expr.Replace(kv.Key, kv.Value.ToString());
            }

            // If string expression (e.g., "Hello" + "World")
            if (expr.Contains("\""))
            {
                var parts = expr.Split(new[] { '+' }, StringSplitOptions.RemoveEmptyEntries);
                string result = "";
                foreach (var part in parts)
                {
                    string trimmed = part.Trim().Trim('"');
                    result += trimmed;
                }
                return result;
            }

            // Otherwise numeric
            try { return SimpleEval(expr); }
            catch { return expr; }
        }
        private double SimpleEval(string expr)
        {
            var tokens = expr.Split(' ');
            double result = double.Parse(tokens[0]);

            for (int i = 1; i < tokens.Length; i += 2)
            {
                string op = tokens[i];
                double next = double.Parse(tokens[i + 1]);

                result = op switch
                {
                    "+" => result + next,
                    "-" => result - next,
                    "*" => result * next,
                    "/" => result / next,
                    _ => throw new Exception("Unknown operator")
                };
            }
            return result;
        }

    }
}
