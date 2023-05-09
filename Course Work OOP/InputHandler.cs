namespace Course_Work_OOP;

public abstract class InputHandler
{

    public static void PrintOptions()
    {
        Console.WriteLine("\nChoose an option:");
        Console.WriteLine("0 - Exit");
        Console.WriteLine("1 - Add new artist");
        Console.WriteLine("2 - Add new album");
        Console.WriteLine("3 - Add new song");
        Console.WriteLine("4 - Print all artists");
        Console.WriteLine("5 - Print all albums");
        Console.WriteLine("6 - Print all songs");
        Console.WriteLine("7 - Print all songs by artist");
        Console.WriteLine("8 - Print all songs by album");
        Console.WriteLine("9 - Print all albums by artist");
        Console.WriteLine("10 - Print all albums by year");
        Console.WriteLine("11 - Sort songs by name");
        Console.WriteLine("12 - Sort songs by artist");
        Console.WriteLine("13 - Sort songs by album");
        Console.WriteLine("14 - Sort albums by year");
        Console.WriteLine("15 - Delete artist");
        Console.WriteLine("16 - Delete album");
        Console.WriteLine("17 - Delete song");
        Console.WriteLine("18 - Edit artist");
        Console.WriteLine("19 - Edit album");
        Console.WriteLine("20 - Edit song");
        
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
}