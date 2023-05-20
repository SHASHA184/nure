using System.Text.Json;

namespace Course_Work_OOP;

public class MusicBaseSongs: MusicBase
{
    
    
    // print info about song/songs
    public static void PrintSongs(bool withId = false)
    {
        foreach (var song in Songs)
        {
            song.PrintInfo(withId);
        }
    }

    public static void PrintArtistBySong(string songName)
    {
        var song = GetSong("Name", songName);
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
        List<Album> albums = Albums.Where(a => a.ArtistId == artist.Id).ToList();
        List<Song> songs = Songs.Where(s => s.ArtistId == artist.Id).ToList();
        artist.PrintInfo();
    }
    
    
    
    public static void PrintSongsByGenre(string genre)
    {
        var songsByGenre = Songs.Where(s => s.Genre == genre).ToList();
        foreach (var song in songsByGenre)
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
        var sortedSongs = Songs.OrderBy(s => s.GetType().GetProperty(field)?.GetValue(s, null)).ToList();
        foreach (var song in sortedSongs)
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
        var album = MusicBaseAlbums.GetAlbum("Name", albumName);
        if (album == null)
        {
            Console.WriteLine();
            InputHandler.PrintTopAndBottomLine();
            InputHandler.PrintTextWithSides("Album not found");
            InputHandler.PrintTopAndBottomLine();
            Console.WriteLine();
            return;
        }
        var song = GetSong("Name", name);
        if (song != null)
        {
            Console.WriteLine();
            InputHandler.PrintTopAndBottomLine();
            InputHandler.PrintTextWithSides("Song already exists");
            InputHandler.PrintTopAndBottomLine();
            Console.WriteLine();
            return;
        }
        var artist = MusicBaseArtists.GetArtist("Name", artistName);
        if (artist == null)
        {
            Console.WriteLine();
            InputHandler.PrintTopAndBottomLine();
            InputHandler.PrintTextWithSides("Artist not found");
            InputHandler.PrintTopAndBottomLine();
            Console.WriteLine();
            return;
        }
        song = new Song(GetLastId("songs"), name, duration, genre, album.Id, album.ArtistId);
        Songs.Add(song);
        MusicBaseAlbums.UpdateAlbumSongs(album, song, "add");
        MusicBaseArtists.UpdateArtistSongs(artist, song);
    }

