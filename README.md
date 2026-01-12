# CLI Bridge MCP Server

This project created a CLI Bridge HTTP (SSE) MCP Server.

The MCP Server interfaces will be exposed though a .Net Core Web API. 

Commands will be received in the Controller endpoints and the results will be sent back to the clients using Server-Sent Events (SSE).

The `ModelContextProtocol.AspNetCore` NuGet package is used to implement the MCP Server.

## How to Compile

From this folder execute:

```shell
docker-compose -f ./build/docker-compose.yaml build
```

## How to Run

From this folder execute:

```shell
docker-compose -f ./build/docker-compose.yaml up -d
```

Than you can connect in the following URL. The Swagger documentation will be displayed:

[http://localhost:8000](http://localhost:8000)

The Swagger documentation contains the description of the available endpoints.

## How to Stop

From this folder execute:
```shell	
docker-compose -f ./build/docker-compose.yaml down
```

# How to Test the MCP Server Functionality

## Using MCP Inspector

The MCP Inspector is an interactive CLI UI that lets you connect to your server, list tools/resources/prompts, and invoke them with custom inputs—great to validate capability negotiation and error handling.

- Launch Inspector and connect
```shell
npx @modelcontextprotocol/inspector
```

- Choose HTTP (Streamable HTTP) as transport.
- Enter your server URL: http://localhost:5000/mcp
- Use the Tools, Resources, and Prompts tabs to list/inspect and run items

## Using Visual Studio (Agent mode)

This is ideal to validate end‑to‑end experience with GitHub Copilot agent tools selection, auth flows, and dynamic tool discovery.

- Ensure VS 2022 v17.14+ (or newer servicing with MCP features).
- Create .mcp.json (solution root or user profile) and point to your ASP.NET Core MCP server:

```json
{
  "servers": {
    "myMcpServer": {
      "type": "http",
      "url": "http://localhost:5080/mcp"
    }
  }
}
```

Visual Studio detects the server and surfaces CodeLens controls to start/connect and authenticate (OAuth supported for remote servers). 
Then in Copilot Chat → Agent mode, enable the server tools and run them.

## Visual Studio Code (Agent mode)

- Use latest VS Code with Copilot; MCP support is generally available from 1.102.
- Add .vscode/mcp.json:

```json
{
  "servers": {
    "myMcpServer": { "type": "http", "url": "http://localhost:5080/mcp" }
  }
}
```

Switch to Agent mode, enable your server in the Tools picker, and invoke tools.

## Using Curl

You can use curl to test the MCP server endpoints. For example, to list the available tools, you can run:
```shell
curl http://localhost:5000/mcp/tools
```
To invoke a specific tool, you can use the following command:
```shell
curl -X POST http://localhost:5000/mcp/tools/{toolId}/invoke -
     -H "Content-Type: application/json" \
     -d '{"input": "your input here"}'
```
