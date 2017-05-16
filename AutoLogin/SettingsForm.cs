﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace AutoLogin
{
    public partial class SettingsForm : Form
    {
        MainForm mForm;

        public SettingsForm()
        {
            InitializeComponent();
        }

        public void ShowMe(MainForm parent)
        {
            mForm = parent;
            this.ShowDialog(parent);
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            // Load from settings
            txtWowPath.Text = MainForm.SETTINGS.WowPath;
            chkUpdate.Checked = MainForm.SETTINGS.AutoUpdate;
            chkMinimize.Checked = MainForm.SETTINGS.Minimize;
            chkHide.Checked = MainForm.SETTINGS.Hide;
            numEnterWorldDelay.Value = MainForm.SETTINGS.EnterWorldDelay / 1000;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Check to see if Wow.exe exists in the selected folder
            if (!File.Exists(txtWowPath.Text + @"\Wow.exe"))
            {
                MessageBox.Show("Could not find Wow.exe." + Environment.NewLine + "Please browse to your World of Warcraft folder.");
            }
            else
            {
                MainForm.SETTINGS.WowPath = txtWowPath.Text;
                MainForm.SETTINGS.AutoUpdate = chkUpdate.Checked;
                MainForm.SETTINGS.Minimize = chkMinimize.Checked;
                MainForm.SETTINGS.Hide = chkHide.Checked;
                MainForm.SETTINGS.EnterWorldDelay = (int)numEnterWorldDelay.Value * 1000;
                mForm.SaveSettings();
                this.Close();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            // Open new folder browser dialog
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                txtWowPath.Text = fbd.SelectedPath;
            }
        }

        private void btnPassword_Click(object sender, EventArgs e)
        {
            new PasswordForm().ShowDialog(this);
        }
    }
}
