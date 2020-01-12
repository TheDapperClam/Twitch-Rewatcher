using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Dialogs;
using Newtonsoft.Json;

namespace TwitchRewatcher
{
    public enum VideoMode {
        Normal = 0,
        Theater,
    }

    public partial class TwitchRewatcherForm : Form, IMessageFilter
    {
        public delegate void MouseMovedEvent ();
        public event MouseMovedEvent GlobalMouseMove;

        private const int WM_MOUSEMOVE = 0x0200;
        private const double PLAYBACK_TIME_TRAVEL_AMOUNT = 10;
        private readonly string CHAT_HTML_LOCATION_String = Directory.GetCurrentDirectory () + "\\Web\\chat.htm";
        private ChatObject[] chatMessages;
        private int chatIndex;
        private HtmlDocument chatDocument;
        private HtmlElement chatList;

        private VideoMode currentVideoMode;
        private Rectangle oldBounds;
        private bool isMaximized;
        private bool optionsPanelVisible = true;
        private bool chatVisible = true;

        private Point oldCursorPosition;

        private bool minimizeOnClose = true;

        private StreamConfig currentStreamConfig;
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
            if ( obj == null )
                return;

            if ( !IsChatHtmlLoaded () )
                return;

            chatList.InnerHtml += obj.GetFormattedMessage ();
            chatDocument.Window.ScrollTo ( 0, chatDocument.Body.ScrollRectangle.Height );
        }
        private void ClearChat () {
            if ( !IsChatHtmlLoaded () )
                return;

            chatList.InnerHtml = null;
        }

        private double GetPlaybackTime () {
            return videoPlayer.Ctlcontrols.currentPosition;
        }

        private bool IsChatHtmlLoaded () {
            if ( chatDocument == null || chatList == null ) {
                LoadChatHtml ();

                if ( chatDocument == null || chatList == null )
                    return false;
            }
            return true;
        }

        private bool IsPlaying () {
            return videoPlayer.playState == WMPLib.WMPPlayState.wmppsPlaying;
        }

        private bool IsVideoLoaded () {
            return videoPlayer.currentMedia != null;
        }

        private void LoadChat ( string path ) {
            currentChatPath = path;
            chatMessages = ChatLoader.Load ( path );
        }

        private void LoadChatHtml () {
            chatDocument = chatWebBrowser.Document;
            chatList = chatDocument.GetElementById ( "ChatList" );
        }

        private void LoadChatPage () {
            chatWebBrowser.Navigate ( CHAT_HTML_LOCATION_String );
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

            string basePath = Directory.GetParent ( currentVideoPath ).FullName;
            currentStreamConfig.PlaybackTime = GetPlaybackTime ();
            StreamConfigManager.SaveConfig ( basePath, currentStreamConfig );
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
                    TopMost = false;
                    break;
                case VideoMode.Theater:
                    isMaximized = WindowState == FormWindowState.Maximized;
                    oldBounds = RestoreBounds;
                    FormBorderStyle = FormBorderStyle.None;
                    WindowState = FormWindowState.Normal;
                    Bounds = Screen.FromControl ( this ).Bounds;
                    TopMost = true;
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
            LoadChatPage ();
        }

        private void chatWebBrowser_DocumentCompleted( object sender, WebBrowserDocumentCompletedEventArgs e ) {
            LoadChatHtml ();
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

            VodDownloader.KillDownloads ();
            notifyIcon.Visible = false;
            Program.Close ();
        }

        private void downloadVodButton_Click ( object sender, EventArgs e ) {
            if ( downloadForm == null )
                downloadForm = new DownloadVodForm ();

            downloadForm.ShowDialog ();
        }

        private void openStreamButton_Click ( object sender, EventArgs e ) {
            using ( CommonOpenFileDialog dialog = new CommonOpenFileDialog () ) {
                dialog.IsFolderPicker = true;

                if ( dialog.ShowDialog () != CommonFileDialogResult.Ok )
                    return;

                SaveCurrentConfig ();
                bool videoFound = false;
                bool chatFound = false;
                bool cfgFound = false;
                string[] directories = Directory.GetFiles ( dialog.FileName );

                foreach ( string d in directories ) {
                    string extension = Path.GetExtension ( d );

                    if ( !videoFound && extension == ".mp4" ) {
                        videoFound = true;
                        LoadVideo ( d );
                    }

                    if ( !chatFound && extension == ".dtc" ) {
                        chatFound = true;
                        LoadChat ( d );
                    }

                    if ( !cfgFound && extension == ".dtj" ) {
                        cfgFound = true;
                        LoadStreamConfig ( d );
                    }

                    if ( chatFound && videoFound && cfgFound )
                        break;
                }

                if ( videoFound || chatFound )
                    ClearChat ();
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
            this.Close ();
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
