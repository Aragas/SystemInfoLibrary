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
            InitializeComponent();
            // Must be set to true in order for data to be sent
            LittleSoftwareStats.Config.Enabled = true;
            // Tells library to start collecting data
            this.m_watcher.Start("YOURAPPID", "YOURAPPVERSION");
        }
 
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Make sure libary is still started
            if (this._watcher.Started)
                // If it is then stop it and send data before application exits
                this._watcher.Stop();
        }
    }
}