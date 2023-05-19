

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
    
    public void PrintInfo()
    {
        List<Album> albums = MusicBaseAlbums.GetAlbums();
        List<Song> songs = MusicBaseSongs.GetSongs();
        Console.ForegroundColor = ConsoleColor.Blue;
        InputHandler.PrintTopAndBottomLine(40);
        InputHandler.PrintTextWithSides(Name);
        InputHandler.PrintTopAndBottomLine(40);
        Console.ResetColor();
        Console.WriteLine("\nAlbums:");
        foreach (var albumId in AlbumIds)
        {
            var album = albums.Find(a => a.Id == albumId);
            album?.PrintInfo();
        }
        Console.WriteLine("Songs:");
        foreach (var songId in SongIds)
        {
            var song = songs.Find(s => s.Id == songId);
            song?.PrintInfo();
        }
    }


}