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
        /// <param name="token">Teams token from previous connection</param>
        /// <exception cref="ArgumentNullException"></exception>
        public TeamsSocketOptions(string? token = null)
        {
            //if (string.IsNullOrEmpty(token)) throw new ArgumentNullException(nameof(Token), "Token is required!");
            Token = token ?? Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Port to connect to, default 8124
        /// </summary>
        public int Port { get; set; } = 8124;

        /// <summary>
        /// App name to send to Teams
        /// </summary>
        public string App { get; set; } = " SvR.TeamsMonitor";

        /// <summary>
        /// Device to send to Teams
        /// </summary>
        public string Device { get; set; } = " SvR.TeamsMonitor";

        /// <summary>
        /// Manufacturer to send to Teams
        /// </summary>
        public string Manufacturer { get; set; } = "Stephan";

        /// <summary>
        /// App version to send to Teams
        /// </summary>
        public string AppVersion { get; set; } = "2.0.26";

        /// <summary>
        /// Teams API Token, Settings -> Privacy -> Manage API
        /// </summary>
        /// <seealso href="https://support.microsoft.com/en-us/office/connect-third-party-devices-to-teams-aabca9f2-47bb-407f-9f9b-81a104a883d6"/>
        public string Token { get; set; }

        /// <summary>
        /// Automatically pair with Teams
        /// </summary>
        /// <remarks>
        /// This means it will send the reaction in <see cref="AutoPairReaction"/> to the service, as soon as it notices that the app is able to pair.
        /// </remarks>
        public bool AutoPair { get; set; }

        /// <summary>
        /// Reaction to send to Teams when <see cref="AutoPair"/> is true
        /// </summary>
        public string AutoPairReaction { get; set; } = "like";

        // Thanks to wireshark on 127.0.0.1:8124
        internal Uri SocketUri => new Uri($"ws://localhost:{Port}?token={Token}&protocol-version=2.0.0&manufacturer={Manufacturer}&device={Device}&app={App}&app-version={AppVersion}");
    }
}