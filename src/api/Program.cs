using System;
using System.IO;
using System.Reflection;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddRouting(options => options.LowercaseUrls = true);

Assembly entryAssembly = Assembly.GetEntryAssembly();
string version = entryAssembly?.GetName().Version?.ToString() ?? "0.0.0";
builder.Services.AddSwaggerGen(swaggerConfig =>
{
    swaggerConfig.SwaggerDoc("v2", new OpenApiInfo
    {
        Version = version,
        Title = "CLI Bridge MCP Server",
        Description = "HTTP (SSE) MCP Server which bridges commands to a CLI",
        Contact = new OpenApiContact
        {
            Name = "Roger Santos",
            Email = string.Empty,
            Url = new Uri("https://github.com/RogerioDosSantos")
        }
    });
    string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    swaggerConfig.IncludeXmlComments(xmlPath);
});

string dataProtectionKeyDir = Path.Combine(Path.GetTempPath(), "api", "data_protection", "keys");
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(dataProtectionKeyDir));

// MCP Server Endpoints
builder.Services.AddMcpServer(o => {
    o.ServerInfo = new() { Name = "CliBridgeMcpServer", Version = "0.1.0" };
})
    .WithHttpTransport()
    .WithToolsFromAssembly();

var app = builder.Build();

// MCP Server Mapping
app.MapMcp("/mcp");

// Swagger Configuration
app.UseSwagger(c =>
{
    c.PreSerializeFilters.Add((swagger, httpReq) =>
    {
        string host = string.Empty;
        string appServiceHostName = string.Empty;
        if (httpReq.Headers.ContainsKey("x-original-host"))
        {
            host = httpReq.Headers["x-original-host"];
            appServiceHostName = httpReq.Headers["host"];
        }
    });
});

Assembly executingAssembly = Assembly.GetEntryAssembly();
string uiVersion = executingAssembly?.GetName().Version?.ToString() ?? "0.0.0";
app.UseSwaggerUI(swaggerConfig =>
{
    swaggerConfig.SwaggerEndpoint("../swagger/v2/swagger.json", uiVersion);
    swaggerConfig.RoutePrefix = "";
});

app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
