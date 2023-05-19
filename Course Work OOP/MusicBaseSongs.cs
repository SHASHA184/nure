using System.Text.Json;

namespace Course_Work_OOP;

public class MusicBaseSongs: MusicBase
{
    
    
    // print info about song/songs
    public static void PrintSongs()
    {
        foreach (var song in Songs)
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

    public static void PrintArtistBySong(string songName)
    {
        var song = GetSong("Name", songName);
        if (song == null)
        {
            Console.WriteLine("Song not found");
            return;
        }
        Artist? artist = MusicBaseArtists.GetArtist("Id", song.ArtistId);
        if (artist == null)
        {
            Console.WriteLine("Artist not found");
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
            Console.WriteLine("Album not found");
            return;
        }
        var song = GetSong("Name", name);
        if (song != null)
        {
            Console.WriteLine("Song already exists");
            return;
        }
        var artist = MusicBaseArtists.GetArtist("Name", artistName);
        if (artist == null)
        {
            Console.WriteLine("Artist not found");
            return;
        }
        song = new Song(GetLastId("songs"), name, duration, genre, album.Id, album.ArtistId);
        Songs.Add(song);
        string jsonString = JsonSerializer.Serialize(Songs);
        JsonHandler.WriteJson("songs.json", jsonString);
        MusicBaseAlbums.UpdateAlbumSongs(album, song, "add");
        MusicBaseArtists.UpdateArtistSongs(artist, song);
    }
    
    public static void EditSongName(string artistName, string albumName, string songName, string newSongName)
    {
        var artist = MusicBaseArtists.GetArtist("Name", artistName);
        if (artist == null)
        {
            Console.WriteLine("Artist not found");
            return;
        }
        var album = MusicBaseAlbums.GetAlbum("Name", albumName);
        if (album == null)
        {
            Console.WriteLine("Album not found");
            return;
        }
        var song = GetSong("Name", songName);
        if (song == null)
        {
            Console.WriteLine("Song not found");
            return;
        }
        song.Name = newSongName;
        Songs[Songs.FindIndex(s => s.Id == song.Id)] = song;
        string jsonString = JsonSerializer.Serialize(Songs);
        JsonHandler.WriteJson("songs.json", jsonString);
    }
    
    public static void EditSongGenre(string artistName, string albumName, string songName, string newGenre)
    {
        var artist = MusicBaseArtists.GetArtist("Name", artistName);
        if (artist == null)
        {
            Console.WriteLine("Artist not found");
            return;
        }
        var album = MusicBaseAlbums.GetAlbum("Name", albumName);
        if (album == null)
        {
            Console.WriteLine("Album not found");
            return;
        }
        var song = GetSong("Name", songName);
        if (song == null)
        {
            Console.WriteLine("Song not found");
            return;
        }
        song.Genre = newGenre;
        Songs[Songs.FindIndex(s => s.Id == song.Id)] = song;
        string jsonString = JsonSerializer.Serialize(Songs);
        JsonHandler.WriteJson("songs.json", jsonString);
    }
    
    public static void EditSongDuration(string artistName, string albumName, string songName, string newDuration)
    {
        var artist = MusicBaseArtists.GetArtist("Name", artistName);
        if (artist == null)
        {
            Console.WriteLine("Artist not found");
            return;
        }
        var album = MusicBaseAlbums.GetAlbum("Name", albumName);
        if (album == null)
        {
            Console.WriteLine("Album not found");
            return;
        }
        var song = GetSong("Name", songName);
        if (song == null)
        {
            Console.WriteLine("Song not found");
            return;
        }
        string difference = (Convert.ToDateTime(newDuration) - Convert.ToDateTime(song.Duration)).ToString();
        
        song.Duration = newDuration;
        Songs[Songs.FindIndex(s => s.Id == song.Id)] = song;
        MusicBaseAlbums.UpdateAlbumSongs(album, song, "edit");
        string jsonString = JsonSerializer.Serialize(Songs);
        JsonHandler.WriteJson("songs.json", jsonString);
    }
    
    public static void DeleteSong(string artistName, string albumName, string songName)
    {
        var artist = MusicBaseArtists.GetArtist("Name", artistName);
        if (artist == null)
        {
            Console.WriteLine("Artist not found");
            return;
        }
        var album = MusicBaseAlbums.GetAlbum("Name", albumName);
        if (album == null)
        {
            Console.WriteLine("Album not found");
            return;
        }
        var song = GetSong("Name", songName);
        if (song == null)
        {
            Console.WriteLine("Song not found");
            return;
        }
        int songIndex = Songs.FindIndex(s => s.Id == song.Id);
        if (songIndex == -1) 
        {
            return;
        }
        Songs.RemoveAt(songIndex);
        string jsonString = JsonSerializer.Serialize(Songs);
        JsonHandler.WriteJson("songs.json", jsonString);
        
        MusicBaseAlbums.UpdateAlbumSongs(album, song, "delete");
        
        Artists?.Find(a => a.Id == song.ArtistId)?.SongIds.Remove(song.Id);
        jsonString = JsonSerializer.Serialize(Artists);
        JsonHandler.WriteJson("artists.json", jsonString);
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
}