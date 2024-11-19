using AskGenAi.Core.Interfaces;

namespace AskGenAi.Infrastructure.FileSystem;

public class FileSystem : IFileSystem
{
    public bool FileExists(string path)
    {
        return File.Exists(path);
    }

    public string ReadAllText(string path)
    {
        return File.ReadAllText(path);
    }

    public void WriteAllText(string path, string content)
    {
        File.WriteAllText(path, content);
    }

    public Task WriteAllTextAsync(string path, string content)
    {
        return File.WriteAllTextAsync(path, content);
    }
}