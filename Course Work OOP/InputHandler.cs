namespace Course_Work_OOP;

public abstract class InputHandler
{
    private const int Length = 40;

    public static void DisplayMainMenu()
    {
        PrintTopAndBottomLine();
        PrintTextWithSides("Choose an option:");
        PrintTopAndBottomLine();
        PrintTextWithSides("0 | Exit");
        PrintTopAndBottomLine();
        PrintTextWithSides("1 | Add");
        PrintTextWithSides("2 | Print");
        PrintTextWithSides("3 | Sort");
        PrintTextWithSides("4 | Delete");
        PrintTextWithSides("5 | Edit");
        PrintTopAndBottomLine();
    }
    
    public static void HandleAddSubMenu()
    {
        string choice = "";
        while (choice != "0")
        {
            DisplayAddSubMenu();
            PrintTopAndBottomLine();
            choice = GetString("Enter your choice:");
            switch (choice)
            {
                case "0":
                    Console.WriteLine();
                    PrintTopAndBottomLine();
                    PrintTextWithSides("Returning to the main menu.");
                    PrintTopAndBottomLine();
                    Console.WriteLine();
                    break;
                case "1":
                    AddNewArtist();
                    break;
                case "2":
                    AddNewAlbum();
                    break;
                case "3":
                    AddNewSong();
                    break;
                case "4":
                    AddPlaylist();
                    break;
                default:
                    Console.WriteLine();
                    PrintTopAndBottomLine();
                    PrintTextWithSides("Invalid choice");
                    PrintTopAndBottomLine();
                    Console.WriteLine();
                    break;
            }
        }
    }

    private static void DisplayAddSubMenu()
    {
        PrintTopAndBottomLine();
        PrintTextWithSides("Choose an option:");
        PrintTopAndBottomLine();
        PrintTextWithSides("0 | Back");
        PrintTextWithSides("1 | Add new artist");
        PrintTextWithSides("2 | Add new album");
        PrintTextWithSides("3 | Add new song");
        PrintTextWithSides("4 | Add new playlist");
    }

    private static void AddNewArtist()
    {
        string name = GetString("Enter artist name:");
        MusicBaseArtists.AddArtist(name);
    }

    private static void AddNewAlbum()
    {
        string name = GetString("Enter album name:");
        int year = GetInt("Enter album year:");
        string artistName = GetString("Enter artist name:");
        string genre = GetString("Enter genre:");
        MusicBaseAlbums.AddAlbum(name, year, genre, artistName);
    }

    private static void AddNewSong()
    {
        string name = GetString("Enter song name:");
        string albumName = GetString("Enter album name:");
        string artistName = GetString("Enter artist name:");
        string genre = GetString("Enter genre:");
        string duration = GetDuration("Enter duration in mm:ss format:", "mmss");
        MusicBaseSongs.AddSong(name, albumName, artistName, genre, duration);
    }

    private static void AddPlaylist()
    {
        string name = GetString("Enter playlist name:");
        string description = GetString("Enter playlist description:");
        string genre = GetString("Enter playlist genre (type All if not necessary):");
        string duration = GetDuration("Enter playlist duration in hh:mm:ss format:", "hhmmss");
        int yearFrom = GetInt("Enter playlist year from:");
        int yearTo = GetInt("Enter playlist year to:");
        Playlists.Add(name, description, genre, duration, yearFrom, yearTo);
    }

    private static void DisplayPrintSubMenu()
{
        PrintTopAndBottomLine();
        PrintTextWithSides("Choose an option:");
        PrintTopAndBottomLine();
        PrintTextWithSides("0 | Back");
        PrintTopAndBottomLine();
        // Print all
        PrintTextWithSides("1 | Print all artists");
        PrintTextWithSides("2 | Print all albums");
        PrintTextWithSides("3 | Print all songs");
        // Print songs by
        PrintTopAndBottomLine();
        PrintTextWithSides("4 | Print all songs by artist");
        PrintTextWithSides("5 | Print all songs by album");
        PrintTextWithSides("6 | Print all songs by genre");
        // Print albums by
        PrintTopAndBottomLine();
        PrintTextWithSides("7 | Print all albums by artist");
        PrintTextWithSides("8 | Print all albums by year");
        PrintTextWithSides("9 | Print all albums by genre");
        // Print artist by
        PrintTopAndBottomLine();
        PrintTextWithSides("10 | Print artist by song");
        PrintTopAndBottomLine();
    }
    
