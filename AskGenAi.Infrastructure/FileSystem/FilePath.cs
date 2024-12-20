﻿using AskGenAi.Core.Interfaces;
using AskGenAi.Core.Models;

namespace AskGenAi.Infrastructure.FileSystem;

// </inheritdoc>
public class FilePath : IFilePath
{
    public const string JsonExtension = ".json";
    public const string DisciplineFilename = "discipline";
    public const string DisciplineFullFilename = "disciplineV1";
    public const string ResponseFullFilename = "response";
    public const string QuestionsFilename = "questions1";
    public const string QuestionsFullFilename = "question";
    public const string ReportFullFilename = "report";
    public const string FilesPath = "AskGenAi.Infrastructure/Resources/";

    // if needed Debug to change path, change it here by removing Split("AskGenAi.Presentation")[0]
    public readonly string LocalPath = AppDomain.CurrentDomain.BaseDirectory.Split("AskGenAi.Presentation")[0];

    // </inheritdoc>
    public string GetReportPath(string fileName, string fileExtension)
    {
        return Path.Combine(LocalPath, FilesPath, fileName + "." + fileExtension);
    }

    // </inheritdoc>
    public string GetFullReportPath(string fileExtension)
    {
        return Path.Combine(LocalPath, FilesPath, ReportFullFilename + "." + fileExtension);
    }

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
            not null when type == typeof(QuestionOnPremises) => GetLocalQuestionsFullPath(),
            not null when type == typeof(DisciplineOnPremises) => GetLocalDisciplinesFullPath(),
            not null when type == typeof(ResponseOnPremises) => GetLocalResponsesFullPath(),
            _ => string.Empty
        };
    }

    // </inheritdoc>
    public string GetLocalQuestionsFullPath()
    {
        return Path.Combine(LocalPath, FilesPath, QuestionsFullFilename + JsonExtension);
    }

    // </inheritdoc>
    public string GetLocalDisciplinesFullPath()
    {
        return Path.Combine(LocalPath, FilesPath, DisciplineFullFilename + JsonExtension);
    }

    public string GetLocalResponsesFullPath()
    {
        return Path.Combine(LocalPath, FilesPath, ResponseFullFilename + JsonExtension);
    }

    // </inheritdoc>
    public string GetLocalDisciplinePath()
    {
        return Path.Combine(LocalPath, FilesPath, DisciplineFilename + JsonExtension);
    }

    // </inheritdoc>
    public string GetLocalNewDisciplinePath(string newVersion)
    {
        return Path.Combine(LocalPath, FilesPath, DisciplineFilename + "V" + newVersion + JsonExtension);
    }

    // </inheritdoc>
    public string GetLocalNewQuestionsPath(string newVersion)
    {
        return Path.Combine(LocalPath, FilesPath, QuestionsFilename + "V" + newVersion + JsonExtension);
    }

    // </inheritdoc>
    public string GetLocalQuestionsPath()
    {
        return Path.Combine(LocalPath, FilesPath, QuestionsFilename + JsonExtension);
    }

    // </inheritdoc>
    public string GetLocalQuestionsPath(string questionsFilename)
    {
        return Path.Combine(LocalPath, FilesPath, questionsFilename + JsonExtension);
    }
}