using System;
using System.Windows.Forms;

namespace RealSenseWallAppUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            InitializeCustomComponents();
        }

        private void InitializeCustomComponents()
        {
            // Button for Calibration
            Button btnCalibrate = new Button();
            btnCalibrate.Text = "Calibrate Wall";
            btnCalibrate.Location = new System.Drawing.Point(20, 20);
            btnCalibrate.Click += BtnCalibrate_Click;
            Controls.Add(btnCalibrate);

            // Button to Start Detection
            Button btnStartDetection = new Button();
            btnStartDetection.Text = "Start Detection";
            btnStartDetection.Location = new System.Drawing.Point(20, 60);
            btnStartDetection.Click += BtnStartDetection_Click;
            Controls.Add(btnStartDetection);

            // Button to Test System
            Button btnTestSystem = new Button();
            btnTestSystem.Text = "Test System";
            btnTestSystem.Location = new System.Drawing.Point(20, 100);
            btnTestSystem.Click += BtnTestSystem_Click;
            Controls.Add(btnTestSystem);

            // Button to Exit
            Button btnExit = new Button();
            btnExit.Text = "Exit";
            btnExit.Location = new System.Drawing.Point(20, 140);
            btnExit.Click += BtnExit_Click;
            Controls.Add(btnExit);
        }

        private void BtnCalibrate_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Calibration Started");
            // Code to start the calibration process
        }

        private void BtnStartDetection_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Detection Started");
            // Code to start the detection process
        }

        private void BtnTestSystem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Testing System");
            // Code to test system
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
