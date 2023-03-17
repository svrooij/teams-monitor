using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.NamingConventionBinder;
using System.Text;
using System.Text.Json;
using TeamsMonitor.Core;
using TeamsMonitor.Core.Models;

public sealed class MonitorCommand : RootCommand {
    private readonly HttpClient httpClient;
    private MonitorCommandOptions? _options;
    public MonitorCommand() : base("Monitor your Teams status") {
        this.AddArgument(new Argument<string?>("teams-token",() => Environment.GetEnvironmentVariable("TEAMS_TOKEN") ?? null, "Teams local API Token, see: https://support.microsoft.com/en-us/office/connect-third-party-devices-to-teams-aabca9f2-47bb-407f-9f9b-81a104a883d6"));
        // this.AddOption(new Option<string>("--teams-token", "Teams local API token."));
        this.AddOption(new Option<Uri?>("--webhook",() => {
            var webhook = Environment.GetEnvironmentVariable("TEAMS_WEBHOOK");
            return !string.IsNullOrEmpty(webhook) && Uri.TryCreate(webhook, UriKind.Absolute, out var result) ? result : null;
        }, "Webhook URL to post the new status"));
        httpClient = new HttpClient();
        Handler = CommandHandler.Create<InvocationContext, MonitorCommandOptions, CancellationToken>(Run);
    }

    private async Task Run(InvocationContext context, MonitorCommandOptions options, CancellationToken cancellationToken) {
        try {
            this._options = options;

            Console.WriteLine("Connecting to Microsoft Teams");
            
            var socket = new TeamsSocket(new TeamsSocketOptions(options.TeamsToken));
            socket.Update += HandleUpdate;
            await socket.ConnectAsync(false, cancellationToken);
            Console.WriteLine("Started listening...          CTRL+C to exit");
            while(true)
            {
                Console.ReadLine();
            }

        } catch (Exception e){
            Console.WriteLine("Error starting monitor {0}", e.Message);
            context.ExitCode = 100;
        }
    }

    private async void HandleUpdate(object? o, MeetingUpdate? update) {
        Console.WriteLine("Update [IsInMeeting]: {0}", update?.MeetingState?.IsInMeeting);
        try {
            if (_options?.Webhook != null) {
                Console.WriteLine("--> Sending update to webhook");
                await httpClient.PostAsync(_options.Webhook, new StringContent(JsonSerializer.Serialize(update, TeamsSocket.SerializerOptions), Encoding.UTF8, "application/json"));
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