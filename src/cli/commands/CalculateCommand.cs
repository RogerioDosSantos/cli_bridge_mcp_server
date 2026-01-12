using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;

namespace CliBridgeMCPServer.Commands
{
    [Command(
        Name = "calculate",
        Description = "Execute a calculation based on day, month, year",
        OptionsComparison = System.StringComparison.InvariantCultureIgnoreCase
        )]

    [HelpOption("--help")]
    class CalculateCommand
    {
        private readonly ILogger<CalculateCommand> _logger = null;
        private IConsole _console = null;

        [Option(
            CommandOptionType.SingleValue, 
            ShortName = "d", 
            LongName = "day", 
            Description = "day of the month", 
            ValueName = "E.g.: 19", 
            ShowInHelpText = true
            )]
        public int DayInput { get; set; } = 1;

        [Option(
            CommandOptionType.SingleValue,
            ShortName = "m",
            LongName = "month",
            Description = "month of the year",
            ValueName = "E.g.: 7",
            ShowInHelpText = true
            )]
        public int MonthInput { get; set; } = 1;

        [Option(
            CommandOptionType.SingleValue,
            ShortName = "y",
            LongName = "year",
            Description = "year in 04 digits",
            ValueName = "E.g.: 2022",
            ShowInHelpText = true
            )]
        public int YearInput { get; set; } = 2000;

        public CalculateCommand(ILoggerFactory loggerFactory, IConsole console)
        {
            _logger = loggerFactory.CreateLogger<CalculateCommand>();
            _console = console;
        }

        private async Task<int> OnExecute(CommandLineApplication app)
        {
            await Task.Delay(0);
            int result = (YearInput * 10000) + (MonthInput * 100) + DayInput;
            _console.WriteLine($"Result = {result}");
            return 0;
        }

    }
}
