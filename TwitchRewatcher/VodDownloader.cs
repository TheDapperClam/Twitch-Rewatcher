﻿using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace TwitchRewatcher {
    public static class VodDownloader
    {
        private const string VIDEO_DL_PATH = "Tools\\Video-DL\\yt-dlp.exe";
        private const string RECHAT_TOOL_PATH = "Tools\\RechatTool\\RechatTool.exe";
        private static readonly string ILLEGAL_PATH_CHARS = new string ( Path.GetInvalidFileNameChars () ) + new string ( Path.GetInvalidPathChars () );

        public static float VideoDownloadProgress { get; private set; }
        public static float ChatDownloadProgress { get; private set; }
        public static bool IsDownloading { get { return IsDownloadingVideo || IsDownloadingChat; } }
        public static bool IsDownloadingVideo { get { return videoDlProcess != null; } }
        public static bool IsDownloadingChat { get { return reChatToolProcess != null; } }

        private static Process videoDlProcess = null;
        private static Process videoDlUpdateProcess = null;
        private static Process reChatToolProcess = null;

        public delegate void VodEventHandler ();
        public static event VodEventHandler OnDownloadFinished;
        public static event VodEventHandler OnDownloadProgress;
        public static event VodEventHandler OnVideoDlUpdated;

        private static void ChatDataRecieved ( object sender, DataReceivedEventArgs e ) {
            OnDownloadProgress?.Invoke ();
        }

        private static void ChatExit ( object sender, EventArgs e ) {
            ChatDownloadProgress = 100.0f;
            reChatToolProcess.Dispose ();
            reChatToolProcess = null;
            CheckDownloadFinished ();
            OnDownloadProgress?.Invoke ();
        }

        private static void CheckDownloadFinished () {
            if ( IsDownloading )
                return;

            VideoDownloadProgress = 0.0f;
            ChatDownloadProgress = 0.0f;
            OnDownloadFinished?.Invoke ();
        }

        public static void Download ( string title, string source, string output = null ) {
            string id = Regex.Match ( source, @"videos/([0-9]+?)\b" ).Groups[ 1 ].Value;
            string baseDirectory = Path.Combine ( output, title );
            System.Diagnostics.Debug.WriteLine ( baseDirectory + " " + source );
            VideoDownloadProgress = 0.0f;
            ChatDownloadProgress = 0.0f;
            Directory.CreateDirectory ( baseDirectory );

            ProcessStartInfo videoDlInfo = new ProcessStartInfo ( Path.Combine ( Environment.CurrentDirectory, VIDEO_DL_PATH ) );
            ProcessStartInfo rechatToolInfo = new ProcessStartInfo ( Path.Combine ( Environment.CurrentDirectory, RECHAT_TOOL_PATH ) );
            videoDlInfo.UseShellExecute = false;
            rechatToolInfo.UseShellExecute = false;
            videoDlInfo.RedirectStandardOutput = true;
            rechatToolInfo.RedirectStandardOutput = true;
            videoDlInfo.CreateNoWindow = true;
            rechatToolInfo.CreateNoWindow = true;
            rechatToolInfo.WindowStyle = ProcessWindowStyle.Hidden;
            videoDlInfo.Arguments = string.Format ( "--newline -o \"{0}\\video.%(ext)s\" \"{1}\"", baseDirectory, source );
            rechatToolInfo.Arguments = string.Format ( "-d {0} \"{1}\"", id, baseDirectory + "/chat.dtc" );

            videoDlProcess = Process.Start ( videoDlInfo );
            reChatToolProcess = Process.Start ( rechatToolInfo );
            videoDlProcess.EnableRaisingEvents = true;
            reChatToolProcess.EnableRaisingEvents = true;
            videoDlProcess.BeginOutputReadLine ();
            reChatToolProcess.BeginOutputReadLine ();

            videoDlProcess.Exited += new EventHandler ( VideoExit );
            reChatToolProcess.Exited += new EventHandler ( ChatExit );
            videoDlProcess.OutputDataReceived += new DataReceivedEventHandler ( VideoDataRecieved );
            reChatToolProcess.OutputDataReceived += new DataReceivedEventHandler ( ChatDataRecieved );
        }

        public static string GetIllegalChars ( string path ) {
            string illegalChars = "";

            foreach ( char c in ILLEGAL_PATH_CHARS ) {
                string cString = c.ToString ();

                if ( !path.Contains ( cString ) )
                    continue;

                illegalChars += cString;
            }

            return illegalChars;
        }

        public static void KillDownloads () {
            bool downloadFinished = !IsDownloadingChat && !IsDownloadingVideo;

            if ( IsDownloadingVideo && !videoDlProcess.HasExited )
                videoDlProcess.Kill ();

            if ( IsDownloadingChat && !reChatToolProcess.HasExited )
                reChatToolProcess.Kill ();
        }

        public static float TotalDownloadProgress () {
            float total = Math.Max ( 0f, 100f / 200f * ( VideoDownloadProgress + ChatDownloadProgress ) );
            return total;
        }

        public static void UpdateVideoDl () {
            ProcessStartInfo videoDlUpdateInfo = new ProcessStartInfo ( Path.Combine ( Environment.CurrentDirectory, VIDEO_DL_PATH ) );
            videoDlUpdateInfo.UseShellExecute = false;
            videoDlUpdateInfo.CreateNoWindow = true;
            videoDlUpdateInfo.Arguments = "-U";
            videoDlUpdateProcess = Process.Start ( videoDlUpdateInfo );
            videoDlUpdateProcess.EnableRaisingEvents = true;
            videoDlUpdateProcess.Exited += new EventHandler ( VideoDlUpdateFinished );
        }

        private static void VideoDataRecieved ( object sender, DataReceivedEventArgs e ) {
            if ( string.IsNullOrWhiteSpace ( e.Data ) )
                return;

            string data = e.Data.Replace ( " ", "" );
            Regex r = new Regex ( "](.+?)%" );
            string match = r.Match ( data ).Groups[ 1 ].Value;
            float.TryParse ( match, out float progress );
            VideoDownloadProgress = progress;
            OnDownloadProgress?.Invoke ();
        }

        private static void VideoExit ( object sender, EventArgs e ) {
            videoDlProcess.Dispose ();
            videoDlProcess = null;
            CheckDownloadFinished ();
            OnDownloadProgress?.Invoke ();
        }

        private static void VideoDlUpdateFinished ( object sender, EventArgs e ) {
            videoDlUpdateProcess.Dispose ();
            videoDlUpdateProcess = null;
            OnVideoDlUpdated?.Invoke ();
        }
    }
}
