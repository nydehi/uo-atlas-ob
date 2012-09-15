using System;
using System.IO;
using System.Threading;
using FirebirdSql.Data.FirebirdClient;
using UO_Atlas.Dialogs;



namespace UO_Atlas
{
    class MapLabelImporter : IStatusProvider, IDisposable
    {
        private ProgressRunningDialog _ProcessDialog;
        private Thread _WorkerThread;
        private string[] _FilesToImportAsLabels;


        public event StatusChangedEventHandler StatusChanged = delegate { };

        


        public MapLabelImporter(string[] fullPathsToFilesToImportAsLabels)
        {
            if(fullPathsToFilesToImportAsLabels == null)
            {
                throw new ArgumentNullException();
            }

            _FilesToImportAsLabels = fullPathsToFilesToImportAsLabels;
        }



        public void Start()
        {
            _ProcessDialog = new ProgressRunningDialog(this, Abort);
            _ProcessDialog.FormClosed += OnProcessDialogFormClosed;

            _WorkerThread = new Thread(Import);
            _WorkerThread.Start();

            _ProcessDialog.ShowDialog();
        }



        private void OnProcessDialogFormClosed(object sender, EventArgs e)
        {
            Dispose();
        }



        private void Import()
        {
            int progressDelta = Convert.ToInt32(Math.Round(100 / (double)_FilesToImportAsLabels.Length));


            using(FbConnection dbConnection = new FbConnection(Atlas.MapLabelsDatabaseConnectionString))
            {
                dbConnection.Open();

                using(FbCommand dbCommand = new FbCommand("Insert Into Labels(X, Y, Facet, Category, Text) Values(@X, @Y, @Facet, @Category, @Text)", dbConnection))
                {
                    FbParameter dbParameterX = dbCommand.Parameters.Add("@X", FbDbType.SmallInt);
                    FbParameter dbParameterY = dbCommand.Parameters.Add("@Y", FbDbType.SmallInt);
                    FbParameter dbParameterFacet = dbCommand.Parameters.Add("@Facet", FbDbType.SmallInt);
                    FbParameter dbParameterCategory = dbCommand.Parameters.Add("@Category", FbDbType.VarChar);
                    FbParameter dbParameterText = dbCommand.Parameters.Add("@Text", FbDbType.VarChar);
                    

                    for (int fileIndex = 0; fileIndex < _FilesToImportAsLabels.Length; fileIndex += 1)
                    {
                        string fileToImportAsLabels = _FilesToImportAsLabels[fileIndex];
                        ReportProgress(fileIndex * progressDelta + 1, String.Format("Importing labels in {0}... (this may take a few minutes)", Path.GetFileName(fileToImportAsLabels)));

                        if (string.IsNullOrEmpty(fileToImportAsLabels) || !File.Exists(fileToImportAsLabels))
                        {
                            continue;
                        }

                        using (StreamReader reader = new StreamReader(fileToImportAsLabels))
                        {
                            // The first line is a version number that can be ignored.
                            reader.ReadLine();

                            Label label;
                            do
                            {
                                label = Label.LoadFrom(reader);
                                if (label != null)
                                {
                                    //Console.WriteLine(label.Category.Name + ": " + label.Text);
                                    dbParameterX.Value = label.X;
                                    dbParameterY.Value = label.Y;
                                    dbParameterFacet.Value = label.Facet;
                                    dbParameterCategory.Value = label.Category.Name;
                                    dbParameterText.Value = label.Text;

                                    dbCommand.ExecuteNonQuery();
                                }
                            } while (label != null);
                        }
                    }
                }
            }

            ReportProgress(100, "Label import complete.");
        }



        private void ReportProgress(int percentOfProgress, string status)
        {
            StatusChanged(percentOfProgress, status);
        }



        public void Abort()
        {
            _WorkerThread.Abort();
        }



        #region Implementation of IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            if(_ProcessDialog != null)
            {
                _ProcessDialog.FormClosed -= OnProcessDialogFormClosed;
                _ProcessDialog.Dispose();
                _ProcessDialog = null;
            }

            _WorkerThread = null;
            _FilesToImportAsLabels = null;
            StatusChanged = null;
        }


        #endregion
    }
}