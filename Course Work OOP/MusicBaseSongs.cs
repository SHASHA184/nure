using System.Text.Json;

namespace Course_Work_OOP;

public class MusicBaseSongs: MusicBase
{
    
    
    // print info about song/songs
    public static void PrintSongs(bool withId = false)
    {
        foreach (Song song in Songs)
        {
            song.PrintInfo(withId);
        }
    }

    public static void PrintArtistBySong(string name)
    {
        Song? song = GetSong("Name", name);
        if (song == null)
        {
            Console.WriteLine();
            InputHandler.PrintTopAndBottomLine();
            InputHandler.PrintTextWithSides("Song not found");
            InputHandler.PrintTopAndBottomLine();
            Console.WriteLine();
            return;
        }
        Artist? artist = MusicBaseArtists.GetArtist("Id", song.ArtistId);
        if (artist == null)
        {
            return;
        }
        artist.PrintInfo();
    }
    
    public static void PrintAlbumBySong(string name)
    {
        Song? song = GetSong("Name", name);
        if (song == null)
        {
            Console.WriteLine();
            InputHandler.PrintTopAndBottomLine();
            InputHandler.PrintTextWithSides("Song not found");
            InputHandler.PrintTopAndBottomLine();
            Console.WriteLine();
            return;
        }
        Album? album = MusicBaseAlbums.GetAlbum("Id", song.AlbumId);
        if (album == null)
        {
            return;
        }
        album.PrintInfo();
    }
    
    
    
    public static void PrintSongsByGenre(string genre)
    {
        List<Song> songsByGenre = Songs.Where(s => s.Genre == genre).ToList();
        foreach (Song song in songsByGenre)
        {
            Artist? artist = MusicBaseArtists.GetArtist("Id", song.ArtistId);
            Album? album = MusicBaseAlbums.GetAlbum("Id", song.AlbumId);
            if (artist == null || album == null)
            {
                continue;
            }
            song.PrintInfo();
        }
    }
    
    
    public static void PrintSortedSongsBy(string field)
    {
        List<Song> sortedSongs = Songs.OrderBy(s => s.GetType().GetProperty(field)?.GetValue(s, null)).ToList();
        foreach (Song song in sortedSongs)
        {
            Artist? artist = MusicBaseArtists.GetArtist("Id", song.ArtistId);
            Album? album = MusicBaseAlbums.GetAlbum("Id", song.AlbumId);
            if (artist == null || album == null)
            {
                continue;
            }
            song.PrintInfo();
        }
    }

    // all actions with song/songs
    public static void AddSong(string name, string albumName, string artistName, string genre, string duration)
    {
        Album? album = MusicBaseAlbums.GetAlbum("Name", albumName);
        if (album == null)
        {
            Console.WriteLine();
            InputHandler.PrintTopAndBottomLine();
            InputHandler.PrintTextWithSides("Album not found");
            InputHandler.PrintTopAndBottomLine();
            Console.WriteLine();
            return;
        }
        Song? checkSong = Songs.FirstOrDefault(s => s.Name == name && s.AlbumId == album.Id);
        if (checkSong != null)
        {
            Console.WriteLine();
            InputHandler.PrintTopAndBottomLine();
            InputHandler.PrintTextWithSides("Song already exists");
            InputHandler.PrintTopAndBottomLine();
            Console.WriteLine();
            return;
        }

        Artist? artist = MusicBaseArtists.GetArtist("Name", artistName);
        if (artist == null)
        {
            Console.WriteLine();
            InputHandler.PrintTopAndBottomLine();
            InputHandler.PrintTextWithSides("Artist not found");
            InputHandler.PrintTopAndBottomLine();
            Console.WriteLine();
            return;
        }
        Song song = new Song(GetLastId("songs"), name, duration, genre, album.Id, album.ArtistId);
        Songs.Add(song);
        MusicBaseAlbums.UpdateAlbumSongs(album, song, "add");
        MusicBaseArtists.UpdateArtistSongs(artist, song);
    }

    public static void EditSong(int id, string field, string newValue)
    {
        Song? song = GetSong("Id", id);
        if (song == null)
        {
            Console.WriteLine();
            InputHandler.PrintTopAndBottomLine();
            InputHandler.PrintTextWithSides("Song not found");
            InputHandler.PrintTopAndBottomLine();
            Console.WriteLine();
            return;
        }

        Artist? artist = MusicBaseArtists.GetArtist("Id", song.ArtistId);
        if (artist == null)
        {
            Console.WriteLine();
            InputHandler.PrintTopAndBottomLine();
            InputHandler.PrintTextWithSides("Artist not found");
            InputHandler.PrintTopAndBottomLine();
            Console.WriteLine();
            return;
        }

        Album? album = MusicBaseAlbums.GetAlbum("Id", song.AlbumId);
        if (album == null)
        {
            Console.WriteLine();
            InputHandler.PrintTopAndBottomLine();
            InputHandler.PrintTextWithSides("Album not found");
            InputHandler.PrintTopAndBottomLine();
            Console.WriteLine();
            return;
        }

        song.GetType().GetProperty(field)?.SetValue(song, newValue);
        Songs[Songs.FindIndex(s => s.Id == song.Id)] = song;
    }
    public static void DeleteSong(int id)
    {
        Song? song = GetSong("Id", id);
        
        if (song == null)
        {
            Console.WriteLine();
            InputHandler.PrintTopAndBottomLine();
            InputHandler.PrintTextWithSides("Song not found");
            InputHandler.PrintTopAndBottomLine();
            Console.WriteLine();
            return;
        }
        
        Album? album = MusicBaseAlbums.GetAlbum("Id", song.AlbumId);
        
        if (album == null)
        {
            Console.WriteLine();
            InputHandler.PrintTopAndBottomLine();
            InputHandler.PrintTextWithSides("Album not found");
            InputHandler.PrintTopAndBottomLine();
            Console.WriteLine();
            return;
        }
        
        int songIndex = Songs.FindIndex(s => s.Id == id);
        if (songIndex == -1) 
        {
            return;
        }
        
        Songs.RemoveAt(songIndex);
        
        MusicBaseAlbums.UpdateAlbumSongs(album, song, "delete");
        
        Artists?.Find(a => a.Id == song.ArtistId)?.SongIds.Remove(song.Id);
    }

    public static Song? GetSong<T>(string field, T value)
    {
        Song? song = Songs?.FirstOrDefault(a => a.GetType().GetProperty(field)?.GetValue(a)?.Equals(value) == true);
        return song;
    }
    
    public static List<Song> GetSongsFromJson()
    {
        string jsonString = FileHandler.ReadFile("songs.json");
        if (jsonString == "")
        {
            return new List<Song>();
        }
        List<Song>? songs = JsonSerializer.Deserialize<List<Song>>(jsonString);
        return songs ?? new List<Song>();
    }
    
    public static List<Song> GetSongs()
    {
        return Songs;
    }

    public static void SaveSongs()
    {
        string jsonString = JsonSerializer.Serialize(Songs);
        FileHandler.WriteFile("songs.json", jsonString);
    }
}