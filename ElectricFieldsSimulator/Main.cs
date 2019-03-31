using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using PhysicsElectricFieldsSimulator.GUI;

namespace PhysicsElectricFieldsSimulator
{
    public partial class Main : Form
    {
        //Timer time variable and game state variable
        public static float deltaTime;
        public static bool simEnabled;
        public static bool simRunning;
        // Stopwatches to record time since last update and refresh
        Stopwatch timeCounter = new Stopwatch();
        Stopwatch refreshCounter = new Stopwatch();
        const int FRAME_RATE = 60;
        const float FRAME_TIME = 1f / FRAME_RATE;
        const int MAX_CLIENT_HEIGHT = 800;
        const int MAX_CLIENT_WIDTH = 1300;
        // Number of active charges
        public static int numOfCharges = 0;
        // Stores size of the form
        public static Size size;

        // Proportionality constant to take the place of coulomb's constant to slow down simulation speed
        const int PROPORTIONALITY = 100;

        // GUI
        static UI gui;
        // Placement of charges
        static Placement placement = new Placement(false, null);
        // Physics engine
        static Physics physics;
        // Point charges
        static PCharge[] charges = new PCharge[numOfCharges];

        public Main()
        {
            InitializeComponent();

            ClientSize = new Size(MAX_CLIENT_WIDTH, MAX_CLIENT_HEIGHT);
            // Lock program size
            MaximizeBox = false;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            size = ClientSize;

            txtCharge.Enabled = false;
            txtMass.Enabled = false;
            btnReset.Enabled = false;
            btnCharge.Enabled = false;

            // GUI
            gui = new UI();
            // Physics
            physics = new Physics(PROPORTIONALITY);
        }

        void Timer()
        {
            timeCounter.Start();
            refreshCounter.Start();

            // Store previous and current frame's time
            float previousTime = 0;
            float currentTime = 0;

            // Timer loop that runs while sim is running, main program loop
            while (simEnabled == true)
            {
                // Store time passed as time elapsed since stop watch started last (stores the previous time that the loop ran (in milliseconds)
                currentTime = (float)timeCounter.Elapsed.TotalSeconds;
                // Set delta time as time change between last frame and current
                deltaTime = currentTime - previousTime;
                // Set previous time as current time for next frame to use
                previousTime = currentTime;

                // *Timer Activities*
                if (simRunning == true)
                {
                    charges = physics.ApplyEForce(charges);
                    RemoveCharges();
                    gui.Update(charges);
                }

                // If time from last refresh is greater than the frame time
                if (refreshCounter.Elapsed.TotalSeconds >= FRAME_TIME)
                {
                    // Redraws the screen (refresh)
                    Refresh();
                    // Restart stopwatch
                    refreshCounter.Restart();
                }
                // Allow other interactions/code while this loop is active
                Application.DoEvents();
            }
        }

        bool IllegalArea(Rectangle bounds)
        {
            foreach (Control control in Controls)
            {
                if (control.Bounds.IntersectsWith(bounds) == true)
                    return true;
            }
            foreach (PCharge charge in charges)
            {
                if ((charge.Bounds.IntersectsWith(bounds) == true || ClientRectangle.Contains(bounds) == false) && charge.Active == true)
                    return true;
            }
            return false;
        }

        void RemoveCharges()
        {
            for (int i = 0; i < charges.Length; i++)
            {
                if (ClientRectangle.Contains(Point.Round(charges[i].Bounds.Location)) == false && charges[i].Active == true)
                {
                    PCharge[] tempCharges = new PCharge[numOfCharges];
                    tempCharges = charges;
                    numOfCharges--;
                    charges = new PCharge[numOfCharges];
                    for (int j = 0; j < i; j++)
                    {
                        charges[j] = tempCharges[j];
                    }
                    for (int j = i; j < charges.Length; j++)
                    {
                        charges[j] = tempCharges[j + 1];
                    }
                    gui.RemoveChargeInfoWidget(i);
                }
            }
        }

        private void Main_MouseClick(object sender, MouseEventArgs e)
        {
            if (placement.Active == true)
            {
                charges[numOfCharges - 1] = placement.MouseClick(e, IllegalArea(Rectangle.Round(placement.Charge.Bounds)));
            }
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            simEnabled = false;
            Application.Exit();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // Antialiasing for text
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;

            foreach (PCharge charge in charges)
                charge.Paint(e);
            if (simEnabled == true)
                gui.Paint(e);
        }

        private void BtnCharge_Click(object sender, EventArgs e)
        {
            if (placement.Active == false)
            {
                float charge = 0;
                float mass = 0;
                bool fixedCharge = false;
                float.TryParse(txtCharge.Text, out charge);
                float.TryParse(txtMass.Text, out mass);

                if (chkFixed.CheckState == CheckState.Checked)
                    fixedCharge = true;

                if (mass > 0 && charge != 0)
                {
                    PCharge[] tempCharges = new PCharge[numOfCharges];
                    tempCharges = charges;
                    numOfCharges++;
                    charges = new PCharge[numOfCharges];

                    for (int i = 0; i < tempCharges.Length; i++)
                    {
                        charges[i] = tempCharges[i];
                    }

                    charges[numOfCharges - 1] = new PCharge(false, fixedCharge, charge, mass, 0, 0, 50, 50);
                    placement = new Placement(true, charges[numOfCharges - 1]);
                    gui.AddChargeInfoWidget();
                    lblStatus.Text = "";
                }
                else
                {
                    lblStatus.Text = "Invalid Charge/Mass. Retry.";
                }
            }
        }

        private void Main_MouseMove(object sender, MouseEventArgs e)
        {
            if (placement.Active == true)
            {
                charges[numOfCharges - 1] = placement.MouseMovement(e);
                gui.Update(charges);
                Refresh();
            }
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            if (simEnabled == false)
            {
                simEnabled = true;
                txtCharge.Enabled = true;
                txtMass.Enabled = true;
                btnReset.Enabled = true;
                btnCharge.Enabled = true;
                btnStart.Text = "RUN";
                lblStatus.Text = "Input charge/mass, press Place Charges, and press Run";
                Timer();
            }

            if (simRunning == false)
            {
                simRunning = true;
                btnStart.Text = "PAUSE";
                lblStatus.Text = "";
            }
            else
            {
                simRunning = false;
                btnStart.Text = "CONTINUE";
                lblStatus.Text = "PAUSED. Press Continue to resume";
            }
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            if (simEnabled == true && placement.Active == false)
            {
                numOfCharges = 0;
                charges = new PCharge[numOfCharges];
                UiWidgets.numOfWidgets = 0;
                gui = new UI();
                simRunning = false;
                btnStart.Text = "RUN";
                lblStatus.Text = "Simulator was reset.";
                Refresh();
            }
        }
    }
}
