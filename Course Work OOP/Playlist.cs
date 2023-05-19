namespace Course_Work_OOP;

public class Playlist
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Genre { get; set; }
    public string Duration { get; set; }
    public int YearFrom { get; set; }
    public int YearTo { get; set; }
    public List<Song> PlaylistSongs { get; set; }

    public Playlist(string name, string description, string genre, string duration, int yearFrom, int yearTo, List<Song> playlistSongs)
    {
        Name = name;
        Description = description;
        Genre = genre;
        Duration = duration;
        YearFrom = yearFrom;
        YearTo = yearTo;
        PlaylistSongs = playlistSongs;
    }
    
    public void WriteToFile(string filePath)
    {
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            writer.WriteLine($"Name: {Name}");
            writer.WriteLine($"Description: {Description}");
            writer.WriteLine($"Genre: {Genre}");
            writer.WriteLine($"Duration: {Duration}");
            writer.WriteLine($"From year: {YearFrom}");
            writer.WriteLine($"To year: {YearTo}");
            writer.WriteLine("Songs:");
            
            foreach (var song in PlaylistSongs)
            {
                Artist? artist = MusicBaseArtists.GetArtist("Id", song.ArtistId);
                Album? album = MusicBaseAlbums.GetAlbum("Id", song.AlbumId);
                if (artist == null || album == null)
                {
                    continue;
                }
                writer.WriteLine();
                writer.WriteLine($"Name: {song.Name}");
                writer.WriteLine($"Artist: {artist.Name}");
                writer.WriteLine($"Album: {album.Name}");
                writer.WriteLine($"Genre: {song.Genre}");
                writer.WriteLine($"Duration: {song.Duration}");
            }
        }
    }
}