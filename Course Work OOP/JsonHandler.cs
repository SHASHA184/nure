namespace Course_Work_OOP;

public abstract class JsonHandler
{
    public static string ReadJson(string fileName)
    {
        if (!File.Exists(fileName))
        {
            File.Create(fileName);
        }
        return File.ReadAllText(fileName);
    }
    
    public static void WriteJson(string fileName, string jsonString)
    {
        File.WriteAllText(fileName, jsonString);
    }
}