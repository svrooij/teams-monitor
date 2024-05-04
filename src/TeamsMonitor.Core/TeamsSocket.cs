using System.Net.WebSockets;
using System.Text.Json;
using TeamsMonitor.Core.Models;

namespace TeamsMonitor.Core
{
    /// <summary>
    /// TeamsSocket is responsible for connecting to your local Teams Client
    /// </summary>
    public class TeamsSocket : IDisposable
    {
        /// <summary>
        /// JsonSerializerOptions used for communicating with Teams
        /// </summary>
        public static readonly JsonSerializerOptions SerializerOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        private readonly TeamsSocketOptions options;
        private readonly ClientWebSocket webSocket;
        private MeetingUpdate? lastUpdate;
        private int nextRequestId = 0;
        private bool shouldPair;
        private Task? backgroundTask;
        private bool disposedValue;

        /// <summary>
        /// Create new TeamsSocket
        /// </summary>
        /// <param name="options">Set options to connect to Teams</param>
        public TeamsSocket(TeamsSocketOptions options)
        {
            this.options = options;
            this.shouldPair = options.AutoPair;
            webSocket = new ClientWebSocket();
        }

        /// <summary>
        /// Update will fire on every update by Teams
        /// </summary>
        public event EventHandler<MeetingUpdate>? Update;

        /// <summary>
        /// When pairing with Teams, you get a new token to use for the next connection. This event will fire when that happens
        /// </summary>
        public event EventHandler<string>? NewToken;

        /// <summary>
        /// When calling a service, you get a response. This event will fire when that happens
        /// </summary>
        public event EventHandler<ServiceResponse>? ServiceResponse;

        /// <summary>
        /// Give your applause
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<int> ApplaudAsync(CancellationToken cancellationToken) => SendReaction("applause", cancellationToken);

        /// <summary>
        /// Call any service in Teams
        /// </summary>
        /// <param name="action">Action name</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <param name="parameters">Optional parameters</param>
        /// <remarks>Please let me know if you found services or actions not already defined. https://github.com/svrooij/teams-monitor</remarks>
        public async Task<int> CallServiceAsync(string action, CancellationToken cancellationToken, object? parameters = default)
        {
            nextRequestId++;
            var message = new ServiceRequest(action, nextRequestId, parameters);
            using var stream = new MemoryStream();
            await JsonSerializer.SerializeAsync(stream, message, SerializerOptions, cancellationToken);
            stream.Seek(0, SeekOrigin.Begin);
            if (stream.TryGetBuffer(out var buffer))
            {
                await webSocket.SendAsync(buffer, WebSocketMessageType.Text, true, cancellationToken);
            }

            return nextRequestId;
        }

        /// <summary>
        /// Send any reaction to Teams
        /// </summary>
        /// <param name="reaction">Any available reaction `like`, `love`, `applause`, `wow`, `laugh`</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns></returns>
        public async Task<int> SendReaction(string reaction, CancellationToken cancellationToken) => await CallServiceAsync("send-reaction", cancellationToken, new { @Type = reaction });

