namespace Course_Work_OOP;

public class Album
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Year { get; set; }
    public List<int> SongIds { get; set; }
    public int ArtistId { get; set; }
    
    public Album(int id, string name, int year, int artistId)
    {
        Id = id;
        Name = name;
        Year = year;
        ArtistId = artistId;
        SongIds = new List<int>();
    }
    
    public void PrintInfo(string artistName)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"\nName: {Name}");
        Console.WriteLine($"Year: {Year}");
        Console.WriteLine($"Artist: {artistName}");
        Console.ResetColor();
    }
    
}