using Accord.Video.FFMPEG;
using System.Drawing;
using System.IO;

namespace TwitchRewatcher {
    public static class VideoFrameExtractor {
        public static Bitmap[] Extract ( string path ) {
            if ( !File.Exists ( path ) )
                return null;

            using ( VideoFileReader reader = new VideoFileReader () ) {
                reader.Open ( path );
                Bitmap[] frames = new Bitmap[ reader.FrameCount ];

                for ( int i = 0; i < reader.FrameCount; i++ )
                    frames[i] = reader.ReadVideoFrame ();

                reader.Close ();
                return frames;
            }
        }
    }
}
