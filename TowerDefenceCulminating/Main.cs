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
        static EnemyList enemyCurrentRound;
        //set the stop watch that checks the gaptime between each wave
        Stopwatch gapTimeBetweenWaves = new Stopwatch();
        //set the vairbles stores if each round is initialized or not
        bool newRoundSetted = false;
        //store in the starting location of the enemy
        const float ENEMY_STARTING_X_LOCATION = 735, ENEMY_STARTING_Y_LOCATION = -100;
        //store in the speed of the enemy
        const float FIRST_ROUND_ENEMY_SPEED = 100;
        //store in the distance between each enemy
        const float GAP_DISTANCE_BETWEEN_ENEMIES = 70;
        //store in the size of the enemy
        SizeF enemySize = new SizeF(30, 30);
        const int FIRST_ROUND_ENEMY_HEALTH = 150;
        //store in the number of enemies
        const int FIRST_ROUND_NUMBER_OF_ENEMIES = 10;
        const int GAP_TIME_BETWEEN_EACH_ROUND = 6;
        public Main()
        {
        //store in the health of the enemy
            InitializeComponent();
            ClientSize = new Size(MAX_CLIENT_WIDTH, MAX_CLIENT_HEIGHT);
            // Lock program size
            MaximizeBox = false;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            size = ClientSize;
            //set the first round
            enemyCurrentRound = new EnemyList(FIRST_ROUND_NUMBER_OF_ENEMIES, ENEMY_STARTING_X_LOCATION, ENEMY_STARTING_Y_LOCATION, FIRST_ROUND_ENEMY_SPEED, GAP_DISTANCE_BETWEEN_ENEMIES, enemySize, FIRST_ROUND_ENEMY_HEALTH, 25, Properties.Resources.Enemy1);
            //tell the program we have already finished setting the new round
            newRoundSetted = true;
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
                enemyCurrentRound.Update();
                //round1.Update();
                gui.Update(ref enemyCurrentRound.enemies);
                // *Timer Activities*
                //set the new round
                RoundSettings();
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
        /*
         * This function sets the gap tiem between each round
         */
        void RoundSettings()
        {
            //if one round is finished, start the stop watch
            if (enemyCurrentRound.enemies.Length == 0)
            {
                if (newRoundSetted == true)
                {
                    gapTimeBetweenWaves.Restart();
                    newRoundSetted = false;
                }
            }
            //if it reaches gap time, start another round and stop the timer
            if (gapTimeBetweenWaves.Elapsed.TotalSeconds >= GAP_TIME_BETWEEN_EACH_ROUND)
            {
                if (newRoundSetted == false)
                {
                    UI.user.NextWave();
                    SetNewRound(UI.user.WavesSurvived + 1);
                    gapTimeBetweenWaves.Reset();
                    newRoundSetted = true;
                }
            }
        }
        /*
         * This function sets a new round. It takes the new round number as its argument.
         */
        void SetNewRound(int newRoundNumber)
        {
            //store in the values needed for the enemy
            const int NUMBER_OF_ENEMY_EXPENDING_RATE = 2;
            const int ENEMY_SPEED_EXPENDING_RATE = 10;
            const int ENEMY_HEALTH_EXPENDING_RATE = 75;
            const int MONEY_EXPENDING_RATE = 5;
            //store in the values needed for the round number
            const int ROUND_TWO = 2, ROUND_THREE = 3;
            //set the round depending on the input round number. Each round gets harder
            switch(newRoundNumber)
            {
                case ROUND_TWO:
                    enemyCurrentRound = new EnemyList(FIRST_ROUND_NUMBER_OF_ENEMIES + newRoundNumber * NUMBER_OF_ENEMY_EXPENDING_RATE, ENEMY_STARTING_X_LOCATION, ENEMY_STARTING_Y_LOCATION, FIRST_ROUND_ENEMY_SPEED + ENEMY_SPEED_EXPENDING_RATE * newRoundNumber, GAP_DISTANCE_BETWEEN_ENEMIES, enemySize, FIRST_ROUND_ENEMY_HEALTH + ENEMY_HEALTH_EXPENDING_RATE * newRoundNumber, 25 + newRoundNumber * MONEY_EXPENDING_RATE, Properties.Resources.Enemy2);
                    break;
                case ROUND_THREE:
                    enemyCurrentRound = new EnemyList(FIRST_ROUND_NUMBER_OF_ENEMIES + newRoundNumber * NUMBER_OF_ENEMY_EXPENDING_RATE, ENEMY_STARTING_X_LOCATION, ENEMY_STARTING_Y_LOCATION, FIRST_ROUND_ENEMY_SPEED + ENEMY_SPEED_EXPENDING_RATE * newRoundNumber, GAP_DISTANCE_BETWEEN_ENEMIES, enemySize, FIRST_ROUND_ENEMY_HEALTH + ENEMY_HEALTH_EXPENDING_RATE * newRoundNumber, 25 + newRoundNumber * MONEY_EXPENDING_RATE, Properties.Resources.Enemy3);
                    break;
                default:
                    break;
            }
        }
        public static void Restart()
        {
            enemyCurrentRound = new EnemyList(10, 735, -100, 100, 70, new SizeF(30, 30), 200, 25, Properties.Resources.Enemy1);
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
                enemyCurrentRound.Paint(e);
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
