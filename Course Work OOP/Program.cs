

using System.Text.Json;

namespace Course_Work_OOP
{
    class Program
    {
        static void Main(string[] args)
        {
            MusicBase musicBase = new MusicBase();
            string choice = "";
            while (choice != "0")
            {
                InputHandler.DisplayMainMenu();
                choice = InputHandler.GetString("Enter your choice:");
                switch (choice)
                {
                    case "0":
                        Console.WriteLine("Goodbye!");
                        musicBase.Save();
                        break;
                    case "1":
                        InputHandler.HandleAddSubMenu();
                        break;
                    case "2":
                        InputHandler.HandlePrintSubMenu();
                        break;
                    case "3":
                        InputHandler.HandleSortSubMenu();
                        break;
                    case "4":
                        InputHandler.HandleDeleteSubMenu();
                        break;
                    case "5":
                        InputHandler.HandleEditSubMenu();
                        break;
                    default:
                        InputHandler.PrintTopAndBottomLine();
                        InputHandler.PrintTextWithSides("Invalid choice");
                        break;
                }
            }
            

        }
    }
}
