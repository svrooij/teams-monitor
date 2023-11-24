using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.NamingConventionBinder;
using System.Text;
using System.Text.Json;
using TeamsMonitor.Core;
using TeamsMonitor.Core.Models;

public sealed class MonitorCommand : RootCommand
{
    private readonly HttpClient httpClient;
    private readonly ILoggerProvider loggerProvider;
    private MonitorCommandOptions? _options;
    public MonitorCommand() : base("Monitor your Teams status")
    {
        this.AddOption(new Option<string>("--storage", () => GetDefaultStorageLocation(), "Path to the storage file, default: %APPDATA%\\TeamsMonitor\\t.txt"));
        this.AddOption(new Option<Uri?>("--webhook", () =>
        {
            var webhook = Environment.GetEnvironmentVariable("TEAMS_WEBHOOK");
            return !string.IsNullOrEmpty(webhook) && Uri.TryCreate(webhook, UriKind.Absolute, out var result) ? result : null;
        }, "Webhook URL to post the new status"));
        httpClient = new HttpClient();
        Handler = CommandHandler.Create<InvocationContext, MonitorCommandOptions>(Run);
        loggerProvider = new ConsoleLoggerProvider(new TeamsMonitor.Internal.OptionsMonitor<ConsoleLoggerOptions>(new ConsoleLoggerOptions()));
    }

    private async Task Run(InvocationContext context, MonitorCommandOptions options)
    {
        try
        {
            this._options = options;
            var cancellationToken = context.GetCancellationToken();

            Console.WriteLine("Connecting to Microsoft Teams        CTRL+C to exit");

            var socket = new TeamsSocket(new TeamsSocketOptions() { AutoPair = true, SettingsLocation = _options.Storage }, loggerProvider.CreateLogger(nameof(TeamsSocket)));
            socket.Update += HandleUpdate;
            socket.ServiceResponse += HandleServiceResponse;
            await socket.ConnectAsync(true, cancellationToken);
            Console.WriteLine("Closing application");
            socket.Dispose();

        }
        catch (TaskCanceledException)
        {
            // ignore, it's send by the user
        }
        catch (Exception e)
        {
            Console.WriteLine("Error starting monitor {0}", e.Message);
            context.ExitCode = 100;
        }
    }

    private async void HandleUpdate(object? o, MeetingUpdate? update)
    {
        try
        {
            if (_options?.Webhook != null)
            {
                await httpClient.PostAsync(_options.Webhook, new StringContent(JsonSerializer.Serialize(update, TeamsSocket.SerializerOptions), Encoding.UTF8, "application/json"));
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("    Error posting to webhook url, {0}", e.Message);
        }
    }


    private void HandleServiceResponse(object? o, ServiceResponse response)
    {
        Console.WriteLine("Service response: {0}", response);
    }

    private static string GetDefaultStorageLocation()
    {
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var storagePath = Path.Combine(appData, "TeamsMonitor");
        if (!Directory.Exists(storagePath))
        {
            Directory.CreateDirectory(storagePath);
        }
        return Path.Combine(storagePath, "t.txt");
    }

}

class MonitorCommandOptions
{
    public string? Storage { get; set; }
    public Uri? Webhook { get; set; }
}
