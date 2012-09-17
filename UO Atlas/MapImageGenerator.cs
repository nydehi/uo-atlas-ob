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
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.ComponentModel;
using System.Threading;
using System.Drawing.Imaging;


namespace UO_Atlas
{
    public class MapImageGenerator : IStatusProvider
    {
        private Dialogs.ProgressRunningDialog _ProcessDialog;
        private Thread m_WorkerThread;

        public event StatusChangedEventHandler StatusChanged;
        


        public void Start()
        {
            _ProcessDialog = new Dialogs.ProgressRunningDialog(this, Abort);

            m_WorkerThread = new Thread(Calculate);
            m_WorkerThread.Start();

            _ProcessDialog.ShowDialog();
        }



        private void Calculate()
        {
            string savePath = Path.Combine(Atlas.MapImagesFolder, "map{0}-{1}.bmp");

            Array mapNames = Enum.GetValues(typeof(MapName));
            int progressDelta = Convert.ToInt32(Math.Round(100 / (double)mapNames.Length));
            foreach (MapName mapName in mapNames)
            {
                int mapIndex = (int) mapName;

                Map currentMap = Map.Get(mapName);


                ReportProgress(mapIndex * progressDelta + 1, String.Format("Extracting Map {0}... (this may take a while)", mapIndex));

                Image mapImage = currentMap.ToImage(true);

                // Zoom 1/16
                Image mapImageResized = new Bitmap(mapImage, new Size(currentMap.Width / 16, currentMap.Height / 16));
                mapImageResized.Save(String.Format(savePath, mapIndex, "6.25%"), ImageFormat.Png);
                mapImageResized.Dispose();

                // Zoom 1/8
                mapImageResized = new Bitmap(mapImage, new Size(currentMap.Width / 8, currentMap.Height / 8));
                mapImageResized.Save(String.Format(savePath, mapIndex, "12.5%"), ImageFormat.Png);
                mapImageResized.Dispose();

                // Zoom 1/4
                mapImageResized = new Bitmap(mapImage, new Size(currentMap.Width / 4, currentMap.Height / 4));
                mapImageResized.Save(String.Format(savePath, mapIndex, "25%"), ImageFormat.Png);
                mapImageResized.Dispose();

                // Zoom 1/2
                mapImageResized = new Bitmap(mapImage, new Size(currentMap.Width / 2, currentMap.Height / 2));
                mapImageResized.Save(String.Format(savePath, mapIndex, "50%"), ImageFormat.Png);
                mapImageResized.Dispose();

                // Zoom 100%
                mapImage.Save(String.Format(savePath, mapIndex, "100%"), ImageFormat.Bmp);
                mapImage.Dispose();

                GC.Collect();
                // GC.WaitForPendingFinalizers();
            }
            
            ReportProgress(100, "Image Update completed");
        }


        private void ReportProgress(int percentOfProgress, string status)
        {
            StatusChanged(percentOfProgress, status);
        }


        public void Abort()
        {
            m_WorkerThread.Abort();
        }
    }
}
