using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamsMonitor.Internal;

namespace Microsoft.Extensions.Logging.Console;

internal static class ConsoleLoggerProviderExtensions
{
    public static ILoggerFactory AddConsole(this ILoggerFactory factory, ConsoleLoggerOptions? consoleLoggerOptions = null)
    {
        factory.AddProvider(new ConsoleLoggerProvider(new OptionsMonitor<ConsoleLoggerOptions>(consoleLoggerOptions ?? new ConsoleLoggerOptions())));
        return factory;
    }
}
