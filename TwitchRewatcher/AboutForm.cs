using System.Windows.Forms;
using System.Diagnostics;

namespace TwitchRewatcher
{
    public partial class AboutForm : Form
    {
        public AboutForm () {
            InitializeComponent ();
        }

        private void linkLabel1_LinkClicked ( object sender, LinkLabelLinkClickedEventArgs e ) {
            Process.Start ( "www.dappercoding.com" );
        }

        private void linkLabel2_LinkClicked ( object sender, LinkLabelLinkClickedEventArgs e ) {
            Process.Start ( "https://github.com/yt-dlp/yt-dlp" );
        }

        private void linkLabel3_LinkClicked ( object sender, LinkLabelLinkClickedEventArgs e ) {
            Process.Start ( "https://github.com/jdpurcell/RechatTool" );
        }
    }
}
