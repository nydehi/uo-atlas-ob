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
using System.Text;
using System.IO;

namespace UO_Atlas
{
    public class Utility
    {
        /// <summary>
        /// Verifies if a directory exists and creates it otherwise
        /// </summary>
        /// <param name="path">The folder to ensure</param>
        public static void EnsureDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }



        public static void DeleteDirectory(DirectoryInfo directory)
        {
            if (!directory.Exists)
            {
                return;
            }

            DirectoryInfo[] directories = directory.GetDirectories();
            foreach(DirectoryInfo subDirectory in directories)
            {
                DeleteDirectory(subDirectory);
            }

            FileInfo[] files = directory.GetFiles();
            foreach(FileInfo f in files)
            {
                f.Delete();
            }

            directory.Delete();
        }
    }
}
