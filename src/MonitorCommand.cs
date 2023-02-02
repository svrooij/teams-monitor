using System.CommandLine;
using System.CommandLine.NamingConventionBinder;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

public sealed class MonitorCommand : RootCommand {
    private readonly HttpClient httpClient;
    private MonitorCommandOptions? _options;
    public MonitorCommand() : base("Monitor your Teams status") {
        this.AddArgument(new Argument<string>("teams-token",() => Environment.GetEnvironmentVariable("TEAMS_TOKEN") ?? string.Empty, "Teams local API Token, see: https://support.microsoft.com/en-us/office/connect-third-party-devices-to-teams-aabca9f2-47bb-407f-9f9b-81a104a883d6"));
        // this.AddOption(new Option<string>("--teams-token", "Teams local API token."));
        this.AddOption(new Option<Uri?>("--webhook",() => {
            var webhook = Environment.GetEnvironmentVariable("TEAMS_WEBHOOK");
            return !string.IsNullOrEmpty(webhook) && Uri.TryCreate(webhook, UriKind.Absolute, out var result) ? result : null;
        }, "Webhook URL to post the new status"));
        httpClient = new HttpClient();
        Handler = CommandHandler.Create<MonitorCommandOptions, CancellationToken>(Run);
    }

    private async Task Run(MonitorCommandOptions options, CancellationToken cancellationToken) {
        this._options = options;

        Console.WriteLine("Connecting to Microsoft Teams");
        // Thanks to Martijn Smit https://lostdomain.notion.site/Microsoft-Teams-WebSocket-API-5c042838bc3e4731bdfe679e864ab52a
        var wsUrl = new Uri($"ws://localhost:8124?token={options.TeamsToken}&protocol-version=1.0.0&manufacturer=MuteDeck&device=MuteDeck&app=MuteDeck&app-version=1.4");
        using var socket = new ClientWebSocket();
        await socket.ConnectAsync(wsUrl, cancellationToken);
        Console.WriteLine("Started listening...          CTRL+C to exit");
        
        var parser = new SocketParser();
        parser.OnUpdate += HandleUpdate;
        await parser.StartReceivingAsync(socket, cancellationToken);
    }

    private async void HandleUpdate(object? o, TeamsMeetingUpdate? update) {
        Console.WriteLine("Update [IsInMeeting]: {0}", update?.MeetingState?.IsInMeeting);
        try {
            if (_options?.Webhook != null) {
                Console.WriteLine("--> Sending update to webhook");
                await httpClient.PostAsync(_options.Webhook, new StringContent(JsonSerializer.Serialize(update, SocketParser.SerializerOptions), Encoding.UTF8, "application/json"));
            }
        } catch (Exception e) {
            Console.WriteLine("    Error posting to webhook url, {0}", e.Message);
        }
        
    }
}

class MonitorCommandOptions {
    public string? TeamsToken { get; set; }
    public Uri? Webhook {get;set;}
}