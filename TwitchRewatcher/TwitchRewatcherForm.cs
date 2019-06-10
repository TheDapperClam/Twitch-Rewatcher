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
    public partial class TwitchRewatcherForm : Form
    {
        private const double PLAYBACK_TIME_TRAVEL_AMOUNT = 60;
        private readonly string CHAT_HTML_LOCATION_String = Directory.GetCurrentDirectory () + "\\Web\\chat.htm";
        private ChatObject[] chatMessages;
        private int chatIndex;
        private HtmlDocument chatDocument;
        private HtmlElement chatList;

        private StreamConfig currentStreamConfig;
        private string currentVideoPath;
        private string currentChatPath;

        private double bufferedPlaybackTime = 0;
        private double startingTime = 0;

        private DownloadVodForm downloadForm = new DownloadVodForm ();

        private void AddChatMessage ( ChatObject message ) {
            if ( message == null )
                return;

            if ( !IsChatHtmlLoaded () )
                return;

            string userColor = message.Message.Color;
            string userName = message.Commenter.DisplayName;
            string userMessage = message.Message.Body;
            string formattedMessage = string.Format ( "<li><span style='color:{0}'><b>{1}</b></span>: {2}</li>", userColor, userName, userMessage );
            chatList.InnerHtml += formattedMessage;
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

        private void UpdateChatMessages() {
            if ( chatMessages == null )
                return;

            if ( chatIndex < 0 || chatIndex >= chatMessages.Length )
                return;

            ChatObject newMessage = chatMessages[ chatIndex ];

            if ( GetPlaybackTime () < newMessage.PostTime )
                return;

            AddChatMessage ( newMessage );
            chatIndex++;
        }

        private void UpdateVideoControls () {
            int playbackTime = (int) GetPlaybackTime ();
            SetPlaybackTimeLabel ( playbackTime );
            SetPlaybackTrackBar ( playbackTime );
        }

        public TwitchRewatcherForm() {
            InitializeComponent ();
        }

        private void videoPlayer_PlayStateChange( object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e ) {
            Image playButtonIcon = IsPlaying () ? Properties.Resources.PauseButton : Properties.Resources.PlayButton;

            playButton.BackgroundImage = playButtonIcon;
        }

        private void playButton_Click( object sender, EventArgs e ) {
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
            LoadChatPage ();
        }

        private void chatWebBrowser_DocumentCompleted( object sender, WebBrowserDocumentCompletedEventArgs e ) {
            LoadChatHtml ();
        }

        private void TwitchRewatcherForm_FormClosing( object sender, FormClosingEventArgs e ) {
            SaveCurrentConfig ();

            if ( VodDownloader.IsDownloadingChat || VodDownloader.IsDownloadingVideo ) {
                DialogResult r = MessageBox.Show ( "You are currently downloading a VOD; quitting will result in the download being cancelled. Would you like to quit?",
                    "VOD Downloading",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning );

                if ( r == DialogResult.No ) {
                    e.Cancel = true;
                    return;
                }
            }

            VodDownloader.KillDownloads ();
        }

        private void downloadVodButton_Click ( object sender, EventArgs e ) {
            if ( downloadForm == null )
                downloadForm = new DownloadVodForm ();

            downloadForm.Show ();
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
            AboutForm form = new AboutForm ();
            form.ShowDialog ();
        }
    }
}
