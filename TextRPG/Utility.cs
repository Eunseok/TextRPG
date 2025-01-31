// Utility.cs

namespace TextRPG;

public static class Utility
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
        Console.SetCursorPosition(0, Console.CursorTop - line);
        Console.Write(new string(' ', Console.WindowWidth));
        Console.Write("\r");
    }

    public static int Confirm(int max, int min = 1)
    {
        while (true)
        {
            PrintColor(">> ", ConsoleColor.Yellow);
            if (int.TryParse(Console.ReadLine(), out int num) && num >= min && num <= max)
            {
                RemoveLine(0);
                return num;
            }


            InfoMessage();
        }
    }

    public static void InfoMessage(string message = "잘못된 입력입니다. 다시 입력하세요.", ConsoleColor color = ConsoleColor.Red)
    {
        RemoveLine(0);
        PrintColor(message, color);
        RemoveLine();
    }

    public static bool Save(string message = "")
    {
        RemoveLine();
        Console.WriteLine(message);
        Console.WriteLine("저장하시겠습니까? (1.저장 / 2.취소)");
        if (Confirm(2) == 1)
            return true;

        return false;
    }
}