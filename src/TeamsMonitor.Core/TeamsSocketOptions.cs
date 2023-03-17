using static System.Net.WebRequestMethods;

namespace TeamsMonitor.Core
{
    /// <summary>
    /// TeamsSocketOptions
    /// </summary>
    public class TeamsSocketOptions
    {
        /// <summary>
        /// Create new TeamsSocketOptions
        /// </summary>
        /// <param name="token">Teams token from privacy -> Api token</param>
        /// <exception cref="ArgumentNullException"></exception>
        public TeamsSocketOptions(string token)
        {
            if (string.IsNullOrEmpty(token)) throw new ArgumentNullException(nameof(Token), "Token is required!");
            Token = token;
        }

        /// <summary>
        /// Port to connect to, default 8124
        /// </summary>
        public int Port { get; set; } = 8124;

        /// <summary>
        /// App name to send to Teams
        /// </summary>
        public string App { get; set; } = " MuteDeck";

        /// <summary>
        /// Device to send to Teams
        /// </summary>
        public string Device { get; set; } = " MuteDeck";

        /// <summary>
        /// Manufacturer to send to Teams
        /// </summary>
        public string Manufacturer { get; set; } = "MuteDeck";

        /// <summary>
        /// App version to send to Teams
        /// </summary>
        public string AppVersion { get; set; } = "1.4";

        /// <summary>
        /// Teams API Token, Settings -> Privacy -> Manage API
        /// </summary>
        /// <seealso href="https://support.microsoft.com/en-us/office/connect-third-party-devices-to-teams-aabca9f2-47bb-407f-9f9b-81a104a883d6"/>
        public string Token { get; set; }

        // Thanks to Martijn Smit https://lostdomain.notion.site/Microsoft-Teams-WebSocket-API-5c042838bc3e4731bdfe679e864ab52a
        internal Uri SocketUri => new Uri($"ws://localhost:{Port}?token={Token}&protocol-version=1.0.0&manufacturer={Manufacturer}&device={Device}&app={App}&app-version={AppVersion}");
    }
}