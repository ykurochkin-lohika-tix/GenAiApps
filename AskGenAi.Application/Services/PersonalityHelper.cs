﻿using AskGenAi.Core.Enums;

namespace AskGenAi.Application.Services;

/// <summary>
/// Represents a helper class that provides personality-related functionality , but make a table for the switch statement
/// </summary>
public static class PersonalityHelper
{
    private const string DetNetDeveloper = ".Net C# developer";
    private const string Default = "Developer";

    public static string GetPersonality(DisciplineType disciplineType)
    {
        return disciplineType switch
        {
            DisciplineType.NetRuntime or
                DisciplineType.NetBasicLanguage or
                DisciplineType.NetAdvancedLanguage or
                DisciplineType.NetTesting or
                DisciplineType.DatabasesRelational or
                DisciplineType.DatabasesOrm or
                DisciplineType.DatabasesNonRelational or
                DisciplineType.DatabasesEventProcessing or
                DisciplineType.WebCoreHttp or
                DisciplineType.WebCoreApis or
                DisciplineType.WebCoreSecurity or
                DisciplineType.AspNetCoreWeb or
                DisciplineType.AspNetCoreMonitoring => DetNetDeveloper,
            _ => Default
        };
    }
}