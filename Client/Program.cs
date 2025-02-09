using Client.Library;
using System.Drawing;

Console.CursorVisible = false;

Console.WriteLine("Loading...");

var apiArray = await Api.GetStringArrayAsync();
int index = 0;

Console.Clear();

while (true)
{
    DisplayApiArray(apiArray);

    // Highlight Current Selection
    Screen.Print($"{index + 1}. {apiArray[index]}", new Point(0, index), ConsoleColor.Black, ConsoleColor.White);

    // Do not continue until one of these keys are pressed
    var key = Screen.Listen(ConsoleKey.Enter, ConsoleKey.Escape, ConsoleKey.UpArrow, ConsoleKey.DownArrow);

    if (key == ConsoleKey.Enter)
    {
        DisplaySelectedItem(apiArray, index);

        // Reset
        apiArray = await Api.GetStringArrayAsync();
        index = 0;
    }
    else if (key == ConsoleKey.UpArrow)
    {
        index = ChangeSelection(apiArray, index, "up");
    }
    else if (key == ConsoleKey.DownArrow)
    {
        index = ChangeSelection(apiArray, index, "down");
    }
    else if (key == ConsoleKey.Escape)
    {
        // Reset list
        Console.Clear();
        apiArray = await Api.GetStringArrayAsync();
        index = 0;
    }
}

static void DisplayApiArray(string[] apiArray)
{
    var i = 0;
    Console.SetCursorPosition(0, 0);
    foreach (var item in apiArray)
    {
        Console.WriteLine($"{++i}. {item}");
    }
}

static void DisplaySelectedItem(string[] apiArray, int index)
{
    Console.Clear();

    string message = "You selected: " + apiArray[index];

    Screen.Print(message, new Point(1, 1), ConsoleColor.Black, ConsoleColor.White);
    Screen.SurroundWithBorder(new Point(0, 0), new Size(message.Length, 1), Screen.BorderStyle.@single);

    Screen.Print("Press any key to continue.", new Point(1, 20), ConsoleColor.Yellow, ConsoleColor.Blue);


    Console.ReadKey(true);
    Console.Clear();
}

static int ChangeSelection(string[] apiArray, int index, string direction)
{
    if (direction == "up")
    {
        if (index == 0)
        {
            index = apiArray.Length - 1;
        }
        else
        {
            index--;
        }
    }
    else if (direction == "down")
    {
        if (index == apiArray.Length - 1)
        {
            index = 0;
        }
        else
        {
            index++;
        }
    }

    return index;
}