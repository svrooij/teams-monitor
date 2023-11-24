using Microsoft.Extensions.DependencyInjection;
using System.Text;
using System.Text.Json;
using TeamsMonitor.Core;
using TeamsMonitor.Core.Models;

namespace TeamsMonitor.Tray
{
    public partial class MainForm : Form
    {
        private bool _shouldExitOnClose = false;
        private readonly TeamsSocket _teamsSocket;
        private MeetingUpdate? meetingUpdate;
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        public MainForm()
        {
            InitializeComponent();
            trayIcon.ContextMenuStrip = new ContextMenuStrip();
            trayIcon.ContextMenuStrip.Items.Add("Configure", null, (s, e) => { Show(); });
            trayIcon.ContextMenuStrip.Items.Add("Exit Teams Monitor", null, (s, e) => { _shouldExitOnClose = true; Close(); });

            _teamsSocket = Program.ServiceProvider.GetRequiredService<TeamsSocket>();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_shouldExitOnClose && cbRunInBackground.Checked)
            {
                e.Cancel = true;
                Hide();
                return;
            }
            else
            {
                cancellationTokenSource.Cancel();
            }
        }

        private async void MainForm_Load(object sender, EventArgs e)
        {
            this.tbWebhook.Text = Environment.GetEnvironmentVariable("TEAMS_WEBHOOK");
            _teamsSocket.Update += _teamsSocket_Update;
            await _teamsSocket.ConnectAsync(false, cancellationTokenSource.Token);
            await Task.Delay(500, cancellationTokenSource.Token);
            this.Hide();
        }

        private async void _teamsSocket_Update(object? sender, MeetingUpdate e)
        {
            if (e is not null)
            {
                meetingUpdate = e;
                this.BeginInvoke(() =>
                {
                    if (e.MeetingState is null)
                    {
                        lbStatus.Text = "Will trigger pairing when you join a meeting";
                    }
                    else
                    {
                        lbStatus.Text = e.MeetingState.IsInMeeting == true ? "In a meeting" : "Not in a meeting";
                    }
                });

                if (e.MeetingState is not null && !string.IsNullOrEmpty(this.tbWebhook.Text) && Uri.TryCreate(this.tbWebhook.Text, UriKind.Absolute, out var uri))
                {
                    var client = new HttpClient();
                    var json = JsonSerializer.Serialize(e, TeamsSocket.SerializerOptions);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(uri, content, cancellationTokenSource.Token);
                }

            }

        }

        private void btnRepository_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/svrooij/teams-monitor");
        }
    }
}
