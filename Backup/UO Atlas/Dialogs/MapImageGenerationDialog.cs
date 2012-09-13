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
using System.Threading;


namespace UO_Atlas.Dialogs
{
    public partial class MapImageGenerationDialog : Form
    {
        private delegate void ProgressReportedHandler(int percentOfProgress, string status);

        public MapImageGenerationDialog()
        {
            InitializeComponent();
            
            MapImageGenerator.OnImageUpdate += new MapImageGenerator.OnImageUpdateHandler(ProgressReported);
            btAbort.Enabled = true;
            btDone.Enabled = false;
        }

        public void ProgressReported(int percentOfProgress, string status)
        {
            if(InvokeRequired)
            {
                BeginInvoke(new ProgressReportedHandler(ProgressReported), percentOfProgress, status);
            }
            else
            {
                progressBar.Value = percentOfProgress;
                lblStatus.Text = status;

                if (percentOfProgress == 100)
                {
                    btAbort.Enabled = false;
                    btDone.Enabled = true;
                }
            }
        }

        private void btAbort_Click(object sender, EventArgs e)
        {
            MapImageGenerator.Abort();
            Close();
        }

        private void btDone_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}