using System.Text.Json;

namespace Course_Work_OOP;

public class MusicBase
{
    protected static List<Artist> Artists { get; set; }
    protected static List<Album> Albums { get; set; }
    protected static List<Song> Songs { get; set; }

    public MusicBase()
    {
        Artists = MusicBaseArtists.GetArtistsFromJson();
        Albums = MusicBaseAlbums.GetAlbumsFromJson();
        Songs = MusicBaseSongs.GetSongsFromJson();
    }
    
    // common methods
    protected static int GetLastId(string key)
    {
        if (key == "artists")
        {
            if (Artists.Count > 0)
            {
                return Artists.Last().Id + 1;
            }
        }
        else if (key == "albums")
        {
            if (Albums.Count > 0)
            {
                return Albums.Last().Id + 1;
            }
        }
        else if (key == "songs")
        {
            if (Songs.Count > 0)
            {
                return Songs.Last().Id + 1;
            }
        }
        return 1;
    }

    public void Save()
    {
        MusicBaseArtists.SaveArtists();
        MusicBaseAlbums.SaveAlbums();
        MusicBaseSongs.SaveSongs();
    }

}