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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace UO_Atlas
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private bool m_TrackPlayer = false;
        private PlayerLocation m_LastKnownLocation;

        private bool m_HideControls = false;

        /// <summary>
        /// Get or set whether the player's location will be automatically centered
        /// </summary>
        public bool TrackPlayer
        {
            get { return m_TrackPlayer; }
            set
            {
                if (!m_TrackPlayer && value)
                {
                    CreatePlayerTrackingBackgroundWorker();
                }

                m_TrackPlayer = value;
            }
        }



        private void CreatePlayerTrackingBackgroundWorker()
        {
            BackgroundWorker bw = new BackgroundWorker();
            bw.WorkerReportsProgress = true;
            bw.DoWork += PlayerTrackingBackgroundWorkerDoWork;
            bw.ProgressChanged += PlayerTrackingBackgroundWorkerProgressChanged;
            bw.RunWorkerAsync();
        }



        private void PlayerTrackingBackgroundWorkerProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage < 0)
            {
                lblPlayerCoords.Text = "(?,?)";
                return;
            }

            PlayerLocation location = (PlayerLocation) e.UserState;
            if (m_LastKnownLocation.X == location.X && m_LastKnownLocation.Y == location.Y && m_LastKnownLocation.Facet == location.Facet)
            {
                // There has been no change in location.
                return;
            }

            m_LastKnownLocation = location;

            if (mapViewer.Map != location.Facet)
            {
                mapViewer.Map = location.Facet;
            }

            mapViewer.SetLocation(location.X, location.Y);

            lblPlayerCoords.Text = string.Concat('(', location.X, ',', location.Y, ')');
        }

        private void PlayerTrackingBackgroundWorkerDoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker bw = (BackgroundWorker) sender;

            int trackPlayerDelayInMilliseconds = 1000;
            DateTime nextPlayerTrackingTimestamp = DateTime.Now.AddMilliseconds(-trackPlayerDelayInMilliseconds);

            while (m_TrackPlayer)
            {
                while (nextPlayerTrackingTimestamp > DateTime.Now && m_TrackPlayer)
                {
                    System.Threading.Thread.Sleep(100);
                }
                if (!m_TrackPlayer)
                {
                    bw.ReportProgress(-1);
                    return;
                }

                int x = 0;
                int y = 0;
                int z = 0;
                int facet = 0;
                if (!Ultima.Client.FindLocation(ref x, ref y, ref z, ref facet))
                {
                    Ultima.Client.Calibrate();
                    if (!Ultima.Client.FindLocation(ref x, ref y, ref z, ref facet))
                    {
                        bw.ReportProgress(-1);
                    }
                }

                PlayerLocation location = new PlayerLocation();
                location.Facet = Map.Get((MapName) facet);
                location.X = x;
                location.Y = y;
                location.Z = z;

                bw.ReportProgress(1, location);
                nextPlayerTrackingTimestamp = DateTime.Now.AddMilliseconds(trackPlayerDelayInMilliseconds);
            }
        }


        /// <summary>
        /// Get or set the option to hide all controls except the MapViewer
        /// </summary>
        public bool HideControls
        {
            get { return m_HideControls; }
            set
            {
                m_HideControls = value;

                menuStrip.Visible = !m_HideControls;
                statusStrip.Visible = !m_HideControls;

                if (m_HideControls)
                    FormBorderStyle = FormBorderStyle.None;
                else
                    FormBorderStyle = FormBorderStyle.Sizable;

                menuHideControls.Checked = m_HideControls;
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            LabelCategory.LoadCache();

            mapViewer.OnMapChanged += new EventHandler<MapViewerEventArgs>(OnMapChanged);
            mapViewer.OnZoomLevelChanged += new EventHandler<MapViewerEventArgs>(OnZoomLevelChanged);
            mapViewer.OnError += new ErrorEventHandler(OnError);

            mapViewer.Map =  Map.Get(MapName.Felucca);
            cbZoom.SelectedIndex = (int)ZoomLevel.PercentOneHundred;

            menuTrackPlayer.Checked = true;
        }

        private void OnMapChanged(object sender, MapViewerEventArgs e)
        {
            tabControlMaps.SelectedIndex = e.Map.Index;
        }

        private void OnZoomLevelChanged(object sender, MapViewerEventArgs e)
        {
           cbZoom.SelectedIndex = (int)mapViewer.ZoomLevel ;
        }

        private void OnError(object sender, string ErrorMessage)
        {
            MessageBox.Show(ErrorMessage, "An error occured:", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void MainForm_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta < 0)
            {
                mapViewer.Zoom(-1);
            }
            else if (e.Delta > 0)
            {
                mapViewer.Zoom(1);
            }
        }

        private void mapViewer_MouseMove(object sender, MouseEventArgs e)
        {
            // Show the hovered coordinates in the statusbar
            lblHoveredCoords.Text = string.Concat('(', mapViewer.HoveredLocation.X, ',', mapViewer.HoveredLocation.Y, ')');
        }

        private void menuTrackPlayer_CheckedChanged(object sender, EventArgs e)
        {
            TrackPlayer = menuTrackPlayer.Checked;
        }

        private void menuHideControls_CheckedChanged(object sender, EventArgs e)
        {
            HideControls = menuHideControls.Checked;

            if (HideControls)
            {
                TopMost = true;
            }
            else if (!menuStayOnTop.Checked)
            {
                TopMost = false;
            }            
        }

        private void menuSettings_Click(object sender, EventArgs e)
        {
            Point location = mapViewer.PaintLocation;
            mapViewer.Dispose();
            mapViewer = null;
            MapImageGenerator.Start();
            Application.Restart();
        }

        private void menuExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void menuAbout_Click(object sender, EventArgs e)
        {
            Dialogs.AboutBox about = new UO_Atlas.Dialogs.AboutBox();
            about.ShowDialog();
        }

        private void tabControlMaps_SelectedIndexChanged(object sender, EventArgs e)
        {
            mapViewer.Map = Map.Get((MapName)tabControlMaps.SelectedIndex);
        }

        private void cbZoom_SelectedIndexChanged(object sender, EventArgs e)
        {
            mapViewer.ZoomLevel = (ZoomLevel)cbZoom.SelectedIndex;
        }

        private void mapViewer_DoubleClick(object sender, EventArgs e)
        {
            HideControls = !HideControls;
        }

        private void menuStayOnTop_CheckedChanged(object sender, EventArgs e)
        {
            TopMost = menuStayOnTop.Checked;
        }

        private void menuImportLabels_Click(object sender, EventArgs e)
        {
            string[] filesToImportAsLabels;

            using(OpenFileDialog d = new OpenFileDialog())
            {
                d.CheckFileExists = true;
                d.CheckPathExists = true;
                d.Multiselect = true;
                d.ReadOnlyChecked = true;
                
                if(d.ShowDialog(this) != DialogResult.OK)
                {
                    return;
                }

                filesToImportAsLabels = d.FileNames;
            }

            foreach (string fileToImportAsLabels in filesToImportAsLabels)
            {
                using (StreamReader reader = new StreamReader(fileToImportAsLabels))
                {
                    // The first line is a version number that can be ignored.
                    reader.ReadLine();

                    UO_Atlas.Label label;
                    do
                    {
                        label = Label.LoadFrom(reader);
                        if (label != null)
                        {
                            System.Diagnostics.Debug.WriteLine(label.Category.Name + ": " + label.Text);
                        }
                    } while (label != null);
                    
                }
            }
        }
    }
}