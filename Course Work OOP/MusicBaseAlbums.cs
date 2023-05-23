using System.Text.Json;

namespace Course_Work_OOP;

public class MusicBaseAlbums: MusicBase
{
    

    // print info about album/albums
    public static void PrintAlbums(bool withId = false)
    {
        foreach (var album in Albums)
        {
            album.PrintInfo(withId);
        }
    }
    
    public static void PrintAlbumsByYear(int year)
    {
        var albumsByYear = Albums.Where(a => a.Year == year).ToList();
        foreach (var album in albumsByYear)
        {
            Artist? artist = MusicBaseArtists.GetArtist("Id", album.ArtistId);
            Console.WriteLine($"Name: {album.Name}, Artist: {artist?.Name}");
        }
    }
    
    public static void PrintSongsByAlbum(string albumName)
    {
        var album = GetAlbum("Name", albumName);
        if (album == null)
        {
            Console.WriteLine("Album not found");
            return;
        }
        var albumSongs = Songs.Where(s => album.SongIds.Contains(s.Id)).ToList();
        foreach (Song albumSong in albumSongs)
        {
            Artist? artist = MusicBaseArtists.GetArtist("Id", albumSong.ArtistId);
            if (artist == null)
            {
                continue;
            }
            albumSong.PrintInfo();
        }
    }
    
    public static void PrintAlbumsByGenre(string genre)
    {
        var albumsByGenre = Albums.Where(a => a.Genre == genre).ToList();
        foreach (var album in albumsByGenre)
        {
            Artist? artist = MusicBaseArtists.GetArtist("Id", album.ArtistId);
            if (artist == null)
            {
                continue;
            }
            album.PrintInfo();
        }
    }

    
    
    public static void PrintSortedSongsByAlbum()
    {
        var sortedSongs = Songs.OrderBy(s => GetAlbum("Id", s.AlbumId)?.Year).ToList();
        foreach (var song in sortedSongs)
        {
            Artist? artist = MusicBaseArtists.GetArtist("Id", song.ArtistId);
            if (artist == null)
            {
                continue;
            }
            Album? album = GetAlbum("Id", song.AlbumId);
            if (album == null)
            {
                continue;
            }
            song.PrintInfo();
        }
    }
    

    public static void PrintSortedAlbumsBy(string field)
    {
        var sortedAlbums = Albums.OrderBy(a => a.GetType().GetProperty(field)?.GetValue(a)).ToList();
        foreach (var album in sortedAlbums)
        {
            Artist? artist = MusicBaseArtists.GetArtist("Id", album.ArtistId);
            if (artist == null)
            {
                continue;
            }
            album.PrintInfo();
        }
    }

    // all actions with album
    public static void AddAlbum(string name, int year, string genre, string artistName)
    {
        var artist = MusicBaseArtists.GetArtist("Name", artistName);
        if (artist == null)
        {
            Console.WriteLine("Artist not found");
            return;
        }
        var album = GetAlbum("Name", name);
        if (album != null)
        {
            Console.WriteLine("Album already exists");
            return;
        }
        album = new Album(GetLastId("albums"), name, year, genre, artist.Id);
        Albums.Add(album);
        MusicBaseArtists.UpdateArtistAlbums(artist, album);
    }

    public static Album? GetAlbum<T>(string field, T value)
    {
        Album? album = Albums.FirstOrDefault(a => a.GetType().GetProperty(field)?.GetValue(a)?.Equals(value) == true);
        return album;
    }
    
    public static List<Album> GetAlbums()
    {
        string jsonString = FileHandler.ReadFile("albums.json");
        if (jsonString == "")
        {
            return new List<Album>();
        }
        List<Album>? albums = JsonSerializer.Deserialize<List<Album>>(jsonString);
        return albums ?? new List<Album>();
    }
    
    public static void EditAlbum<T>(int id, string field, T newValue)
    {
        var album = GetAlbum("Id", id);
        if (album == null)
        {
            Console.WriteLine("Album not found");
            return;
        }
        album.GetType().GetProperty(field)?.SetValue(album, newValue);
    }

    
    public static void UpdateAlbumSongs(Album album, Song song, string action)
    {
        string albumDuration = album.Duration;
        string newDuration;
        if (action == "add")
        {
            if (album.SongIds.Contains(song.Id))
            {
                return;
            }
            Albums?.Find(a => a.Id == album.Id)?.SongIds.Add(song.Id);
            newDuration = TimeHandler.AddDuration(albumDuration, song.ConvertedDuration());
        }
        
        else if (action == "delete")
        {
            Albums?.Find(a => a.Id == album.Id)?.SongIds.Remove(song.Id);
            newDuration = TimeHandler.SubtractDuration(albumDuration, song.ConvertedDuration());
        }
        else
        {
            List<string> allDurations = Songs.Where(s => album.SongIds.Contains(s.Id)).Select(s => s.ConvertedDuration()).ToList();
            newDuration = TimeHandler.CalculateDuration(allDurations);
        }

        if (Albums != null)
        {
            Albums[Albums.FindIndex(a => a.Id == album.Id)].Duration = newDuration;
        }
    }
    
    public static void DeleteAlbum(int id)
    {
        var artist = MusicBaseArtists.GetArtist("Id", id);
        if (artist == null)
        {
            Console.WriteLine("Artist not found");
            return;
        }
        var album = GetAlbum("Id", id);
        if (album == null)
        {
            Console.WriteLine("Album not found");
            return;
        }

        foreach (int songId in album.SongIds)
        {
            Song? song = MusicBaseSongs.GetSong("Id", songId);
            if (song == null)
            {
                continue;
            }
            MusicBaseSongs.DeleteSong(song.Id);
        }
        MusicBaseArtists.DeleteArtistAlbum(artist, album);
        
        int albumIndex = Albums.FindIndex(a => a.Id == album.Id);
        if (albumIndex == -1) 
        {
            Console.WriteLine("Album not found");
            return;
        }
        Albums.RemoveAt(albumIndex);
    }
    
    public static void SaveAlbums()
    {
        string jsonString = JsonSerializer.Serialize(Albums);
        FileHandler.WriteFile("albums.json", jsonString);
    }
}