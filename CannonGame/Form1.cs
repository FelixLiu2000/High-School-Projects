/*
 * Felix Liu
 * September 9th, 2016
 * Assignment 1 angry birds based cannon game
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

namespace FelixA1Cannons
{
    public partial class Form1 : Form
    {
        // ----------------------------------------------------------------------

        // Target frames per second
        const int FRAME_RATE = 60;
        // 1000 divided by 60 frames per second is target frame rate
        const float FRAME_TIME = 1000 / FRAME_RATE;

        // Constant game states (off, in the main menu, instructions menu, running, paused, won, lost, panning stage [e.g. game panning at game start and projectile reset])
        const int GAME_OFF = 0;
        const int GAME_MENU = 1;
        const int GAME_INSTRUCTIONS = 2;
        const int GAME_RUNNING = 3;
        const int GAME_PAUSED = 4;
        const int GAME_WON = 5;
        const int GAME_LOST = 6;
        const int GAME_PAN_STAGE = 7;

        // Constant screen dimensions
        const int MAXIMUM_SCREEN_WIDTH = 1300;
        const int MAXIMUM_SCREEN_HEIGHT = 700;

        // Projectile cooldown max counter value
        const int ENEMY_PROJECTILE_COOLDOWN_FINISHED = 80;

        // Constant of gravity's effect on the projectile
        const int PROJECTILE_GRAVITY = 1;

        // Constant of maximum projectile power value
        const int PROJECTILE_MAX_POWER = 50;

        // Constant of minimum projectile power value
        const int PROJECTILE_MIN_POWER = 1;

        // Constant of maximum projectile angle value
        const int PROJECTILE_MAX_ANGLE = 90;

        // Constant of minimum projectile angle value
        const int PROJECTILE_MIN_ANGLE = 1;

        // Constant of power and angle increment per button press
        const int POWER_ANGLE_INCREMENT = 1;

        // Constant of maximum attempts allowed to hit target
        const int MAXIMUM_ALLOWED_ATTEMPTS = 3;

        // Constant of pan speed during game start scene
        const int GAME_START_PAN_SPEED = 15;

        // ----------------------------------------------------------------------

        // Controls the elements that show based on the state of the game
        int gameState = GAME_MENU;

        // Stores angle that projectile is to be fired at
        int projectileAngle = PROJECTILE_MIN_ANGLE;

        // Stores original projectile x location
        int projectileStartingX;

        // Stores original projectile y location
        int projectileStartingY;

        // Stores number of attempts used to hit target
        int attemptsUsed;

        // ----------------------------------------------------------------------

        // Stores projectile power specified
        double projectilePower = PROJECTILE_MIN_POWER;

        // Projectile speeds along x and y axis
        float projectileXSpeed;
        float projectileYSpeed;

        // ----------------------------------------------------------------------

        // Boolean state variable for background panning, whether background is currently panning with projectile
        bool backgroundPanning = false;

        // Boolean state variable for projectile, whether player has fired the projectile and is valid
        bool projectileAwayFromCannon = false;

        // Boolean state variable for projectile, whether projectile has left boundaries or hit target
        bool projectileInvalidated = false;

        // Boolean state variable for cannon, whether cannon can commit suicide
        bool cannonCanSuicide = false;

        // Boolean state variable for start game button, whether it is currently being hovered over
        bool startGameButtonHover = false;

        // Boolean state variable for instructions button, whether it is currently being hovered over
        bool instructionsButtonHover = false;

        // ----------------------------------------------------------------------

        // Background shade location data
        Point backgroundShadeLocation;

        // Game name graphics data 1 ("Angry")
        Point gameNameImage1Location;
        Size gameNameImage1Size;
        Rectangle gameNameImage1HitBox;

        // Game name string data ("Squirrels")
        Point gameNameImage2Location;
        Size gameNameImage2Size;
        Rectangle gameNameImage2HitBox;

        // Start game button data
        Point startGameButtonLocation;
        Size startGameButtonSize;
        Rectangle startGameButtonHitBox;
        Font startGameButtonFont;
        StringFormat startGameButtonFormat;

        // Instructions button data
        Point instructionsButtonLocation;
        Size instructionsButtonSize;
        Rectangle instructionsButtonHitBox;
        Font instructionsButtonFont;
        StringFormat instructionsButtonFormat;

        // Instructions string data
        Point instructionsLocation;
        Size instructionsSize;
        Rectangle instructionsHitBox;
        Font instructionsFont;
        StringFormat instructionsFormat;

        // Forest background image graphics data
        Point backgroundForestLocation;
        Size backgroundForestSize;
        Rectangle backgroundForestHitBox;

        // Squirrel graphics data
        Point squirrelLocation;
        Size squirrelSize;
        Rectangle squirrelHitBox;

        // Cannon graphics data
        Point cannonLocation;
        Size cannonSize;
        Rectangle cannonHitBox;

        // Acorn graphics data
        Point acornLocation;
        Size acornSize;
        Rectangle acornHitBox;

        // Owl graphics data
        Point owlLocation;
        Size owlSize;
        Rectangle owlHitBox;

        // Projectile power string data
        Point projectileDataLocation;
        Size projectileDataSize;
        Rectangle projectileDataHitBox;
        Font projectileDataFont;

        // Game over string data
        Point gameOverLocation;
        Size gameOverSize;
        Rectangle gameOverHitBox;
        Font gameOverFont;
        StringFormat gameOverFormat;

        // User prompt string data
        Point userPromptLocation;
        Size userPromptSize;
        Rectangle userPromptHitBox;
        Font userPromptFont;
        StringFormat userPromptFormat;

        // Attempts used graphics data (different data for images 1 2 3)
        Point attemptsUsedLocation1;
        Point attemptsUsedLocation2;
        Point attemptsUsedLocation3;
        Size attemptsUsedSize1;
        Size attemptsUsedSize2;
        Size attemptsUsedSize3;
        Rectangle attemptsUsedHitBox1;
        Rectangle attemptsUsedHitBox2;
        Rectangle attemptsUsedHitBox3;

        // ----------------------------------------------------------------------

        public Form1()
        {
            InitializeComponent();

            // Set and lock program size
            ClientSize = new Size(MAXIMUM_SCREEN_WIDTH, MAXIMUM_SCREEN_HEIGHT);
            MaximizeBox = false;
            FormBorderStyle = FormBorderStyle.FixedSingle;

            // Setup game name image data
            SetupGameNameImage1();

            // Setup game name string data
            SetupGameNameImage2();

            // Setup start game button
            SetupStartGameButton();

            // Setup instructions button
            SetupInstructionsButton();

            // Setup instructions string data
            SetupInstructions();

            // Setup background data
            SetupBackground();
            
            // Setup squirrel data
            SetupSquirrel();

            // Setup cannon data
            SetupCannon();

            // Setup acorn data
            SetupAcorn();

            // Setup owl
            SetupOwl();

            // Setup projectile power and angle displays
            SetupProjectileDataDisplay();

            // Setup game over display
            SetupGameOverDisplay();

            // Setup user prompt display
            SetupUserPromptDisplay();

            // Setup attempts used display
            SetupAttemptsUsed();

            // Setup background shade location (no subprogram needed)
            backgroundShadeLocation = new Point(0, 0);
        }

        // Contains main program loop that updates user interface and conducts in game operations
        void FrameRateLoop()
        {
            // Stores the previous time that the loop ran (in milliseconds)
            int previousTime;

            // Start time set as current time
            previousTime = Environment.TickCount;

            // Set game as running
            gameState = GAME_RUNNING;

            // Timer loop that runs at 60 fps (every 16ms), main program loop
            while (gameState == GAME_RUNNING)
            {
                // Calculate time passed by subtracting current time by previous time set in loop
                float timePassed = Environment.TickCount - previousTime;

                // If 16 ms has passed
                if (timePassed >= FRAME_TIME)
                {
                    // Check collisions and boundary violations
                    CheckCollisions();

                    // Move projectile
                    MoveProjectile();

                    // Pan background to follow projectile
                    PanBackgroundImage();

                    // Redraws the screen
                    Refresh();
                }

                // Allow other interactions/code while this loop is active
                Application.DoEvents();
            }
        }

        // Setup main menu game name image 1 size location and hitbox
        void SetupGameNameImage1()
        {
            // Game name starts off at location (x,y) = (190, 75)
            gameNameImage1Location = new Point(190, 75);
            // Game name is 500 by 150
            gameNameImage1Size = new Size(500, 200);
            // Combine the location and the size
            gameNameImage1HitBox = new Rectangle(gameNameImage1Location, gameNameImage1Size);
        }

        // Setup main menu game name image 2 size location and hitbox
        void SetupGameNameImage2()
        {
            // Game name string starts off at right side of game name image ("Angry") minus 5 pixels
            gameNameImage2Location = new Point(gameNameImage1Location.X + gameNameImage1HitBox.Width - 5, 120);
            // Game name string size is 600 by 150
            gameNameImage2Size = new Size(600, 150);
            // Combine the location and the size
            gameNameImage2HitBox = new Rectangle(gameNameImage2Location, gameNameImage2Size);
        }

        // Setup start game button
        void SetupStartGameButton()
        {
            // Start game button starts off at location (x,y) = (400, 350)
            startGameButtonLocation = new Point(450, 350);
            // Start game button size is 750 by 128
            startGameButtonSize = new Size(750, 128);
            // Combine the location and the size
            startGameButtonHitBox = new Rectangle(startGameButtonLocation, startGameButtonSize);
            // Set font
            startGameButtonFont = new Font("Century Gothic", 72, FontStyle.Bold);
            // Set format alignment to center
            startGameButtonFormat = new StringFormat();
            startGameButtonFormat.Alignment = StringAlignment.Near;
            startGameButtonFormat.LineAlignment = StringAlignment.Center;
        }

        // Setup instructions button
        void SetupInstructionsButton()
        {
            // Instructions button starts off at x location of start game plus 150 pixels, y location of start game plus 100 pixels
            instructionsButtonLocation = new Point(startGameButtonLocation.X + 150, startGameButtonLocation.Y + 200);
            // Instructions button size is 675 by 100
            instructionsButtonSize = new Size(675, 100);
            // Combine the location and the size
            instructionsButtonHitBox = new Rectangle(instructionsButtonLocation, instructionsButtonSize);
            // Set font
            instructionsButtonFont = new Font("Century Gothic", 60, FontStyle.Bold);
            // Set format alignment to center
            instructionsButtonFormat = new StringFormat();
            instructionsButtonFormat.Alignment = StringAlignment.Near;
            instructionsButtonFormat.LineAlignment = StringAlignment.Center;
        }

        // Setup instructions string
        void SetupInstructions()
        {
            // Instructions starts off at location (x,y) = 0,0
            instructionsLocation = new Point(0, 0);
            // Instructions size is the program size
            instructionsSize = new Size(ClientSize.Width, ClientSize.Height);
            // Combine the location and the size
            instructionsHitBox = new Rectangle(instructionsLocation, instructionsSize);
            // Set font
            instructionsFont = new Font("Century Gothic", 30, FontStyle.Bold);
            // Set format alignment to center
            instructionsFormat = new StringFormat();
            instructionsFormat.Alignment = StringAlignment.Center;
            instructionsFormat.LineAlignment = StringAlignment.Center;
        }

        // Starting scene of game, start with target in view and all other elements off the screen, then move to the player
        void GameStartScene()
        {
            // If the game is currently in its starting scene
            if (gameState == GAME_PAN_STAGE)
            {
                // Background starts off at location (x,y) = (Background image width multiplied by negative 1 plus program width, 0)
                backgroundForestLocation = new Point((-backgroundForestSize.Width) + ClientSize.Width, 0);
                // Combine the location and the size
                backgroundForestHitBox = new Rectangle(backgroundForestLocation, backgroundForestSize);

                // Squirrel starts off at location (x,y) = (Background image width multiplied by negative 1 plus program width plus original squirrel x location, 600)
                squirrelLocation = new Point((-backgroundForestSize.Width) + ClientSize.Width + squirrelLocation.X, 600);
                // Combine the location and the size
                squirrelHitBox = new Rectangle(squirrelLocation, squirrelSize);

                // Cannon starts off at location (x,y) = (Background image width multiplied by negative 1 plus program width plus original cannon x location, 550)
                cannonLocation = new Point((-backgroundForestSize.Width) + ClientSize.Width + cannonLocation.X, 550);
                // Combine the location and the size
                cannonHitBox = new Rectangle(cannonLocation, cannonSize);

                // Acorn starts off at location (x,y) = (Background image width multiplied by negative 1 plus program width plus original acorn x location, 580)
                acornLocation = new Point((-backgroundForestSize.Width) + ClientSize.Width + acornLocation.X, 580);
                // Combine the location and the size
                acornHitBox = new Rectangle(acornLocation, acornSize);

                // Owl starts off at location (x,y) = (Background image width multiplied by negative 1 plus program width plus original owl x location, 550)
                owlLocation = new Point((-backgroundForestSize.Width) + ClientSize.Width + owlLocation.X, 550);
                // Combine the location and the size
                owlHitBox = new Rectangle(owlLocation, owlSize);


                // While the left side of the background forest is still less than the left side of the screen, pan back to the cannon
                PanBackToCannon();
            }
        }

        // Setup background size location and hitbox
        void SetupBackground()
        {
            // Background starts off at location (x,y) = (0, 0)
            backgroundForestLocation = new Point(0, 0);
            // Background size is size of program, with width multiplied by 2
            backgroundForestSize = new Size(ClientSize.Width * 2, ClientSize.Height);
            // Combine the location and the size
            backgroundForestHitBox = new Rectangle(backgroundForestLocation, backgroundForestSize);
        }

        // Setup squirrel cannon crew size location and hitbox
        void SetupSquirrel()
        {
            // Squirrel starts off at location (x,y) = (125, 600)
            squirrelLocation = new Point(125, 600);
            // Squirrel size is 64 by 64
            squirrelSize = new Size(64, 64);
            // Combine the location and the size
            squirrelHitBox = new Rectangle(squirrelLocation, squirrelSize);
        }

        // Setup cannon size location and hitbox
        void SetupCannon()
        {
            // Cannon starts off at location (x,y) = (150, 550)
            cannonLocation = new Point(150, 550);
            // Cannon size is 256 by 128
            cannonSize = new Size(256, 128);
            // Combine the location and the size
            cannonHitBox = new Rectangle(cannonLocation, cannonSize);
        }

        // Setup acorn cannon ball size location and hitbox
        void SetupAcorn()
        {
            // Acorn starts off at location (x,y) = (380, 580)
            acornLocation = new Point(380, 580);
            // Acorn size is 32 by 32
            acornSize = new Size(32, 32);
            // Combine the location and the size
            acornHitBox = new Rectangle(acornLocation, acornSize);

            // Set starting X location of acorn as current acorn X location
            projectileStartingX = acornLocation.X;
            // Set starting Y location of acorn as current acorn Y location
            projectileStartingY = acornLocation.Y;
        }

        // Setup owl target size location and hitbox
        void SetupOwl()
        {
            // Owl starts off at location (x,y) = (2200, 550)
            owlLocation = new Point(2200, 550);
            // Owl size is 128 by 64
            owlSize = new Size(128, 64);
            // Combine the location and the size
            owlHitBox = new Rectangle(owlLocation, owlSize);
        }

        // Setup projectile data display string data
        void SetupProjectileDataDisplay()
        {
            // Projectile data display starts off at location (x,y) = (25, 25)
            projectileDataLocation = new Point(25, 25);
            // Projectile data display size is 128 by 32
            projectileDataSize = new Size(256, 32);
            // Combine the location and the size
            projectileDataHitBox = new Rectangle(projectileDataLocation, projectileDataSize);
            // Set font
            projectileDataFont = new Font("Century Gothic", 18, FontStyle.Bold);
        }

        // Setup game over display string data
        void SetupGameOverDisplay()
        {
            // Game over display starts off at 0, and one third of program height
            gameOverLocation = new Point(0, ClientSize.Height / 3);
            // Game over display size is program width by 200
            gameOverSize = new Size(ClientSize.Width, 200);
            // Combine the location and the size
            gameOverHitBox = new Rectangle(gameOverLocation, gameOverSize);
            // Set font
            gameOverFont = new Font("Century Gothic", 120, FontStyle.Bold);
            // Set format alignment to center
            gameOverFormat = new StringFormat();
            gameOverFormat.Alignment = StringAlignment.Center;
            gameOverFormat.LineAlignment = StringAlignment.Center;
        }

        // Setup user prompt display string data
        void SetupUserPromptDisplay()
        {
            // User prompt display starts off at 0, and one third of program height
            userPromptLocation = new Point(0, ClientSize.Height / 3);
            // User prompt display size is program width by 200
            userPromptSize = new Size(ClientSize.Width, 200);
            // Combine the location and the size
            userPromptHitBox = new Rectangle(userPromptLocation, userPromptSize);
            // Set font
            userPromptFont = new Font("Century Gothic", 50, FontStyle.Bold);
            // Set format alignment to center
            userPromptFormat = new StringFormat();
            userPromptFormat.Alignment = StringAlignment.Center;
            userPromptFormat.LineAlignment = StringAlignment.Center;
        }

        // Setup attempts used
        void SetupAttemptsUsed()
        {
            // Attempts used 1st image size is 32 by 32
            attemptsUsedSize1 = new Size(32, 32);
            // Attempts used 2nd image size is 48 by 48
            attemptsUsedSize2 = new Size(48, 48);
            // Attempts used 3rd image size is 64 by 64
            attemptsUsedSize3 = new Size(64, 64);

            // Attempts used 1st image starts at location (x,y) = (1250, 15)
            attemptsUsedLocation1 = new Point(1250, 15);
            // Attempts used 2nd image starts at location (x,y) = (x = image 1's x location subtracted by image 2's width translated left by 2 pixels, 15)
            attemptsUsedLocation2 = new Point(attemptsUsedLocation1.X - attemptsUsedSize2.Width - 2, 15);
            // Attempts used 3rd image starts at location (x,y) = (x = image 2's x location subtracted by image 3's width translated left by 2 pixels, 15)
            attemptsUsedLocation3 = new Point(attemptsUsedLocation2.X - attemptsUsedSize3.Width - 2, 15);

            // Combine the location and the size for image 1
            attemptsUsedHitBox1 = new Rectangle(attemptsUsedLocation1, attemptsUsedSize1);
            // Combine the location and the size for image 2
            attemptsUsedHitBox2 = new Rectangle(attemptsUsedLocation2, attemptsUsedSize2);
            // Combine the location and the size for image 3
            attemptsUsedHitBox3 = new Rectangle(attemptsUsedLocation3, attemptsUsedSize3);
        }

        // Setup cannon ball projectile data
        void SetupProjectile()
        {
            // Calculate projectile y and x axis speeds by multiplying projectile power by Sine or Cosine of projectile angle (expressed in degrees)
            projectileYSpeed = (float)(projectilePower * Math.Sin(projectileAngle * (Math.PI / 180)));
            projectileXSpeed = (float)(projectilePower * Math.Cos(projectileAngle * (Math.PI / 180)));
        }

        // Move projectile
        void MoveProjectile()
        {
            // If projectile has been fired and is currently valid
            if (projectileAwayFromCannon == true && projectileInvalidated == false)
            {
                // Move projectile along y axis based on projectile speeds
                acornLocation.Y = (int)(acornLocation.Y - projectileYSpeed);

                // If background is not currently panning
                if (backgroundPanning == false)
                {
                    // Move projectile along x axis based on projectile speeds
                    acornLocation.X = (int)(acornLocation.X + projectileXSpeed);
                }

                // Refresh projectile hitbox
                acornHitBox.Location = acornLocation;

                // Decrease projectile y speed by gravity constant
                projectileYSpeed = projectileYSpeed - PROJECTILE_GRAVITY;
            }
        }

        // Pan background image to follow projectile
        void PanBackgroundImage()
        {
            // If the projectile has been fired and is currently valid
            if (projectileAwayFromCannon == true && projectileInvalidated == false)
            {
                // If the projectile has moved 200 pixels or more from its starting location and the right side of the background has reached the right side of the screen plus the speed at which the background is moving at
                if (acornLocation.X - projectileStartingX >= 200 && backgroundForestHitBox.Right <= ClientRectangle.Right + projectileXSpeed)
                {
                    // Disable background panning
                    backgroundPanning = false;
                }

                // If the projectile has moved 200 pixels or more from its starting location and the right side of the background has not reached the right side of the screen
                else if (acornLocation.X - projectileStartingX >= 200 && backgroundForestHitBox.Right > ClientRectangle.Right)
                {
                    // Enable background panning
                    backgroundPanning = true;

                    // Translate background, squirrel, cannon, and owl along x axis at the same speed as the projectile but opposite direction (converted to integer value)
                    backgroundForestLocation.X = backgroundForestLocation.X - (int)projectileXSpeed;
                    squirrelLocation.X = squirrelLocation.X - (int)projectileXSpeed;
                    cannonLocation.X = cannonLocation.X - (int)projectileXSpeed;
                    owlLocation.X = owlLocation.X - (int)projectileXSpeed;

                    // Refresh background, squirrel and cannon hitboxes
                    backgroundForestHitBox.Location = backgroundForestLocation;
                    squirrelHitBox.Location = squirrelLocation;
                    cannonHitBox.Location = cannonLocation;
                    owlHitBox.Location = owlLocation;
                }
            }
        }

        // Check for collisions between hitboxes and define boundaries
        void CheckCollisions()
        {
            // If the projectile has been fired and is currently valid
            if (projectileAwayFromCannon == true && projectileInvalidated == false)
            {
                // If acorn's hitbox has gone past the program's left, right or bottom
                if (acornHitBox.Right < backgroundForestHitBox.Left || acornHitBox.Left > backgroundForestHitBox.Right || acornHitBox.Top > backgroundForestHitBox.Bottom)
                {
                    // Set projectile as invalid
                    projectileInvalidated = true;

                    // Disable background panning
                    backgroundPanning = false;

                    // Increase number of attempts that have been used
                    attemptsUsed++;
                    
                    // If the number of allowed attempts has been reached with no success
                    if (attemptsUsed == MAXIMUM_ALLOWED_ATTEMPTS)
                    {
                        // Set game state as game lost;
                        gameState = GAME_LOST;
                    }
                }

                // If acorn's hitbox has hit the owl (target)
                else if (acornHitBox.IntersectsWith(owlHitBox))
                {
                    // Set projectile as invalid
                    projectileInvalidated = true;

                    // Disable background panning
                    backgroundPanning = false;

                    // Set game state as game won
                    gameState = GAME_WON;
                }

                // If the acorn has travelled more than 100 pixels upward
                if (acornLocation.Y <= (projectileStartingY - 100))
                {
                    // Allow cannon to commit suicide
                    cannonCanSuicide = true;
                }
                
                // If the acorn has hit the cannon and has travelled more than 100 pixels vertically
                if (acornHitBox.IntersectsWith(cannonHitBox) && cannonCanSuicide == true)
                {
                    // Disable projectile movement
                    projectileAwayFromCannon = false;

                    // Set projectile as invalid
                    projectileInvalidated = true;

                    // Disable background panning
                    backgroundPanning = false;

                    // Set game state as game lost
                    gameState = GAME_LOST;
                }
            }
        }

        // Pan back to cannon
        void PanBackToCannon()
        {
            // Pan back to the cannon while the left side of the background forest is still less than the left side of the screen and game is currently in its starting scene or is running
            while (backgroundForestLocation.X < ClientRectangle.X && (gameState == GAME_PAN_STAGE || gameState == GAME_RUNNING))
            {
                // Game start pan speed is added to the location of all elements
                backgroundForestLocation.X += GAME_START_PAN_SPEED;
                squirrelLocation.X += GAME_START_PAN_SPEED;
                cannonLocation.X += GAME_START_PAN_SPEED;
                acornLocation.X += GAME_START_PAN_SPEED;
                owlLocation.X += GAME_START_PAN_SPEED;

                // Refresh all hitboxes
                backgroundForestHitBox.Location = backgroundForestLocation;
                squirrelHitBox.Location = squirrelLocation;
                cannonHitBox.Location = cannonLocation;
                acornHitBox.Location = acornLocation;
                owlHitBox.Location = owlLocation;

                // Refresh program screen
                Refresh();

                // Allow program to do other events
                Application.DoEvents();
            }

            // Resets all graphics locations to their original location (to make sure position does not pass the intended spot)
            SetupAcorn();
            SetupCannon();
            SetupSquirrel();
            SetupOwl();
            SetupBackground();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // Antialiasing for text elements
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            
            // Draw image regardless of states
            e.Graphics.DrawImage(Properties.Resources.ForestBackground, backgroundForestHitBox);

            // If the game is currently not running (in main menu)
            if (gameState == GAME_MENU)
            {
                // Draw main menu background shade (translucent black background, darkens game elements)
                e.Graphics.DrawImage(Properties.Resources.mainMenuBackgroundShade, ClientRectangle);

                // Draw main menu game name image 2 ("Squirrels")
                e.Graphics.DrawImage(Properties.Resources.Squirrels, gameNameImage2HitBox);

                // Draw main menu game name image 1 ("Angry")
                e.Graphics.DrawImage(Properties.Resources.Angry, gameNameImage1HitBox);

                // Draw outline of start game button
                e.Graphics.DrawRectangle(Pens.White, startGameButtonHitBox);

                // Draw outline of instructions button
                e.Graphics.DrawRectangle(Pens.Gray, instructionsButtonHitBox);
                

                // If the mouse is currently on the start game button
                if (startGameButtonHover == true)
                {
                    // Fill in rectangle of start game button
                    e.Graphics.FillRectangle(Brushes.DimGray, startGameButtonHitBox);
                    // Draw start game string
                    e.Graphics.DrawString(" START GAME", startGameButtonFont, Brushes.WhiteSmoke, startGameButtonHitBox, startGameButtonFormat);
                }

                // Otherwise if it is not
                else
                {
                    // Fill in rectangle of start game button
                    e.Graphics.FillRectangle(Brushes.Black, startGameButtonHitBox);
                    // Draw start game string
                    e.Graphics.DrawString(" START GAME", startGameButtonFont, Brushes.Gray, startGameButtonHitBox, startGameButtonFormat);
                }
                
                // If the mouse is currently on the instructions button
                if (instructionsButtonHover == true)
                {
                    // Fill in rectangle of instructions button
                    e.Graphics.FillRectangle(Brushes.DimGray, instructionsButtonHitBox);
                    // Draw instructions string
                    e.Graphics.DrawString(" INSTRUCTIONS", instructionsButtonFont, Brushes.WhiteSmoke, instructionsButtonHitBox, instructionsButtonFormat);
                }

                // Otherwise if it is not
                else
                {
                    // Fill in rectangle of instructions button
                    e.Graphics.FillRectangle(Brushes.Black, instructionsButtonHitBox);
                    // Draw instructions string
                    e.Graphics.DrawString(" INSTRUCTIONS", instructionsButtonFont, Brushes.Gray, instructionsButtonHitBox, instructionsButtonFormat);
                }
            }

            // If the game is currently paused
            else if (gameState == GAME_PAUSED)
            {
                // Draw background shade (translucent black background, darkens game elements)
                e.Graphics.DrawImage(Properties.Resources.BackgroundShade, backgroundShadeLocation);

                // Draw user prompt string
                e.Graphics.DrawString("PAUSED, PRESS ESCAPE TO RESUME", userPromptFont, Brushes.White, userPromptHitBox, userPromptFormat);
            }

            // If the game is currently in the start scene stage
            else if (gameState == GAME_PAN_STAGE)
            {
                e.Graphics.DrawImage(Properties.Resources.Cannon, cannonHitBox);
                e.Graphics.DrawImage(Properties.Resources.SquirrelCannon, squirrelHitBox);
                e.Graphics.DrawImage(Properties.Resources.Acorn, acornHitBox);
                e.Graphics.DrawImage(Properties.Resources.Owl, owlHitBox);

            }

            // If the game is currently running
            else if (gameState == GAME_RUNNING)
            {
                e.Graphics.DrawImage(Properties.Resources.Cannon, cannonHitBox);
                e.Graphics.DrawImage(Properties.Resources.SquirrelCannon, squirrelHitBox);
                e.Graphics.DrawImage(Properties.Resources.Acorn, acornHitBox);
                e.Graphics.DrawImage(Properties.Resources.Owl, owlHitBox);

                // If the projectile has not been fired and is valid
                if (projectileAwayFromCannon == false && projectileInvalidated == false)
                {
                    // Reset projectile data y location to original location (y = 25)
                    projectileDataLocation.Y = 25;
                    // Refresh hitbox
                    projectileDataHitBox.Location = projectileDataLocation;

                    // Display projectile power as a string
                    e.Graphics.DrawString("POWER: " + projectilePower.ToString() + "/" + PROJECTILE_MAX_POWER, projectileDataFont, Brushes.White, projectileDataHitBox);

                    // Increase the projectile data location to reuse for projectile angle display
                    projectileDataLocation.Y = projectileDataLocation.Y + 25;
                    // Refresh hitbox
                    projectileDataHitBox.Location = projectileDataLocation;

                    // Display projectile angle as a string
                    e.Graphics.DrawString("ANGLE: " + projectileAngle.ToString() + "°", projectileDataFont, Brushes.White, projectileDataHitBox);
                }

                // If the projectile is away from the cannon and is invalid
                else if (projectileAwayFromCannon == true && projectileInvalidated == true)
                {
                    // Draw user prompt string
                    e.Graphics.DrawString("PRESS ENTER TO TRY AGAIN", userPromptFont, Brushes.White, userPromptHitBox, userPromptFormat);
                }

                // If one attempt to hit the target has been used
                if (attemptsUsed == 1)
                {
                    e.Graphics.DrawImage(Properties.Resources.AttemptsUsed1, attemptsUsedHitBox1);
                }

                // If two attempts to hit the target has been used
                else if (attemptsUsed == 2)
                {
                    e.Graphics.DrawImage(Properties.Resources.AttemptsUsed1, attemptsUsedHitBox1);
                    e.Graphics.DrawImage(Properties.Resources.AttemptsUsed2, attemptsUsedHitBox2);
                }

                // If three attempt to hit the target has been used
                else if (attemptsUsed == 3)
                {
                    e.Graphics.DrawImage(Properties.Resources.AttemptsUsed1, attemptsUsedHitBox1);
                    e.Graphics.DrawImage(Properties.Resources.AttemptsUsed2, attemptsUsedHitBox2);
                    e.Graphics.DrawImage(Properties.Resources.AttemptsUsed3, attemptsUsedHitBox3);
                }
            }

            // If the game has been won
            else if (gameState == GAME_WON)
            {
                // Draw background shade (translucent black background, darkens game elements)
                e.Graphics.DrawImage(Properties.Resources.BackgroundShade, backgroundShadeLocation);

                // Draw game won string
                e.Graphics.DrawString("YOU WON!", gameOverFont, Brushes.Gold, gameOverHitBox, gameOverFormat);
            }

            // If the game has been lost
            else if (gameState == GAME_LOST)
            {
                // Draw background shade (translucent black background, darkens game elements)
                e.Graphics.DrawImage(Properties.Resources.BackgroundShade, backgroundShadeLocation);

                // Draw game lost string
                e.Graphics.DrawString("YOU LOST!", gameOverFont, Brushes.Red, gameOverHitBox, gameOverFormat);
            }

             // If the instructions button was clicked
            else if (gameState == GAME_INSTRUCTIONS)
            {
                // Draw background shade (translucent black background, darkens game elements)
                e.Graphics.DrawImage(Properties.Resources.BackgroundShade, backgroundShadeLocation);

                // Draw game lost string
                e.Graphics.DrawString("Help the squirrel defeat the owl using its acorn cannon!\r\nControl the cannon's angle using the up and down arrows, and the cannon's power using the left and right arrows. The cannon has a maximum angle of 90 degrees, a minimum angle of 1 degree, and a maximum power of 50. Press the enter button to fire when ready. Don't fire outside of the boundaries, this includes the area past the owl and the ground. You have three attempts to hit the owl, run out and you lose!\r\nGood luck!\r\n (PRESS ESCAPE TO RETURN TO MAIN MENU)", instructionsFont, Brushes.White, instructionsHitBox, instructionsFormat);
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            // Turns on game when space is pressed
            if (e.KeyCode == Keys.Escape)
            {
                // Turn loop on or off

                // If game is paused
                if (gameState == GAME_PAUSED)
                {
                    // Start frame rate loop (which will also set game as running)
                    FrameRateLoop();
                    
                    // Refresh program screen
                    Refresh();
                }

                // If game is on
                else if (gameState == GAME_RUNNING)
                {
                    // Set game as paused
                    gameState = GAME_PAUSED;

                    // Refresh program screen
                    Refresh();
                }

                // If the game is in the instructions menu
                else if (gameState == GAME_INSTRUCTIONS)
                {
                    // Set game as in the main menu
                    gameState = GAME_MENU;

                    // Redraw screen
                    Refresh();
                }
            }

            // If the game is currently running
            if (gameState == GAME_RUNNING)
            {
                // Fires projectile (acorn cannon ball) if enter is pressed and projectile has not already been fired
                if (e.KeyCode == Keys.Enter && projectileAwayFromCannon == false && projectileInvalidated == false)
                {
                    // Set projectile as fired
                    projectileAwayFromCannon = true;

                    // Setup projectile using power and angle specified
                    SetupProjectile();
                }

                // Resets projectile (pans back to cannon) if enter is pressed and projectile is currently invalid having left boundaries
                else if (e.KeyCode == Keys.Enter && projectileInvalidated == true)
                {
                    // Set game state as game panning stage
                    gameState = GAME_PAN_STAGE;

                    // Validate projectile (necessary to hide the user prompt to press enter in this case)
                    projectileInvalidated = false;

                    // Pan back to the cannon while the left side of the background forest is still less than the left side of the screen
                    PanBackToCannon();

                    // Set projectile as not currently being fired
                    projectileAwayFromCannon = false;

                    // Disable cannon suicide (projectile must travel more than 100 pixels to allow suicide once again)
                    cannonCanSuicide = false;

                    // Set game state as game running stage
                    gameState = GAME_RUNNING;
                }

                // If projectile has not been fired and is not invalidated
                if (projectileAwayFromCannon == false && projectileInvalidated == false)
                {
                    // Increases projectile power
                    if (e.KeyCode == Keys.Right && projectilePower < PROJECTILE_MAX_POWER)
                    {
                        // Increase projectile power by 1
                        projectilePower = projectilePower + POWER_ANGLE_INCREMENT;
                    }

                    // Decreases projectile power
                    else if (e.KeyCode == Keys.Left && projectilePower > PROJECTILE_MIN_POWER)
                    {
                        // Decrease projectile power by 1
                        projectilePower = projectilePower - POWER_ANGLE_INCREMENT;
                    }

                    // Increases projectile angle if projectile has not been fired and angle is less than maximum angle
                    else if (e.KeyCode == Keys.Up && projectileAngle < PROJECTILE_MAX_ANGLE)
                    {
                        // Increase projectile angle by 1 degree until reaching maximum projectile angle
                        projectileAngle = projectileAngle + POWER_ANGLE_INCREMENT;
                    }

                    // Increases projectile angle if projectile has not been fired and angle is greater than minimum angle
                    else if (e.KeyCode == Keys.Down && projectileAngle > PROJECTILE_MIN_ANGLE)
                    {
                        // Decrease projectile angle by 1 degree until reaching maximum projectile angle
                        projectileAngle = projectileAngle - POWER_ANGLE_INCREMENT;
                    }
                }
            }
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            // If the game is off (in the main menu)
            if (gameState == GAME_MENU)
            {
                // If the left mouse button is been pressed
                if (e.Button == MouseButtons.Left)
                {
                    // If the cursor is currently on the start game button
                    if (startGameButtonHitBox.Contains(e.Location))
                    {
                        // Set game state as game starting scene
                        gameState = GAME_PAN_STAGE;

                        // Disable minimize button to prevent location glitches in game
                        MinimizeBox = false;

                        // Begin the start game scene
                        GameStartScene();

                        // Set game as running
                        gameState = GAME_RUNNING;

                        // Start game loop to begin game
                        FrameRateLoop();
                    }

                    // If the cursor is currently on the instructions game button
                    else if (instructionsButtonHitBox.Contains(e.Location))
                    {
                        // Set game state as game instrucitons to show instructions
                        gameState = GAME_INSTRUCTIONS;

                        // Redraw screen
                        Refresh();
                    }
                }
            }
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            // If the game is currently off (in main menu)
            if (gameState == GAME_MENU)
            {
                // If the mouse is over the start game button
                if (startGameButtonHitBox.Contains(e.Location))
                {
                    // Change color of start game button
                    startGameButtonHover = true;

                    // Refresh screen graphics
                    Refresh();
                }

                // Otherwise
                else
                {
                    // Change color of start game button
                    startGameButtonHover = false;

                    // Refresh screen graphics
                    Refresh();
                }

                // If the mouse if over the instructions button
                if (instructionsButtonHitBox.Contains(e.Location))
                {
                    // Change color of instructions button
                    instructionsButtonHover = true;

                    // Refresh screen graphics
                    Refresh();
                }

                // Otherwise
                else
                {
                    // Change color of start game button
                    instructionsButtonHover = false;

                    // Refresh screen graphics
                    Refresh();
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Turn off game loop and all game events
            gameState = GAME_OFF;

            // Exit from application
            Application.Exit();
        }
    }
}
