using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using ModelContextProtocol.Server;

namespace CliBridgeMCPServer.McpTools
{
    [McpServerToolType]
    public static class CliMcpTool
    {
        [McpServerTool, Description("Echoes the message back to the client.")]
        public static string Echo(string message)
        {
            return $"hello {message}";
        }

        [McpServerTool, Description("Return the AI prompt instructions on how to use CLI Bridge MCP Server")]
        public static string Instructions()
        {
            string assemblyDir = System.AppContext.BaseDirectory;
            string instructionsPath = Path.Combine(assemblyDir, "AiPrompts", "Instructions.md");
            if (!File.Exists(instructionsPath))
            {
                return "Error: Instructions.md file not found in AiPrompts folder.";
            }
            try
            {
                return File.ReadAllText(instructionsPath);
            }
            catch (System.Exception ex)
            {
                return $"Error reading Instructions.md: {ex.Message}";
            }
        }

            [McpServerTool, Description("Execute a command on the CLI")]
        public static string Cli(string cliArgs = "--help")
        {
            string cliName = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows) ? "cli.exe" : "cli";
            string cliDir = System.AppContext.BaseDirectory;
            string cliPath = Path.Combine(cliDir, cliName);
            if (!File.Exists(cliPath))
            {
                string cliRelativeDir = "../../../../../src/cli/bin/Debug/net10.0/";
                cliDir = Path.GetFullPath(Path.Combine(cliDir, cliRelativeDir));
                cliPath = Path.Combine(cliDir, cliName);
            }

            if (!File.Exists(cliPath))
            {
                return $"Error: Executable {cliName} not found";
            }

            try
            {
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = cliPath,
                        Arguments = cliArgs,
                        WorkingDirectory = cliDir,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };

                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();

                return string.IsNullOrWhiteSpace(error) ? output : $"Error: {error}";
            }
            catch (System.Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }
    }
}
