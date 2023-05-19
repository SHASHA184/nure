

namespace Course_Work_OOP;

public class Song
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Duration { get; set; }
    public string Genre { get; set; }
    public int AlbumId { get; set; }
    public int ArtistId { get; set; }
    
    public Song(int id, string name, string duration, string genre, int albumId, int artistId)
    {
        Id = id;
        Name = name;
        Duration = duration;
        Genre = genre;
        AlbumId = albumId;
        ArtistId = artistId;
    }
    
    public void PrintInfo()
    {
        Artist? artist = MusicBaseArtists.GetArtist("Id", ArtistId);
        Album? album = MusicBaseAlbums.GetAlbum("Id", AlbumId);
        if (artist == null || album == null)
        {
            return;
        }
        Console.ForegroundColor = ConsoleColor.Yellow;
        InputHandler.PrintTopAndBottomLine(40);
        InputHandler.PrintTextWithSides($"Name: {Name}");
        InputHandler.PrintTextWithSides($"Artist: {artist.Name}");
        InputHandler.PrintTextWithSides($"Album: {album.Name}");
        InputHandler.PrintTextWithSides($"Genre: {Genre}");
        InputHandler.PrintTextWithSides($"Duration: {Duration}");
        InputHandler.PrintTopAndBottomLine(40);
        Console.WriteLine();
        Console.ResetColor();
    }
    
    public string ConvertedDuration()
    {
        string[] duration = Duration.Split(":");
        int minutes = int.Parse(duration[0]);
        int seconds = int.Parse(duration[1]);
        return "00:" + minutes + ":" + seconds;
    }


}