    public static void HandlePrintSubMenu()
    {
        string choice = "";
        while (choice != "0")
        {
            DisplayPrintSubMenu();
            choice = GetString("Enter your choice:");
            switch (choice)
            {
                case "0":
                    Console.WriteLine();
                    PrintTopAndBottomLine();
                    PrintTextWithSides("Returning to the main menu.");
                    PrintTopAndBottomLine();
                    Console.WriteLine();
                    break;
                case "1":
                    PrintAllArtists();
                    break;
                case "2":
                    PrintAllAlbums();
                    break;
                case "3":
                    PrintAllSongs();
                    break;
                case "4":
                    PrintSongsByArtist();
                    break;
                case "5":
                    PrintSongsByAlbum();
                    break;
                case "6":
                    PrintSongsByGenre();
                    break;
                case "7":
                    PrintAlbumsByArtist();
                    break;
                case "8":
                    PrintAlbumsByYear();
                    break;
                case "9":
                    PrintAlbumsByGenre();
                    break;
                case "10":
                    PrintArtistBySong();
                    break;
                default:
                    Console.WriteLine();
                    PrintTopAndBottomLine();
                    PrintTextWithSides("Invalid choice");
                    PrintTopAndBottomLine();
                    Console.WriteLine();
                    break;
            }
        }
    }
    
    private static void PrintAllArtists(bool withId = false)
    {
        MusicBaseArtists.PrintArtists(withId);
    }
    
    private static void PrintAllAlbums(bool withId = false)
    {
        MusicBaseAlbums.PrintAlbums(withId);

    }
    
    private static void PrintAllSongs(bool withId = false)
    {
        MusicBaseSongs.PrintSongs(withId);
    }
    
    private static void PrintSongsByArtist()
    {
        string artistName = GetString("Enter artist name:");
        MusicBaseArtists.PrintSongsByArtist(artistName);
    }
    
    private static void PrintSongsByAlbum()
    {
        string albumName = GetString("Enter album name:");
        MusicBaseAlbums.PrintSongsByAlbum(albumName);
    }
    
    private static void PrintSongsByGenre()
    {
        string genre = GetString("Enter genre:");
        MusicBaseSongs.PrintSongsByGenre(genre);
    }
    
    private static void PrintAlbumsByArtist()
    {
        string artistName = GetString("Enter artist name:");
        MusicBaseArtists.PrintAlbumsByArtist(artistName);
    }
    
    private static void PrintAlbumsByYear()
    {
        int year = GetInt("Enter year:");
        MusicBaseAlbums.PrintAlbumsByYear(year);
    }
    
    private static void PrintAlbumsByGenre()
    {
        string genre = GetString("Enter genre:");
        MusicBaseAlbums.PrintAlbumsByGenre(genre);
    }
    
    private static void PrintArtistBySong()
    {
        string songName = GetString("Enter song name:");
        MusicBaseSongs.PrintArtistBySong(songName);
    }
    
    

    public static void HandleSortSubMenu()
    {
        string choice = "";
        while (choice != "0")
        {
            DisplaySortSubMenu();
            choice = GetString("Enter your choice:");
            switch (choice)
            {
                case "0":
                    Console.WriteLine();
                    PrintTopAndBottomLine();
                    PrintTextWithSides("Returning to the main menu.");
                    PrintTopAndBottomLine();
                    Console.WriteLine();
                    break;
                case "1":
                    SortSongsByArtist();
                    break;
                case "2":
                    SortSongsByAlbum();
                    break;
                case "3":
                    SortSongsByGenre();
                    break;
                case "4":
                    SortSongsByDuration();
                    break;
                case "5":
                    SortAlbumsByYear();
                    break;
                case "6":
                    SortAlbumsByGenre();
                    break;
                case "7":
                    SortAlbumsByDuration();
                    break;
                default:
                    Console.WriteLine();
                    PrintTopAndBottomLine();
                    PrintTextWithSides("Invalid choice");
                    PrintTopAndBottomLine();
                    Console.WriteLine();
                    break;
            }
        }
    }
    
    private static void DisplaySortSubMenu()
    {
        PrintTopAndBottomLine();
        PrintTextWithSides("Choose an option");
        PrintTopAndBottomLine();
        PrintTextWithSides("0 | Back");
        PrintTopAndBottomLine();
        PrintTextWithSides("1 | Sort songs by artist");
        PrintTextWithSides("2 | Sort songs by album");
        PrintTextWithSides("3 | Sort songs by genre");
        PrintTextWithSides("4 | Sort songs by duration");
        PrintTextWithSides("5 | Sort albums by year");
        PrintTextWithSides("6 | Sort albums by genre");
        PrintTextWithSides("7 | Sort albums by duration");
        PrintTopAndBottomLine();
    }
    
