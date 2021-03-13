using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace TwitchRewatcher {
    public partial class LoadingForm : Form {
        private TwitchRewatcherForm rewatcherForm = new TwitchRewatcherForm ();
        private int loadCount = 2;

        private void LoadFinished () {
            loadCount--;
            if ( loadCount > 0 )
                return;
            Invoke ( new MethodInvoker ( delegate () {
                rewatcherForm.Show ();
                Hide ();
            } ) );
        }

        private void OnOfficitalEmoticonsLoaded ( BTTVEmoticon[] emoticons ) {
            Debug.WriteLine ( "Loaded official emoticons" );
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
            VodDownloader.OnYoutubeDLUpdated += new VodDownloader.VodEventHandler ( OnYoutubeDLUpdated );
            BTTVEmoticonLoader.OnOfficialEmoticonsLoaded += new BTTVEmoticonLoader.BTTVEmoticonEventHandler ( OnOfficitalEmoticonsLoaded );
        }

        private void LoadingForm_FormClosing ( object sender, FormClosingEventArgs e ) {
            rewatcherForm.Dispose ();
        }

        private void LoadingForm_Shown ( object sender, EventArgs e ) {
            VodDownloader.UpdateYoutubeDL ();
            BTTVEmoticonLoader.LoadOfficialEmoticons ();
        }
    }
}
