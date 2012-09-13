/***************************************************************************
 *
 *   This program is free software; you can redistribute it and/or modify
 *   it under the terms of the GNU General Public License as published by
 *   the Free Software Foundation; either version 2 of the License, or
 *   (at your option) any later version.
 *
 ***************************************************************************/

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;

namespace UO_Atlas
{
    public delegate void ErrorEventHandler(object sender, string message);

    public static class Atlas
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        /// <summary>
        /// Gets the location of the UO Atlas application data folder
        /// </summary>
        public static string ApplicationDataFolder
        {
            get
            {
                string folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "UO Atlas");
                Utility.EnsureDirectory(folder);
                return folder;
            }
        }

        /// <summary>
        /// Gets the location of the folder where the map images are stored
        /// </summary>
        public static string MapImagesFolder
        {
            get
            {
                string folder = Path.Combine(ApplicationDataFolder, "Maps");
                Utility.EnsureDirectory(folder);
                return folder;
            }
        }
    }
}