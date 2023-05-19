namespace Course_Work_OOP;

public class Album
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Year { get; set; }
    public string Genre { get; set; }
    public string Duration { get; set; }
    public List<int> SongIds { get; set; }
    public int ArtistId { get; set; }
    
    public Album(int id, string name, int year, string genre, int artistId)
    {
        Id = id;
        Name = name;
        Year = year;
        Genre = genre;
        Duration = "00:00:00";
        ArtistId = artistId;
        SongIds = new List<int>();
    }
    
    public void PrintInfo()
    {
        Artist? artist = MusicBaseArtists.GetArtist("Id", ArtistId);
        if (artist == null)
        {
            return;
        }
        Console.ForegroundColor = ConsoleColor.Green;
        InputHandler.PrintTopAndBottomLine(40);
        InputHandler.PrintTextWithSides($"Name: {Name}");
        InputHandler.PrintTextWithSides($"Year: {Year}");
        InputHandler.PrintTextWithSides($"Artist: {artist.Name}");
        InputHandler.PrintTextWithSides($"Genre: {Genre}");
        InputHandler.PrintTextWithSides($"Duration: {Duration}");
        InputHandler.PrintTopAndBottomLine(40);
        Console.WriteLine();
        Console.ResetColor();
    }
    
}