﻿namespace AskGenAi.Infrastructure.AIServices;

public class AzureOpenAiSettings
{
    public string DeploymentName { get; set; } = string.Empty;
    public string Endpoint { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
}