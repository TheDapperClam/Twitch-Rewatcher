using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace TwitchRewatcher {
    public partial class DownloadVodForm : Form
    {
        private BindingList<VodDownloadEntry> downloadQueue = new BindingList<VodDownloadEntry> ();

        private void OnDownloadFinished () {
            if ( downloadQueue == null )
                return;

            downloadQueue.RemoveAt ( 0 );
            OnDownloadProgress ();

            if ( downloadQueue.Count <= 0 )
                return;

            VodDownloadEntry nextEntry = downloadQueue[ 0 ];
            VodDownloader.Download ( nextEntry.Name, nextEntry.URL, nextEntry.Destination );
        }

        private void OnDownloadProgress () {
            if ( !IsHandleCreated )
                return;

            int downloadProgress = (int) VodDownloader.TotalDownloadProgress ();

            downloadProgressBar.BeginInvoke ( (MethodInvoker) delegate () {
                downloadProgressBar.Value = downloadProgress;
            } );

            downloadProgressLabel.BeginInvoke ( (MethodInvoker) delegate () {
                downloadProgressLabel.Text = "Progress: " + downloadProgress + "%";
            } );
        }

        public DownloadVodForm () {
            InitializeComponent ();
        }

        private void downloadButton_Click ( object sender, EventArgs e ) {
            string illegalChars = VodDownloader.GetIllegalChars ( titleTextBox.Text );
            if ( illegalChars != null && illegalChars.Length > 0 ) {
                DialogResult result = MessageBox.Show (
                    "Title '" + titleTextBox.Text + "' contains illegal characters '" + illegalChars + "' that will be removed if used.",
                    "Illegal Characters",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Warning );

                if ( result == DialogResult.Cancel )
                    return;

                foreach ( char c in illegalChars )
                    titleTextBox.Text.Replace ( c.ToString (), "" );
            }

            if (string.IsNullOrWhiteSpace(titleTextBox.Text)) {
                MessageBox.Show ( "Please provide a valid stream title.", "Data Error", MessageBoxButtons.OK, MessageBoxIcon.Error );
                titleTextBox.Focus ();
                return;
            }

            if ( downloadQueue == null )
                downloadQueue = new BindingList<VodDownloadEntry> ();

            downloadQueue.Add ( new VodDownloadEntry ( urlTextBox.Text, titleTextBox.Text, destinationTextBox.Text ) );

            if ( !VodDownloader.IsDownloadingChat && !VodDownloader.IsDownloadingVideo ) {
                VodDownloader.Download ( titleTextBox.Text, urlTextBox.Text, destinationTextBox.Text );
            }

            urlTextBox.Clear ();
            titleTextBox.Clear ();
            urlTextBox.Focus ();
        }

        private void destinationBrowseButton_Click ( object sender, EventArgs e ) {
            using ( CommonOpenFileDialog dialog = new CommonOpenFileDialog () ) {
                dialog.IsFolderPicker = true;

                if ( dialog.ShowDialog () == CommonFileDialogResult.Ok )
                    destinationTextBox.Text = dialog.FileName;

                Focus ();
            }
        }

        private void DownloadVodForm_Load ( object sender, EventArgs e ) {
            VodDownloader.OnDownloadProgress += new VodDownloader.VodEventHandler ( OnDownloadProgress );
            VodDownloader.OnDownloadFinished += new VodDownloader.VodEventHandler ( OnDownloadFinished );
            vodDownloadQueueDataGridView.DataSource = downloadQueue;
        }

        private void DownloadVodForm_FormClosing ( object sender, FormClosingEventArgs e ) {
            this.Hide ();
            e.Cancel = true;
        }

        private void vodDownloadQueueDataGridView_SelectionChanged ( object sender, EventArgs e ) {
            deleteDownloadButton.Enabled = vodDownloadQueueDataGridView.SelectedRows.Count > 0;
        }

        private void deleteDownloadButton_Click ( object sender, EventArgs e ) {
            foreach ( DataGridViewRow row in vodDownloadQueueDataGridView.SelectedRows ) {
                if ( row.Index == 0 ) {
                    continue;
                    /*switch ( MessageBox.Show ( "Would you like to delete the current download?", "Deleting Download", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning ) ) {
                        case DialogResult.No:
                            continue;
                        case DialogResult.Cancel:
                            return;
                    }
                    downloadIndex = -1;
                    VodDownloader.KillDownloads ();*/
                }
                downloadQueue.RemoveAt ( row.Index );
            }
        }
    }
}
