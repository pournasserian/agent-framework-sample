# Agentic AI Patterns (Comprehensive Guide)

This document lists the major **Agentic AI design patterns** used in
modern LLM-based systems such as Microsoft Agent Framework, LangGraph,
AutoGen, Semantic Kernel, and other agent orchestration platforms.

------------------------------------------------------------------------

# 1. Reasoning Patterns

## Chain-of-Thought (CoT)

Step-by-step reasoning before producing an answer.

    Problem → reasoning steps → answer

Used for: - math reasoning - complex logic - multi-step tasks

------------------------------------------------------------------------

## Self-Consistency

Generate multiple reasoning paths and select the most consistent result.

    Prompt → multiple CoT outputs → voting → final answer

------------------------------------------------------------------------

## Tree of Thoughts (ToT)

Explores multiple reasoning branches similar to a search tree.

    Root
     ├─ Thought A
     │   ├─ A1
     │   └─ A2
     └─ Thought B
         ├─ B1
         └─ B2

Useful for planning and strategy problems.

------------------------------------------------------------------------

## Graph of Thoughts (GoT)

A generalization of Tree-of-Thought where reasoning paths form a
**graph** instead of a strict tree, allowing merging and revisiting
states.

------------------------------------------------------------------------

## Program of Thoughts (PoT)

The model generates **executable code** instead of natural language
reasoning.

    LLM → Python code → execution → result

Often used for: - mathematical reasoning - data analysis

------------------------------------------------------------------------

## ReAct (Reason + Act)

Alternates between reasoning and tool usage.

    Thought → Action → Observation → Thought → Action → Answer

Very common in tool-enabled agents.

------------------------------------------------------------------------

## Reflection / Self-Critique

The agent evaluates its own output and improves it.

    Generate → Critique → Revise

Used frequently for: - coding agents - QA systems - writing assistants

------------------------------------------------------------------------

## Self-Refine

Iterative self-improvement without external feedback.

    draft → critique → improved draft → final

------------------------------------------------------------------------

# 2. Planning Patterns

## Plan-and-Execute

Agent creates a plan first, then executes it.

    User Goal
       ↓
    Planner
       ↓
    Step1 → Step2 → Step3

------------------------------------------------------------------------

## Replanning

Agent updates its plan dynamically based on new observations.

------------------------------------------------------------------------

## Task Decomposition

Breaks large tasks into smaller subtasks.

    Build website
     ├─ design UI
     ├─ create API
     └─ deploy

------------------------------------------------------------------------

## Hierarchical Planning

Multiple planning levels exist.

    High-level planner
         ↓
    Sub planners
         ↓
    Workers

------------------------------------------------------------------------

# 3. Tool Use Patterns

## Toolformer Pattern

The model inserts tool calls inside reasoning.

    LLM → tool call → tool result → reasoning

------------------------------------------------------------------------

## Function Calling Pattern

Structured invocation of tools using JSON schemas.

    LLM → JSON function call → tool → response

------------------------------------------------------------------------

## Tool Selection Pattern

Agent selects the most appropriate tool among several available tools.

------------------------------------------------------------------------

## Tool-Augmented Generation

Combines LLM reasoning with external tools or APIs.

Example:

    LLM → search → retrieve → answer

------------------------------------------------------------------------

# 4. Memory Patterns

## Short-Term Memory

Conversation context stored within the prompt window.

------------------------------------------------------------------------

## Long-Term Memory

External storage such as:

-   Pinecone
-   Qdrant
-   Neo4j
-   SQLite vector

------------------------------------------------------------------------

## Episodic Memory

Stores past agent experiences.

------------------------------------------------------------------------

## Semantic Memory

Structured knowledge about entities and relationships.

------------------------------------------------------------------------

## Memory Reflection

Agent summarizes previous experiences and stores insights.

------------------------------------------------------------------------

# 5. Multi-Agent Collaboration Patterns

## Role-Based Agents

Each agent has a defined role.

Example:

    Architect Agent
    Coder Agent
    QA Agent
    Security Agent

------------------------------------------------------------------------

## Supervisor Pattern

A coordinator agent manages worker agents.

    Supervisor
     ├─ Worker 1
     ├─ Worker 2
     └─ Worker 3

------------------------------------------------------------------------

## Debate Pattern

Agents argue different perspectives and a judge selects the best answer.

    Agent A → argument
    Agent B → counter argument
    Judge → decision

------------------------------------------------------------------------

## Consensus Pattern

Multiple agents vote for the best result.

------------------------------------------------------------------------

## Specialist Swarm

A swarm of specialized agents collaborate to solve a task.

------------------------------------------------------------------------

## Blackboard Pattern

Agents communicate through a shared memory board.

------------------------------------------------------------------------

# 6. Execution Patterns

## Agent Loop

The core runtime loop for most agents.

    while not done:
        think
        act
        observe

------------------------------------------------------------------------

## Event-Driven Agents

Agents respond to system events.

Example:

    GitHub PR → review agent
    Security alert → security agent

------------------------------------------------------------------------

## Workflow Agents

Agents operate inside predefined workflow graphs.

------------------------------------------------------------------------

# 7. Verification Patterns

## Critic Pattern

A second agent verifies outputs.

    Generator → Critic → Fix

------------------------------------------------------------------------

## Guardrail Pattern

Safety and validation layers.

Examples: - hallucination checks - policy enforcement

------------------------------------------------------------------------

## Verification Agent

Fact-checking agents that validate outputs using external sources.

------------------------------------------------------------------------

# 8. Learning and Adaptation Patterns

## Reinforcement Learning Agents

Agents improve based on rewards.

------------------------------------------------------------------------

## Experience Replay

Agents learn from stored past experiences.

------------------------------------------------------------------------

## Feedback Learning

Agents improve based on human or automated feedback.

------------------------------------------------------------------------

# 9. Retrieval Patterns

## Retrieval-Augmented Generation (RAG)

    query → retrieve → augment → answer

------------------------------------------------------------------------

## GraphRAG

Retrieval based on knowledge graphs.

------------------------------------------------------------------------

## Hybrid Retrieval

Combination of vector search and keyword search.

------------------------------------------------------------------------

## Iterative Retrieval

Agents repeatedly retrieve additional knowledge to refine answers.

------------------------------------------------------------------------

# 10. Interaction Patterns

## Conversational Agents

Chat-based interaction with users.

------------------------------------------------------------------------

## Autonomous Agents

Agents operate independently without continuous human input.

------------------------------------------------------------------------

## Human-in-the-Loop

Human approval is required before executing critical steps.

    Agent plan → human approve → execution

------------------------------------------------------------------------

# 11. Emerging Advanced Patterns

## Agent-to-Agent Communication (A2A)

Agents communicate directly with each other.

------------------------------------------------------------------------

## MCP Pattern (Model Context Protocol)

Standard protocol enabling tools and AI agents to interoperate.

------------------------------------------------------------------------

## Reflection Loops

Continuous improvement cycles through repeated evaluation.

------------------------------------------------------------------------

## Simulation Pattern

Agents simulate outcomes before executing real-world actions.

------------------------------------------------------------------------

## Environment Interaction

Agents interact with simulated or real environments.

------------------------------------------------------------------------

# Summary

Major categories of agentic AI patterns:

-   Reasoning
-   Planning
-   Tool Use
-   Memory
-   Multi-Agent Collaboration
-   Execution
-   Verification
-   Learning
-   Retrieval
-   Interaction

These patterns form the **foundation of modern agentic AI
architectures**.
