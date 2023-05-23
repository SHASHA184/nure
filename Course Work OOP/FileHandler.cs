namespace Course_Work_OOP;

public abstract class FileHandler
{
    public static string ReadFile(string fileName)
    {
        if (!File.Exists(fileName))
        {
            File.Create(fileName);
        }
        return File.ReadAllText(fileName);
    }
    
    public static void WriteFile(string fileName, string text)
    {
        File.WriteAllText(fileName, text);
    }
}