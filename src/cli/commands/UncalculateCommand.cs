using System;
using System.Reflection;
using System.Threading.Tasks;
using CliBridgeMCPServer.Interfaces;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;

namespace CliBridgeMCPServer.Commands
{
    [Command(
        Name = "uncalculate",
        Description = "From a number of 08 digits return a day, month, and year",
        OptionsComparison = System.StringComparison.InvariantCultureIgnoreCase
        )]

    [HelpOption("--help")]
    class UncalculateCommand
    {
        private readonly ILogger<CalculateCommand> _logger = null;
        private IConsole _console = null;

        [Option(
            CommandOptionType.SingleValue, 
            ShortName = "n", 
            LongName = "number", 
            Description = "Number that will be used on the calculation", 
            ValueName = "E.g.: 20221105", 
            ShowInHelpText = true
            )]
        public int Number { get; set; } = 20221105;

        public UncalculateCommand(ILoggerFactory loggerFactory, IConsole console)
        {
            _logger = loggerFactory.CreateLogger<CalculateCommand>();
            _console = console;
        }

        private async Task<int> OnExecute(CommandLineApplication app)
        {
            await Task.Delay(0);
            // Parse Number in YYYYMMDD format
            int year = Number / 10000;
            int month = (Number / 100) % 100;
            int day = Number % 100;
            string result = $"{month:D2}/{day:D2}/{year:D4}";
            _console.WriteLine($"Result = {result}");
            return 0;
        }

    }
}
