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
using System.Drawing;
using System.IO.Compression;
using System.Windows.Forms;
using System.IO;
using FirebirdSql.Data.FirebirdClient;
using UO_Atlas.Properties;

namespace UO_Atlas
{
    public delegate void ErrorEventHandler(object sender, string message);



    public static class Atlas
    {

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



        /// <summary>
        /// Gets the location of the folder where the map images are stored
        /// </summary>
        public static string MapLabelsFolder
        {
            get
            {
                string folder = Path.Combine(ApplicationDataFolder, "Labels");
                Utility.EnsureDirectory(folder);
                return folder;
            }
        }



        /// <summary>
        /// Gets the connection string for database containing the map labels.
        /// </summary>
        internal static string MapLabelsDatabaseConnectionString
        {
            get
            {
                string dbPath = Path.Combine(MapLabelsFolder, "data.fdb");

                FbConnectionStringBuilder connectionStringBuilder = new FbConnectionStringBuilder();
                connectionStringBuilder.Database = dbPath;
                connectionStringBuilder.UserID = "Trox";
                connectionStringBuilder.Password = "Oblivion Reloaded";
                connectionStringBuilder.ServerType = FbServerType.Embedded;

                string connectionString = connectionStringBuilder.ToString();

                if(!File.Exists(dbPath))
                {
                    FbConnection.CreateDatabase(connectionString);
                }

                return connectionString;
            }
        }







        private static void EnsureFireBirdSetup()
        {
            string[] requiredFiles = new string[]
                                         {
                                             Path.Combine(Application.StartupPath, "FirebirdSql.Data.FirebirdClient.dll"),
                                             Path.Combine(Application.StartupPath, "fbembed.dll"),
                                             Path.Combine(Application.StartupPath, "icudt30.dll"),
                                             Path.Combine(Application.StartupPath, "icuin30.dll"),
                                             Path.Combine(Application.StartupPath, "icuuc30.dll"),
                                             Path.Combine(Application.StartupPath, "ib_util.dll"),
                                             Path.Combine(Application.StartupPath, "firebird.conf"),
                                             Path.Combine(Application.StartupPath, "firebird.msg"),
                                         };
            bool allFilesExist = true;

            foreach (string requiredFile in requiredFiles)
            {
                if (!File.Exists(requiredFile))
                {
                    allFilesExist = false;
                    break;
                }
            }
            if (allFilesExist)
            {
                return;
            }


            string fireBirdZipFile = Path.Combine(Application.StartupPath, "fireBirdFiles.zip");
            using (FileStream outStream = new FileStream(fireBirdZipFile, FileMode.Create))
            {
                byte[] streamData = Resources.FireBirdFiles;
                outStream.Write(streamData, 0, streamData.Length);
            }

            EnsureShell32();

            ExtractFireBirdFiles(fireBirdZipFile);
        }



        private static void EnsureShell32()
        {
            string shell32ReferenceFile = Path.Combine(Application.StartupPath, "Interop.Shell32.dll");
            if (File.Exists(shell32ReferenceFile))
            {
                return;
            }

            using (FileStream outStream = new FileStream(shell32ReferenceFile, FileMode.Create))
            {
                byte[] streamData = Resources.Shell32;
                outStream.Write(streamData, 0, streamData.Length);
            }
        }



        private static void ExtractFireBirdFiles(string zipFile)
        {
            string zipFileExtractionPath = Path.Combine(Application.StartupPath, Path.GetFileNameWithoutExtension(zipFile));
            Utility.EnsureDirectory(zipFileExtractionPath);

            Shell32.ShellClass shellClass = new Shell32.ShellClass();
            Shell32.Folder srcFolder = shellClass.NameSpace(zipFile);
            Shell32.Folder destFolder = shellClass.NameSpace(zipFileExtractionPath);
            destFolder.CopyHere(srcFolder.Items(), 16 | 256 | 512);

            File.Delete(zipFile);


            string destinationPath = Path.Combine(Application.StartupPath, "FirebirdSql.Data.FirebirdClient.dll");
            File.Delete(destinationPath);
            File.Move(Path.Combine(zipFileExtractionPath, "FirebirdSql.Data.FirebirdClient.dll"), destinationPath);

            string basePath = Path.Combine(zipFileExtractionPath, "x86");
            if (IntPtr.Size == 8)
            {
                basePath = Path.Combine(zipFileExtractionPath, "x64");
            }

            string[] remainingFiles = new string[] { "fbembed.dll", "icudt30.dll", "icuin30.dll", "icuuc30.dll", "ib_util.dll", "firebird.conf", "firebird.msg" };

            foreach (string file in remainingFiles)
            {
                string fileToMove = Path.Combine(basePath, file);
                destinationPath = Path.Combine(Application.StartupPath, file);
                File.Delete(destinationPath);
                File.Move(fileToMove, destinationPath);
            }

            Utility.DeleteDirectory(new DirectoryInfo(zipFileExtractionPath));
        }



        internal static void EnsureLabelDatabase()
        {
            EnsureFireBirdSetup();
            using (FbConnection connection = new FbConnection(Atlas.MapLabelsDatabaseConnectionString))
            {
                connection.Open();

                using (FbCommand command = new FbCommand())
                {
                    command.CommandText = "Select First 1 1 From Labels";
                    command.Connection = connection;

                    try
                    {
                        command.ExecuteNonQuery();
                        return;
                    }
                    catch (FbException dbError)
                    {
                        if (!dbError.Message.Contains("SQL error code = -204\r\nTable unknown\r\nLABELS"))
                        {
                            throw;
                        }
                    }

                    command.CommandText = "Create Table Labels(X SmallInt Not Null, Y SmallInt Not Null, Facet SmallInt Not Null, Category VarChar(1000) Not Null, Text VarChar(1000) Not Null);";
                    command.ExecuteNonQuery();
                }
            }
        }




        internal static readonly Dictionary<string, LabelCategory> LabelCategories = new Dictionary<string, LabelCategory>();



        internal static Label[] GetLabels(Rectangle bounds)
        {
            throw new NotImplementedException();
        }



        internal static void DeleteAllLabels()
        {
            EnsureFireBirdSetup();
            EnsureLabelDatabase();

            using (FbConnection connection = new FbConnection(Atlas.MapLabelsDatabaseConnectionString))
            {
                connection.Open();

                using (FbCommand command = new FbCommand())
                {
                    command.CommandText = "Delete From Labels";
                    command.Connection = connection;
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}