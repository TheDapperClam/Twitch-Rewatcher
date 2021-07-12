using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace TwitchRewatcher {
    public partial class LoadingForm : Form {
        private TwitchRewatcherForm rewatcherForm = new TwitchRewatcherForm ();
        private int loadCount = 3;

        private void LoadFinished () {
            loadCount--;
            if ( loadCount > 0 )
                return;
            Invoke ( new MethodInvoker ( delegate () {
                rewatcherForm.Show ();
                Hide ();
            } ) );
        }

        private void OnBTTVEmoticonsLoaded ( BTTVEmoticon[] emoticons ) {
            Debug.WriteLine ( "Loaded official emoticons" );
            LoadFinished ();
        }

        private void OnTwitchBadgesLoaded () {
            Debug.WriteLine ( "Twitch badges loaded" );
            LoadFinished ();
        }

        private void OnYoutubeDLUpdated () {
            Debug.WriteLine ( "Updated Youtube-dl" );
            LoadFinished ();
        }

        public LoadingForm () {
            InitializeComponent ();
        }

        private void LoadingForm_Load ( object sender, EventArgs e ) {
            VodDownloader.OnYoutubeDLUpdated += OnYoutubeDLUpdated;
            TwitchBadgeLoader.OnGlobalBadgesLoaded += OnTwitchBadgesLoaded;
            BTTVEmoticonLoader.OnOfficialEmoticonsLoaded += OnBTTVEmoticonsLoaded;
        }

        private void LoadingForm_FormClosing ( object sender, FormClosingEventArgs e ) {
            rewatcherForm.Dispose ();
        }

        private void LoadingForm_Shown ( object sender, EventArgs e ) {
            VodDownloader.UpdateYoutubeDL ();
            TwitchBadgeLoader.LoadGlobalBadges ();
            BTTVEmoticonLoader.LoadOfficialEmoticons ();
        }
    }
}