    public static void EditSong(int id, string field, string newValue)
    {
        var song = GetSong("Id", id.ToString());
        if (song == null)
        {
            Console.WriteLine();
            InputHandler.PrintTopAndBottomLine();
            InputHandler.PrintTextWithSides("Song not found");
            InputHandler.PrintTopAndBottomLine();
            Console.WriteLine();
            return;
        }

        var artist = MusicBaseArtists.GetArtist("Id", song.ArtistId);
        if (artist == null)
        {
            Console.WriteLine();
            InputHandler.PrintTopAndBottomLine();
            InputHandler.PrintTextWithSides("Artist not found");
            InputHandler.PrintTopAndBottomLine();
            Console.WriteLine();
            return;
        }

        var album = MusicBaseAlbums.GetAlbum("Id", song.AlbumId);
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

    public static void EditSongName(string artistName, string albumName, string songName, string newSongName)
    {
        var artist = MusicBaseArtists.GetArtist("Name", artistName);
        if (artist == null)
        {
            Console.WriteLine();
            InputHandler.PrintTopAndBottomLine();
            InputHandler.PrintTextWithSides("Artist not found");
            InputHandler.PrintTopAndBottomLine();
            Console.WriteLine();
            return;
        }
        var album = MusicBaseAlbums.GetAlbum("Name", albumName);
        if (album == null)
        {
            Console.WriteLine();
            InputHandler.PrintTopAndBottomLine();
            InputHandler.PrintTextWithSides("Album not found");
            InputHandler.PrintTopAndBottomLine();
            Console.WriteLine();
            return;
        }
        var song = GetSong("Name", songName);
        if (song == null)
        {
            Console.WriteLine();
            InputHandler.PrintTopAndBottomLine();
            InputHandler.PrintTextWithSides("Song not found");
            InputHandler.PrintTopAndBottomLine();
            Console.WriteLine();
            return;
        }
        song.Name = newSongName;
        Songs[Songs.FindIndex(s => s.Id == song.Id)] = song;
    }
    
    public static void EditSongGenre(string artistName, string albumName, string songName, string newGenre)
    {
        var artist = MusicBaseArtists.GetArtist("Name", artistName);
        if (artist == null)
        {
            Console.WriteLine();
            InputHandler.PrintTopAndBottomLine();
            InputHandler.PrintTextWithSides("Artist not found");
            InputHandler.PrintTopAndBottomLine();
            Console.WriteLine();
            return;
        }
        var album = MusicBaseAlbums.GetAlbum("Name", albumName);
        if (album == null)
        {
            Console.WriteLine();
            InputHandler.PrintTopAndBottomLine();
            InputHandler.PrintTextWithSides("Album not found");
            InputHandler.PrintTopAndBottomLine();
            Console.WriteLine();
            return;
        }
        var song = GetSong("Name", songName);
        if (song == null)
        {
            Console.WriteLine();
            InputHandler.PrintTopAndBottomLine();
            InputHandler.PrintTextWithSides("Song not found");
            InputHandler.PrintTopAndBottomLine();
            Console.WriteLine();
            return;
        }
        song.Genre = newGenre;
        Songs[Songs.FindIndex(s => s.Id == song.Id)] = song;
    }
    
    public static void EditSongDuration(string artistName, string albumName, string songName, string newDuration)
    {
        var artist = MusicBaseArtists.GetArtist("Name", artistName);
        if (artist == null)
        {
            Console.WriteLine();
            InputHandler.PrintTopAndBottomLine();
            InputHandler.PrintTextWithSides("Artist not found");
            InputHandler.PrintTopAndBottomLine();
            Console.WriteLine();
            return;
        }
        var album = MusicBaseAlbums.GetAlbum("Name", albumName);
        if (album == null)
        {
            Console.WriteLine();
            InputHandler.PrintTopAndBottomLine();
            InputHandler.PrintTextWithSides("Album not found");
            InputHandler.PrintTopAndBottomLine();
            Console.WriteLine();
            return;
        }
        var song = GetSong("Name", songName);
        if (song == null)
        {
            Console.WriteLine();
            InputHandler.PrintTopAndBottomLine();
            InputHandler.PrintTextWithSides("Song not found");
            InputHandler.PrintTopAndBottomLine();
            Console.WriteLine();
            return;
        }
        song.Duration = newDuration;
        Songs[Songs.FindIndex(s => s.Id == song.Id)] = song;
        MusicBaseAlbums.UpdateAlbumSongs(album, song, "edit");
    }
    
    public static void DeleteSong(int id)
    {
        var song = GetSong("Id", id);
        
        if (song == null)
        {
            Console.WriteLine();
            InputHandler.PrintTopAndBottomLine();
            InputHandler.PrintTextWithSides("Song not found");
            InputHandler.PrintTopAndBottomLine();
            Console.WriteLine();
            return;
        }
        
        var album = MusicBaseAlbums.GetAlbum("Id", song.AlbumId);
        
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
        string jsonString = JsonHandler.ReadJson("songs.json");
        if (jsonString == "")
        {
            return null;
        }
        List<Song>? songs = JsonSerializer.Deserialize<List<Song>>(jsonString);
        return songs?.FirstOrDefault(a => a.GetType().GetProperty(field)?.GetValue(a)?.Equals(value) == true);
    }
    
    public static List<Song> GetSongs()
    {
        string jsonString = JsonHandler.ReadJson("songs.json");
        if (jsonString == "")
        {
            return new List<Song>();
        }
        List<Song>? songs = JsonSerializer.Deserialize<List<Song>>(jsonString);
        return songs ?? new List<Song>();
    }

    public static void SaveSongs()
    {
        string jsonString = JsonSerializer.Serialize(Songs);
        JsonHandler.WriteJson("songs.json", jsonString);
    }
}