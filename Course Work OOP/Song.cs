

namespace Course_Work_OOP;

public class Song
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int AlbumId { get; set; }
    public int ArtistId { get; set; }
    
    public Song(int id, string name, int albumId, int artistId)
    {
        Id = id;
        Name = name;
        AlbumId = albumId;
        ArtistId = artistId;
    }
    
    public void PrintInfo(string artistName, string albumName)
    {
        Console.WriteLine($"\nName: {Name}");
        Console.WriteLine($"Artist: {artistName}");
        Console.WriteLine($"Album: {albumName}");
    }
    

}