# Microsoft Agent Framework Samples

A **Blazor Web Application** built on **.NET 10** that showcases **23 interactive samples** demonstrating the [Microsoft Agent Framework](https://learn.microsoft.com/en-us/dotnet/ai/microsoft-extensions-ai) (`Microsoft.Extensions.AI.Agents`) across **6 categories** — from basic request/response through advanced prompting strategies.

> **Live Demo**: Run the app and navigate to `http://localhost:5xxx` to explore the interactive samples.

---

## ✨ Features

- 23 interactive agent samples organized in 6 categories
- Modern, responsive UI built with Bootstrap 5 and a central CSS design system
- Real-time streaming responses
- Multi-turn conversation threads
- Function/tool calling with and without user approval
- Structured JSON output extraction
- Persisted conversation storage
- Multimodal image analysis
- Remote MCP (Model Context Protocol) server integration
- Background/async response processing
- Middleware pipeline (logging, function override, content redaction)
- Declarative agent configuration via YAML
- Advanced reasoning with OpenAI o4-mini
- 7 prompting strategy demonstrations (CoT, ToT, GoT, PoT, ReAct, Reflection, Self-Refine)

---

## 🛠 Technology Stack

| Technology | Details |
|---|---|
| **.NET 10 / Blazor Web App** | Server-Side Rendering + Interactive Server components |
| **Microsoft Agent Framework** | `Microsoft.Extensions.AI.Agents` |
| **Bootstrap 5** | Responsive UI framework |
| **OpenAI API (via OpenRouter)** | OpenAI-compatible model access |
| **Semantic Kernel** | Plugin and middleware integration samples |

---

## 📋 Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- An API key from [OpenRouter](https://openrouter.ai/) (supports OpenAI-compatible models)

---

## ⚙️ Configuration

Set your OpenRouter API key using **one** of the following methods:

### Option 1 — `appsettings.json`

Edit `0-Agents/AgentsWebUI/appsettings.json`:

```json
{
  "OpenRouter": {
    "ApiKey": "your-api-key-here"
  }
}
```

### Option 2 — Environment variable

```bash
OPEN_ROUTER_API_KEY=your-api-key-here
```

---

## 🚀 Running the Application

```bash
cd 0-Agents/AgentsWebUI
dotnet run
```

Then open your browser to the URL shown in the terminal output (e.g., `http://localhost:5xxx`).

---

## 📚 Sample Categories

### 🚀 Basics (3 samples)

| Agent | Name | Description |
|-------|------|-------------|
| Agent 01 | Basic Agent | Demonstrates creating and running an agent. Shows simple request/response and streaming response. |
| Agent 02 | Agent with Thread | Multi-turn conversations using threads. The agent remembers context of previous messages. |
| Agent 05 | Structured Output | Extracts structured JSON data from unstructured text. Populates a `PersonInfo` object from natural language. |

---

### 🔧 Functions & Tools (4 samples)

| Agent | Name | Description |
|-------|------|-------------|
| Agent 03 | Agent with Function/Tool | Provides `Add` and `Multiply` functions to an agent to solve math problems. |
| Agent 04 | Functions with User Approval | Adds a user-approval step before any function is executed. |
| Agent 10 | Agent as Function | Uses one agent as a callable tool for another agent. |
| Agent 13 | Plugins | Plugin integration adding external services (weather, current time) via dependency injection. |

---

### 💾 Storage & Persistence (3 samples)

| Agent | Name | Description |
|-------|------|-------------|
| Agent 06 | Persisted Conversation | Serializes a conversation thread to disk and resumes it in a later interaction. |
| Agent 07 | Custom Thread Storage | Uses a custom `VectorChatMessageStore` to persist conversation history in memory. |
| Agent 11 | Chat Reduction | Chat reduction techniques using message store reducers to limit context size. |

---

### 🌐 Multimodal & External Services (3 samples)

| Agent | Name | Description |
|-------|------|-------------|
| Agent 08 | Agent Using Image | Multimodal model analyzes an image from a URL using a text prompt. |
| Agent 09 | Using Remote MCP Server | Connects to the Microsoft Learn MCP server to answer questions about Microsoft technologies. |
| Agent 12 | Background Responses | Asynchronous background responses with response resumption and continuous streaming. |

---

### ⚡ Advanced Features (3 samples)

| Agent | Name | Description |
|-------|------|-------------|
| Agent 14 | Middleware | Middleware for logging, function overriding, and content redaction. |
| Agent 15 | Declarative Agent | Agent configured via a YAML definition with an output schema returning structured JSON. |
| Agent 16 | Reasoning | Advanced reasoning with OpenAI's `o4-mini` model, exposing step-by-step reasoning content. |

---

### 🧠 Prompting Strategies (7 samples)

| Agent | Name | Description |
|-------|------|-------------|
| Agent 17 | Chain-of-Thought (CoT) | Step-by-step reasoning that guides the model to think through problems logically before answering. |
| Agent 18 | Tree of Thoughts (ToT) | Explores multiple reasoning branches in parallel and selects the most promising solution. |
| Agent 19 | Graph of Thoughts (GoT) | Builds a flexible reasoning graph where thoughts can merge, branch, and reference each other. |
| Agent 20 | Program of Thoughts (PoT) | Generates algorithmic pseudocode to separate computation from reasoning. |
| Agent 21 | ReAct | Interleaves reasoning traces with tool actions in a Thought→Action→Observation loop. |
| Agent 22 | Reflection | Generates an initial response then applies self-criticism to identify improvements. |
| Agent 23 | Self-Refine | Iteratively improves output through multiple critique-and-refine rounds. |

---

## 📁 Project Structure

```
agent-framework-sample/
├── 0-Agents/
│   └── AgentsWebUI/
│       ├── Components/
│       │   ├── Layout/          # MainLayout, NavMenu
│       │   └── Pages/           # Home + Agent01-Agent23
│       ├── wwwroot/
│       │   └── app.css          # Central design system CSS
│       ├── Program.cs
│       └── appsettings.json
├── plans/                       # UI design documentation
└── readme.md
```

---

## 📄 License

This project is licensed under the MIT License. See [LICENSE](LICENSE) for details.
