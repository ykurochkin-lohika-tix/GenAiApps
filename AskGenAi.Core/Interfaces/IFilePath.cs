﻿namespace AskGenAi.Core.Interfaces;

/// <summary>
/// Represents a service that provides file paths helpful for the application
/// </summary>
public interface IFilePath
{
    /// <summary>
    /// Gets the path to the local discipline full file
    /// </summary>
    /// <returns></returns>
    string GetLocalQuestionsFullPath();

    /// <summary>
    /// Gets the path to the local discipline file
    /// </summary>
    /// <returns></returns>
    string GetLocalDisciplinePath();

    /// <summary>
    /// Gets the path to the local discipline full file
    /// </summary>
    /// <returns></returns>
    string GetLocalDisciplinesFullPath();

    /// <summary>
    /// Gets the path to the local responses file
    /// </summary>
    /// <returns></returns>
    string GetLocalResponsesFullPath();

    /// <summary>
    /// Gets the path to the local responses file with a given filename
    /// </summary>
    /// <returns></returns>
    string GetLocalNewDisciplinePath(string newVersion);

    /// <summary>
    /// Gets the path to the local responses file
    /// </summary>
    /// <returns></returns>
    string GetLocalQuestionsPath();

    /// <summary>
    /// Gets the path to the local questions file with a given filename
    /// </summary>
    /// <param name="questionsFilename"></param>
    /// <returns></returns>
    string GetLocalQuestionsPath(string questionsFilename);

    /// <summary>
    /// Gets the path to the local questions file with a new version
    /// </summary>
    /// <param name="newVersion"></param>
    /// <returns></returns>
    string GetLocalNewQuestionsPath(string newVersion);

    /// <summary>
    /// Gets the list of predefined questions filenames
    /// </summary>
    /// <returns></returns>
    string[] GetQuestionsListFilename();

    /// <summary>
    /// Gets the local full path by type
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    string GetLocalFullPathByType(Type type);

    /// <summary>
    /// Gets the report path
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="fileExtension"></param>
    /// <returns></returns>
    string GetReportPath(string fileName, string fileExtension);

    /// <summary>
    /// Gets the full report path
    /// </summary>
    /// <param name="fileExtension"></param>
    /// <returns></returns>
    string GetFullReportPath(string fileExtension);
}