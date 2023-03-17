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
        private Task? backgroundTask;
        private bool disposedValue;

        /// <summary>
        /// Create new TeamsSocket
        /// </summary>
        /// <param name="options">Set options to connect to Teams</param>
        public TeamsSocket(TeamsSocketOptions options)
        {
            this.options = options;
            webSocket = new ClientWebSocket();
        }

        /// <summary>
        /// Update will fire on every update by Teams
        /// </summary>
        public event EventHandler<MeetingUpdate>? Update;

        /// <summary>
        /// Give your applause
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task ApplaudAsync(CancellationToken cancellationToken) => CallServiceAsync("call", "react-applause", cancellationToken);

        /// <summary>
        /// Call any service in Teams
        /// </summary>
        /// <param name="service">Service name</param>
        /// <param name="action">Action name</param>
        /// <param name="cancellationToken"></param>
        /// <remarks>Please let me know if you found services or actions not already defined. https://github.com/svrooij/teams-monitor</remarks>
        public async Task CallServiceAsync(string service, string action, CancellationToken cancellationToken)
        {
            var message = new ServiceRequest(service, action);
            using var stream = new MemoryStream();
            await JsonSerializer.SerializeAsync<ServiceRequest>(stream, message, SerializerOptions, cancellationToken);
            stream.Seek(0, SeekOrigin.Begin);
            if (stream.TryGetBuffer(out var buffer))
            {
                await webSocket.SendAsync(buffer, WebSocketMessageType.Text, true, cancellationToken);
            }
        }

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
        public Task LaughAsync(CancellationToken cancellationToken) => CallServiceAsync("call", "react-laugh", cancellationToken);

        /// <summary>
        /// Leave the current call
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task LeaveCallAsync(CancellationToken cancellationToken) => CallServiceAsync("call", "leave-call", cancellationToken);

        /// <summary>
        /// Love reaction
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task LoveAsync(CancellationToken cancellationToken) => CallServiceAsync("call", "react-love", cancellationToken);

        /// <summary>
        /// Like reaction
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task ThumbsUpAsync(CancellationToken cancellationToken) => CallServiceAsync("call", "react-like", cancellationToken);

        /// <summary>
        /// Toggle background blur
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task ToggleBackgroundBlurAsync(CancellationToken cancellationToken) => CallServiceAsync("background-blur", "toggle-background-blur", cancellationToken);

        /// <summary>
        /// Toggle mute
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task ToggleMuteAsync(CancellationToken cancellationToken) => CallServiceAsync("toggle-mute", "toggle-mute", cancellationToken);

        /// <summary>
        /// Toggle Raise hand
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task ToggleRaiseHandAsync(CancellationToken cancellationToken) => CallServiceAsync("raise-hand", "toggle-hand", cancellationToken);

        /// <summary>
        /// Toggle recording
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task ToggleRecordingAsync(CancellationToken cancellationToken) => CallServiceAsync("recording", "toggle-recording", cancellationToken);

        /// <summary>
        /// Toggle Video
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task ToggleVideoAsync(CancellationToken cancellationToken) => CallServiceAsync("toggle-video", "toggle-video", cancellationToken);

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
        /// Emit event
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnUpdate(MeetingUpdate? e)
        {
            //Raise the Tick event (see below for an explanation of this)
            var updateEvent = Update;
            if (updateEvent != null && e is not null)
                updateEvent(this, e);
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
                    OnUpdate(message?.MeetingUpdate);
                }
            }
            catch
            {
            }
        }
    }
}