    private static void SortSongsByArtist()
    {
        MusicBaseArtists.PrintSortedSongsByArtist();
    }
    
    private static void SortSongsByAlbum()
    {
        MusicBaseAlbums.PrintSortedSongsByAlbum();
    }
    
    private static void SortSongsByGenre()
    {
        MusicBaseSongs.PrintSortedSongsBy("Genre");
    }
    
    private static void SortSongsByDuration()
    {
        MusicBaseSongs.PrintSortedSongsBy("Duration");
    }
    
    private static void SortAlbumsByYear()
    {
        MusicBaseAlbums.PrintSortedAlbumsBy("Year");
    }
    
    private static void SortAlbumsByGenre()
    {
        MusicBaseAlbums.PrintSortedAlbumsBy("Genre");
    }
    
    private static void SortAlbumsByDuration()
    {
        MusicBaseAlbums.PrintSortedAlbumsBy("Duration");
    }
    
    public static void HandleDeleteSubMenu()
    {
        string choice = "";
        while (choice != "0")
        {
            DisplayDeleteSubMenu();
            choice = GetString("Enter your choice:");
            switch (choice)
            {
                case "0":
                    Console.WriteLine();
                    PrintTopAndBottomLine();
                    PrintTextWithSides("Returning to the main menu.");
                    PrintTopAndBottomLine();
                    Console.WriteLine();
                    break;
                case "1":
                    DeleteArtist();
                    break;
                case "2":
                    DeleteAlbum();
                    break;
                case "3":
                    DeleteSong();
                    break;
                default:
                    Console.WriteLine();
                    PrintTopAndBottomLine();
                    PrintTextWithSides("Invalid choice");
                    PrintTopAndBottomLine();
                    Console.WriteLine();
                    break;
            }
        }
    }
    
    private static void DisplayDeleteSubMenu()
    {
        PrintTopAndBottomLine();
        PrintTextWithSides("Choose an option:");
        PrintTopAndBottomLine();
        PrintTextWithSides("0 | Back");
        PrintTopAndBottomLine();
        PrintTextWithSides("1 | Delete artist");
        PrintTextWithSides("2 | Delete album");
        PrintTextWithSides("3 | Delete song");
        PrintTopAndBottomLine();
    }
    
    private static void DeleteArtist()
    {
        PrintAllArtists(true);
        int id = GetInt("Enter the id of the artist you want to delete:");
        MusicBaseArtists.DeleteArtist(id);
    }
    
    private static void DeleteAlbum()
    {
        PrintAllAlbums(true);
        int id = GetInt("Enter the id of the album you want to delete:");
        MusicBaseAlbums.DeleteAlbum(id);
    }
    
    private static void DeleteSong()
    {
        PrintAllSongs(true);
        int id = GetInt("Enter the id of the song you want to delete:");
        MusicBaseSongs.DeleteSong(id);
    }
    
    public static void HandleEditSubMenu()
    {
        string choice = "";
        while (choice != "0")
        {
            DisplayEditSubMenu();
            choice = GetString("Enter your choice:");
            switch (choice)
            {
                case "0":
                    Console.WriteLine();
                    PrintTopAndBottomLine();
                    PrintTextWithSides("Returning to the main menu.");
                    PrintTopAndBottomLine();
                    Console.WriteLine();
                    break;
                case "1":
                    EditArtist();
                    break;
                case "2":
                    EditAlbumName();
                    break;
                case "3":
                    EditAlbumYear();
                    break;
                case "4":
                    EditAlbumGenre();
                    break;
                case "5":
                    EditSongName();
                    break;
                case "6":
                    EditSongDuration();
                    break;
                case "7":
                    EditSongGenre();
                    break;
                default:
                    Console.WriteLine();
                    PrintTopAndBottomLine();
                    PrintTextWithSides("Invalid choice");
                    PrintTopAndBottomLine();
                    Console.WriteLine();
                    break;
            }
        }
    }
    
