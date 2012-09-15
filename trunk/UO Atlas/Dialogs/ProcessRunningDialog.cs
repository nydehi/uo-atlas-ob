/***************************************************************************
 *
 *   This program is free software; you can redistribute it and/or modify
 *   it under the terms of the GNU General Public License as published by
 *   the Free Software Foundation; either version 2 of the License, or
 *   (at your option) any later version.
 *
 ***************************************************************************/

using System;
using System.Windows.Forms;



namespace UO_Atlas.Dialogs
{
    public partial class ProgressRunningDialog : Form
    {
        private IStatusProvider _StatusProvider;
        private Action _CancelProcessAction;


        public ProgressRunningDialog()
        {
            InitializeComponent();

            btDone.Enabled = false;
        }


        public ProgressRunningDialog(IStatusProvider statusProvider, Action cancelProcessAction)        {
            statusProvider.StatusChanged += ProgressReported;
            _StatusProvider = statusProvider;
            _CancelProcessAction = cancelProcessAction;

            InitializeComponent();

            btDone.Enabled = false;
            btAbort.Enabled = cancelProcessAction != null;
        }



        public void ProgressReported(int percentOfProgress, string status)
        {
            if(InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(delegate { ProgressReported(percentOfProgress, status); }));
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



        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if(components != null)
                {
                    components.Dispose();
                }

                _CancelProcessAction = null;

                if(_StatusProvider != null)
                {
                    _StatusProvider.StatusChanged -= ProgressReported;
                    _StatusProvider = null;
                }
            }
            base.Dispose(disposing);
        }
    }
}