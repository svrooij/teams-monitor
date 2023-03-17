using System.Net.WebSockets;
using System.Text.Json;
using System.Timers;

internal class SocketParser{
    private readonly System.Timers.Timer timer;
    internal static readonly JsonSerializerOptions SerializerOptions = new JsonSerializerOptions{ PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
    private TeamsMeetingUpdate? nextUpdate;
    internal SocketParser() {
        timer = new System.Timers.Timer(1000);
        timer.Elapsed += timerTick;
        timer.AutoReset = true;
        timer.Enabled = true;
    }

    private void timerTick(object? sender, ElapsedEventArgs e) {
        var update = nextUpdate;
        if (update != null && OnUpdate != null) {
            nextUpdate = null;
            OnUpdate.Invoke(sender, update);
        }
    }
    public event EventHandler<TeamsMeetingUpdate>? OnUpdate;
    internal async Task StartReceivingAsync(ClientWebSocket socket, CancellationToken cancellationToken) {
        
        while(!cancellationToken.IsCancellationRequested) {
            var buffer = new ArraySegment<byte>(new byte[1024]);
            WebSocketReceiveResult result;
            using var ms = new MemoryStream();
            do {
                result = await socket.ReceiveAsync(buffer, cancellationToken);
                await ms.WriteAsync(buffer.Take(result.Count).ToArray(), 0, result.Count, cancellationToken);
            } while (!result.EndOfMessage);

            ms.Seek(0, SeekOrigin.Begin);
            var message = await JsonSerializer.DeserializeAsync<TeamsApiResponse>(ms, SerializerOptions, cancellationToken: cancellationToken);
            nextUpdate = message?.MeetingUpdate;
        }
    }
}