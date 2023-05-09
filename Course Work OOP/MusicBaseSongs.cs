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
            song.PrintInfo(artist.Name, album.Name);
        }
    }
    
    
    public static void PrintSortedSongsByName()
    {
        Songs.Sort((s1, s2) => string.Compare(s1.Name, s2.Name, StringComparison.Ordinal));
        Console.WriteLine("Songs sorted by name");
        foreach (var song in Songs)
        {
            Artist? artist = MusicBaseArtists.GetArtist("Id", song.ArtistId);
            Album? album = MusicBaseAlbums.GetAlbum("Id", song.AlbumId);
            if (artist == null || album == null)
            {
                continue;
            }
            song.PrintInfo(artist.Name, album.Name);
        }
        
    }
    

    
    
    // all actions with song/songs
    public static void AddSong(string name, string artistName, string albumName)
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
        song = new Song(GetLastId("songs"), name, album.Id, album.ArtistId);
        Songs.Add(song);
        string jsonString = JsonSerializer.Serialize(Songs);
        JsonHelper.WriteJson("songs.json", jsonString);
        MusicBaseAlbums.UpdateAlbumSongs(album, song);
        MusicBaseArtists.UpdateArtistSongs(artist, song);
    }
    
    public static void EditSong(string artistName, string albumName, string songName, string newSongName)
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
        JsonHelper.WriteJson("songs.json", jsonString);
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
        JsonHelper.WriteJson("songs.json", jsonString);
        Artists?.Find(a => a.Id == song.ArtistId)?.SongIds.Remove(song.Id);
        jsonString = JsonSerializer.Serialize(Artists);
        JsonHelper.WriteJson("artists.json", jsonString);
        Albums?.Find(a => a.Id == song.AlbumId)?.SongIds.Remove(song.Id);
        jsonString = JsonSerializer.Serialize(Albums);
        JsonHelper.WriteJson("albums.json", jsonString);
    }

    public static Song? GetSong<T>(string field, T value)
    {
        string jsonString = JsonHelper.ReadJson("songs.json");
        if (jsonString == "")
        {
            return null;
        }
        List<Song>? songs = JsonSerializer.Deserialize<List<Song>>(jsonString);
        return songs?.FirstOrDefault(a => a.GetType().GetProperty(field)?.GetValue(a)?.Equals(value) == true);
    }
    
    public static List<Song> GetSongs()
    {
        string jsonString = JsonHelper.ReadJson("songs.json");
        if (jsonString == "")
        {
            return new List<Song>();
        }
        List<Song>? songs = JsonSerializer.Deserialize<List<Song>>(jsonString);
        return songs ?? new List<Song>();
    }
    

}