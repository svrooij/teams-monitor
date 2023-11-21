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
    private TeamsMonitorOptions? _monitorOptions;
    public MonitorCommand() : base("Monitor your Teams status")
    {
        this.AddOption(new Option<string>("--storage", () => GetDefaultStorageLocation(), "Path to the storage file, default: %APPDATA%\\TeamsMonitor\\storage.json"));
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
            this._monitorOptions = await LoadOptionsFromPath(options.Storage);

            var cancellationToken = context.GetCancellationToken();

            Console.WriteLine("Connecting to Microsoft Teams        CTRL+C to exit");

            var socket = new TeamsSocket(new TeamsSocketOptions(_monitorOptions?.Token) { AutoPair = true }, loggerProvider.CreateLogger(nameof(TeamsSocket)));
            socket.Update += HandleUpdate;
            socket.NewToken += HandleNewToken;
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
        //Console.WriteLine("Update: {0}", JsonSerializer.Serialize(update, TeamsSocket.SerializerOptions));
        try
        {
            if (_options?.Webhook != null)
            {
                //Console.WriteLine("--> Sending update to webhook");
                await httpClient.PostAsync(_options.Webhook, new StringContent(JsonSerializer.Serialize(update, TeamsSocket.SerializerOptions), Encoding.UTF8, "application/json"));
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("    Error posting to webhook url, {0}", e.Message);
        }
    }

    private async void HandleNewToken(object? o, string token)
    {
        Console.WriteLine("New token: {0}", token);
        if (_monitorOptions != null)
        {
            _monitorOptions.Token = token;

        }
        else
        {
            _monitorOptions = new TeamsMonitorOptions { Token = token };
        }
        await SaveOptionsToPath(_options!.Storage!, _monitorOptions);
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
        return Path.Combine(storagePath, "storage.json");
    }

    private static async Task<TeamsMonitorOptions?> LoadOptionsFromPath(string? path)
    {
        if (path is not null && File.Exists(path))
        {
            using var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, FileOptions.Asynchronous);
            var options = await JsonSerializer.DeserializeAsync<TeamsMonitorOptions>(stream, TeamsSocket.SerializerOptions);
            if (!string.IsNullOrEmpty(options?.Token))
            {
                return options;
            }

        }
        return null;
    }

    private static async Task SaveOptionsToPath(string path, TeamsMonitorOptions options)
    {
        using var stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None, 4096, FileOptions.Asynchronous);
        await JsonSerializer.SerializeAsync(stream, options, TeamsSocket.SerializerOptions);
    }
}

class MonitorCommandOptions
{
    public string? Storage { get; set; }
    public Uri? Webhook { get; set; }
}

public class TeamsMonitorOptions
{
    public string? Token { get; set; }
}
