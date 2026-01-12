using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CliBridgeMCPServer.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class ApiOnly : ControllerBase
    {
        private readonly ILogger _logger = null;
        public ApiOnly(ILogger<ApiOnly> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Execute a shell command
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="406">Invalid Parameter Value</response>
        /// <response code="400">Invalid Parameter Format</response>
        /// <response code="500">Internal Error</response>
        [HttpGet("ApiEcho")]
        public async Task<string> ApiEcho(string input)
        {
            await Task.Delay(0);
            try
            {
                return $"Return from API Echo: {input}";
            }
            catch (Exception ex)
            {
                _logger?.LogError($"Unknown Exception. Type: {ex.GetType().ToString()} ; Message: {ex.Message} ; Details: {ex.ToString()}");
                return "Not OK";
            }
        }
    }
}
