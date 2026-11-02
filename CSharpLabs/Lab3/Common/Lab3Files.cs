namespace CSharpLabs.Lab3.Common;

internal static class Lab3Files
{
    public static string GetDataPath(string name)
    {
        string directory = Path.Combine("labs", "Lab3", "data");
        Directory.CreateDirectory(directory);
        return Path.Combine(directory, name);
    }
}
