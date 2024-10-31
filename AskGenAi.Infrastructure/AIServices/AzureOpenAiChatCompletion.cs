using AskGenAi.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace AskGenAi.Infrastructure.AIServices;

// </inheritdoc>
public class AzureOpenAiChatCompletion : IChatModelManager
{
    private readonly Kernel _kernel;
    private readonly IChatCompletionService _chatCompletionService;
    private readonly ILogger<AzureOpenAiChatCompletion> _logger;
    private ChatHistory? _history;

    public ChatHistory History => _history ??= [];

    public AzureOpenAiChatCompletion(IOptions<AzureOpenAiSettings> options, ILogger<AzureOpenAiChatCompletion> logger)
    {
        _logger = logger;
        var settings = options.Value;
        LogAnySettingsEmpty(settings);

        // Create a kernel with Azure OpenAI chat completion
        var builder = Kernel.CreateBuilder().AddAzureOpenAIChatCompletion(settings.DeploymentName, settings.Endpoint, settings.ApiKey);

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

    private void LogAnySettingsEmpty(AzureOpenAiSettings settings)
    {
        if (string.IsNullOrEmpty(settings.DeploymentName))
        {
            _logger.LogError("AzureOpenAI:DeploymentName is not configured.");
        }
        if (string.IsNullOrEmpty(settings.Endpoint))
        {
            _logger.LogError("AzureOpenAI:Endpoint is not configured.");
        }
        if (string.IsNullOrEmpty(settings.ApiKey))
        {
            _logger.LogError("AzureOpenAI:ApiKey is not configured.");
        }
    }
}