namespace Course_Work_OOP;

public class Playlist
{
    public string Name { get; set; }
    private string Description { get; set; }
    private List<string> Artists { get; set; }
    private List<string> Genres { get; set; }
    public string Duration { get; set; }
    private int YearFrom { get; set; }
    private int YearTo { get; set; }
    public List<Song> PlaylistSongs { get; set; }

    public Playlist(string name, string description, List<string> artists, List<string> genres, string duration, int yearFrom, int yearTo,
        List<Song> playlistSongs)
    {
        Name = name;
        Description = description;
        Artists = artists;
        Genres = genres;
        Duration = duration;
        YearFrom = yearFrom;
        YearTo = yearTo;
        PlaylistSongs = playlistSongs;
    }

    public void WriteToFile(string filePath)
    {
        string playlistInfo = $"Name: {Name}\n" +
                              $"Description: {Description}\n" +
                              $"Artists: {string.Join(", ", Artists)}\n" +
                              $"Genres: {string.Join(", ", Genres)}\n" +
                              $"Duration: {Duration}\n" +
                              $"From year: {YearFrom}\n" +
                              $"To year: {YearTo}\n\n" +
                              $"Songs:\n";

        foreach (Song song in PlaylistSongs)
        {
            Artist? artist = MusicBaseArtists.GetArtist("Id", song.ArtistId);
            if (artist != null)
            {
                playlistInfo += $"Name: {song.Name}\n" +
                                $"Artist: {artist.Name}\n" +
                                $"Genre: {song.Genre}\n" +
                                $"Duration: {song.Duration}\n\n";
            }
        }
        FileHandler.WriteFile(filePath, playlistInfo);
    }

    public void PrintInfo()
    {
        InputHandler.PrintTopAndBottomLine();
        InputHandler.PrintTextWithSides($"Name: {Name}");
        InputHandler.PrintTextWithSides($"Description: {Description}");
        InputHandler.PrintTextWithSides($"Genres: {string.Join(", ", Genres)}");
        InputHandler.PrintTextWithSides($"Duration: {Duration}");
        InputHandler.PrintTextWithSides($"From year: {YearFrom}");
        InputHandler.PrintTextWithSides($"To year: {YearTo}");
        InputHandler.PrintTextWithSides("Songs:");
        InputHandler.PrintTopAndBottomLine();

        foreach (Song song in PlaylistSongs)
        {
            song.PrintInfo();
        }
    }
}