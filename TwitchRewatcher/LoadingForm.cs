using System;
using System.Windows.Forms;

namespace TwitchRewatcher {
    public partial class LoadingForm : Form {
        private TwitchRewatcherForm rewatcherForm = new TwitchRewatcherForm ();

        private void OnOfficitalEmoticonsLoaded ( BTTVEmoticon[] emoticons ) {
            rewatcherForm.Show ();
            Hide ();
        }

        public LoadingForm () {
            InitializeComponent ();
        }

        private void LoadingForm_Load ( object sender, EventArgs e ) {
            BTTVEmoticonLoader.OnOfficialEmoticonsLoaded += new BTTVEmoticonLoader.BTTVEmoticonEventHandler ( OnOfficitalEmoticonsLoaded );
        }

        private void LoadingForm_FormClosing ( object sender, FormClosingEventArgs e ) {
            rewatcherForm.Dispose ();
        }

        private void LoadingForm_Shown ( object sender, EventArgs e ) {
            BTTVEmoticonLoader.LoadOfficialEmoticons ();
        }
    }
}
