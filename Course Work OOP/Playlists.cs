namespace Course_Work_OOP;

public class Playlists
{
    public static void Add(string name, string description, List<string> artists, List<string> genres, string duration, int yearFrom, int yearTo)

    {
        List<Song> playlistSongs = new List<Song>();
        string songsDuration = "00:00:00";
        List<Song> songs = GetAvailableSongs(artists, genres, yearFrom, yearTo);
        if (songs.Count == 0)
        {
            Console.WriteLine("There are no songs that match your criteria");
            return;
        }

        foreach (Song song in songs)
        {
            if (TimeHandler.CompareDurations(songsDuration, duration))
            {
                break;
            }
            playlistSongs.Add(song);
            songsDuration = TimeHandler.AddDuration(songsDuration, song.ConvertedDuration());
        }
        
        Playlist playlist = new Playlist(name, description, artists, genres, songsDuration, yearFrom, yearTo, playlistSongs);
        InputHandler.HandlePlaylistSaveMenu(playlist);
    }

    public static void SavePlaylist(Playlist playlist)
    {
        WritePlaylistInTextFile(playlist);
    }
    
    public static void AddSongToPlaylist(Playlist playlist, int songId)
    {
        Song? song = MusicBaseSongs.GetSong("Id", songId);
        if (song == null)
        {
            Console.WriteLine();
            InputHandler.PrintTopAndBottomLine();
            InputHandler.PrintTextWithSides("There is no such song");
            InputHandler.PrintTopAndBottomLine();
            Console.WriteLine();
            return;
        }
        if (playlist.PlaylistSongs.Any(s => s.Id == songId))
        {
            Console.WriteLine();
            InputHandler.PrintTopAndBottomLine();
            InputHandler.PrintTextWithSides("This song is already in the playlist");
            InputHandler.PrintTopAndBottomLine();
            Console.WriteLine();
            return;
        }
        playlist.PlaylistSongs.Add(song);
        playlist.Duration = TimeHandler.AddDuration(playlist.Duration, song.ConvertedDuration());
    }
    
    public static void RemoveSongFromPlaylist(Playlist playlist, int songId)
    {
        Song? song = MusicBaseSongs.GetSong("Id", songId);
        if (song == null)
        {
            Console.WriteLine("There is no such song");
            return;
        }
        int index = playlist.PlaylistSongs.FindIndex(s => s.Id == songId);
        if (index == -1)
        {
            Console.WriteLine("There is no such song in the playlist");
            return;
        }
        playlist.PlaylistSongs.RemoveAt(index);
        playlist.Duration = TimeHandler.SubtractDuration(playlist.Duration, song.ConvertedDuration());
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
        foreach (Song song in new List<Song>(songs))
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
    

    private static List<T> Shuffle<T>(List<T> list)
    {
        Random random = new Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            (list[k], list[n]) = (list[n], list[k]);
        }
        return list;
    }
    
    private static List<Song> GetAvailableSongs(List<string> artists, List<string> genres, int yearFrom, int yearTo)
    {
        List<Song> songs = new List<Song>();
        if (genres.Count == 0 && artists.Count == 0)
        {
            return Shuffle(MusicBaseSongs.GetSongs());
        }
        if (genres.Count != 0)
        {
            songs.AddRange(GetSongsByGenres(genres));
        }
        if (artists.Count != 0)
        {
            songs.AddRange(GetSongsByArtists(artists));
        }
        DeleteOutsideYearRange(songs, yearFrom, yearTo);
        return songs;
    }
    
    private static List<Song> GetSongsByArtists(List<string> artists)
    {
        List<Song> songs = new List<Song>();
        foreach (Song song in Shuffle(MusicBaseSongs.GetSongs()))
        {
            Artist? artist = MusicBaseArtists.GetArtist("Id", song.ArtistId);
            if (artist == null)
            {
                continue;
            }
            if (artists.Any(a => a.Trim().Equals(artist.Name, StringComparison.OrdinalIgnoreCase)))
            {
                songs.Add(song);
            }
        }
        return songs;
    }
    
    private static List<Song> GetSongsByGenres(List<string> genres)
    {
        List<Song> songs = new List<Song>();
        foreach (Song song in Shuffle(MusicBaseSongs.GetSongs()))
        {
            if (genres.Any(g => g.Trim().Equals(song.Genre, StringComparison.OrdinalIgnoreCase)))
            {
                songs.Add(song);
            }
        }
        return songs;
    }
    
}