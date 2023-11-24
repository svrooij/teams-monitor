namespace TeamsMonitor.Tray
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            trayIcon = new NotifyIcon(components);
            lbStatus = new Label();
            tbWebhook = new TextBox();
            lbWebhook = new Label();
            label1 = new Label();
            cbRunInBackground = new CheckBox();
            btnRepository = new Button();
            SuspendLayout();
            // 
            // trayIcon
            // 
            trayIcon.Icon = (Icon)resources.GetObject("trayIcon.Icon");
            trayIcon.Text = "Teams Monitor";
            trayIcon.Visible = true;
            // 
            // lbStatus
            // 
            lbStatus.AutoSize = true;
            lbStatus.Location = new Point(12, 9);
            lbStatus.Name = "lbStatus";
            lbStatus.Size = new Size(99, 20);
            lbStatus.TabIndex = 0;
            lbStatus.Text = "Disconnected";
            // 
            // tbWebhook
            // 
            tbWebhook.Location = new Point(159, 49);
            tbWebhook.Name = "tbWebhook";
            tbWebhook.Size = new Size(411, 27);
            tbWebhook.TabIndex = 1;
            // 
            // lbWebhook
            // 
            lbWebhook.AutoSize = true;
            lbWebhook.Location = new Point(12, 52);
            lbWebhook.Name = "lbWebhook";
            lbWebhook.Size = new Size(72, 20);
            lbWebhook.TabIndex = 2;
            lbWebhook.Text = "Webhook";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 89);
            label1.Name = "label1";
            label1.Size = new Size(133, 20);
            label1.TabIndex = 3;
            label1.Text = "Run in background";
            // 
            // cbRunInBackground
            // 
            cbRunInBackground.AutoSize = true;
            cbRunInBackground.Checked = true;
            cbRunInBackground.CheckState = CheckState.Checked;
            cbRunInBackground.Location = new Point(159, 85);
            cbRunInBackground.Name = "cbRunInBackground";
            cbRunInBackground.Size = new Size(188, 24);
            cbRunInBackground.TabIndex = 4;
            cbRunInBackground.Text = "Minimize to tray on exit";
            cbRunInBackground.UseVisualStyleBackColor = true;
            // 
            // btnRepository
            // 
            btnRepository.Location = new Point(159, 179);
            btnRepository.Name = "btnRepository";
            btnRepository.Size = new Size(214, 29);
            btnRepository.TabIndex = 5;
            btnRepository.Text = "Open Repository";
            btnRepository.UseVisualStyleBackColor = true;
            btnRepository.Click += btnRepository_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(582, 253);
            Controls.Add(btnRepository);
            Controls.Add(cbRunInBackground);
            Controls.Add(label1);
            Controls.Add(lbWebhook);
            Controls.Add(tbWebhook);
            Controls.Add(lbStatus);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "MainForm";
            Text = "Ugly Teams Monitor by @svrooij";
            FormClosing += MainForm_FormClosing;
            Load += MainForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private NotifyIcon trayIcon;
        private Label lbStatus;
        private TextBox tbWebhook;
        private Label lbWebhook;
        private Label label1;
        private CheckBox cbRunInBackground;
        private Button btnRepository;
    }
}
