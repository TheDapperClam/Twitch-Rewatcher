namespace TwitchRewatcher
{
    partial class TwitchRewatcherForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing ) {
            if ( disposing && ( components != null ) ) {
                components.Dispose ();
            }
            base.Dispose ( disposing );
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TwitchRewatcherForm));
            this.videoPlayer = new AxWMPLib.AxWindowsMediaPlayer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.aboutButton = new System.Windows.Forms.Button();
            this.openStreamButton = new System.Windows.Forms.Button();
            this.downloadVodButton = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.soundButton = new System.Windows.Forms.Button();
            this.maxTimeLabel = new System.Windows.Forms.Label();
            this.currentTimeLabel = new System.Windows.Forms.Label();
            this.playButton = new System.Windows.Forms.Button();
            this.playbackTimeTrackBar = new System.Windows.Forms.TrackBar();
            this.playbackCheckTimer = new System.Windows.Forms.Timer(this.components);
            this.chatWebBrowser = new System.Windows.Forms.WebBrowser();
            this.autoSaveTimer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.videoPlayer)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.playbackTimeTrackBar)).BeginInit();
            this.SuspendLayout();
            // 
            // videoPlayer
            // 
            this.videoPlayer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.videoPlayer.Enabled = true;
            this.videoPlayer.Location = new System.Drawing.Point(81, 0);
            this.videoPlayer.Name = "videoPlayer";
            this.videoPlayer.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("videoPlayer.OcxState")));
            this.videoPlayer.Size = new System.Drawing.Size(629, 528);
            this.videoPlayer.TabIndex = 0;
            this.videoPlayer.PlayStateChange += new AxWMPLib._WMPOCXEvents_PlayStateChangeEventHandler(this.videoPlayer_PlayStateChange);
            this.videoPlayer.PositionChange += new AxWMPLib._WMPOCXEvents_PositionChangeEventHandler(this.videoPlayer_PositionChange);
            this.videoPlayer.MediaChange += new AxWMPLib._WMPOCXEvents_MediaChangeEventHandler(this.videoPlayer_MediaChange);
            this.videoPlayer.MouseUpEvent += new AxWMPLib._WMPOCXEvents_MouseUpEventHandler(this.videoPlayer_MouseUpEvent);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.panel1.Controls.Add(this.aboutButton);
            this.panel1.Controls.Add(this.openStreamButton);
            this.panel1.Controls.Add(this.downloadVodButton);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(81, 572);
            this.panel1.TabIndex = 2;
            // 
            // aboutButton
            // 
            this.aboutButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.aboutButton.Location = new System.Drawing.Point(12, 138);
            this.aboutButton.Name = "aboutButton";
            this.aboutButton.Size = new System.Drawing.Size(57, 57);
            this.aboutButton.TabIndex = 3;
            this.aboutButton.Text = "About";
            this.aboutButton.UseVisualStyleBackColor = true;
            this.aboutButton.Click += new System.EventHandler(this.aboutButton_Click);
            // 
            // openStreamButton
            // 
            this.openStreamButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.openStreamButton.Location = new System.Drawing.Point(12, 75);
            this.openStreamButton.Name = "openStreamButton";
            this.openStreamButton.Size = new System.Drawing.Size(57, 57);
            this.openStreamButton.TabIndex = 1;
            this.openStreamButton.Text = "Open Stream";
            this.openStreamButton.UseVisualStyleBackColor = true;
            this.openStreamButton.Click += new System.EventHandler(this.openStreamButton_Click);
            // 
            // downloadVodButton
            // 
            this.downloadVodButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.downloadVodButton.Location = new System.Drawing.Point(12, 12);
            this.downloadVodButton.Name = "downloadVodButton";
            this.downloadVodButton.Size = new System.Drawing.Size(57, 57);
            this.downloadVodButton.TabIndex = 0;
            this.downloadVodButton.Text = "Download VOD";
            this.downloadVodButton.UseVisualStyleBackColor = true;
            this.downloadVodButton.Click += new System.EventHandler(this.downloadVodButton_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Black;
            this.panel2.Controls.Add(this.soundButton);
            this.panel2.Controls.Add(this.maxTimeLabel);
            this.panel2.Controls.Add(this.currentTimeLabel);
            this.panel2.Controls.Add(this.playButton);
            this.panel2.Controls.Add(this.playbackTimeTrackBar);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(81, 528);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(629, 44);
            this.panel2.TabIndex = 3;
            // 
            // soundButton
            // 
            this.soundButton.BackColor = System.Drawing.Color.Transparent;
            this.soundButton.BackgroundImage = global::TwitchRewatcher.Properties.Resources.SoundButton;
            this.soundButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.soundButton.FlatAppearance.BorderSize = 0;
            this.soundButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.soundButton.ForeColor = System.Drawing.Color.White;
            this.soundButton.Location = new System.Drawing.Point(49, 3);
            this.soundButton.Name = "soundButton";
            this.soundButton.Size = new System.Drawing.Size(37, 37);
            this.soundButton.TabIndex = 7;
            this.soundButton.UseVisualStyleBackColor = false;
            // 
            // maxTimeLabel
            // 
            this.maxTimeLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.maxTimeLabel.AutoSize = true;
            this.maxTimeLabel.ForeColor = System.Drawing.Color.White;
            this.maxTimeLabel.Location = new System.Drawing.Point(574, 5);
            this.maxTimeLabel.Name = "maxTimeLabel";
            this.maxTimeLabel.Size = new System.Drawing.Size(49, 13);
            this.maxTimeLabel.TabIndex = 6;
            this.maxTimeLabel.Text = "00:00:00";
            this.maxTimeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // currentTimeLabel
            // 
            this.currentTimeLabel.AutoSize = true;
            this.currentTimeLabel.ForeColor = System.Drawing.Color.White;
            this.currentTimeLabel.Location = new System.Drawing.Point(92, 5);
            this.currentTimeLabel.Name = "currentTimeLabel";
            this.currentTimeLabel.Size = new System.Drawing.Size(49, 13);
            this.currentTimeLabel.TabIndex = 4;
            this.currentTimeLabel.Text = "00:00:00";
            this.currentTimeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // playButton
            // 
            this.playButton.BackColor = System.Drawing.Color.Transparent;
            this.playButton.BackgroundImage = global::TwitchRewatcher.Properties.Resources.PlayButton;
            this.playButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.playButton.FlatAppearance.BorderSize = 0;
            this.playButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.playButton.ForeColor = System.Drawing.Color.White;
            this.playButton.Location = new System.Drawing.Point(6, 3);
            this.playButton.Name = "playButton";
            this.playButton.Size = new System.Drawing.Size(37, 37);
            this.playButton.TabIndex = 4;
            this.playButton.UseVisualStyleBackColor = false;
            this.playButton.Click += new System.EventHandler(this.playButton_Click);
            // 
            // playbackTimeTrackBar
            // 
            this.playbackTimeTrackBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.playbackTimeTrackBar.AutoSize = false;
            this.playbackTimeTrackBar.Location = new System.Drawing.Point(92, 18);
            this.playbackTimeTrackBar.Name = "playbackTimeTrackBar";
            this.playbackTimeTrackBar.Size = new System.Drawing.Size(531, 22);
            this.playbackTimeTrackBar.TabIndex = 5;
            this.playbackTimeTrackBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.playbackTimeTrackBar.Scroll += new System.EventHandler(this.playbackTimeTrackBar_Scroll);
            this.playbackTimeTrackBar.MouseDown += new System.Windows.Forms.MouseEventHandler(this.playbackTimeTrackBar_MouseDown);
            this.playbackTimeTrackBar.MouseUp += new System.Windows.Forms.MouseEventHandler(this.playbackTimeTrackBar_MouseUp);
            // 
            // playbackCheckTimer
            // 
            this.playbackCheckTimer.Enabled = true;
            this.playbackCheckTimer.Tick += new System.EventHandler(this.playbackCheckTimer_Tick);
            // 
            // chatWebBrowser
            // 
            this.chatWebBrowser.AllowWebBrowserDrop = false;
            this.chatWebBrowser.Dock = System.Windows.Forms.DockStyle.Right;
            this.chatWebBrowser.IsWebBrowserContextMenuEnabled = false;
            this.chatWebBrowser.Location = new System.Drawing.Point(710, 0);
            this.chatWebBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.chatWebBrowser.Name = "chatWebBrowser";
            this.chatWebBrowser.Size = new System.Drawing.Size(346, 572);
            this.chatWebBrowser.TabIndex = 5;
            this.chatWebBrowser.Url = new System.Uri("", System.UriKind.Relative);
            this.chatWebBrowser.WebBrowserShortcutsEnabled = false;
            this.chatWebBrowser.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.chatWebBrowser_DocumentCompleted);
            // 
            // autoSaveTimer
            // 
            this.autoSaveTimer.Enabled = true;
            this.autoSaveTimer.Interval = 60000;
            this.autoSaveTimer.Tick += new System.EventHandler(this.autoSaveTimer_Tick);
            // 
            // TwitchRewatcherForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(1056, 572);
            this.Controls.Add(this.videoPlayer);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.chatWebBrowser);
            this.Controls.Add(this.panel1);
            this.DoubleBuffered = true;
            this.Name = "TwitchRewatcherForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Twitch Rewatcher";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TwitchRewatcherForm_FormClosing);
            this.Load += new System.EventHandler(this.TwitchRewatcherForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.videoPlayer)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.playbackTimeTrackBar)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private AxWMPLib.AxWindowsMediaPlayer videoPlayer;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button playButton;
        private System.Windows.Forms.TrackBar playbackTimeTrackBar;
        private System.Windows.Forms.Label maxTimeLabel;
        private System.Windows.Forms.Label currentTimeLabel;
        private System.Windows.Forms.Timer playbackCheckTimer;
        private System.Windows.Forms.Button soundButton;
        private System.Windows.Forms.WebBrowser chatWebBrowser;
        private System.Windows.Forms.Button openStreamButton;
        private System.Windows.Forms.Button downloadVodButton;
        private System.Windows.Forms.Timer autoSaveTimer;
        private System.Windows.Forms.Button aboutButton;
    }
}

