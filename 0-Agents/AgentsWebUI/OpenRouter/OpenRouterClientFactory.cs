namespace AgentsWebUI.OpenRouter;

public class OpenRouterClientFactory(string apiKey)
{
    public ChatClient CreateChatClient(OpenRouterClientOptions options)
    {
        var aiClient = new OpenAIClientOptions
        {
            Endpoint = options.Endpoint,
        };

        var credential = new ApiKeyCredential(apiKey);
        var client = new ChatClient(model: options.Model, credential: credential, options: aiClient);
        return client;
    }
}
