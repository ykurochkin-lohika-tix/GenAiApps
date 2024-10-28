using AskGenAi.Core.Entities;
using AskGenAi.Core.Interfaces;

namespace AskGenAi.Infrastructure.FileSystem;

// </inheritdoc>
public class FilePath : IFilePath
{
    public const string Extension = ".json";
    public const string DisciplineFilename = "discipline";
    public const string DisciplineFullFilename = "disciplineV1";
    public const string ResponseFullFilename = "response";
    public const string QuestionsFilename = "questions1";
    public const string QuestionsFullFilename = "question";
    public const string FilesPath = "AskGenAi.Infrastructure/Resources/";

    // if needed Debug to change path, change it here by removing Split("AskGenAi.Presentation")[0]
    public readonly string LocalPath = AppDomain.CurrentDomain.BaseDirectory.Split("AskGenAi.Presentation")[0];

    // </inheritdoc>
    public string[] GetQuestionsListFilename()
    {
        return
        [
            "questions1", "questions2", "questions3", "questions4",
            "questions11", "questions12", "questions13", "questions14",
            "questions21", "questions22", "questions23",
            "questions31", "questions32"
        ];
    }

    // </inheritdoc>
    public string GetLocalFullPathByType(Type type)
    {
        return type switch
        {
            not null when type == typeof(Question) => GetLocalQuestionsFullPath(),
            not null when type == typeof(Discipline) => GetLocalDisciplinesFullPath(),
            not null when type == typeof(Response) => GetLocalResponsesFullPath(),
            _ => string.Empty
        };
    }

    // </inheritdoc>
    public string GetLocalQuestionsFullPath()
    {
        return Path.Combine(LocalPath, FilesPath, QuestionsFullFilename + Extension);
    }

    // </inheritdoc>
    public string GetLocalDisciplinesFullPath()
    {
        return Path.Combine(LocalPath, FilesPath, DisciplineFullFilename + Extension);
    }

    public string GetLocalResponsesFullPath()
    {
        return Path.Combine(LocalPath, FilesPath, ResponseFullFilename + Extension);
    }

    // </inheritdoc>
    public string GetLocalDisciplinePath()
    {
        return Path.Combine(LocalPath, FilesPath, DisciplineFilename + Extension);
    }

    // </inheritdoc>
    public string GetLocalNewDisciplinePath(string newVersion)
    {
        return Path.Combine(LocalPath, FilesPath, DisciplineFilename + "V" + newVersion + Extension);
    }

    // </inheritdoc>
    public string GetLocalQuestionsPath()
    {
        return Path.Combine(LocalPath, FilesPath, QuestionsFilename + Extension);
    }

    // </inheritdoc>
    public string GetLocalNewQuestionsPath(string newVersion)
    {
        return Path.Combine(LocalPath, FilesPath, QuestionsFilename + "V" + newVersion + Extension);
    }

    // </inheritdoc>
    public string GetLocalQuestionsPath(string questionsFilename)
    {
        return Path.Combine(LocalPath, FilesPath, questionsFilename + Extension);
    }
}