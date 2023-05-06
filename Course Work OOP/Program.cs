

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
                        musicBase.AddArtist(name);
                        break;
                    }
                    case "2":
                    {
                        string name = InputHandler.GetString("Enter album name:");
                        int year = InputHandler.GetInt("Enter album year:");
                        string artistName = InputHandler.GetString("Enter artist name:");
                        musicBase.AddAlbum(name, year, artistName);
                        break;
                    }
                    case "3":
                    {
                        string name = InputHandler.GetString("Enter song name:");
                        string albumName = InputHandler.GetString("Enter album name:");
                        string artistName = InputHandler.GetString("Enter artist name:");
                        musicBase.AddSong(name, artistName, albumName);
                        break;
                    }
                    case "4":
                    {
                        musicBase.PrintArtists();
                        break;
                    }
                    case "5":
                    {
                        musicBase.PrintAlbums();
                        break;
                    }
                    case "6":
                    {
                        musicBase.PrintSongs();
                        break;
                    }
                    case "7":
                    {
                        string artistName = InputHandler.GetString("Enter artist name:");
                        musicBase.PrintSongsByArtist(artistName);
                        break;
                    }
                    case "8":
                    {
                        string albumName = InputHandler.GetString("Enter album name:");
                        musicBase.PrintSongsByAlbum(albumName);
                        break;
                    }
                    case "9":
                    {
                        string artistName = InputHandler.GetString("Enter artist name:");
                        musicBase.PrintAlbumsByArtist(artistName);
                        break;
                    }
                    case "10":
                    {
                        int year = InputHandler.GetInt("Enter year:");
                        musicBase.PrintAlbumsByYear(year);
                        break;
                    }
                    case "11":
                    {
                        musicBase.PrintSortedSongsByName();
                        break;
                    }
                    case "12":
                    {
                        string artistName = InputHandler.GetString("Enter artist name:");
                        musicBase.DeleteArtist(artistName);
                        break;
                    }
                    case "13":
                    {
                        string artistName = InputHandler.GetString("Enter artist name:");
                        string albumName = InputHandler.GetString("Enter album name:");
                        musicBase.DeleteAlbum(artistName, albumName);
                        break;
                    }
                    case "14":
                    {
                        string artistName = InputHandler.GetString("Enter artist name:");
                        string albumName = InputHandler.GetString("Enter album name:");
                        string songName = InputHandler.GetString("Enter song name:");
                        musicBase.DeleteSong(artistName, albumName, songName);
                        break;
                    }
                    case "15":
                    {
                        string artistName = InputHandler.GetString("Enter artist name:");
                        string newName = InputHandler.GetString("Enter new name:");
                        musicBase.EditArtist(artistName, newName);
                        break;
                    }
                    case "16":
                    {
                        string artistName = InputHandler.GetString("Enter artist name:");
                        string albumName = InputHandler.GetString("Enter album name:");
                        string newName = InputHandler.GetString("Enter new name:");
                        int newYear = InputHandler.GetInt("Enter new year:");
                        musicBase.EditAlbum(artistName, albumName, newName, newYear);
                        break;
                    }
                    case "17":
                    {
                        string artistName = InputHandler.GetString("Enter artist name:");
                        string albumName = InputHandler.GetString("Enter album name:");
                        string songName = InputHandler.GetString("Enter song name:");
                        string newName = InputHandler.GetString("Enter new name:");
                        musicBase.EditSong(artistName, albumName, songName, newName);
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
