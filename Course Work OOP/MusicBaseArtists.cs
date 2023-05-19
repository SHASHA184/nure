using System.Text.Json;

namespace Course_Work_OOP;

public class MusicBaseArtists: MusicBase
{
    
    // print info about artist/artists
    public static void PrintArtists()
    {
        foreach (var artist in Artists)
        {
            artist.PrintInfo();
        }
    }
    
    public static void PrintSongsByArtist(string artistName)
    {
        var artist = GetArtist("Name", artistName);
        if (artist == null)
        {
            
            Console.WriteLine("Artist not found");
            return;
        }
        var artistSongs = Songs.Where(s => artist.SongIds.Contains(s.Id)).ToList();
        foreach (var artistSong in artistSongs)
        {
            artistSong.PrintInfo();
        }
    }
    
    public static void PrintAlbumsByArtist(string artistName)
    {
        var artist = GetArtist("Name", artistName);
        if (artist == null)
        {
            Console.WriteLine("Artist not found");
            return;
        }
        var artistAlbums = Albums.Where(a => artist.AlbumIds.Contains(a.Id)).ToList();
        foreach (var artistAlbum in artistAlbums)
        {
            artistAlbum.PrintInfo();
            
        }
    }

    public static void PrintSortedSongsByArtist()
    {
        
        var sortedArtists = Artists.OrderBy(a => a.Name).ToList();
        
        foreach (var artist in sortedArtists)
        {
            List<Song> sortedSongs = Songs.Where(s => artist.SongIds.Contains(s.Id)).OrderBy(s => s.Name).ToList();
            foreach (var song in sortedSongs)
            {
                song.PrintInfo();
            }
        }
    }
    
    
    // all actions with artist
    public static void AddArtist(string name)
    {
        var artist = GetArtist("Name", name);
        if (artist != null)
        {
            Console.WriteLine("Artist already exists");
            return;
        }
        artist = new Artist(GetLastId("artists"), name);
        Artists.Add(artist);
        string jsonString = JsonSerializer.Serialize(Artists);
        JsonHandler.WriteJson("artists.json", jsonString);

    }

    public static Artist? GetArtist<T>(string field, T value)
    {
        
        string jsonString = JsonHandler.ReadJson("artists.json");
        if (jsonString == "")
        {
            return null;
        }
        List<Artist>? artists = JsonSerializer.Deserialize<List<Artist>>(jsonString);
        return artists?.FirstOrDefault(a => a.GetType().GetProperty(field)?.GetValue(a)?.Equals(value) == true);
    }
    
    public static List<Artist> GetArtists()
    {
        string jsonString = JsonHandler.ReadJson("artists.json");
        if (jsonString == "")
        {
            return new List<Artist>();
        }
        List<Artist>? artists = JsonSerializer.Deserialize<List<Artist>>(jsonString);
        return artists ?? new List<Artist>();
    }
    
    public static void EditArtist(string artistName, string newArtistName)
    {
        Artist? artist = GetArtist("Name", artistName);
        if (artist == null)
        {
            Console.WriteLine("Artist not found");
            return;
        }
        artist.Name = newArtistName;
        Artists[Artists.FindIndex(a => a.Id == artist.Id)] = artist;
        string jsonString = JsonSerializer.Serialize(Artists);
        JsonHandler.WriteJson("artists.json", jsonString);
    }

    public static void UpdateArtistAlbums(Artist artist, Album album)
    {

        if (artist.AlbumIds.Contains(album.Id))
        {
            return;
        }
        Artists.Find(a => a.Id == artist.Id)?.AlbumIds.Add(album.Id);
        string jsonString = JsonSerializer.Serialize(Artists);
        JsonHandler.WriteJson("artists.json", jsonString);

    }
    
    public static void UpdateArtistSongs(Artist artist, Song song)
    {
        if (artist.SongIds.Contains(song.Id))
        {
            return;
        }
        Artists.Find(a => a.Id == artist.Id)?.SongIds.Add(song.Id);
        string jsonString = JsonSerializer.Serialize(Artists);
        JsonHandler.WriteJson("artists.json", jsonString);
    }
    
    public static void DeleteArtist(string artistName)
    {
        Artist? artist = GetArtist("Name", artistName);
        if (artist == null)
        {
            Console.WriteLine("Artist not found");
            return;
        }
        foreach (int albumId in artist.AlbumIds)
        {
            Album? album = MusicBaseAlbums.GetAlbum("Id", albumId);
            if (album == null)
            {
                continue;
            }
            MusicBaseAlbums.DeleteAlbum(artistName, album.Name);
        }

        int artistIndex = Artists.FindIndex(a => a.Id == artist.Id);
        if (artistIndex == -1) 
        {
            Console.WriteLine("Artist not found");
            return;
        }
        Artists.RemoveAt(artistIndex);
        string jsonString = JsonSerializer.Serialize(Artists);
        JsonHandler.WriteJson("artists.json", jsonString);


    }


    public static void DeleteArtistAlbum(Artist artist, Album album)
    {
        int albumIndex = Artists.Find(a => a.Id == artist.Id)?.AlbumIds.IndexOf(album.Id) ?? -1;
        if (albumIndex == -1) 
        {
            return;
        }
        Artists.Find(a => a.Id == artist.Id)?.AlbumIds.RemoveAt(albumIndex);
        string jsonString = JsonSerializer.Serialize(Artists);
        JsonHandler.WriteJson("artists.json", jsonString);
    }
}