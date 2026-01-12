using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using ModelContextProtocol.Server;

namespace CliBridgeMCPServer.McpTools
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    [McpServerToolType]
    public class ApiAndMcp : ControllerBase
    {
        [HttpGet("ApiAndMcpEcho")]
        [McpServerTool, Description("Echo the input")]
        public string ApiAndMcpEcho(string input)
        {
            return $"Return from API and MCP Echo: {input}";
        }
    }
}