        /// <summary>
        /// Connect to Teams Client
        /// </summary>
        /// <param name="blocking">Blocking will only return when cancelled, not blocking will start listening in background</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task ConnectAsync(bool blocking, CancellationToken cancellationToken)
        {
            await webSocket.ConnectAsync(options.SocketUri, cancellationToken);
            if (blocking)
                await ReadUntilCancelled(cancellationToken);
            else
                backgroundTask = Task.Run(() => ReadUntilCancelled(cancellationToken), cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Laugh reaction
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<int> LaughAsync(CancellationToken cancellationToken) => SendReaction("laugh", cancellationToken);

        /// <summary>
        /// Leave the current call
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<int> LeaveCallAsync(CancellationToken cancellationToken) => CallServiceAsync("leave-call", cancellationToken);

        /// <summary>
        /// Love reaction
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<int> LoveAsync(CancellationToken cancellationToken) => SendReaction("love", cancellationToken);

        /// <summary>
        /// Like reaction
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<int> ThumbsUpAsync(CancellationToken cancellationToken) => SendReaction("like", cancellationToken);

        /// <summary>
        /// Toggle background blur
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<int> ToggleBackgroundBlurAsync(CancellationToken cancellationToken) => CallServiceAsync("toggle-background-blur", cancellationToken);

        /// <summary>
        /// Toggle mute
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<int> ToggleMuteAsync(CancellationToken cancellationToken) => CallServiceAsync("toggle-mute", cancellationToken);

        /// <summary>
        /// Toggle Raise hand
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<int> ToggleRaiseHandAsync(CancellationToken cancellationToken) => CallServiceAsync("toggle-hand", cancellationToken);

        /// <summary>
        /// Toggle recording
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<int> ToggleRecordingAsync(CancellationToken cancellationToken) => CallServiceAsync("toggle-recording", cancellationToken);

        /// <summary>
        /// Toggle Video
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<int> ToggleVideoAsync(CancellationToken cancellationToken) => CallServiceAsync("toggle-video", cancellationToken);

        /// <summary>
        /// Toggle the Chat UI
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<int> ToggleChatAsync(CancellationToken cancellationToken) => CallServiceAsync("toggle-ui", cancellationToken);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    backgroundTask?.Dispose();
                    webSocket.Dispose();
                }
                disposedValue = true;
            }
        }

        /// <summary>
        /// Emit MeetingUpdate
        /// </summary>
        /// <param name="e">The data got from Teams</param>
        protected virtual void OnUpdate(MeetingUpdate e)
        {
            if (Update is not null)
                Update(this, e);

        }

        /// <summary>
        /// Emit NewToken event
        /// </summary>
        protected virtual void OnNewToken(string? e)
        {
            if (NewToken is not null && !string.IsNullOrWhiteSpace(e))
                NewToken(this, e);
        }

        /// <summary>
        /// Emit ServiceResponse event
        /// </summary>
        protected virtual void OnServiceResponse(ServiceResponse e)
        {
            if (ServiceResponse is not null)
                ServiceResponse(this, e);
        }

        private async Task ReadUntilCancelled(CancellationToken cancellationToken)
        {
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    var buffer = new ArraySegment<byte>(new byte[1024]);
                    WebSocketReceiveResult result;
                    using var ms = new MemoryStream();
                    do
                    {
                        result = await webSocket.ReceiveAsync(buffer, cancellationToken);
                        await ms.WriteAsync(buffer.Take(result.Count).ToArray(), 0, result.Count, cancellationToken);
                    } while (!result.EndOfMessage);

                    ms.Seek(0, SeekOrigin.Begin);
                    var message = await JsonSerializer.DeserializeAsync<TeamsMessage>(ms, SerializerOptions, cancellationToken: cancellationToken);
                    if (message?.MeetingUpdate is not null)
                    {
                        if (!message.MeetingUpdate.Equals(lastUpdate))
                        {
                            OnUpdate(message.MeetingUpdate);
                            lastUpdate = message.MeetingUpdate;
                            if (message.MeetingUpdate.MeetingPermissions?.CanPair is true && this.shouldPair)
                            {
                                await Task.Delay(1000, cancellationToken); // Wait a bit before pairing
                                if (shouldPair)
                                {
                                    //await CallServiceAsync(options.AutoPairAction, cancellationToken);
                                    await SendReaction(this.options.AutoPairReaction, cancellationToken);
                                }
                            }
                        }
                    }

                    if (message?.TokenRefresh is not null)
                    {
                        OnNewToken(message.TokenRefresh);
                        shouldPair = false;
                    }

                    if (message?.RequestId is not null && message?.Response is not null)
                    {
                        OnServiceResponse(new ServiceResponse { RequestId = message.RequestId.Value, Response = message.Response });
                    }

                }
                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Cancelled", CancellationToken.None);
            }
            catch (TaskCanceledException)
            {
                // ignore, it's send by the user
            }
            catch (Exception e)
            {
                Console.WriteLine("Error reading from socket {0}", e.Message);
            }
        }
    }
}
