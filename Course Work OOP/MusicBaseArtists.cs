using System.Text.Json;

namespace Course_Work_OOP;

public class MusicBaseArtists: MusicBase
{
    
    // print info about artist/artists
    public static void PrintArtists(bool withId = false)
    {
        foreach (var artist in Artists)
        {
            artist.PrintInfo(withId);
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

    }

    public static Artist? GetArtist<T>(string field, T value)
    {
        
        Artist? artist = Artists?.FirstOrDefault(a => a.GetType().GetProperty(field)?.GetValue(a)?.Equals(value) == true);
        return artist;
    }
    
    public static List<Artist> GetArtists()
    {
        string jsonString = FileHandler.ReadFile("artists.json");
        if (jsonString == "")
        {
            return new List<Artist>();
        }
        List<Artist>? artists = JsonSerializer.Deserialize<List<Artist>>(jsonString);
        return artists ?? new List<Artist>();
    }
    
    public static void EditArtist(int id, string newArtistName)
    {
        Artist? artist = GetArtist("Id", id);
        if (artist == null)
        {
            Console.WriteLine("Artist not found");
            return;
        }
        artist.Name = newArtistName;
        Artists[Artists.FindIndex(a => a.Id == artist.Id)] = artist;
    }

    public static void UpdateArtistAlbums(Artist artist, Album album)
    {

        if (artist.AlbumIds.Contains(album.Id))
        {
            return;
        }
        Artists.Find(a => a.Id == artist.Id)?.AlbumIds.Add(album.Id);

    }
    
    public static void UpdateArtistSongs(Artist artist, Song song)
    {
        if (artist.SongIds.Contains(song.Id))
        {
            return;
        }
        Artists.Find(a => a.Id == artist.Id)?.SongIds.Add(song.Id);
    }
    
    public static void DeleteArtist(int id)
    {
        
        Artist? artist = GetArtist("Id", id);
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
            MusicBaseAlbums.DeleteAlbum(album.Id);
        }

        int artistIndex = Artists.FindIndex(a => a.Id == id);
        if (artistIndex == -1) 
        {
            Console.WriteLine("Artist not found");
            return;
        }
        Artists.RemoveAt(artistIndex);
    }


    public static void DeleteArtistAlbum(Artist artist, Album album)
    {
        int albumIndex = Artists.Find(a => a.Id == artist.Id)?.AlbumIds.IndexOf(album.Id) ?? -1;
        if (albumIndex == -1) 
        {
            return;
        }
        Artists.Find(a => a.Id == artist.Id)?.AlbumIds.RemoveAt(albumIndex);
    }
    
    public static void SaveArtists()
    {
        string jsonString = JsonSerializer.Serialize(Artists);
        FileHandler.WriteFile("artists.json", jsonString);
    }
}