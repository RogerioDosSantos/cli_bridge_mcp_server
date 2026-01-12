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

## How to Stop

From this folder execute:
```shell	
docker-compose -f ./build/docker-compose.yaml down
```

