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
        private bool m_HideControls = false;

        /// <summary>
        /// Get or set whether the player's location will be automatically centered
        /// </summary>
        public bool TrackPlayer
        {
            get { return m_TrackPlayer; }
            set
            {
                m_TrackPlayer = value;

                timerTrackPlayer.Enabled = value;
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
            //Ultima.Client.Calibrate();

            mapViewer.OnMapChanged += new EventHandler<MapViewerEventArgs>(OnMapChanged);
            mapViewer.OnZoomLevelChanged += new EventHandler<MapViewerEventArgs>(OnZoomLevelChanged);
            mapViewer.OnError += new ErrorEventHandler(OnError);

            mapViewer.Map = Map.Get(MapName.Felucca);
            cbZoom.SelectedIndex = (int)ZoomLevel.Percent100;
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
            lblHoveredCoords.Text = String.Format("({0}, {1})", mapViewer.HoveredLocation.X, mapViewer.HoveredLocation.Y);
        }

        private void menuTrackPlayer_CheckedChanged(object sender, EventArgs e)
        {
            TrackPlayer = menuTrackPlayer.Checked;
        }

        private void timerTrackPlayer_Tick(object sender, EventArgs e)
        {
            /*int x = 0, y = 0, z = 0, mapIndex = 0;

            if (Ultima.Client.FindLocation(ref x, ref y, ref z, ref mapIndex))
            {
                Map playerMap = Map.Get((MapName)mapIndex);

                if (mapViewer.Map != playerMap)
                    mapViewer.Map = playerMap;

                mapViewer.SetLocation(x, y);
            }*/
        }

        private void menuHideControls_CheckedChanged(object sender, EventArgs e)
        {
            HideControls = menuHideControls.Checked;
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
    }
}