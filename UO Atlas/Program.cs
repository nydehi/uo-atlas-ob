using System;
using System.Drawing;
using System.Windows.Forms;
using UO_Atlas.Controls;



namespace UO_Atlas
{
    internal class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main(string[] args)
        {
            Atlas.EnsureLabelDatabase();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());

            //OutputMemoryUsage();

            //using(Form f = new Form())
            //{
            //    UO_Atlas.Controls.ImageViewer viewer = new ImageViewer();
            //    viewer.Dock = DockStyle.Fill;
            //    f.Controls.Add(viewer);

            //    viewer.LoadImage(@"C:\Users\JWilliams\AppData\Roaming\UO Atlas\Maps\map1-100%.png");

            //    Application.Run(f);  
            //}

            //Application.Run(new UO_Atlas.Controls.CodeProjectImageScroller());

            //Bitmap b = (Bitmap) Image.FromFile(@"C:\Users\JWilliams\AppData\Roaming\UO Atlas\Maps\map1-100%.png");

            //OutputMemoryUsage();

            //b.Dispose();

            //GC.Collect();

            //OutputMemoryUsage();
        }



        private static void OutputMemoryUsage()
        {
            Console.WriteLine(GC.GetTotalMemory(true) / 1024 + "kb memory used according to GC.GetTotalMemory(true).");
            Console.WriteLine(System.Diagnostics.Process.GetCurrentProcess().WorkingSet64 / 1024 + "kb memory used according to Process.WorkingSet64.");
        }
    }
}