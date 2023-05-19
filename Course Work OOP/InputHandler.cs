namespace Course_Work_OOP;

public abstract class InputHandler
{

    public static void PrintOptions()
    {
        PrintTopAndBottomLine(40);
        PrintTextWithSides("Choose an option:");
        PrintTopAndBottomLine(40);
        PrintTextWithSides("0 | Exit");
        PrintTopAndBottomLine(40);
        // Add new
        PrintTextWithSides("1 | Add new artist");
        PrintTextWithSides("2 | Add new album");
        PrintTextWithSides("3 | Add new song");
        PrintTextWithSides("4 | Create playlist");
        // Print all
        // PrintLine();
        PrintTopAndBottomLine(40);
        PrintTextWithSides("5 | Print all artists");
        PrintTextWithSides("6 | Print all albums");
        PrintTextWithSides("7 | Print all songs");
        // Print songs by
        PrintTopAndBottomLine(40);
        PrintTextWithSides("8 | Print all songs by artist");
        PrintTextWithSides("9 | Print all songs by album");
        PrintTextWithSides("10 | Print all songs by genre");
        // Print albums by
        PrintTopAndBottomLine(40);
        PrintTextWithSides("11 | Print all albums by artist");
        PrintTextWithSides("12 | Print all albums by year");
        PrintTextWithSides("13 | Print all albums by genre");
        // Print artist by
        PrintTopAndBottomLine(40);
        PrintTextWithSides("14 | Print artist by song");
        // Sort by
        PrintTopAndBottomLine(40);
        PrintTextWithSides("15 | Sort songs by artist");
        PrintTextWithSides("16 | Sort songs by album");
        PrintTextWithSides("17 | Sort songs by genre");
        PrintTextWithSides("18 | Sort songs by duration");
        PrintTextWithSides("19 | Sort albums by year");
        PrintTextWithSides("20 | Sort albums by genre");
        PrintTextWithSides("21 | Sort albums by duration");
        // Delete
        PrintTopAndBottomLine(40);
        PrintTextWithSides("22 | Delete artist");
        PrintTextWithSides("23 | Delete album");
        PrintTextWithSides("24 | Delete song");
        // Edit
        PrintTopAndBottomLine(40);
        PrintTextWithSides("25 | Edit artist");
        PrintTextWithSides("26 | Edit album name");
        PrintTextWithSides("27 | Edit album year");
        PrintTextWithSides("28 | Edit song name");
        PrintTextWithSides("29 | Edit song duration");
        PrintTextWithSides("30 | Edit song genre");
        PrintTopAndBottomLine(40);
    }
    
    public static string GetString(string message)
    {
        Console.WriteLine(message);
        var input = Console.ReadLine();
        while (string.IsNullOrWhiteSpace(input))
        {
            Console.WriteLine(message);
            input = Console.ReadLine();
            
        }
        return input;
        
    }
    
    public static int GetInt(string message)
    {
        Console.WriteLine(message);
        var input = Console.ReadLine();
        while (!int.TryParse(input, out _))
        {
            Console.WriteLine(message);
            input = Console.ReadLine();
        }
        return int.Parse(input);
    }
    
    public static string GetDuration(string message, string format)
    {
        string input = GetString(message);
        while (!TimeHandler.IsValidDuration(input, format))
        {
            input = GetString(message);
        }
        return input;
    }
    
    public static void PrintTopAndBottomLine(int length)
    {
        Console.WriteLine("+" + new string('–', length) + "+");
    }
    
    public static void PrintTextWithSides(string text)
    {
        Console.WriteLine("| " + text + new string(' ', 38 - text.Length) + " |");
    }
    
}