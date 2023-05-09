using System.Text.Json;

namespace Course_Work_OOP;

public abstract class MusicBaseAlbums: MusicBase
{
    

    // print info about album/albums
    public static void PrintAlbums()
    {
        foreach (var album in Albums)
        {
            Artist? artist = MusicBaseArtists.GetArtist("Id", album.ArtistId);
            if (artist == null)
            {
                continue;
            }
            album.PrintInfo(artist.Name);
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
            albumSong.PrintInfo(artist.Name, albumName);
        }
    }

    public static void PrintSortedAlbumsByYear()
    {
        var sortedAlbums = Albums.OrderBy(a => a.Year).ToList();
        foreach (var album in sortedAlbums)
        {
            Artist? artist = MusicBaseArtists.GetArtist("Id", album.ArtistId);
            if (artist == null)
            {
                continue;
            }
            album.PrintInfo(artist.Name);
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
            song.PrintInfo(artist.Name, album.Name);
        }
    }

    // all actions with album
    public static void AddAlbum(string name, int year, string artistName)
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
        album = new Album(GetLastId("albums"), name, year, artist.Id);
        Albums.Add(album);
        string jsonString = JsonSerializer.Serialize(Albums);
        JsonHelper.WriteJson("albums.json", jsonString);
        MusicBaseArtists.UpdateArtistAlbums(artist, album);
    }

    public static Album? GetAlbum<T>(string field, T value)
    {
        string jsonString = JsonHelper.ReadJson("albums.json");
        if (jsonString == "")
        {
            return null;
        }
        List<Album>? albums = JsonSerializer.Deserialize<List<Album>>(jsonString);
        return albums?.FirstOrDefault(a => a.GetType().GetProperty(field)?.GetValue(a)?.Equals(value) == true);
    }
    
    public static List<Album> GetAlbums()
    {
        string jsonString = JsonHelper.ReadJson("albums.json");
        if (jsonString == "")
        {
            return new List<Album>();
        }
        List<Album>? albums = JsonSerializer.Deserialize<List<Album>>(jsonString);
        return albums ?? new List<Album>();
    }
    
    public static void EditAlbum(string artistName, string albumName, string newAlbumName, int newYear)
    {
        var artist = MusicBaseArtists.GetArtist("Name", artistName);
        if (artist == null)
        {
            Console.WriteLine("Artist not found");
            return;
        }
        var album = GetAlbum("Name", albumName);
        if (album == null)
        {
            Console.WriteLine("Album not found");
            return;
        }
        album.Name = newAlbumName;
        album.Year = newYear;
        Albums[Albums.FindIndex(a => a.Id == album.Id)] = album;
        string jsonString = JsonSerializer.Serialize(Albums);
        JsonHelper.WriteJson("albums.json", jsonString);
    }
    
    public static void UpdateAlbumSongs(Album album, Song song)
    {
        if (album.SongIds.Contains(song.Id))
        {
            return;
        }
        Albums?.Find(a => a.Id == album.Id)?.SongIds.Add(song.Id);
        string jsonString = JsonSerializer.Serialize(Albums);
        JsonHelper.WriteJson("albums.json", jsonString);
    }
    
    public static void DeleteAlbum(string artistName, string albumName)
    {
        var artist = MusicBaseArtists.GetArtist("Name", artistName);
        if (artist == null)
        {
            Console.WriteLine("Artist not found");
            return;
        }
        var album = GetAlbum("Name", albumName);
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
            MusicBaseSongs.DeleteSong(artistName, albumName, song.Name);
        }
        MusicBaseArtists.DeleteArtistAlbum(artist, album);
        
        int albumIndex = Albums.FindIndex(a => a.Id == album.Id);
        if (albumIndex == -1) 
        {
            Console.WriteLine("Album not found");
            return;
        }
        Albums.RemoveAt(albumIndex);
        string jsonString = JsonSerializer.Serialize(Albums);
        JsonHelper.WriteJson("albums.json", jsonString);
    }
    
}