using System.Text.Json;

namespace Course_Work_OOP;

public class MusicBase
{
    protected static List<Artist> Artists { get; set; }
    protected static List<Album> Albums { get; set; }
    protected static List<Song> Songs { get; set; }

    public MusicBase()
    {
        Artists = MusicBaseArtists.GetArtists();
        Albums = MusicBaseAlbums.GetAlbums();
        Songs = MusicBaseSongs.GetSongs();
    }
    
    // common methods
    protected static int GetLastId(string key)
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