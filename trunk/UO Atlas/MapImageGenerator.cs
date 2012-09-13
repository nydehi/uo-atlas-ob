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
    public static class MapImageGenerator
    {
        private static Dialogs.MapImageGenerationDialog MapImageGenerationDialog = new Dialogs.MapImageGenerationDialog();
        private static Thread calcThread;

        public static event OnImageUpdateHandler OnImageUpdate;
        public delegate void OnImageUpdateHandler(int PercentOfProgress, string StatusMessage);

        public static void Start()
        {
            calcThread = new Thread(new ThreadStart(Calculate));
            calcThread.Start();

            MapImageGenerationDialog.ShowDialog();
        }

        public static void Calculate()
        {
            string savePath = Path.Combine(Atlas.MapImagesFolder, "map{0}-{1}.png");

            Array mapNames = Enum.GetValues(typeof(MapName));
            int progressDelta = Convert.ToInt32(Math.Round(100 / (double)mapNames.Length));
            foreach (MapName mapName in mapNames)
            {
                int mapIndex = (int) mapName;

                Map currentMap = Map.Get(mapName);


                ReportProgress(mapIndex * progressDelta + 1, String.Format("Extracting Map {0}... (this may take a while)", mapIndex));

                Image mapImage = currentMap.ToImage(true);

                //// Zoom 50%
                //Image mapImage_resized = new Bitmap(mapImage, new Size(currentMap.Width / 16, currentMap.Height / 16));
                //mapImage_resized.Save(String.Format(savePath, mapIndex, "50%"), ImageFormat.Png);
                //mapImage_resized.Dispose();

                //// Zoom 100%
                //mapImage_resized = new Bitmap(mapImage, new Size(currentMap.Width / 8, currentMap.Height / 8));
                //mapImage_resized.Save(String.Format(savePath, mapIndex, "100%"), ImageFormat.Png);
                //mapImage_resized.Dispose();

                //// Zoom 200%
                //mapImage_resized = new Bitmap(mapImage, new Size(currentMap.Width / 4, currentMap.Height / 4));
                //mapImage_resized.Save(String.Format(savePath, mapIndex, "200%"), ImageFormat.Png);
                //mapImage_resized.Dispose();

                //// Zoom 400%
                //mapImage_resized = new Bitmap(mapImage, new Size(currentMap.Width / 2, currentMap.Height / 2));
                //mapImage_resized.Save(String.Format(savePath, mapIndex, "400%"), ImageFormat.Png);
                //mapImage_resized.Dispose();

                //// Zoom 800%
                //mapImage.Save(String.Format(savePath, mapIndex, "800%"), ImageFormat.Png);
                //mapImage.Dispose();

                // Zoom 1/16
                Image mapImage_resized = new Bitmap(mapImage, new Size(currentMap.Width / 16, currentMap.Height / 16));
                mapImage_resized.Save(String.Format(savePath, mapIndex, "6.25%"), ImageFormat.Png);
                mapImage_resized.Dispose();

                // Zoom 1/8
                mapImage_resized = new Bitmap(mapImage, new Size(currentMap.Width / 8, currentMap.Height / 8));
                mapImage_resized.Save(String.Format(savePath, mapIndex, "12.5%"), ImageFormat.Png);
                mapImage_resized.Dispose();

                // Zoom 1/4
                mapImage_resized = new Bitmap(mapImage, new Size(currentMap.Width / 4, currentMap.Height / 4));
                mapImage_resized.Save(String.Format(savePath, mapIndex, "25%"), ImageFormat.Png);
                mapImage_resized.Dispose();

                // Zoom 1/2
                mapImage_resized = new Bitmap(mapImage, new Size(currentMap.Width / 2, currentMap.Height / 2));
                mapImage_resized.Save(String.Format(savePath, mapIndex, "50%"), ImageFormat.Png);
                mapImage_resized.Dispose();

                // Zoom 100%
                mapImage.Save(String.Format(savePath, mapIndex, "100%"), ImageFormat.Png);
                mapImage.Dispose();

                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            
            ReportProgress(100, "Image Update completed");
        }

        private static void ReportProgress(int PercentOfProgress, string status)
        {
            if (OnImageUpdate != null)
                OnImageUpdate(PercentOfProgress, status);
        }

        public static void Abort()
        {
            calcThread.Abort();
        }
    }
}
