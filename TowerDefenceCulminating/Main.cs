/*
 * Bosen Qu and Felix Liu
 * December 23rd, 2016
 * Final Culminating Project, Tower Defence
 */

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
using BosenQuFelixLiuFinalCulminatingProject.GUI;

namespace BosenQuFelixLiuFinalCulminatingProject
{
    public partial class Main : Form
    {
        //Timer time variable and game state variable
        public static float deltaTime;
        public static bool gameRunning;
        // Stopwatches to record time since last update and refresh
        Stopwatch timeCounter = new Stopwatch();
        Stopwatch refreshCounter = new Stopwatch();
        const int FRAME_RATE = 60;
        const float FRAME_TIME = 1f / FRAME_RATE;

        const int MAX_CLIENT_HEIGHT = 800;
        const int MAX_CLIENT_WIDTH = 1300;
        // GUI
        static UI gui;
        // Stores size of the form
        public static Size size;
        // Enemy and healthbar object arrays, size 10 to begin with
        static EnemyList enemies;
        
        public Main()
        {
            InitializeComponent();
            ClientSize = new Size(MAX_CLIENT_WIDTH, MAX_CLIENT_HEIGHT);
            // Lock program size
            MaximizeBox = false;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            size = ClientSize;
            enemies = new EnemyList(10, 735, -100, 100, 70, new SizeF(30, 30), 150, 25, Properties.Resources.Enemy1);
            // GUI
            gui = new UI(UIState.Start);
        }

        void Timer()
        {
            timeCounter.Start();
            refreshCounter.Start();

            // Store previous and current frame's time
            float previousTime = 0;
            float currentTime = 0;

            // Timer loop that runs while sim is running, main program loop
            while (gameRunning == true)
            {
                // Store time passed as time elapsed since stop watch started last (stores the previous time that the loop ran (in milliseconds)
                currentTime = (float)timeCounter.Elapsed.TotalSeconds;
                // Set delta time as time change between last frame and current
                deltaTime = currentTime - previousTime;
                // Set previous time as current time for next frame to use
                previousTime = currentTime;
                
                // *Timer Activities*

                enemies.Update();
                //round1.Update();
                gui.Update(ref enemies.enemies);

                // *Timer Activities*

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

        public static void Restart()
        {
            enemies = new EnemyList(10, 735, -100, 100, 70, new SizeF(30, 30), 200, 25, Properties.Resources.Enemy1);
            gui.RestartGame();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // Antialiasing for text
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;

            gui.Paint(e);

            if (gui.CurrentState == UIState.Game)
            {
                enemies.Paint(e);
            }
        }

        private void Main_MouseClick(object sender, MouseEventArgs e)
        {
            gui.OnClick(e);

            if (gui.GameStart == true)
            {
                gui.GameStart = false;
                gameRunning = true;
                Timer();
            }
        }
       
        private void Main_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.ClientRectangle.Contains(e.Location))
            {
                gui.OnMove(e, this);
            }
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Turn off game
            gameRunning = false;
            Application.Exit();
        }

        private void Main_ResizeBegin(object sender, EventArgs e)
        {
            if (gameRunning == true)
            {
                timeCounter.Stop();
                refreshCounter.Stop();
            }
        }

        private void Main_ResizeEnd(object sender, EventArgs e)
        {
            if (gameRunning == true)
            {
                timeCounter.Start();
                refreshCounter.Start();
            }
        }
    }
}
