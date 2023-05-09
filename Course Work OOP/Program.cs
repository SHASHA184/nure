

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
                choice = InputHandler.GetString("Enter your choice:");
                MusicBase musicBase = new MusicBase();
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
                        MusicBaseAlbums.AddAlbum(name, year, artistName);
                        break;
                    }
                    case "3":
                    {
                        string name = InputHandler.GetString("Enter song name:");
                        string albumName = InputHandler.GetString("Enter album name:");
                        string artistName = InputHandler.GetString("Enter artist name:");
                        MusicBaseSongs.AddSong(name, artistName, albumName);
                        break;
                    }
                    case "4":
                    {
                        MusicBaseArtists.PrintArtists();
                        break;
                    }
                    case "5":
                    {
                        MusicBaseAlbums.PrintAlbums();
                        break;
                    }
                    case "6":
                    {
                        MusicBaseSongs.PrintSongs();
                        break;
                    }
                    case "7":
                    {
                        string artistName = InputHandler.GetString("Enter artist name:");
                        MusicBaseArtists.PrintSongsByArtist(artistName);
                        break;
                    }
                    case "8":
                    {
                        string albumName = InputHandler.GetString("Enter album name:");
                        MusicBaseAlbums.PrintSongsByAlbum(albumName);
                        break;
                    }
                    case "9":
                    {
                        string artistName = InputHandler.GetString("Enter artist name:");
                        MusicBaseArtists.PrintAlbumsByArtist(artistName);
                        break;
                    }
                    case "10":
                    {
                        int year = InputHandler.GetInt("Enter year:");
                        MusicBaseAlbums.PrintAlbumsByYear(year);
                        break;
                    }
                    case "11":
                    {
                        MusicBaseAlbums.PrintSortedAlbumsByYear();
                        break;
                    }
                    case "12":
                    {
                        MusicBaseSongs.PrintSortedSongsByName();
                        break;
                    }
                    case "13":
                    {
                        MusicBaseArtists.PrintSortedSongsByArtist();
                        break;
                    }
                    case "14":
                    {
                        MusicBaseAlbums.PrintSortedSongsByAlbum();
                        break;
                    }
                    case "15":
                    {
                        string artistName = InputHandler.GetString("Enter artist name:");
                        MusicBaseArtists.DeleteArtist(artistName);
                        break;
                    }
                    case "16":
                    {
                        string artistName = InputHandler.GetString("Enter artist name:");
                        string albumName = InputHandler.GetString("Enter album name:");
                        MusicBaseAlbums.DeleteAlbum(artistName, albumName);
                        break;
                    }
                    case "17":
                    {
                        string artistName = InputHandler.GetString("Enter artist name:");
                        string albumName = InputHandler.GetString("Enter album name:");
                        string songName = InputHandler.GetString("Enter song name:");
                        MusicBaseSongs.DeleteSong(artistName, albumName, songName);
                        break;
                    }
                    case "18":
                    {
                        string artistName = InputHandler.GetString("Enter artist name:");
                        string newName = InputHandler.GetString("Enter new name:");
                        MusicBaseArtists.EditArtist(artistName, newName);
                        break;
                    }
                    case "19":
                    {
                        string artistName = InputHandler.GetString("Enter artist name:");
                        string albumName = InputHandler.GetString("Enter album name:");
                        string newName = InputHandler.GetString("Enter new name:");
                        int newYear = InputHandler.GetInt("Enter new year:");
                        MusicBaseAlbums.EditAlbum(artistName, albumName, newName, newYear);
                        break;
                    }
                    case "20":
                    {
                        string artistName = InputHandler.GetString("Enter artist name:");
                        string albumName = InputHandler.GetString("Enter album name:");
                        string songName = InputHandler.GetString("Enter song name:");
                        string newName = InputHandler.GetString("Enter new name:");
                        MusicBaseSongs.EditSong(artistName, albumName, songName, newName);
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
