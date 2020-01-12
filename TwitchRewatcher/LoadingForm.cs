using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TwitchRewatcher {
    public partial class LoadingForm : Form {
        private TwitchRewatcherForm rewatcherForm = new TwitchRewatcherForm ();

        private void OnOfficitalEmoticonsLoaded () {
            rewatcherForm.Show ();
            WindowState = FormWindowState.Minimized;
        }

        public LoadingForm () {
            InitializeComponent ();
        }

        private void LoadingForm_Load ( object sender, EventArgs e ) {
            BTTVEmoticonLoader.OnOfficialEmoticonsLoaded += new BTTVEmoticonLoader.BTTVEmoticonEventHandler ( OnOfficitalEmoticonsLoaded );
            BTTVEmoticonLoader.LoadOfficialEmoticons ();
        }

        private void LoadingForm_FormClosing ( object sender, FormClosingEventArgs e ) {
            rewatcherForm.Dispose ();
        }
    }
}
