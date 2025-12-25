# Microsoft Agent Framework with Blazor Web App

This project demonstrates the usage of the Microsoft Agent Framework in a Blazor Web Application. It includes several sample agents, each showcasing a different feature of the framework.

## How to Use

### Prerequisites

*   [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) or later.
*   An API key from [OpenRouter](https://openrouter.ai/).

### Configuration

1.  Open the `0-Agents/AgentsWebUI/appsettings.json` file.
2.  Add your OpenRouter API key to the file:

    ```json
    {
      "Logging": {
        "LogLevel": {
          "Default": "Information",
          "Microsoft.AspNetCore": "Warning"
        }
      },
      "AllowedHosts": "*",
      "OPEN_ROUTER_API_KEY": "YOUR_API_KEY_HERE"
    }
    ```
3. Alternatively, you can set the `OPEN_ROUTER_API_KEY` as an environment variable.

### Running the Application

1.  Navigate to the `0-Agents/AgentsWebUI` directory in your terminal.
2.  Run the following command:

    ```bash
    dotnet run
    ```

3.  Open your browser and navigate to the URL provided in the terminal output (usually `http://localhost:5xxx`).

## Agent Samples

Here are the available agent samples:

### Agent 01 - Basic Agent

This agent demonstrates the basic functionality of creating and running an agent. It takes instructions and a message as input and returns a response from the AI model. It shows both a simple request/response and a streaming response.

### Agent 02 - Agent with Thread

This sample showcases how to use threads to have a multi-turn conversation with an agent. The agent remembers the context of the previous messages in the same thread.

### Agent 03 - Agent with Function/Tool

This agent demonstrates how to provide functions (tools) to an agent. In this example, the agent uses `Add` and `Multiply` functions to solve a math problem that it wouldn't be able to solve on its own.

### Agent 04 - Functions with User Approval

This agent builds upon the previous example by adding a user approval step before a function is executed. The agent asks for confirmation from the user before calling the `Add` or `Multiply` functions.

### Agent 05 - Structured Output

This sample shows how to get structured data (JSON) from an unstructured text input. The agent extracts information about a person into a `PersonInfo` object.

### Agent 06 - Persisted Conversation

This agent demonstrates how to persist and resume a conversation. The conversation thread is serialized to a file and then deserialized to continue the conversation in a subsequent interaction.

### Agent 07 - Custom Thread Storage

This agent demonstrates how to use a custom `VectorChatMessageStore` to persist conversation history in memory. This allows for more flexible and scalable storage of conversation threads.

### Agent 08 - Agent Using Image

This sample shows how to use a multimodal model to analyze an image. The agent takes an image URL and a text prompt as input and returns a description of the image content.

### Agent 09 - Using Remote MCP Server

This agent demonstrates how to use a remote MCP (Model Context Protocol) server. It connects to the Microsoft Learn MCP server to answer questions about Microsoft technologies, using the tools provided by the server.