    private static void DisplayEditSubMenu()
    {
        PrintTopAndBottomLine();
        PrintTextWithSides("Choose an option:");
        PrintTopAndBottomLine();
        PrintTextWithSides("0 | Back");
        PrintTopAndBottomLine();
        PrintTextWithSides("1 | Edit artist");
        PrintTextWithSides("2 | Edit album name");
        PrintTextWithSides("3 | Edit album year");
        PrintTextWithSides("4 | Edit album genre");
        PrintTextWithSides("5 | Edit song name");
        PrintTextWithSides("6 | Edit song duration");
        PrintTextWithSides("7 | Edit song genre");
        PrintTopAndBottomLine();
    }
    
    private static void EditArtist()
    {
        PrintAllArtists(true);
        int id = GetInt("Enter the id of the artist you want to edit:");
        string name = GetString("Enter the new name of the artist:");
        MusicBaseArtists.EditArtist(id, name);
    }
    
    private static void EditAlbumName()
    {
        PrintAllAlbums(true);
        int id = GetInt("Enter the id of the album you want to edit:");
        string name = GetString("Enter the new name of the album:");
        MusicBaseAlbums.EditAlbum(id, "Name", name);
    }
    
    private static void EditAlbumYear()
    {
        PrintAllAlbums(true);
        int id = GetInt("Enter the id of the album you want to edit:");
        int year = GetInt("Enter the new year of the album:");
        MusicBaseAlbums.EditAlbum(id, "Year", year);
    }
    
    private static void EditAlbumGenre()
    {
        PrintAllAlbums(true);
        int id = GetInt("Enter the id of the album you want to edit:");
        string genre = GetString("Enter the new genre of the album:");
        MusicBaseAlbums.EditAlbum(id, "Genre", genre);
    }
    
    private static void EditSongName()
    {
        PrintAllSongs(true);
        int id = GetInt("Enter the id of the song you want to edit:");
        string name = GetString("Enter the new name of the song:");
        MusicBaseSongs.EditSong(id, "Name", name);
    }
    
    private static void EditSongDuration()
    {
        PrintAllSongs(true);
        int id = GetInt("Enter the id of the song you want to edit:");
        string duration = GetDuration("Enter the new duration of the song:", "mmss");
        MusicBaseSongs.EditSong(id, "Duration", duration);
    }
    
    private static void EditSongGenre()
    {
        PrintAllSongs(true);
        int id = GetInt("Enter the id of the song you want to edit:");
        string genre = GetString("Enter the new genre of the song:");
        MusicBaseSongs.EditSong(id, "Genre", genre);
    }
    
    
    
    
    // validation methods
    public static string GetString(string message)
    {
        PrintTopAndBottomLine();
        PrintTextWithSides(message);
        PrintTopAndBottomLine();
        var input = Console.ReadLine();
        while (string.IsNullOrWhiteSpace(input))
        {
            PrintTopAndBottomLine();
            PrintTextWithSides(message);
            PrintTopAndBottomLine();
            input = Console.ReadLine();
            
        }
        return input;
    }

    private static int GetInt(string message)
    {
        PrintTopAndBottomLine();
        PrintTextWithSides(message);
        PrintTopAndBottomLine();
        var input = Console.ReadLine();
        while (!int.TryParse(input, out _))
        {
            PrintTopAndBottomLine();
            PrintTextWithSides(message);
            PrintTopAndBottomLine();
            input = Console.ReadLine();
        }
        return int.Parse(input);
    }

    private static string GetDuration(string message, string format)
    {
        string input = GetString(message);
        PrintTopAndBottomLine();
        while (!TimeHandler.IsValidDuration(input, format))
        {
            input = GetString(message);
        }
        return input;
    }
    
    // print methods
    public static void PrintTopAndBottomLine()
    {
        Console.WriteLine("+" + new string('–', Length) + "+");
    }
    
    public static void PrintTextWithSides(string text)
    {
        if (text.Length <= Length - 2)
        {
            Console.WriteLine("| " + text + new string(' ', Length - 2 - text.Length) + " |");
        }
        else
        {
            List<string> lines = new List<string>();
            string[] words = text.Split(' ');

            string currentLine = string.Empty;
            foreach (string word in words)
            {
                if (currentLine.Length + word.Length + 1 <= Length - 2)
                {
                    currentLine += word + ' ';
                }
                else
                {
                    lines.Add(currentLine.Trim());
                    currentLine = word + ' ';
                }
            }

            if (!string.IsNullOrWhiteSpace(currentLine))
            {
                lines.Add(currentLine.Trim());
            }

            foreach (string line in lines)
            {
                Console.WriteLine("| " + line + new string(' ', Length - 2 - line.Length) + " |");
            }
        }
    }

    

}