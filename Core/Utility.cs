using System.Drawing;

namespace Core;

public class Utility
{
    public static void PrintColor(string text, ConsoleColor color = ConsoleColor.Red)
    {
        Console.ForegroundColor = color;
        Console.Write(text);
        Console.ResetColor();
    }

    public static void PrintColorLine(string text, ConsoleColor color = ConsoleColor.Red)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(text);
        Console.ResetColor();
    }

    public static void RemoveLine(int line = 1)
    {
        for (int i = 0; i < line; i++)
        {
            Console.SetCursorPosition(0, Console.CursorTop - 1);
            for (int j = 0; j < Console.WindowWidth; j++)
                Console.Write(" ");
            
            Console.Write("\r");
        }
    }

    public static void CleanEnter()
    {
        for (int j = 0; j < Console.WindowWidth; j++)
            Console.Write(" ");
        
        Console.Write("\n");
    }
}