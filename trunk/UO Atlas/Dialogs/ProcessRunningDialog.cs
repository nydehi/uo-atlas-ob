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
    public partial class ProgressRunningDialog : Form
    {
        private Action _CancelProcessAction;


        public ProgressRunningDialog()
        {
            InitializeComponent();

            btDone.Enabled = false;
        }


        public ProgressRunningDialog(IStatusProvider statusProvider, Action cancelProcessAction)        {
            statusProvider.StatusChanged += ProgressReported;
            _CancelProcessAction = cancelProcessAction;

            InitializeComponent();

            btDone.Enabled = false;
            btAbort.Enabled = cancelProcessAction != null;
        }



        public void ProgressReported(int percentOfProgress, string status)
        {
            if(InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(delegate() { ProgressReported(percentOfProgress, status); }));
                return;
            }

            progressBar.Value = percentOfProgress;
            lblStatus.Text = status;

            if (percentOfProgress == 100)
            {
                btAbort.Enabled = false;
                btDone.Enabled = true;
            }
        }



        private void btAbort_Click(object sender, EventArgs e)
        {
            if (_CancelProcessAction != null)
            {
                _CancelProcessAction();
            }

            Close();
        }



        private void btDone_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}