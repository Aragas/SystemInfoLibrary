/*
 * Little Software Stats - .NET Library
 * Copyright (C) 2008-2012 Little Apps (http://www.little-apps.org)
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Lesser General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser General Public License for more details.
 * 
 * You should have received a copy of the GNU Lesser General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LittleSoftwareStatsNET
{
    public partial class Form1 : Form
    {
        LittleSoftwareStats.Watcher _watcher = new LittleSoftwareStats.Watcher();

        public Form1()
        {
            // Tracks Exceptions Automatically
            AppDomain.CurrentDomain.UnhandledException += (s, e) => _watcher.Exception(e.ExceptionObject as Exception);

            // Track WinForms Exceptions Automatically
            Application.ThreadException += (s, e) => _watcher.Exception(e.Exception);

            LittleSoftwareStats.Config.Enabled = true;

            InitializeComponent();

            this.comboBoxLicense.SelectedIndex = 0;
        }

        private void buttonEvent_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.textBoxEventCategory.Text.Trim()) || string.IsNullOrEmpty(this.textBoxEventName.Text.Trim()))
            {
                MessageBox.Show(this, "A required field is empty", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            this._watcher.Event(this.textBoxEventCategory.Text.Trim(), this.textBoxEventName.Text.Trim());
            MessageBox.Show(this, "Tracked: Event", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void buttonEventValue_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.textBoxEventValueCategory.Text.Trim()) || string.IsNullOrEmpty(this.textBoxEventValueName.Text.Trim()) || string.IsNullOrEmpty(this.textBoxEventValueValue.Text.Trim()))
            {
                MessageBox.Show(this, "A required field is empty", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            this._watcher.EventValue(this.textBoxEventValueCategory.Text.Trim(), this.textBoxEventValueName.Text.Trim(), this.textBoxEventValueValue.Text.Trim());
            MessageBox.Show(this, "Tracked: Event Value", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void buttonEventPeriod_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.textBoxEventPeriodCategory.Text.Trim()) || string.IsNullOrEmpty(this.textBoxEventPeriodName.Text.Trim()))
            {
                MessageBox.Show(this, "A required field is empty", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            this._watcher.EventPeriod(this.textBoxEventPeriodCategory.Text.Trim(), this.textBoxEventPeriodName.Text.Trim(), (int)this.numericUpDownEventPeriodDuration.Value, this.checkBoxEventPeriodCompleted.Checked);
            MessageBox.Show(this, "Tracked: Event Period", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void buttonLogMessage_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.textBoxLogMessage.Text.Trim()))
            {
                MessageBox.Show(this, "A required field is empty", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            this._watcher.Log(this.textBoxLogMessage.Text.Trim());
            MessageBox.Show(this, "Tracked: Log Message", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void buttonCustomData_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.textBoxCustomDataName.Text.Trim()) || string.IsNullOrEmpty(this.textBoxCustomDataValue.Text.Trim()))
            {
                MessageBox.Show(this, "A required field is empty", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            this._watcher.CustomData(this.textBoxCustomDataName.Text.Trim(), this.textBoxCustomDataValue.Text.Trim());
            MessageBox.Show(this, "Tracked: Custom Data", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void buttonLicense_Click(object sender, EventArgs e)
        {
            switch (this.comboBoxLicense.SelectedItem as string)
            {
                case "Free":
                    {
                        this._watcher.License(LittleSoftwareStats.Watcher.Licenses.Free);
                        break;
                    }
                case "Trial":
                    {
                        this._watcher.License(LittleSoftwareStats.Watcher.Licenses.Trial);
                        break;
                    }
                case "Demo":
                    {
                        this._watcher.License(LittleSoftwareStats.Watcher.Licenses.Demo);
                        break;
                    }
                case "Registered":
                    {
                        this._watcher.License(LittleSoftwareStats.Watcher.Licenses.Registered);
                        break;
                    }
                case "Cracked":
                    {
                        this._watcher.License(LittleSoftwareStats.Watcher.Licenses.Cracked);
                        break;
                    }
            }

            MessageBox.Show(this, "Tracked: License", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void buttonHandledException_Click(object sender, EventArgs e)
        {
            try
            {
                int i = 0;
                int n = 10 / i;
            }
            catch (DivideByZeroException ex)
            {
                this._watcher.Exception(ex);
            }

            MessageBox.Show(this, "Tracked: Handled exception", "Little Software Stats", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void buttonUnhandledException_Click(object sender, EventArgs e)
        {
            //int i = 0;
            //int n = 10 / i;
            throw new DivideByZeroException();

            MessageBox.Show(this, "Tracked: Unhandled exception", "Little Software Stats", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show(this, "Are You Sure?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                e.Cancel = true;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this._watcher.Started)
                this._watcher.Stop();
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (this.textBoxAppID.Text.Length != 32)
            {
                MessageBox.Show(this, "Invalid application ID", "Little Software Stats", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrEmpty(this.textBoxAppVer.Text) ||
                !System.Text.RegularExpressions.Regex.IsMatch(this.textBoxAppVer.Text, @"^(?:(\d+)\.)?(?:(\d+)\.)?(\*|\d+)$"))
            {
                MessageBox.Show(this, "Invalid application version", "Little Software Stats", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            this._watcher.Start(this.textBoxAppID.Text, this.textBoxAppVer.Text);

            MessageBox.Show(this, "Started application tracking", "Little Software Stats", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            if (!this._watcher.Started)
            {
                MessageBox.Show(this, "Tracking has not been started or it has already been stopped", "Little Software Stats", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            this._watcher.Stop();

            MessageBox.Show(this, "Stopped application tracking", "Little Software Stats", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
