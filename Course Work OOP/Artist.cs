

namespace Course_Work_OOP;

public class Artist
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<int> AlbumIds { get; set; }
    public List<int> SongIds { get; set; }
    
    public Artist(int id, string name)
    {
        Id = id;
        Name = name;
        AlbumIds = new List<int>();
        SongIds = new List<int>();
    }
    
    public void PrintInfo(bool withId = false)
    {
        List<Album> albums = MusicBaseAlbums.GetAlbums();
        List<Song> songs = MusicBaseSongs.GetSongs();
        Console.ForegroundColor = ConsoleColor.Blue;
        InputHandler.PrintTopAndBottomLine();
        if (withId)
        {
            InputHandler.PrintTextWithSides($"Id: {Id}");
        }
        InputHandler.PrintTextWithSides(Name);
        InputHandler.PrintTopAndBottomLine();
        Console.ResetColor();
        Console.WriteLine("\nAlbums:");
        foreach (int albumId in AlbumIds)
        {
            Album? album = albums.Find(a => a.Id == albumId);
            album?.PrintInfo();
        }
    }


}