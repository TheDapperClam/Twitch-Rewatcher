using CefSharp;
using CefSharp.WinForms;
using CefSharp.SchemeHandler;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace TwitchRewatcher {
    public enum VideoMode {
        Normal = 0,
        Theater,
    }

    public partial class TwitchRewatcherForm : Form, IMessageFilter
    {
        public delegate void MouseMovedEvent ();
        public event MouseMovedEvent GlobalMouseMove;

        public const string VIDEO_FILE_NAME = "video.mp4";
        public const string CHAT_FILE_NAME = "chat.dtc";
        public const string CONFIG_FILE_NAME = "config.cfg";

        private const int WM_MOUSEMOVE = 0x0200;
        private const double PLAYBACK_TIME_TRAVEL_AMOUNT = 10;
        private const string CHAT_PAGE_NAME = "chat.htm";
        private const string CHAT_LIST_ID = "ChatList";
        private readonly string WEB_CONTENT_LOCATION = Directory.GetCurrentDirectory () + "\\Web";

        private ChromiumWebBrowser chatWebBrowser;

        private ChatObject[] chatMessages;
        private int chatIndex;

        private VideoMode currentVideoMode;
        private Rectangle oldBounds;
        private bool isMaximized;
        private bool optionsPanelVisible = true;
        private bool chatVisible = true;
        private bool showRunningInBackgroundNotification = true;

        private Point oldCursorPosition;

        private bool minimizeOnClose = true;

        private StreamConfig currentStreamConfig;
        private string currentStreamPath;
        private string currentVideoPath;
        private string currentChatPath;

        private double bufferedPlaybackTime = 0;
        private double startingTime = 0;

        private DownloadVodForm downloadForm = new DownloadVodForm ();

        private bool cursorVisible = true;
        private bool CursorVisible  {
            get { return cursorVisible; }
            set {
                if ( value == cursorVisible )
                    return;

                if ( value )
                    Cursor.Show ();
                else
                    Cursor.Hide ();

                cursorVisible = value;
            }
        }

        private void AddChatObject ( ChatObject obj ) {
            if ( chatWebBrowser == null )
                return;

            if ( chatWebBrowser.IsLoading )
                return;

            if ( obj == null )
                return;

            string addMessageScript = string.Format ( "document.getElementById('{0}').innerHTML += \"{1}\"", CHAT_LIST_ID, obj.Message.Body.ToString () );
            string scrollToBottomScript = "window.scrollTo(0, document.body.scrollHeight)";
            chatWebBrowser.ExecuteScriptAsync ( addMessageScript );
            chatWebBrowser.ExecuteScriptAsync ( scrollToBottomScript );
        }

        private void ClearChat () {
            string script = string.Format ( "document.getElementById('{0}').innerHTML = ''", CHAT_LIST_ID );
            chatWebBrowser.ExecuteScriptAsync ( script );
        }

        private double GetPlaybackTime () {
            return videoPlayer.Ctlcontrols.currentPosition;
        }

        private void InitializeChatWebBrowser () {
            CefSettings settings = new CefSettings ();
            settings.RegisterScheme ( new CefCustomScheme { 
                SchemeName = "localfolder",
                DomainName = "cefsharp",
                SchemeHandlerFactory = new FolderSchemeHandlerFactory (
                    rootFolder: WEB_CONTENT_LOCATION,
                    hostName: "cefsharp",
                    defaultPage: CHAT_PAGE_NAME
                )
            } );

            Cef.Initialize ( settings );
            chatWebBrowser = new ChromiumWebBrowser ( "localfolder://cefsharp/" );
            Controls.Add ( chatWebBrowser );

            chatWebBrowser.Dock = DockStyle.Right;
            chatWebBrowser.Width = 340;
        }

        private bool IsPlaying () {
            return videoPlayer.playState == WMPLib.WMPPlayState.wmppsPlaying;
        }

        private bool IsVideoLoaded () {
            return videoPlayer.currentMedia != null;
        }

        private void LoadChat ( string path ) {
            if ( !File.Exists ( path ) )
                return;

            currentChatPath = path;
            chatMessages = ChatLoader.Load ( path );
        }

        private void LoadStreamConfig ( string path ) {
            currentStreamConfig = StreamConfigManager.LoadConfig ( path );

            if ( currentStreamConfig == null )
                return;

            startingTime = Math.Max ( currentStreamConfig.PlaybackTime - PLAYBACK_TIME_TRAVEL_AMOUNT, 0 );
            bufferedPlaybackTime = startingTime;
            SetPlaybackTime ( bufferedPlaybackTime );
        }

        private void LoadVideo ( string path ) {
            if ( !File.Exists ( path ) )
                return;

            currentVideoPath = path;
            videoPlayer.URL = path;
            SetPlaybackTime ( 0 );
        }

        private void MinimizeToTray () {
            Hide ();

            if ( showRunningInBackgroundNotification )
                notifyIcon.ShowBalloonTip ( 10000 );
            showRunningInBackgroundNotification = false;
        }

        private void OnGlobalMouseMove () {
            if ( ActiveForm != this )
                return;

            if ( oldCursorPosition == Cursor.Position )
                return;

            oldCursorPosition = Cursor.Position;
            SetMouseMoveControlVisibility ( true );
        }

        public bool PreFilterMessage ( ref Message m ) {
            if ( m.Msg == WM_MOUSEMOVE && GlobalMouseMove != null )
                GlobalMouseMove ();
            return false;
        }

        private void RemoveFromTray () {
            Show ();
            BringToFront ();
            SetPauseState ( false );
        }

        private void ResetChat () {
            if ( chatMessages == null )
                return;

            for ( int i = 0; i < chatMessages.Length; i++ ) {
                if ( GetPlaybackTime () > chatMessages[ i ].PostTime )
                    continue;

                chatIndex = i;
                break;
            }

            ClearChat ();
        }

        private void SaveCurrentConfig () {
            if ( !IsVideoLoaded () )
                return;

            if ( currentStreamConfig == null )
                currentStreamConfig = new StreamConfig ();

            currentStreamConfig.PlaybackTime = GetPlaybackTime ();
            StreamConfigManager.SaveConfig ( currentStreamPath, currentStreamConfig );
        }

        private void SetChatVisibility ( bool visible ) {
            chatVisible = visible;
            chatWebBrowser.Visible = chatVisible;
            toggleChatButton.Text = visible ? ">" : "<";
        }

        private void SetMuteState ( bool muted ) {
            if ( !IsVideoLoaded () )
                return;

            videoPlayer.settings.mute = muted;
            soundButton.BackgroundImage = muted ? Properties.Resources.SoundButtonMute : Properties.Resources.SoundButton;
        }

        private void SetOptionsPanelVisibility ( bool visible ) {
            optionsPanelVisible = visible;
            optionsPanel.Visible = optionsPanelVisible;
        }

        private void SetPauseState ( bool paused ) {
            if ( paused )
                videoPlayer.Ctlcontrols.pause ();
            else
                videoPlayer.Ctlcontrols.play ();
        }

        private void SetPlaybackTimeLabel ( int seconds ) {
            TimeSpan t = TimeSpan.FromSeconds ( seconds );
            currentTimeLabel.Text = t.ToString ( @"hh\:mm\:ss" );
        }

        private void SetPlaybackTime ( double time ) {
            if ( !IsVideoLoaded () )
                return;

            if ( time < 0 )
                return;

            if ( time > videoPlayer.currentMedia.duration )
                return;

            videoPlayer.Ctlcontrols.currentPosition = time;
            SetPauseState ( false );
            ResetChat ();
        }

        private void SetPlaybackTrackBar ( int value ) {
            if ( value < playbackTimeTrackBar.Minimum || value > playbackTimeTrackBar.Maximum )
                return;

            playbackTimeTrackBar.Value = value;
        }

        private void SetMouseMoveControlVisibility ( bool visible ) {
            if ( !visible && ActiveForm != this )
                return;

            if ( !visible && !IsPlaying () )
                return;

            controlPanel.Visible = visible;
            toggleChatButton.Visible = visible;
            CursorVisible = visible;
            SetOptionsPanelVisibility ( visible );

            if ( visible ) {
                hideMouseMoveControlsTimer.Enabled = true;
                hideMouseMoveControlsTimer.Stop ();
                hideMouseMoveControlsTimer.Start ();
            }
        }

        private void SetVideoMode ( VideoMode mode ) {
            currentVideoMode = mode;
            switch ( currentVideoMode ) {
                case VideoMode.Normal:
                    FormBorderStyle = FormBorderStyle.Sizable;
                    WindowState = isMaximized ? FormWindowState.Maximized : FormWindowState.Normal;
                    Bounds = oldBounds;
                    break;
                case VideoMode.Theater:
                    isMaximized = WindowState == FormWindowState.Maximized;
                    oldBounds = RestoreBounds;
                    FormBorderStyle = FormBorderStyle.None;
                    WindowState = FormWindowState.Normal;
                    Bounds = Screen.FromControl ( this ).Bounds;
                    SetMouseMoveControlVisibility ( false );
                    break;
            }
        }

        private void UpdateChatMessages() {
            if ( chatMessages == null )
                return;

            if ( chatIndex < 0 || chatIndex >= chatMessages.Length )
                return;

            ChatObject newMessage = chatMessages[ chatIndex ];

            if ( GetPlaybackTime () < newMessage.PostTime )
                return;

            AddChatObject ( newMessage );
            chatIndex++;
        }

        private void UpdateVideoControls () {
            int playbackTime = (int) GetPlaybackTime ();
            SetPlaybackTimeLabel ( playbackTime );
            SetPlaybackTrackBar ( playbackTime );
        }

        public TwitchRewatcherForm() {
            InitializeComponent ();
            InitializeChatWebBrowser ();
            GlobalMouseMove += new MouseMovedEvent ( OnGlobalMouseMove );
            Application.AddMessageFilter ( this );
        }

        private void videoPlayer_PlayStateChange( object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e ) {
            playButton.BackgroundImage = IsPlaying () ? Properties.Resources.PauseButton : Properties.Resources.PlayButton;
        }

        private void playButton_Click( object sender, EventArgs e ) {
            if ( videoPlayer.Ctlcontrols.currentPosition == 0.0f )
                ResetChat ();

            SetPauseState ( IsPlaying () );
        }

        private void videoPlayer_PositionChange( object sender, AxWMPLib._WMPOCXEvents_PositionChangeEvent e ) {
            UpdateVideoControls ();
        }

        private void videoPlayer_MediaChange( object sender, AxWMPLib._WMPOCXEvents_MediaChangeEvent e ) {
            bool isMediaAvailable = IsVideoLoaded ();
            double maxDurationDouble = isMediaAvailable ? videoPlayer.currentMedia.duration : 0;
            string maxDurationString = isMediaAvailable ? videoPlayer.currentMedia.durationString : "00:00:00";

            maxTimeLabel.Text = maxDurationString;
            playbackTimeTrackBar.Maximum = (int) maxDurationDouble;
        }

        private void playbackTimeTrackBar_MouseUp( object sender, MouseEventArgs e ) {
            SetPlaybackTime ( playbackTimeTrackBar.Value );
            playbackCheckTimer.Enabled = true;
        }

        private void videoPlayer_MouseUpEvent( object sender, AxWMPLib._WMPOCXEvents_MouseUpEvent e ) {
            SetPauseState ( IsPlaying () );
        }

        private void playbackCheckTimer_Tick( object sender, EventArgs e ) {
            if ( IsVideoLoaded () && GetPlaybackTime () < bufferedPlaybackTime )
                SetPlaybackTime ( bufferedPlaybackTime );
            else
                bufferedPlaybackTime = 0;

            UpdateVideoControls ();
            UpdateChatMessages ();
        }

        private void playbackTimeTrackBar_Scroll( object sender, EventArgs e ) {
            SetPlaybackTimeLabel ( playbackTimeTrackBar.Value );
        }

        private void playbackTimeTrackBar_MouseDown( object sender, MouseEventArgs e ) {
            playbackCheckTimer.Enabled = false;
        }

        private void TwitchRewatcherForm_Load( object sender, EventArgs e ) {
            oldCursorPosition = Cursor.Position;
            playbackTimeTrackBar.MouseWheel += (a, b) => ( (HandledMouseEventArgs) b ).Handled = true;
            volumeTrackBar.Value = videoPlayer.settings.volume;
        }

        private void TwitchRewatcherForm_FormClosing( object sender, FormClosingEventArgs e ) {
            if ( minimizeOnClose ) {
                e.Cancel = true;
                MinimizeToTray ();
                return;
            }

            SaveCurrentConfig ();

            if ( VodDownloader.IsDownloadingChat || VodDownloader.IsDownloadingVideo ) {
                DialogResult r = MessageBox.Show ( "You are currently downloading a VOD; quitting will result in the download being cancelled. Would you like to quit?",
                    "VOD Downloading",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning );

                if ( r == DialogResult.No ) {
                    e.Cancel = true;
                    minimizeOnClose = true;
                    return;
                }
            }

            Cef.Shutdown ();
            VodDownloader.KillDownloads ();
            notifyIcon.Visible = false;
            Program.Close ();
        }

        private void downloadVodButton_Click ( object sender, EventArgs e ) {
            if ( downloadForm == null )
                downloadForm = new DownloadVodForm ();

            downloadForm.Show ();
        }

        private void openStreamButton_Click ( object sender, EventArgs e ) {
            using ( CommonOpenFileDialog dialog = new CommonOpenFileDialog () ) {
                dialog.IsFolderPicker = true;
                Focus ();

                if ( dialog.ShowDialog () != CommonFileDialogResult.Ok )
                    return;

                currentStreamPath = dialog.FileName;
                string video = Path.Combine ( currentStreamPath, VIDEO_FILE_NAME );
                string chat = Path.Combine ( currentStreamPath, CHAT_FILE_NAME );
                string config = Path.Combine ( currentStreamPath, CONFIG_FILE_NAME );
                SaveCurrentConfig ();
                ClearChat ();
                LoadVideo ( video );
                LoadChat ( chat );
                LoadStreamConfig ( config );
            }
        }

        private void autoSaveTimer_Tick ( object sender, EventArgs e ) {
            SaveCurrentConfig ();
        }

        private void aboutButton_Click ( object sender, EventArgs e ) {
            using ( AboutForm form = new AboutForm () ) {
                form.ShowDialog ();
            }
        }

        private void notifyIcon_MouseDoubleClick ( object sender, MouseEventArgs e ) {
            RemoveFromTray ();
        }

        private void exitToolStripMenuItem_Click ( object sender, EventArgs e ) {
            minimizeOnClose = false;
            Close ();
        }

        private void showToolStripMenuItem_Click ( object sender, EventArgs e ) {
            RemoveFromTray ();
        }

        private void notifyIcon_MouseMove ( object sender, MouseEventArgs e ) {
            notifyIcon.Text = "Twitch Rewatcher" + ( VodDownloader.IsDownloading ? " " + Math.Floor ( VodDownloader.TotalDownloadProgress () ) + "%" : "" );
        }

        private void volumeTrackBar_Scroll ( object sender, EventArgs e ) {
            videoPlayer.settings.volume = volumeTrackBar.Value;
            SetMuteState ( false );
        }

        private void toggleChatButton_Click ( object sender, EventArgs e ) {
            SetChatVisibility ( !chatVisible );
        }

        private void hideMouseMoveControlsTimer_Tick ( object sender, EventArgs e ) {
            SetMouseMoveControlVisibility ( false );
        }

        private void theaterModeButton_Click ( object sender, EventArgs e ) {
            if ( currentVideoMode != VideoMode.Theater )
                SetVideoMode ( VideoMode.Theater );
            else
                SetVideoMode ( VideoMode.Normal );
        }

        private void soundButton_Click ( object sender, EventArgs e ) {
            SetMuteState ( !videoPlayer.settings.mute );
        }
    }
}
