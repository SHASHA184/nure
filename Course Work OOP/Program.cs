

using System.Text.Json;

namespace Course_Work_OOP
{
    class Program
    {
        static void Main(string[] args)
        {
            string choice = "";
            while (choice != "0")
            {
                InputHandler.PrintOptions();
                MusicBase musicBase = new MusicBase();
                choice = InputHandler.GetString("Enter your choice:");
                switch (choice)
                {
                    case "0":
                    {
                        Console.WriteLine("Goodbye!");
                        break;
                    }
                    case "1":
                    {
                        string name = InputHandler.GetString("Enter artist name:");
                        MusicBaseArtists.AddArtist(name);
                        break;
                    }
                    case "2":
                    {
                        string name = InputHandler.GetString("Enter album name:");
                        int year = InputHandler.GetInt("Enter album year:");
                        string artistName = InputHandler.GetString("Enter artist name:");
                        string genre = InputHandler.GetString("Enter genre:");
                        MusicBaseAlbums.AddAlbum(name, year, genre, artistName);
                        break;
                    }
                    case "3":
                    {
                        string name = InputHandler.GetString("Enter song name:");
                        string albumName = InputHandler.GetString("Enter album name:");
                        string artistName = InputHandler.GetString("Enter artist name:");
                        string genre = InputHandler.GetString("Enter genre:");
                        string duration = InputHandler.GetDuration("Enter duration in mm:ss format:", "mmss");
                        MusicBaseSongs.AddSong(name, albumName, artistName, genre, duration);
                        break;
                    }
                    case "4":
                    {
                        string name = InputHandler.GetString("Enter playlist name:");
                        string description = InputHandler.GetString("Enter playlist description:");
                        string genre = InputHandler.GetString("Enter playlist genre:");
                        string duration = InputHandler.GetDuration("Enter playlist duration in hh:mm:ss format:", "hhmmss");
                        int yearFrom = InputHandler.GetInt("Enter playlist year from:");
                        int yearTo = InputHandler.GetInt("Enter playlist year to:");
                        MusicBasePlaylists.Create(name, description, genre, duration, yearFrom, yearTo);
                        break;
                    }
                    case "5":
                    {
                        MusicBaseArtists.PrintArtists();
                        break;
                    }
                    case "6":
                    {
                        MusicBaseAlbums.PrintAlbums();
                        break;
                    }
                    case "7":
                    {
                        MusicBaseSongs.PrintSongs();
                        break;
                    }
                    case "8":
                    {
                        string artistName = InputHandler.GetString("Enter artist name:");
                        MusicBaseArtists.PrintSongsByArtist(artistName);
                        break;
                    }
                    case "9":
                    {
                        string albumName = InputHandler.GetString("Enter album name:");
                        MusicBaseAlbums.PrintSongsByAlbum(albumName);
                        break;
                    }
                    case "10":
                    {
                        string genre = InputHandler.GetString("Enter genre:");
                        MusicBaseSongs.PrintSongsByGenre(genre);
                        break;
                    }
                    case "11":
                    {
                        string artistName = InputHandler.GetString("Enter artist name:");
                        MusicBaseArtists.PrintAlbumsByArtist(artistName);
                        break;
                    }
                    case "12":
                    {
                        int year = InputHandler.GetInt("Enter year:");
                        MusicBaseAlbums.PrintAlbumsByYear(year);
                        break;
                    }
                    case "13":
                    {
                        string genre = InputHandler.GetString("Enter genre:");
                        MusicBaseAlbums.PrintAlbumsByGenre(genre);
                        break;
                    }
                    case "14":
                    {
                        string songName = InputHandler.GetString("Enter song name:");
                        MusicBaseSongs.PrintArtistBySong(songName);
                        break;
                    }
                    case "15":
                    {
                        MusicBaseArtists.PrintSortedSongsByArtist();
                        break;
                    }
                    case "16":
                    {
                        MusicBaseAlbums.PrintSortedSongsByAlbum();
                        break;
                    }
                    case "17":
                    {
                        MusicBaseSongs.PrintSortedSongsBy("Genre");
                        break;
                    }
                    case "18":
                    {
                        MusicBaseSongs.PrintSortedSongsBy("Duration");
                        break;
                    }
                    case "19":

                    {
                        MusicBaseAlbums.PrintSortedAlbumsBy("Year");
                        break;
                    }
                    case "20":
                    {
                        MusicBaseAlbums.PrintSortedAlbumsBy("Genre");
                        break;
                    }
                    case "21":
                    {
                        MusicBaseAlbums.PrintSortedAlbumsBy("Duration");
                        break;
                    }
                    case "22":
                    {
                        string artistName = InputHandler.GetString("Enter artist name:");
                        MusicBaseArtists.DeleteArtist(artistName);
                        break;
                    }
                    case "23":
                    {
                        string artistName = InputHandler.GetString("Enter artist name:");
                        string albumName = InputHandler.GetString("Enter album name:");
                        MusicBaseAlbums.DeleteAlbum(artistName, albumName);
                        break;
                    }
                    case "24":
                    {
                        string artistName = InputHandler.GetString("Enter artist name:");
                        string albumName = InputHandler.GetString("Enter album name:");
                        string songName = InputHandler.GetString("Enter song name:");
                        MusicBaseSongs.DeleteSong(artistName, albumName, songName);
                        break;
                    }
                    case "25":
                    {
                        string artistName = InputHandler.GetString("Enter artist name:");
                        string newName = InputHandler.GetString("Enter new name:");
                        MusicBaseArtists.EditArtist(artistName, newName);
                        break;
                    }
                    case "26":
                    {
                        string artistName = InputHandler.GetString("Enter artist name:");
                        string albumName = InputHandler.GetString("Enter album name:");
                        string newName = InputHandler.GetString("Enter new name:");
                        MusicBaseAlbums.EditAlbumName(artistName, albumName, newName);
                        break;
                    }
                    case "27":
                    {
                        string artistName = InputHandler.GetString("Enter artist name:");
                        string albumName = InputHandler.GetString("Enter album name:");
                        int newYear = InputHandler.GetInt("Enter new year:");
                        MusicBaseAlbums.EditAlbumYear(artistName, albumName, newYear);
                        break;
                    }
                    case "28":
                    {
                        string artistName = InputHandler.GetString("Enter artist name:");
                        string albumName = InputHandler.GetString("Enter album name:");
                        string songName = InputHandler.GetString("Enter song name:");
                        string newName = InputHandler.GetString("Enter new name:");
                        MusicBaseSongs.EditSongName(artistName, albumName, songName, newName);
                        break;
                    }
                    case "29":
                    {
                        string artistName = InputHandler.GetString("Enter artist name:");
                        string albumName = InputHandler.GetString("Enter album name:");
                        string songName = InputHandler.GetString("Enter song name:");
                        string newDuration = InputHandler.GetDuration("Enter new duration in mm:ss format:", "mmss");
                        MusicBaseSongs.EditSongDuration(artistName, albumName, songName, newDuration);
                        break;
                    }
                    case "30":
                    {
                        string artistName = InputHandler.GetString("Enter artist name:");
                        string albumName = InputHandler.GetString("Enter album name:");
                        string songName = InputHandler.GetString("Enter song name:");
                        string newGenre = InputHandler.GetString("Enter new genre:");
                        MusicBaseSongs.EditSongGenre(artistName, albumName, songName, newGenre);
                        break;
                    }
                    default:
                    {
                        Console.WriteLine("Invalid choice");
                        break;
                    }
                }
            }

        }
    }
}
