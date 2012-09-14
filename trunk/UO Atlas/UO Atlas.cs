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
        public static string MapLabelsDatabaseConnectionString
        {
            get
            {
                string dbPath = Path.Combine(Atlas.MapLabelsFolder, "database.fdb");

                

                        //using(MemoryStream ms = new MemoryStream(Resources.FireBirdFiles))
                        //{


                            //DeflateStream zipStream = new DeflateStream(ms, CompressionMode.Decompress);

                            //int bytesRead;
                            //do
                            //{
                            //    bytesRead = zipStream.Read(
                            //}

                            //byte[] decompressedBuffer = new byte[streamData.Length + 100];
                            //// Use the ReadAllBytesFromStream to read the stream.
                            //int totalCount = DeflateTest.ReadAllBytesFromStream(zipStream, decompressedBuffer);
                            //Console.WriteLine("Decompressed {0} bytes", totalCount);
                        //}
                //}
                

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
    }
}