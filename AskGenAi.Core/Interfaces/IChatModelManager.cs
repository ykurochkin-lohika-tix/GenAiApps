namespace AskGenAi.Core.Interfaces;

/// <summary>
/// Represents a service that manages the chat model
/// </summary>
public interface IChatModelManager
{
    /// <summary>
    /// Adds a system message to the chat, and removing all previous messages
    /// </summary>
    /// <param name="systemMessage"></param>
    void AddSystemMessage(string systemMessage);

    /// <summary>
    /// Adds a user message to the chat
    /// </summary>
    /// <param name="userMessage"></param>
    void AddUserMessage(string userMessage);

    /// <summary>
    /// Gets the chat message content, no additional metadata
    /// </summary>
    /// <returns></returns>
    Task<string?> GetChatMessageContentAsync();

    /// <summary>
    /// Gets the chat message content, with additional metadata flag
    /// </summary>
    /// <param name="isAddMessageToMetadata"></param>
    /// <returns></returns>
    Task<string?> GetChatMessageContentAsync(bool isAddMessageToMetadata);
}