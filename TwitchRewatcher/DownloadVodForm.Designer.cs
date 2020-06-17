namespace TwitchRewatcher
{
    partial class DownloadVodForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose ( bool disposing ) {
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
        private void InitializeComponent () {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DownloadVodForm));
            this.downloadProgressBar = new System.Windows.Forms.ProgressBar();
            this.destinationBrowseButton = new System.Windows.Forms.Button();
            this.destinationTextBox = new System.Windows.Forms.TextBox();
            this.titleTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.urlTextBox = new System.Windows.Forms.TextBox();
            this.downloadProgressLabel = new System.Windows.Forms.Label();
            this.downloadButton = new System.Windows.Forms.Button();
            this.vodDownloadQueueDataGridView = new System.Windows.Forms.DataGridView();
            this.deleteDownloadButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.vodDownloadQueueDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // downloadProgressBar
            // 
            this.downloadProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.downloadProgressBar.Location = new System.Drawing.Point(12, 260);
            this.downloadProgressBar.Name = "downloadProgressBar";
            this.downloadProgressBar.Size = new System.Drawing.Size(558, 17);
            this.downloadProgressBar.TabIndex = 9;
            // 
            // destinationBrowseButton
            // 
            this.destinationBrowseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.destinationBrowseButton.Location = new System.Drawing.Point(495, 137);
            this.destinationBrowseButton.Name = "destinationBrowseButton";
            this.destinationBrowseButton.Size = new System.Drawing.Size(75, 23);
            this.destinationBrowseButton.TabIndex = 6;
            this.destinationBrowseButton.Text = "Browse";
            this.destinationBrowseButton.UseVisualStyleBackColor = true;
            this.destinationBrowseButton.Click += new System.EventHandler(this.destinationBrowseButton_Click);
            // 
            // destinationTextBox
            // 
            this.destinationTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.destinationTextBox.Location = new System.Drawing.Point(12, 139);
            this.destinationTextBox.Name = "destinationTextBox";
            this.destinationTextBox.ReadOnly = true;
            this.destinationTextBox.Size = new System.Drawing.Size(477, 20);
            this.destinationTextBox.TabIndex = 5;
            this.destinationTextBox.TabStop = false;
            // 
            // titleTextBox
            // 
            this.titleTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.titleTextBox.Location = new System.Drawing.Point(12, 90);
            this.titleTextBox.Name = "titleTextBox";
            this.titleTextBox.Size = new System.Drawing.Size(558, 20);
            this.titleTextBox.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(12, 123);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Destination";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(12, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(27, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Title";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(12, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "VOD URL";
            // 
            // urlTextBox
            // 
            this.urlTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.urlTextBox.Location = new System.Drawing.Point(12, 41);
            this.urlTextBox.Name = "urlTextBox";
            this.urlTextBox.Size = new System.Drawing.Size(558, 20);
            this.urlTextBox.TabIndex = 1;
            // 
            // downloadProgressLabel
            // 
            this.downloadProgressLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.downloadProgressLabel.AutoSize = true;
            this.downloadProgressLabel.ForeColor = System.Drawing.Color.White;
            this.downloadProgressLabel.Location = new System.Drawing.Point(12, 244);
            this.downloadProgressLabel.Name = "downloadProgressLabel";
            this.downloadProgressLabel.Size = new System.Drawing.Size(68, 13);
            this.downloadProgressLabel.TabIndex = 8;
            this.downloadProgressLabel.Text = "Progress: 0%";
            // 
            // downloadButton
            // 
            this.downloadButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.downloadButton.Location = new System.Drawing.Point(12, 175);
            this.downloadButton.Name = "downloadButton";
            this.downloadButton.Size = new System.Drawing.Size(558, 46);
            this.downloadButton.TabIndex = 7;
            this.downloadButton.Text = "Download";
            this.downloadButton.UseVisualStyleBackColor = true;
            this.downloadButton.Click += new System.EventHandler(this.downloadButton_Click);
            // 
            // vodDownloadQueueDataGridView
            // 
            this.vodDownloadQueueDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.vodDownloadQueueDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.vodDownloadQueueDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.vodDownloadQueueDataGridView.Location = new System.Drawing.Point(12, 283);
            this.vodDownloadQueueDataGridView.Name = "vodDownloadQueueDataGridView";
            this.vodDownloadQueueDataGridView.ReadOnly = true;
            this.vodDownloadQueueDataGridView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.vodDownloadQueueDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.vodDownloadQueueDataGridView.Size = new System.Drawing.Size(558, 200);
            this.vodDownloadQueueDataGridView.TabIndex = 10;
            this.vodDownloadQueueDataGridView.TabStop = false;
            this.vodDownloadQueueDataGridView.SelectionChanged += new System.EventHandler(this.vodDownloadQueueDataGridView_SelectionChanged);
            // 
            // deleteDownloadButton
            // 
            this.deleteDownloadButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.deleteDownloadButton.Enabled = false;
            this.deleteDownloadButton.Location = new System.Drawing.Point(12, 489);
            this.deleteDownloadButton.Name = "deleteDownloadButton";
            this.deleteDownloadButton.Size = new System.Drawing.Size(75, 23);
            this.deleteDownloadButton.TabIndex = 11;
            this.deleteDownloadButton.Text = "Delete";
            this.deleteDownloadButton.UseVisualStyleBackColor = true;
            this.deleteDownloadButton.Click += new System.EventHandler(this.deleteDownloadButton_Click);
            // 
            // DownloadVodForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.ClientSize = new System.Drawing.Size(582, 524);
            this.Controls.Add(this.deleteDownloadButton);
            this.Controls.Add(this.vodDownloadQueueDataGridView);
            this.Controls.Add(this.downloadButton);
            this.Controls.Add(this.downloadProgressLabel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.urlTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.titleTextBox);
            this.Controls.Add(this.destinationTextBox);
            this.Controls.Add(this.destinationBrowseButton);
            this.Controls.Add(this.downloadProgressBar);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DownloadVodForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Download VOD";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DownloadVodForm_FormClosing);
            this.Load += new System.EventHandler(this.DownloadVodForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.vodDownloadQueueDataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar downloadProgressBar;
        private System.Windows.Forms.Button destinationBrowseButton;
        private System.Windows.Forms.TextBox destinationTextBox;
        private System.Windows.Forms.TextBox titleTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox urlTextBox;
        private System.Windows.Forms.Label downloadProgressLabel;
        private System.Windows.Forms.Button downloadButton;
        private System.Windows.Forms.DataGridView vodDownloadQueueDataGridView;
        private System.Windows.Forms.Button deleteDownloadButton;
    }
}