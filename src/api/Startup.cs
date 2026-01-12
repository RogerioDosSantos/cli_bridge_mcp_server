using System;
using System.IO;
using System.Reflection;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace CliBridgeMCPServer
{
    public class Startup
    {
        public IConfiguration _config { get; }

        public Startup(IConfiguration configuration)
        {
            _config = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            IMvcBuilder mvcBuilder = services.AddControllers()
                .AddJsonOptions(options =>
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter())
                );

            services.AddRouting(options => options.LowercaseUrls = true);

            Assembly entryAssembly = Assembly.GetEntryAssembly();
            string version = entryAssembly.GetName().Version.ToString() ?? "0.0.0";
            services.AddSwaggerGen(swaggerConfig =>
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

            // Data Protection Setup
            string dataProtectionKeyDir = Path.Combine(Path.GetTempPath(), "api", "data_protection", "keys");
            services.AddDataProtection()
                .PersistKeysToFileSystem(new DirectoryInfo(dataProtectionKeyDir));

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
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
            string version = executingAssembly.GetName().Version.ToString() ?? "0.0.0";
            app.UseSwaggerUI(swaggerConfig =>
            {
                swaggerConfig.SwaggerEndpoint("../swagger/v2/swagger.json", version);
                swaggerConfig.RoutePrefix = "";
            });

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
