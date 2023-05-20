namespace Course_Work_OOP;

public class Playlists
{
    public static void Add(string name, string description, string genre, string duration, int yearFrom, int yearTo)

    {
        List<Song> playlistSongs = new List<Song>();
        string songsDuration = "00:00:00";
        List<Song> songs = genre == "All" ? MusicBaseSongs.GetSongs() : MusicBaseSongs.GetSongs().Where(s => s.Genre == genre).ToList();
        DeleteOutsideYearRange(songs, yearFrom, yearTo);
        Shuffle(songs);
        foreach (var song in songs.TakeWhile(song => !TimeHandler.CompareDurations(TimeHandler.AddDuration(songsDuration, song.ConvertedDuration()), duration)))
        {
            playlistSongs.Add(song);
            songsDuration = TimeHandler.AddDuration(songsDuration, song.ConvertedDuration());
        }
        Playlist playlist = new Playlist(name, description, genre, songsDuration, yearFrom, yearTo, playlistSongs);
        WritePlaylistInTextFile(playlist);
    }

    private static void WritePlaylistInTextFile(Playlist playlist)
    {
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
        string playlistName = $"{playlist.Name} {timestamp}.txt";
        playlist.WriteToFile(playlistName);
        Console.WriteLine("Playlist created successfully");
        Console.WriteLine($"You can find your playlist in {playlistName}");
    }
    
    private static void DeleteOutsideYearRange(List<Song> songs, int yearFrom, int yearTo)
    {
        foreach (Song song in songs)
        {
            Album? album = MusicBaseAlbums.GetAlbum("Id", song.AlbumId);
            if (album == null)
            {
                continue;
            }
            if (album.Year < yearFrom || album.Year > yearTo)
            {
                songs.Remove(song);
            }
        }
    }

    private static void Shuffle<T>(List<T> list)
    {
        Random random = new Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            (list[k], list[n]) = (list[n], list[k]);
        }
    }
}