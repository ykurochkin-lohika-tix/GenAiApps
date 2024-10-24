using AskGenAi.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace AskGenAi.Infrastructure.AIServices;

// </inheritdoc>
public class AzureOpenAiChatCompletion : IChatModelManager
{
    private readonly Kernel _kernel;
    private readonly IChatCompletionService _chatCompletionService;
    private ChatHistory? _history;

    public ChatHistory History => _history ??= [];

    public AzureOpenAiChatCompletion()
    {
        // Build configuration
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false, true)
            .Build();

        var deploymentName = configuration["AzureOpenAI:DeploymentName"];
        var endpoint = configuration["AzureOpenAI:Endpoint"];
        var apiKey = configuration["AzureOpenAI:ApiKey"];

        // Create a kernel with Azure OpenAI chat completion
        var builder = Kernel.CreateBuilder().AddAzureOpenAIChatCompletion(deploymentName!, endpoint!, apiKey!);

        // Add enterprise components
        builder.Services.AddLogging(services => services.AddConsole().SetMinimumLevel(LogLevel.Trace));

        _kernel = builder.Build();
        _chatCompletionService = _kernel.GetRequiredService<IChatCompletionService>();
    }

    // </inheritdoc>
    public void AddSystemMessage(string systemMessage)
    {
        History.Clear();
        History.AddSystemMessage(systemMessage);
    }

    // </inheritdoc>
    public void AddUserMessage(string userMessage)
    {
        History.AddUserMessage(userMessage);
    }

    // </inheritdoc>
    public Task<string?> GetChatMessageContentAsync()
    {
        return GetChatMessageContentAsync(false);
    }

    // </inheritdoc>
    public async Task<string?> GetChatMessageContentAsync(bool isAddMessageToMetadata)
    {
        var result = await _chatCompletionService.GetChatMessageContentAsync(
            History,
            null,
            _kernel);

        if (isAddMessageToMetadata)
        {
            // Add the message from the agent to the chat history
            History.AddMessage(result.Role, result.Content ?? string.Empty);
        }

        return result.Content;
    }
}