﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoLogin
{
    public partial class AddEditForm : Form
    {
        Account account;
        MainForm mForm;
        GetResolution getRes;
        int addEdit;

        public AddEditForm()
        {
            InitializeComponent();
        }

        public void ShowMe(MainForm parent, int addEdit, Account account = null)
        {
            if (addEdit == 0)
            {
                this.Text = "New Account";
            }
            else
            {
                this.Text = "Edit Account";
            }
            this.account = account;
            this.addEdit = addEdit;
            mForm = parent;
            this.ShowDialog(parent);
        }

        private void AddEditForm_Load(object sender, EventArgs e)
        {
            getRes = new GetResolution();
            lstAccounts.SelectedIndex = 0;
            lstCharacter.SelectedIndex = 0;

            drpRealm.DataSource = Options.GetRealms();

            if (account != null)
            {
                try
                {
                    txtName.Text = account.Name;
                    txtLogin.Text = account.Login;
                    txtPassword.Text = account.Password;
                    chkMultiple.Checked = account.Multiple;
                    if (account.Multiple)
                    {
                        if (account.AccountNames == null)
                        {
                            for (int i = 2; i < account.NumberAccounts; ++i)
                            {
                                numAccounts.Value += 1;
                            }
                        }
                        else
                        {
                            numAccounts.Value = account.NumberAccounts;
                            lstAccounts.Items.Clear();
                            lstAccounts.Items.AddRange(account.AccountNames);
                        }
                        lstAccounts.SelectedIndex = account.SelectedAccount;
                    }
                    chkRealm.Checked = account.SetRealm;
                    drpRealm.SelectedItem = account.Realm;
                    chkCharacter.Checked = account.SetCharacter;
                    lstCharacter.SelectedIndex = account.CharacterSlot;
                    chkEnterWorld.Checked = account.EnterWorld;
                }
                catch (Exception) { }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Check if password is entered
            if (txtLogin.Text == "")
            {
                MessageBox.Show("Login required!");
            }
            else if (txtPassword.Text == "")
            {
                MessageBox.Show("Password required!");
            }
            else
            {
                // If no name set use login
                if (txtName.Text == "")
                {
                    txtName.Text = txtLogin.Text;
                }

                // If "adding" create a new account
                if (account == null)
                {
                    account = new Account();
                    account.Client = "32bit";
                    MainForm.ACCOUNTS.Add(account);
                }

                // Save all account information to account object
                account.Name = txtName.Text;
                account.Login = txtLogin.Text;
                account.Password = txtPassword.Text;
                account.Multiple = chkMultiple.Checked;
                account.NumberAccounts = chkMultiple.Checked ? (int)numAccounts.Value : 0;
                account.AccountNames = new string[lstAccounts.Items.Count];
                lstAccounts.Items.CopyTo(account.AccountNames, 0);
                account.SelectedAccount = chkMultiple.Checked ? lstAccounts.SelectedIndices[0] : 0;
                account.SetRealm = chkRealm.Checked;
                account.Realm = drpRealm.Text;
                account.SetCharacter = chkCharacter.Checked;
                account.CharacterSlot = lstCharacter.SelectedIndex;
                account.EnterWorld = chkEnterWorld.Checked;
                mForm.refreshList(addEdit);
                this.Close();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void chkMultiple_CheckedChanged(object sender, EventArgs e)
        {
            if (chkMultiple.Checked && chkMultiple.Focused)
            {
                MessageBox.Show("In order for account selection to work\nyou need to enter the names as they\nare listed when you log into WoW.\nExample:\nWoW1\nWoW2");
            }
            lstAccounts.Enabled = chkMultiple.Checked ? true : false;
            numAccounts.Enabled = chkMultiple.Checked ? true : false;
        }

        private void numAccounts_ValueChanged(object sender, EventArgs e)
        {
            if (numAccounts.Value > lstAccounts.Items.Count)
            {
                lstAccounts.Items.Add("WoW" + numAccounts.Value);
            }
            else
            {
                lstAccounts.Items.RemoveAt(lstAccounts.Items.Count - 1);
            }
        }

        private void chkRealm_CheckedChanged(object sender, EventArgs e)
        {
            chkCharacter.Enabled = chkRealm.Checked ? true : false;
            drpRealm.Enabled = chkRealm.Checked ? true : false;
            drpRealm.SelectedIndex = 0;
        }

        private void chkCharacter_CheckedChanged(object sender, EventArgs e)
        {
            lstCharacter.Enabled = chkCharacter.Checked ? true : false;
            chkEnterWorld.Enabled = chkCharacter.Checked ? true : false;
        }

        private void lstAccounts_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = lstAccounts.IndexFromPoint(e.Location);
            new EditAccountNameForm().ShowMe(this, index);
        }
    }
}
