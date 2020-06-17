using System;
using System.Windows.Forms;

namespace TwitchRewatcher {
    public static class Program
    {
        private static LoadingForm form;

        public static void Close () {
            if ( form == null )
                return;

            form.Close ();
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main() {
            Application.EnableVisualStyles ();
            Application.SetCompatibleTextRenderingDefault ( false );
            Application.Run ( ( form = new LoadingForm () ) );
        }
    }
}
