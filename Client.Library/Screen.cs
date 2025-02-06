using System.Drawing;

using static System.Console;

namespace Client.Library;

public static class Screen
{
    static Screen()
    {
        CursorVisible = false;
    }

    public static void Print(string text, Point? topLeft = null, ConsoleColor? foreground = null, ConsoleColor? background = null)
    {
        Print(text, topLeft?.X, topLeft?.Y, foreground, background);
    }

    public static void Print(string text, int? x = null, int? y = null, ConsoleColor? foreground = null, ConsoleColor? background = null)
    {
        // Throw error if incorrect screen values are passed
        ArgumentException.ThrowIfNullOrEmpty(text, nameof(text));
        ArgumentOutOfRangeException.ThrowIfLessThan(x ?? 0, 0, nameof(x));
        ArgumentOutOfRangeException.ThrowIfGreaterThan(x ?? 0, WindowWidth - text.Length - 2, nameof(x));
        ArgumentOutOfRangeException.ThrowIfLessThan(y ?? 0, 0, nameof(y));
        ArgumentOutOfRangeException.ThrowIfGreaterThan(y ?? 0, WindowHeight - 3, nameof(y));

        SetCursorPosition(x ?? CursorLeft, y ?? CursorTop);

        ForegroundColor = foreground ?? ForegroundColor;
        BackgroundColor = background ?? BackgroundColor;

        Write(text);
        ResetColor();
    }

    public enum BorderStyle { @double, @single }

    public static void SurroundWithBorder(Point topLeft, Size size, BorderStyle style = BorderStyle.@double)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(topLeft.X, 0, nameof(topLeft.X));
        ArgumentOutOfRangeException.ThrowIfLessThan(topLeft.Y, 0, nameof(topLeft.Y));
        ArgumentOutOfRangeException.ThrowIfLessThan(size.Width, 1, nameof(size.Width));
        ArgumentOutOfRangeException.ThrowIfLessThan(size.Height, 1, nameof(size.Height));

        string neChar = style == BorderStyle.@double ? "╔" : "┌";
        string nwChar = style == BorderStyle.@double ? "╗" : "┐";
        string seChar = style == BorderStyle.@double ? "╚" : "└";
        string swChar = style == BorderStyle.@double ? "╝" : "┘";
        string hChar = style == BorderStyle.@double ? "═" : "─";
        string vChar = style == BorderStyle.@double ? "║" : "│";

        topLeft = new Point(Math.Max(0, topLeft.X - 1), Math.Max(0, topLeft.Y - 1));
        size = new(size.Width, size.Height + 1);

        var topLine = $"{neChar}{new string(hChar[0], size.Width)}{nwChar}";
        Print(topLine, topLeft.X, topLeft.Y);

        for (var y = topLeft.Y + 1; y < topLeft.Y + size.Height; y++)
        {
            Print(vChar, topLeft.X, y);
            Print(vChar, topLeft.X + size.Width + 1, y);
        }

        var bottomLine = $"{seChar}{new string(hChar[0], size.Width)}{swChar}";
        Print(bottomLine, topLeft.X, topLeft.Y + size.Height);
    }

    public static ConsoleKey Listen(params ConsoleKey[] allowed)
    {
        ArgumentOutOfRangeException.ThrowIfZero(allowed.Length, nameof(allowed));

        ClearBuffer();

        while (true)
        {
            var key = ReadKey(true).Key;

            if (allowed.Length == 0 || allowed.Contains(key))
            {
                return key;
            }
        }

        static void ClearBuffer()
        {
            while (KeyAvailable)
            {
                ReadKey(true);
            }
        }
    }
}