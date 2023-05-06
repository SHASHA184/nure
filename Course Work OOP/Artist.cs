

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
    
    public void PrintInfo(List<Album> albums, List<Song> songs)
    {
        Console.WriteLine($"\nName: {Name}");
        Console.WriteLine("Albums:");
        foreach (var albumId in AlbumIds)
        {
            var album = albums.Find(a => a.Id == albumId);
            album?.PrintInfo(Name);
        }
        Console.WriteLine("Songs:");
        foreach (var songId in SongIds)
        {
            var song = songs.Find(s => s.Id == songId);
            if (song == null)
            {
                continue;
            }
            string? albumName = albums.Find(a => a.Id == song.AlbumId)?.Name;
            if (albumName != null)
                song.PrintInfo(Name, albumName);
        }
    }


}