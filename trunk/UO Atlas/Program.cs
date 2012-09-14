using System;
using System.Collections;
using System.IO;
using System.Windows.Forms;
using FirebirdSql.Data.FirebirdClient;
using UO_Atlas.Properties;


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
            Path.Combine(Atlas.MapLabelsFolder, "data.fdb");

            string fireBirdReferenceFile = Path.Combine(Application.StartupPath, "FirebirdSql.Data.FirebirdClient.dll");
            if (!File.Exists(fireBirdReferenceFile))
            {
                string fireBirdZipFile = Path.Combine(Application.StartupPath, "fireBirdFiles.zip");
                using (FileStream outStream = new FileStream(fireBirdZipFile, FileMode.Create))
                {
                    byte[] streamData = Resources.FireBirdFiles;
                    outStream.Write(streamData, 0, streamData.Length);
                }

                EnsureShell32();

                ExtractFireBirdFiles(fireBirdZipFile);
                //Shell32.ShellClass shellClass = new Shell32.ShellClass();
                //Shell32.Folder srcFolder = shellClass.NameSpace(fireBirdZipFile);
                //Shell32.Folder destFolder = shellClass.NameSpace(Application.StartupPath);
                //Shell32.FolderItems items = srcFolder.Items();
                //destFolder.CopyHere(items, 20);


                //using(FbConnection connection = new FbConnection(Atlas.MapLabelsDatabaseConnectionString))
                //{
                //    connection.Open();

                //    using(FbCommand command = new FbCommand())
                //    {
                //        command.CommandText = "Create Table Labels(X SmallInt Not Null, Y SmallInt Not Null, Facet SmallInt Not Null, Text VarChar Not Null)";
                //        command.Connection = connection;

                //        command.ExecuteNonQuery();
                //    }
                //}
            }

            return;


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
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
            string extractionPath = Path.Combine(Application.StartupPath, Path.GetFileNameWithoutExtension(zipFile));
            Utility.EnsureDirectory(extractionPath);

            Shell32.ShellClass shellClass = new Shell32.ShellClass();
            Shell32.Folder srcFolder = shellClass.NameSpace(zipFile);
            Shell32.Folder destFolder = shellClass.NameSpace(extractionPath);
            destFolder.CopyHere(srcFolder.Items(), 16 | 256 | 512);


            string destinationPath = Path.Combine(Application.StartupPath, "FirebirdSql.Data.FirebirdClient.dll");
            File.Delete(destinationPath);
            File.Move(Path.Combine(extractionPath, "FirebirdSql.Data.FirebirdClient.dll"), destinationPath);

            string basePath = Path.Combine(Application.StartupPath, "x86");
            if (IntPtr.Size == 8)
            {
                basePath = Path.Combine(Application.StartupPath, "x64");
            }

            string[] remainingFiles = new string[] { "fbclient.dll", "icudt30.dll", "icuin30.dll", "icuuc30.dll" };
            
            foreach(string file in remainingFiles)
            {
                destinationPath = Path.Combine(basePath, file);
                File.Delete(destinationPath);
                File.Move(Path.Combine(extractionPath, file), destinationPath);
            }

            Utility.DeleteDirectory(extractionPath);
        }
    }
}