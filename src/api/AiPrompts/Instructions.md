# CLI Bridge MCP Server

This MCP Server allows you to interface with a CLI Executable.

The CLI is assessable though the CLI Tool where the cliArgs is where you pass your command line arguments.

If you call the `--help` argument the CLI Tool will return the help information of the CLI Executable.

Each CLI command has the help option which you can call to have more information about the command.

E.g.: `command_a --help` will provide more information about the `command_a` command.

If you need to execute one command, always call the help option to understand what are the arguments that you need to pass.