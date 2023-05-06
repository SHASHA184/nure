using System.Text.Json;

namespace Course_Work_OOP;

public class MusicBase
{
    private List<Artist> Artists { get; set; }
    private List<Album> Albums { get; set; }
    private List<Song> Songs { get; set; }
    
    public MusicBase()
    {
        Artists = GetArtists();
        Albums = GetAlbums();
        Songs = GetSongs();
    }
    
    // print info about artist/artists
    public void PrintArtists()
    {
        foreach (var artist in Artists)
        {
            List<int> albumIds = artist.AlbumIds;
            List<int> songIds = artist.SongIds;
            List<Album> albums = Albums.Where(a => albumIds.Contains(a.Id)).ToList();
            List<Song> songs = Songs.Where(s => songIds.Contains(s.Id)).ToList();
            artist.PrintInfo(albums, songs);
        }
    }
    
    public void PrintSongsByArtist(string artistName)
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
            artistSong.PrintInfo(artistName, albumName: GetAlbum("Id", artistSong.AlbumId)?.Name ?? "Unknown");
        }
    }
    
    public void PrintAlbumsByArtist(string artistName)
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
            artistAlbum.PrintInfo(artistName);
            
        }
    }
    
    // all actions with artist
    public void AddArtist(string name)
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
        JsonHelper.WriteJson("artists.json", jsonString);

    }

    private Artist? GetArtist<T>(string field, T value)
    {
        
        string jsonString = JsonHelper.ReadJson("artists.json");
        if (jsonString == "")
        {
            return null;
        }
        List<Artist>? artists = JsonSerializer.Deserialize<List<Artist>>(jsonString);
        return artists?.FirstOrDefault(a => a.GetType().GetProperty(field)?.GetValue(a)?.Equals(value) == true);
    }
    
    private List<Artist> GetArtists()
    {
        string jsonString = JsonHelper.ReadJson("artists.json");
        if (jsonString == "")
        {
            return new List<Artist>();
        }
        List<Artist>? artists = JsonSerializer.Deserialize<List<Artist>>(jsonString);
        return artists ?? new List<Artist>();
    }
    
    public void EditArtist(string artistName, string newArtistName)
    {
        Artist? artist = GetArtist("Name", artistName);
        if (artist == null)
        {
            Console.WriteLine("Artist not found");
            return;
        }
        artist.Name = newArtistName;
        string jsonString = JsonSerializer.Serialize(Artists);
        JsonHelper.WriteJson("artists.json", jsonString);
    }

    private void UpdateArtistAlbums(Artist artist, Album album)
    {

        if (artist.AlbumIds.Contains(album.Id))
        {
            return;
        }
        Artists.Find(a => a.Id == artist.Id)?.AlbumIds.Add(album.Id);
        string jsonString = JsonSerializer.Serialize(Artists);
        JsonHelper.WriteJson("artists.json", jsonString);

    }
    
    private void UpdateArtistSongs(Artist artist, Song song)
    {
        if (artist.SongIds.Contains(song.Id))
        {
            return;
        }
        Artists.Find(a => a.Id == artist.Id)?.SongIds.Add(song.Id);
        string jsonString = JsonSerializer.Serialize(Artists);
        JsonHelper.WriteJson("artists.json", jsonString);
    }
    
    public void DeleteArtist(string artistName)
    {
        Artist? artist = GetArtist("Name", artistName);
        if (artist == null)
        {
            Console.WriteLine("Artist not found");
            return;
        }
        foreach (int albumId in artist.AlbumIds)
        {
            Album? album = GetAlbum("Id", albumId);
            if (album == null)
            {
                continue;
            }
            DeleteAlbum(artistName, album.Name);
        }

        int artistIndex = Artists.FindIndex(a => a.Id == artist.Id);
        if (artistIndex == -1) 
        {
            Console.WriteLine("Artist not found");
            return;
        }
        Artists.RemoveAt(artistIndex);
        string jsonString = JsonSerializer.Serialize(Artists);
        JsonHelper.WriteJson("artists.json", jsonString);


    }


    private void DeleteArtistAlbum(Artist artist, Album album)
    {
        int albumIndex = Artists.Find(a => a.Id == artist.Id)?.AlbumIds.IndexOf(album.Id) ?? -1;
        if (albumIndex == -1) 
        {
            return;
        }
        Artists.Find(a => a.Id == artist.Id)?.AlbumIds.RemoveAt(albumIndex);
        string jsonString = JsonSerializer.Serialize(Artists);
        JsonHelper.WriteJson("artists.json", jsonString);
    }

    


    // print info about album/albums
    public void PrintAlbums()
    {
        foreach (var album in Albums)
        {
            Artist? artist = GetArtist("Id", album.ArtistId);
            if (artist == null)
            {
                continue;
            }
            album.PrintInfo(artist.Name);
        }
    }
    
    public void PrintAlbumsByYear(int year)
    {
        var albumsByYear = Albums.Where(a => a.Year == year).ToList();
        foreach (var album in albumsByYear)
        {
            Artist? artist = GetArtist("Id", album.ArtistId);
            Console.WriteLine($"Name: {album.Name}, Artist: {artist?.Name}");
        }
    }
    
    public void PrintSongsByAlbum(string albumName)
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
            Artist? artist = GetArtist("Id", albumSong.ArtistId);
            if (artist == null)
            {
                continue;
            }
            albumSong.PrintInfo(artist.Name, albumName);
        }
    }

    // all actions with album
    public void AddAlbum(string name, int year, string artistName)
    {
        var artist = GetArtist("Name", artistName);
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
        UpdateArtistAlbums(artist, album);
    }

    private Album? GetAlbum<T>(string field, T value)
    {
        string jsonString = JsonHelper.ReadJson("albums.json");
        if (jsonString == "")
        {
            return null;
        }
        List<Album>? albums = JsonSerializer.Deserialize<List<Album>>(jsonString);
        return albums?.FirstOrDefault(a => a.GetType().GetProperty(field)?.GetValue(a)?.Equals(value) == true);
    }
    
    private List<Album> GetAlbums()
    {
        string jsonString = JsonHelper.ReadJson("albums.json");
        if (jsonString == "")
        {
            return new List<Album>();
        }
        List<Album>? albums = JsonSerializer.Deserialize<List<Album>>(jsonString);
        return albums ?? new List<Album>();
    }
    
    public void EditAlbum(string artistName, string albumName, string newAlbumName, int newYear)
    {
        var artist = GetArtist("Name", artistName);
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
        string jsonString = JsonSerializer.Serialize(Albums);
        JsonHelper.WriteJson("albums.json", jsonString);
    }
    
    private void UpdateAlbumSongs(Album album, Song song)
    {
        if (album.SongIds.Contains(song.Id))
        {
            return;
        }
        Albums?.Find(a => a.Id == album.Id)?.SongIds.Add(song.Id);
        string jsonString = JsonSerializer.Serialize(Albums);
        JsonHelper.WriteJson("albums.json", jsonString);
    }
    
    public void DeleteAlbum(string artistName, string albumName)
    {
        var artist = GetArtist("Name", artistName);
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
            Song? song = GetSong("Id", songId);
            if (song == null)
            {
                continue;
            }
            DeleteSong(artistName, albumName, song.Name);
        }
        DeleteArtistAlbum(artist, album);
        
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
    
    
    // print info about song/songs
    public void PrintSongs()
    {
        foreach (var song in Songs)
        {
            Artist? artist = GetArtist("Id", song.ArtistId);
            Album? album = GetAlbum("Id", song.AlbumId);
            if (artist == null || album == null)
            {
                continue;
            }
            song.PrintInfo(artist.Name, album.Name);
        }
    }
    
    
    public void PrintSortedSongsByName()
    {
        Songs.Sort((s1, s2) => string.Compare(s1.Name, s2.Name, StringComparison.Ordinal));
        Console.WriteLine("Songs sorted by name");
        foreach (var song in Songs)
        {
            Artist? artist = GetArtist("Id", song.ArtistId);
            Album? album = GetAlbum("Id", song.AlbumId);
            if (artist == null || album == null)
            {
                continue;
            }
            song.PrintInfo(artist.Name, album.Name);
        }
        
    }
    
    // all actions with song/songs
    public void AddSong(string name, string artistName, string albumName)
    {
        var album = GetAlbum("Name", albumName);
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
        var artist = GetArtist("Name", artistName);
        if (artist == null)
        {
            Console.WriteLine("Artist not found");
            return;
        }
        song = new Song(GetLastId("songs"), name, album.Id, album.ArtistId);
        Songs.Add(song);
        string jsonString = JsonSerializer.Serialize(Songs);
        JsonHelper.WriteJson("songs.json", jsonString);
        UpdateAlbumSongs(album, song);
        UpdateArtistSongs(artist, song);
    }
    
    public void EditSong(string artistName, string albumName, string songName, string newSongName)
    {
        var artist = GetArtist("Name", artistName);
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
        var song = GetSong("Name", songName);
        if (song == null)
        {
            Console.WriteLine("Song not found");
            return;
        }
        song.Name = newSongName;
        string jsonString = JsonSerializer.Serialize(Songs);
        JsonHelper.WriteJson("songs.json", jsonString);
    }
    
    public void DeleteSong(string artistName, string albumName, string songName)
    {
        var artist = GetArtist("Name", artistName);
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

    private Song? GetSong<T>(string field, T value)
    {
        string jsonString = JsonHelper.ReadJson("songs.json");
        if (jsonString == "")
        {
            return null;
        }
        List<Song>? songs = JsonSerializer.Deserialize<List<Song>>(jsonString);
        return songs?.FirstOrDefault(a => a.GetType().GetProperty(field)?.GetValue(a)?.Equals(value) == true);
    }
    
    private List<Song> GetSongs()
    {
        string jsonString = JsonHelper.ReadJson("songs.json");
        if (jsonString == "")
        {
            return new List<Song>();
        }
        List<Song>? songs = JsonSerializer.Deserialize<List<Song>>(jsonString);
        return songs ?? new List<Song>();
    }

    // common methods
    private int GetLastId(string key)
    {
        string artistsJson = JsonHelper.ReadJson(key + ".json");
        if (artistsJson == "")
        {
            return 1;
        }
        if (key == "artists")
        {
            List<Artist>? artists = JsonSerializer.Deserialize<List<Artist>>(artistsJson);
            if (artists != null && artists.Count > 0)
            {
                return artists.Last().Id + 1;
            }
        }
        else if (key == "albums")
        {
            List<Album>? albums = JsonSerializer.Deserialize<List<Album>>(artistsJson);
            if (albums != null && albums.Count > 0)
            {
                return albums.Last().Id + 1;
            }
        }
        else if (key == "songs")
        {
            List<Song>? songs = JsonSerializer.Deserialize<List<Song>>(artistsJson);
            if (songs != null && songs.Count > 0)
            {
                return songs.Last().Id + 1;
            }
        }
        return 1;
    }
    

}