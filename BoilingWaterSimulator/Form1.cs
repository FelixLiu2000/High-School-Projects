/*
 * Felix Liu
 * November 3rd, 2016
 * Assignment 2, simulator for water molecules being heated in a kettle/pot
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

namespace FelixA2WaterSimulator
{
    public partial class FelixA2WaterSimulator : Form
    {
        // ----------------------------------------------------------------------

        // Target frames per second
        const int FRAME_RATE = 60;
        // 1 divided by 60 frames per second is target frame rate
        const float FRAME_TIME = 1.0f / FRAME_RATE;

        // Constant simulation states (off, in the main menu, running, paused)
        const int SIM_OFF = 0;
        const int SIM_MENU = 1;
        const int SIM_RUNNING = 2;
        const int SIM_PAUSED = 3;

        // Constant screen dimensions
        const int MAXIMUM_SCREEN_WIDTH = 1300;
        const int MAXIMUM_SCREEN_HEIGHT = 700;

        // Constant for elevation of pot above bottom of screen
        const int POT_ELEVATION = 50;
        // Constant for how far molecules spawn from borders of the pot
        const int SPAWNING_POT_OFFSET = 20;

        // Constant for how far molecules spawn from each other (multiple of the width of the molecule
        const int MOLECULE_DISTANCE = 4;
        // Constants for pot boundary positions
        const int LEFT_POT_BOUNDARY = (int)(MAXIMUM_SCREEN_WIDTH * (1.0 / 7.0));
        const int RIGHT_POT_BOUNDARY = (int)(MAXIMUM_SCREEN_WIDTH * (6.0 / 7.0));
        const int TOP_POT_BOUNDARY = -3;
        const int BOTTOM_POT_BOUNDARY = (int)(MAXIMUM_SCREEN_HEIGHT - POT_ELEVATION);
        // Constant for molecule size
        const int MOLECULE_SIZE = 4;

        // Starting number of molecules drawn
        const int STARTING_MOLECULES = 300;

        // Max number of molecules that can be in the pot
        const int MAX_MOLECULES = 1500;

        // Maximum and minimum bounds for angles being generated
        const int MINIMUM_ANGLE = 0;
        const int MAXIMUM_ANGLE = 359;
        
        // Constant for molecule speed, molecule travels at X pixels per second
        const int MOLECULE_SPEED = 1;

        // Pressure constant
        const float BASE_PRESSURE = 20f;

        // Gravity constant
        const float GRAVITY = 9.8f;

        // Maximum and minimum heat constant
        const int MAX_HEAT_MULTIPLIER = 10;
        const int MIN_HEAT_MULTIPLIER = 0;

        // Base heat fraction of heat multiplier, used to calculate heat level (added to speed)
        const float BASE_HEAT_FRACTION = 0.1f;

        // Rate at which heat will be lost when molecule is not in the heat zone
        const float HEAT_LOSS_RATE = BASE_HEAT_FRACTION / 10;

        // Heat level at which water will boil and turn into a gas
        const int HEAT_LEVEL_BOILING_POINT = 20;

        // Constant number of molecules added by pressing add water molecule button
        const int NUMBER_OF_MOLECULES_ADDED = 300;

        // ----------------------------------------------------------------------

        // Controls the elements that show based on the state of the simulation
        int simState = SIM_MENU;

        // Delta time (time passed between previous frame and current frame
        float deltaTime = 1;

        // Float array for molecule x and y speeds
        float[] moleculeXSpeeds = new float[STARTING_MOLECULES];
        float[] moleculeYSpeeds = new float[STARTING_MOLECULES];

        // Float array for molecule heat levels
        float[] moleculeHeatLevels = new float[STARTING_MOLECULES];

        // Bool array for whether molecule has turned into a gas
        bool[] moleculeIsGas = new bool[STARTING_MOLECULES];

        // State variable for whether heat is turned on or off
        bool heatOn = false;

        // State variable for whether to give molecules colors based on heat
        bool useHeatColors = false;

        // Number of active molecules
        int numOfActiveMolecules = STARTING_MOLECULES;

        // Heat that was inputted by the user via textbox
        int heatMultiplier;

        // Level above pot bottom at which pressure of air on the water body begins to take effect
        int baseAirPressureLevel = STARTING_MOLECULES;

        // Molecule with greatest and lowest heat
        int moleculeGreatestHeat;
        int moleculeLowestHeat;

        // Random number generator for x and y speeds and angles
        Random numberGenerator = new Random();

        // ----------------------------------------------------------------------

        // Graphics Data

        // Array for water molecule locations
        PointF[] moleculeLocations = new PointF[STARTING_MOLECULES];
        // Size of water molecules
        SizeF moleculeSize = new SizeF(MOLECULE_SIZE, MOLECULE_SIZE);
        // Array for water molecule hitboxes
        RectangleF[] moleculeHitboxes = new RectangleF[STARTING_MOLECULES];

        // Pot location
        Point potLocation;
        // Pot size
        Size potSize;
        // Pot rectangle
        Rectangle potRectangle;
        // Pot pen
        Pen potPen = new Pen(Brushes.Black, 5);
        
        // ----------------------------------------------------------------------

        public FelixA2WaterSimulator()
        {
            InitializeComponent();
            
            // Set key preview as true, keys will be processed before on screen controls
            KeyPreview = true;

            // Set and lock program size
            ClientSize = new Size(MAXIMUM_SCREEN_WIDTH, MAXIMUM_SCREEN_HEIGHT);
            MaximizeBox = false;
            FormBorderStyle = FormBorderStyle.FixedSingle;

            // Designer setup

            // Display heat multiplier current value in textbox
            txtHeatInput.Text = heatMultiplier.ToString();
            // Display heat status in label
            lblHeatStatus.Text = "HEAT OFF";
            // Display number of water molecules that can be added by the button
            btnAddWater.Text = "Add " + NUMBER_OF_MOLECULES_ADDED + " Water Molecules";
            // Set text heat input as uneditable (heat switch must be turned on first)
            txtHeatInput.Enabled = false;
            // Set buttons as un-interactable (enter key must be pressed to start simulation first)
            btnHeatSubmit.Enabled = false;
            btnHeatSwitch.Enabled = false;
            btnEnableColors.Enabled = false;
            btnAddWater.Enabled = false;
            

            // Setup pot rectangle data
            SetupPot();

            // Setup molecule rectangle data
            SetupMolecules(0, STARTING_MOLECULES, SPAWNING_POT_OFFSET);

            // Generate randomized directions (x and y speeds) for the new molecules
            GenerateNewMoleculeSpeeds(0, STARTING_MOLECULES);
            
        }

        // Contains main program loop that updates user interface and conducts in simulation operations, game loop uses a "Variable time-step"
        void SimulatorLoop()
        {      
            // Store previous and current frame's time
            float previousTime = 0;
            float currentTime = 0;
            // Set simulation as running
            simState = SIM_RUNNING;

            // Stopwatches to record time since last update and refresh
            Stopwatch timeCounter = Stopwatch.StartNew();
            Stopwatch refreshCounter = Stopwatch.StartNew();

            // Timer loop that runs while sim is running, main program loop
            while (simState == SIM_RUNNING)
            {
                // Store time passed as time elapsed since stop watch started last (stores the previous time that the loop ran (in milliseconds)
                currentTime = (float)timeCounter.Elapsed.TotalSeconds;
                // Set delta time as time change between last frame and current
                deltaTime = currentTime - previousTime;
                // Set previous time as current time for next frame to use
                previousTime = currentTime;

                // Loop through all current molecules (update)
                for (int currentMolecule = 0; currentMolecule < numOfActiveMolecules; currentMolecule++)
                {
                    // Calculate heat levels for molecules
                    DetermineHeatLevels(currentMolecule);
                    // Check collisions between molecules and themselves/pot boundaries
                    CheckCollisions(currentMolecule);
                    // Move water molecules on the screen
                    MoveMolecules(currentMolecule, false);
                    // Remove gas molecules that have left the simulation
                    RemoveMolecules(currentMolecule);
                }

                // If time from last refresh is greater than the frame time
                if (refreshCounter.Elapsed.TotalSeconds >= FRAME_TIME)
                {
                    // Find and display lowest and highest heat molecule stats
                    FindHeatStats();
                    // Redraws the screen (refresh)
                    Refresh();
                    // Restart stopwatch
                    refreshCounter.Restart();
                }

                // Allow other interactions/code while this loop is active
                Application.DoEvents();
            }
        }

        // Setup pot location size and rectangle
        void SetupPot()
        {
            // Pot location (X is the left pot boundary, Y is the pot's top boundary)
            potLocation = new Point(LEFT_POT_BOUNDARY, TOP_POT_BOUNDARY);
            // Pot size (width is the bottom pot boundary, height is the top pot boundary minus its bottom boundary)
            potSize = new Size(RIGHT_POT_BOUNDARY - LEFT_POT_BOUNDARY, BOTTOM_POT_BOUNDARY - TOP_POT_BOUNDARY);
            // Pot rectangle
            potRectangle = new Rectangle(potLocation, potSize);
        }

        // Setup molecule location size and hitboxes
        void SetupMolecules(int indexRangeMinimum, int indexRangeMaximum, int spawningPotHeight)
        {
            // Temporary local variable for molecule y and X location (spawning row and column), Y starts at a constant level above the pot bottom, X starts as 2 pixels right of left side of pot
            int spawningY = (int)(potRectangle.Bottom - spawningPotHeight);
            int spawningX = potRectangle.Left + SPAWNING_POT_OFFSET;

            // Temporary local variable for number of rows created
            int numOfRows = 0;

            // Temporary local variable for index number of first molecule in the row
            int firstMoleculeInRow = 0;

            // Loop through all current molecules in index range
            for (int currentMolecule = indexRangeMinimum; currentMolecule < indexRangeMaximum; currentMolecule++)
            {
                // If the right side of the current molecule's spawning X location has passed the right side of the pot minus the distance molecules spawn from the boundary
                if (spawningX + moleculeSize.Width >= potRectangle.Right - SPAWNING_POT_OFFSET)
                {
                    // Add a new row on top of previous for water molecule location spawns, set previous spawning Y location minus double a molecule's height, increase number of rows counter by 1
                    spawningY -= (int)moleculeSize.Height * 2;
                    numOfRows++;

                    // Reset spawning X location back to beginning of the row
                    spawningX = potRectangle.Left + SPAWNING_POT_OFFSET;

                    // If the difference between the distance from the left side of the first and the right side of the last molecule of the row is not equal to or greater than 0, shift the row right by one to centre the row 
                    while ((((moleculeLocations[firstMoleculeInRow].X - potRectangle.Left) - (potRectangle.Right - (moleculeLocations[currentMolecule - 1].X + moleculeSize.Width))) >= 0) == false)
                    {
                        // Count through each current row molecule
                        for (int currentRowMolecule = firstMoleculeInRow; currentRowMolecule < currentMolecule; currentRowMolecule++)
                        {
                            // Increase row molecule X location by 1
                            moleculeLocations[currentRowMolecule].X++;

                            // Refresh hitbox locations
                            moleculeHitboxes[currentRowMolecule].Location = moleculeLocations[currentRowMolecule];
                        }
                    }
                }

                // Set the first row molecule of the next row as the current molecule in the loop
                firstMoleculeInRow = currentMolecule;

                // Set molecule location as spawning X and Y location
                moleculeLocations[currentMolecule] = new PointF(spawningX, spawningY);

                // Create molecule hitbox using its location and size
                moleculeHitboxes[currentMolecule] = new RectangleF(moleculeLocations[currentMolecule], moleculeSize);

                // Set molecule spawning X location as the previous spawning X plus a multiplier a molecule's width
                spawningX += (int)moleculeSize.Width * MOLECULE_DISTANCE;
                


                // Centre rows of molecules in the pot

                // If the current molecule is part of the top row (original value of spawning Y minus number of rows times distance between each row), move molecule to the middle of the row to centre the top row (top row is not fully filled unlike the other rows)
                if (moleculeLocations[currentMolecule].Y == (potRectangle.Bottom - moleculeSize.Height - SPAWNING_POT_OFFSET) - (numOfRows * (moleculeSize.Height * 2)))
                {
                    // If the difference between the distance from the left side of the first and the right side of the last molecule of the top row is not equal to or greater than 0, shift the top row right by one
                    while ((((moleculeLocations[currentMolecule].X - potRectangle.Left) - (potRectangle.Right - (moleculeLocations[STARTING_MOLECULES - 1].X + moleculeSize.Width))) >= 0) == false)
                    {
                        // Count through each top row molecule
                        for (int currentTopRowMolecule = currentMolecule; currentTopRowMolecule < STARTING_MOLECULES; currentTopRowMolecule++)
                        {
                            // Increase top row molecule X location by 1
                            moleculeLocations[currentTopRowMolecule].X++;

                            // Refresh hitbox locations
                            moleculeHitboxes[currentTopRowMolecule].Location = moleculeLocations[currentTopRowMolecule];
                        }
                    }
                }

                // Set molecule heat levels as 0, set moleculeisgas state variables as false
                moleculeHeatLevels[currentMolecule] = 0;
                moleculeIsGas[currentMolecule] = false;
            }
        }

        // Generate random molecule directions when spawning based on a specified index range
        void GenerateNewMoleculeSpeeds(int indexRangeMinimum, int indexRangeMaximum)
        {
            // Temporary variable to store angle molecule will travel at
            int moleculeAngle = 0;

            // Loop through all molecules within the index range of the molecules
            for (int currentMolecule = indexRangeMinimum; currentMolecule < indexRangeMaximum; currentMolecule++)
            {
                // Generate a random angle at which the molecule will travel
                moleculeAngle = numberGenerator.Next(MINIMUM_ANGLE, MAXIMUM_ANGLE + 1);

                // Calculate molecule y and x axis speeds by multiplying molecule starting speed by Sine or Cosine of molecule angle (expressed in degrees)
                moleculeYSpeeds[currentMolecule] = (float)((Math.Sin(moleculeAngle * (Math.PI / 180.0))) * MOLECULE_SPEED);
                moleculeXSpeeds[currentMolecule] = (float)((Math.Cos(moleculeAngle * (Math.PI / 180.0))) * MOLECULE_SPEED);
            }
        }

        // Water molecules movement
        void MoveMolecules(int arrayIndex, bool collisionDetection)
        {
            // If the simulator is running and 1 or more molecules remain
            if (simState == SIM_RUNNING && numOfActiveMolecules > 0)
            {
                // If the function is being used only for collision detection in this instance
                if (collisionDetection == true)
                {
                    // Add x speeds to current molecule x locations
                    moleculeLocations[arrayIndex].X += deltaTime * moleculeXSpeeds[arrayIndex];

                    // Add y speeds to current molecule y locations: Using "Verlet Integration"
                    moleculeLocations[arrayIndex].Y += deltaTime * moleculeYSpeeds[arrayIndex];

                    // Refresh molecule hitboxes with new locations
                    moleculeHitboxes[arrayIndex].Location = moleculeLocations[arrayIndex];
                }

                // If the function is not being used for collision detection in this instance
                else if (collisionDetection == false)
                {
                    // If the molecule is within the air pressure level above the body of water
                    if (moleculeLocations[arrayIndex].Y <= potRectangle.Bottom - baseAirPressureLevel)
                    {
                        // Add x speeds to current molecule x locations
                        moleculeLocations[arrayIndex].X += deltaTime * moleculeXSpeeds[arrayIndex];

                        // Add y speeds to current molecule y locations
                        moleculeLocations[arrayIndex].Y += deltaTime * moleculeYSpeeds[arrayIndex];

                        // Refresh molecule hitboxes with new locations
                        moleculeHitboxes[arrayIndex].Location = moleculeLocations[arrayIndex];

                        // If the molecule is a gas
                        if (moleculeIsGas[arrayIndex] == true)
                        {
                            // Add gravity to current y speeds
                            moleculeYSpeeds[arrayIndex] += (GRAVITY - moleculeHeatLevels[arrayIndex]) * deltaTime;
                        }
                        // Not a gas
                        else
                        {
                            // Add pressure and gravity to current y speeds
                            float pressure = BASE_PRESSURE * ((potRectangle.Bottom - baseAirPressureLevel) - moleculeLocations[arrayIndex].Y) / 10;
                            moleculeYSpeeds[arrayIndex] += (pressure + GRAVITY - moleculeHeatLevels[arrayIndex]) * deltaTime;
                        }

                    }
                    
                    // If it is not within the air zone
                    else
                    {
                        // Add x speeds to current molecule x locations
                        moleculeLocations[arrayIndex].X += deltaTime * moleculeXSpeeds[arrayIndex];

                        // Add y speeds to current molecule y locations
                        moleculeLocations[arrayIndex].Y += deltaTime * moleculeYSpeeds[arrayIndex];

                        // Refresh molecule hitboxes with new locations
                        moleculeHitboxes[arrayIndex].Location = moleculeLocations[arrayIndex];

                        // Add gravity to current y speeds
                        moleculeYSpeeds[arrayIndex] += (GRAVITY - moleculeHeatLevels[arrayIndex]) * deltaTime;
                    }
                }
            }
        }

        // Check for molecule collisions with pot boundaries and other molecules
        void CheckCollisions(int moleculeA)
        {
            // If the simulator is running and 1 or more molecules remain
            if (simState == SIM_RUNNING && numOfActiveMolecules > 0)
            {
                // Update hitboxes to future position of molecule A, store current in temporary variables
                PointF tempLocationA = moleculeLocations[moleculeA];
                RectangleF tempRectangleA = moleculeHitboxes[moleculeA];
                // Call move molecule for molecule A
                MoveMolecules(moleculeA, true);

                // Check if molecule A is currently colliding with any other molecule
                for (int moleculeB = 0; moleculeB < numOfActiveMolecules; moleculeB++)
                {
                    // Update hitboxes to future position of molecule B, store current in temporary variables
                    PointF tempLocationB = moleculeLocations[moleculeB];
                    RectangleF tempRectangleB = moleculeHitboxes[moleculeB];
                    // Call move molecule for molecule B
                    MoveMolecules(moleculeB, true);

                    // If the molecule A post speed change is intersecting the hitbox of any other molecule (molecule being checked)
                    if (moleculeHitboxes[moleculeA].IntersectsWith(moleculeHitboxes[moleculeB]) && moleculeA != moleculeB)
                    {
                        // If A and B are not gases
                        if (moleculeIsGas[moleculeA] == false && moleculeIsGas[moleculeB] == false)
                        {
                            // Temporary variable to store molecule a's x speed
                            float tempXSpeed = moleculeXSpeeds[moleculeA];
                            // Set molecule a's x speed as the x speed of molecule b
                            moleculeXSpeeds[moleculeA] = moleculeXSpeeds[moleculeB];
                            // Set molecule b's x speed as the x speed of molecule a
                            moleculeXSpeeds[moleculeB] = tempXSpeed;

                            // Temporary variable to store molecule a's y speed
                            float tempYSpeed = moleculeYSpeeds[moleculeA];
                            // Set molecule a's x speed as the y speed of molecule b
                            moleculeYSpeeds[moleculeA] = moleculeYSpeeds[moleculeB];
                            // Set molecule b's x speed as the y speed of molecule a
                            moleculeYSpeeds[moleculeB] = tempYSpeed;

                            // Set locations, hitboxes back to previous molecule B
                            moleculeLocations[moleculeB] = tempLocationB;
                            moleculeHitboxes[moleculeB] = tempRectangleB;
                            moleculeHitboxes[moleculeB].Location = moleculeLocations[moleculeB];
                        }

                        // Exchange heat 

                        // If heat of A is greater than that of B
                        if (moleculeHeatLevels[moleculeA] > moleculeHeatLevels[moleculeB])
                        {
                            // Average the heat difference (add half of the difference between both heat levels to B, subtract from A)
                            float changeInHeat = (moleculeHeatLevels[moleculeA] - moleculeHeatLevels[moleculeB]) / 2;
                            moleculeHeatLevels[moleculeB] += changeInHeat;
                            moleculeHeatLevels[moleculeA] -= changeInHeat;
                        }
                        // If heat of B is greater than that of A
                        else if (moleculeHeatLevels[moleculeB] > moleculeHeatLevels[moleculeA])
                        {
                            // Average the heat difference (add half of the difference between both heat levels to A, subtract from B)
                            float changeInHeat = (moleculeHeatLevels[moleculeB] - moleculeHeatLevels[moleculeA]) / 2;
                            moleculeHeatLevels[moleculeA] += changeInHeat;
                            moleculeHeatLevels[moleculeB] -= changeInHeat;
                        }
                        // Do nothing if equal

                        // Set locations, hitboxes and y speeds back to previous for molecule B
                        moleculeLocations[moleculeB] = tempLocationB;
                        moleculeHitboxes[moleculeB] = tempRectangleB;
                        moleculeHitboxes[moleculeB].Location = moleculeLocations[moleculeB];

                        // Break out of for loop
                        break;
                    }
                    // Set locations, hitboxes and y speeds back to previous for molecule B
                    moleculeLocations[moleculeB] = tempLocationB;
                    moleculeHitboxes[moleculeB] = tempRectangleB;
                    moleculeHitboxes[moleculeB].Location = moleculeLocations[moleculeB];
                }


                // Check for boundary collisions

                // If the right or left side of molecule A hitbox is greater than/less than or equal to the right or left pot boundary
                if (moleculeHitboxes[moleculeA].Right > potRectangle.Right)
                {
                    // Set locations, hitboxes and y speeds back to previous for molecule
                    moleculeLocations[moleculeA] = tempLocationA;
                    moleculeHitboxes[moleculeA] = tempRectangleA;

                    // Set molecule x location to the right boundary
                    moleculeLocations[moleculeA].X = potRectangle.Right - moleculeSize.Width;

                    // Multiply x speed of current molecule by negative 1, reversing travel direction
                    moleculeXSpeeds[moleculeA] *= -1;

                    // Refresh location
                    moleculeHitboxes[moleculeA].Location = moleculeLocations[moleculeA];
                }
                else if (moleculeHitboxes[moleculeA].Left < potRectangle.Left)
                {
                    // Set locations, hitboxes and y speeds back to previous for molecule A
                    moleculeLocations[moleculeA] = tempLocationA;
                    moleculeHitboxes[moleculeA] = tempRectangleA;

                    // Set molecule x location to the right boundary
                    moleculeLocations[moleculeA].X = potRectangle.Left;

                    // Multiply x speed of current molecule by negative 1, reversing travel direction
                    moleculeXSpeeds[moleculeA] *= -1;

                    // Refresh location
                    moleculeHitboxes[moleculeA].Location = moleculeLocations[moleculeA];
                }
                // If the bottom or top side of molecule A hitbox is greater than/less than or equal to the bottom or top pot boundary 
                if (moleculeHitboxes[moleculeA].Bottom > potRectangle.Bottom)
                {
                    // Set locations, hitboxes and y speeds back to previous for molecule A
                    moleculeLocations[moleculeA] = tempLocationA;
                    moleculeHitboxes[moleculeA] = tempRectangleA;

                    // Set molecule y location to the right boundary
                    moleculeLocations[moleculeA].Y = potRectangle.Bottom - moleculeSize.Height;

                    // Multiply y speed of current molecule by negative 1, reversing travel direction
                    moleculeYSpeeds[moleculeA] *= -1;

                    // Refresh location
                    moleculeHitboxes[moleculeA].Location = moleculeLocations[moleculeA];
                }

                // Otherwise
                else
                {
                    // Set locations, hitboxes and y speeds back to previous for molecule A
                    moleculeLocations[moleculeA] = tempLocationA;
                    moleculeHitboxes[moleculeA] = tempRectangleA;
                    // Refresh location
                    moleculeHitboxes[moleculeA].Location = moleculeLocations[moleculeA];
                }
            }
        }

        // Remove molecules from the simulation when needed
        void RemoveMolecules(int currentMolecule)
        {
            // If sim is running and 1 or more molecules remain
            if (simState == SIM_RUNNING && numOfActiveMolecules > 0)
            {
                // If molecule A is gas and has left the simulation space (passed top of pot/screen)
                if (moleculeHitboxes[currentMolecule].Top <= potRectangle.Top && moleculeIsGas[currentMolecule] == true)
                {
                    // Run through all active molecules that come after the current molecule
                    for (int i = currentMolecule; i < numOfActiveMolecules - 1; i++)
                    {
                        // Move all elements in all arrays that are one in front of index 'i' to index 'i', effectively destroying the current molecule's data
                        moleculeLocations[i] = moleculeLocations[i + 1];
                        moleculeHitboxes[i] = moleculeHitboxes[i + 1];
                        moleculeHitboxes[i].Location = moleculeLocations[i];
                        moleculeXSpeeds[i] = moleculeXSpeeds[i + 1];
                        moleculeYSpeeds[i] = moleculeYSpeeds[i + 1];
                        moleculeHeatLevels[i] = moleculeHeatLevels[i + 1];
                        moleculeIsGas[i] = moleculeIsGas[i + 1];
                    }

                    // Decrease number of active molecules by 1
                    numOfActiveMolecules--;

                    // If number of active molecules is divisible by 300 meaning modulus returns 0 remainder (arrays are resized after every 300 is taken out of simulation)
                    if (numOfActiveMolecules % 300 == 0)
                    {
                        // Resize all arrays

                        // Resize locations
                        PointF[] tempLocations = moleculeLocations;
                        moleculeLocations = new PointF[numOfActiveMolecules];
                        // Resize rectangles
                        RectangleF[] tempRectangles = moleculeHitboxes;
                        moleculeHitboxes = new RectangleF[numOfActiveMolecules];
                        // Resize x speeds
                        float[] tempFloats1 = moleculeXSpeeds;
                        moleculeXSpeeds = new float[numOfActiveMolecules];
                        // Resize y speeds
                        float[] tempFloats2 = moleculeYSpeeds;
                        moleculeYSpeeds = new float[numOfActiveMolecules];
                        // Resize heat levels
                        float[] tempFloats3 = moleculeHeatLevels;
                        moleculeHeatLevels = new float[numOfActiveMolecules];
                        // Resize isgas state variables
                        bool[] tempBools = moleculeIsGas;
                        moleculeIsGas = new bool[numOfActiveMolecules];

                        // Run through all indices of current number of active molecules, setting the value in the arrays to their corresponding value in the temp arrays
                        for (int i = 0; i < numOfActiveMolecules; i++)
                        {
                            moleculeLocations[i] = tempLocations[i];
                            moleculeHitboxes[i] = tempRectangles[i];
                            moleculeXSpeeds[i] = tempFloats1[i];
                            moleculeYSpeeds[i] = tempFloats2[i];
                            moleculeHeatLevels[i] = tempFloats3[i];
                            moleculeIsGas[i] = moleculeIsGas[i];
                        } 
                    }
                }

                // Increase base water air pressure level by 50 times the fractional value of molecules added to the starting value if active molecules is greater or equal to starting
                if (numOfActiveMolecules >= STARTING_MOLECULES)
                {
                    baseAirPressureLevel = STARTING_MOLECULES + (int)(50.0f * ((numOfActiveMolecules - STARTING_MOLECULES) / NUMBER_OF_MOLECULES_ADDED));
                }
                else
                {
                    // Set air pressure level to number of active molecules
                    baseAirPressureLevel = numOfActiveMolecules;
                }
            }
        }

        // Determine heat level of current molecule
        void DetermineHeatLevels(int currentMolecule)
        {
            // If sim is running and 1 or more molecules remain
            if (simState == SIM_RUNNING && numOfActiveMolecules > 0)
            {
                // If heat is on
                if (heatOn == true)
                {
                    // If molecule is below the level of the pot's bottom plus twice a molecule's height
                    if (moleculeHitboxes[currentMolecule].Bottom >= potRectangle.Bottom - moleculeSize.Height * 2)
                    {
                        // Add base fraction of heat multiplier to the current molecule's heat level
                        moleculeHeatLevels[currentMolecule] += BASE_HEAT_FRACTION * heatMultiplier;

                    }
                }

                // If molecule is not a gas
                if (moleculeIsGas[currentMolecule] == false)
                {
                    // Lose heat gradually
                    moleculeHeatLevels[currentMolecule] -= HEAT_LOSS_RATE;
                }

                // If the heat level has fallen below 0
                if (moleculeHeatLevels[currentMolecule] < 0)
                {
                    // Set heat level to 0
                    moleculeHeatLevels[currentMolecule] = 0;
                }

                // If the molecule has turned into gas (molecule heat level acceleration is greater than pressure)
                if (moleculeHeatLevels[currentMolecule] >= HEAT_LEVEL_BOILING_POINT)
                {
                    // Set molecule state is gas true
                    moleculeIsGas[currentMolecule] = true;
                }
                // Otherwise set as false
                else if (moleculeHeatLevels[currentMolecule] <= HEAT_LEVEL_BOILING_POINT)
                {
                    // Set molecule state is gas false
                    moleculeIsGas[currentMolecule] = false;
                }
            }
        }

        // Find molecule with greatest heat and molecule with lowest heat
        void FindHeatStats()
        {
            // If sim is running and 1 or more molecules remain
            if (simState == SIM_RUNNING && numOfActiveMolecules > 0)
            {
                // Start at index 0 being the greatest and lowest heat
                moleculeGreatestHeat = 0;
                moleculeLowestHeat = 0;

                // Find greatest heat
                for (int currentMolecule = 1; currentMolecule < numOfActiveMolecules; currentMolecule++)
                {
                    // If molecule that is currently greatest is not greater than the current molecule
                    if (moleculeHeatLevels[moleculeGreatestHeat] < moleculeHeatLevels[currentMolecule])
                    {
                        // Set new greatest as current
                        moleculeGreatestHeat = currentMolecule;
                    }
                    // If molecule that is currently lowest is greater than the current molecule
                    if (moleculeHeatLevels[moleculeLowestHeat] > moleculeHeatLevels[currentMolecule])
                    {
                        // Set new lowest as current
                        moleculeLowestHeat = currentMolecule;
                    }
                }

                // Calculate temperatures to be displayed
                float greatestHeat = (float)Math.Round(((moleculeHeatLevels[moleculeGreatestHeat] / 20.0) * 100.0), 1);
                float lowestHeat = (float)Math.Round(((moleculeHeatLevels[moleculeLowestHeat] / 20.0) * 100.0), 1);

                // Calculate greatest and lowest temperature based on heat level
                if (moleculeHeatLevels[moleculeGreatestHeat] > HEAT_LEVEL_BOILING_POINT)
                {
                    // Set temperature to be displayed as 100 degrees
                    greatestHeat = 100.0f;
                }


                // Update labels with greatest and lowest heat data
                lblGreatestHeat.Text = "Greatest Water Heat:\r\nMolecule #" + moleculeGreatestHeat.ToString() + ", " + greatestHeat.ToString() + "°";
                lblLowestHeat.Text = "Lowest Water Heat:\r\nMolecule #" + moleculeLowestHeat.ToString() + ", " + lowestHeat.ToString() + "°";
                
            }
            
            // If no more molecules remain
            else if (numOfActiveMolecules == 0)
            {
                // Update labels with error message
                lblGreatestHeat.Text = "Error: No Molecules Remain!";
                lblLowestHeat.Text = "Error: No Molecules Remain!";
            }
        }

        // When screen is updated
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // Draw pot outline
            e.Graphics.DrawRectangle(potPen, potRectangle);

            // If the program is running
            if (simState == SIM_RUNNING)
            {
                // Draw each molecule, using all molecule rectangle array elements
                for (int currentMolecule = 0; currentMolecule < numOfActiveMolecules; currentMolecule++)
                {
                    // If molecule isnt a gas and heat colors are disabled
                    if (moleculeIsGas[currentMolecule] == false && useHeatColors == false)
                    {
                        // Draw molecules as blue
                        e.Graphics.FillRectangle(Brushes.Blue, moleculeHitboxes[currentMolecule]);
                    }

                    // If molecule isnt a gas and heat colors are enabled
                    if (moleculeIsGas[currentMolecule] == false && useHeatColors == true)
                    {
                        // Blue when no heat
                        if (moleculeHeatLevels[currentMolecule] == 0)
                        {
                            e.Graphics.FillRectangle(Brushes.Blue, moleculeHitboxes[currentMolecule]);
                        }
                        // Yellow when heat is between 0 and 25 degrees
                        if (moleculeHeatLevels[currentMolecule] > 0 && moleculeHeatLevels[currentMolecule] <= HEAT_LEVEL_BOILING_POINT * 0.25f)
                        {
                            e.Graphics.FillRectangle(Brushes.Yellow, moleculeHitboxes[currentMolecule]);
                        }
                        // Dark orange when heat is between 25 and 50 degrees
                        else if (moleculeHeatLevels[currentMolecule] > HEAT_LEVEL_BOILING_POINT * 0.25f && moleculeHeatLevels[currentMolecule] <= HEAT_LEVEL_BOILING_POINT * 0.5f)
                        {
                            e.Graphics.FillRectangle(Brushes.DarkOrange, moleculeHitboxes[currentMolecule]);
                        }
                        // Red when heat is between 50 and 75
                        else if (moleculeHeatLevels[currentMolecule] > HEAT_LEVEL_BOILING_POINT * 0.5f && moleculeHeatLevels[currentMolecule] <= HEAT_LEVEL_BOILING_POINT * 0.75f)
                        {
                            e.Graphics.FillRectangle(Brushes.Red, moleculeHitboxes[currentMolecule]);
                        }
                        // Maroon when heat is between 75 and less than boiling
                        else if (moleculeHeatLevels[currentMolecule] > HEAT_LEVEL_BOILING_POINT * 0.75f && moleculeHeatLevels[currentMolecule] < HEAT_LEVEL_BOILING_POINT)
                        {
                            e.Graphics.FillRectangle(Brushes.Maroon, moleculeHitboxes[currentMolecule]);
                        }
                    }
                    
                    // If molecule is a gas
                    else if (moleculeIsGas[currentMolecule] == true)
                    {
                        e.Graphics.FillRectangle(Brushes.DarkGray, moleculeHitboxes[currentMolecule]);
                    }
                }
            }
        }

        // When a key is pressed
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            // If the simulation is currently in the menu
            if (simState == SIM_MENU)
            {
                // If enter is being pressed
                if (e.KeyCode == Keys.Enter)
                {
                    // Start simulation by changing current simulation state and starting main program loop
                    simState = SIM_RUNNING;
                    // Enable buttons
                    btnHeatSwitch.Enabled = true;
                    btnAddWater.Enabled = true;

                    // Start main program loop
                    SimulatorLoop();
                }
            }
            // If the simulator is running
            else if (simState == SIM_RUNNING)
            {
                // If the current key is enter
                if (e.KeyCode == Keys.Enter)
                {
                    // Set sim state as paused
                    simState = SIM_PAUSED;
                }
            }
            // If sim is paused
            else if (simState == SIM_PAUSED)
            {
                // If the current key is enter
                if (e.KeyCode == Keys.Enter)
                {
                    // Set sim as running and call main sim loop
                    simState = SIM_RUNNING;
                    SimulatorLoop();
                }
            }
        }

        // When button heatSubmit is clicked
        private void btnHeatSubmit_Click(object sender, EventArgs e)
        {
            // If the input provided in textbox is not an integer
            if ((int.TryParse(txtHeatInput.Text, out heatMultiplier)) == false)
            {
                // Set heat input and textbox text as minimum heat value
                heatMultiplier = MIN_HEAT_MULTIPLIER;
                txtHeatInput.Text = heatMultiplier.ToString();
            }

            // If heat provided is less than minimum heat
            else if (heatMultiplier <= MIN_HEAT_MULTIPLIER)
            {
                // Set heat input and textbox text as min heat value (meaning heat is off)
                heatMultiplier = MIN_HEAT_MULTIPLIER;
                txtHeatInput.Text = heatMultiplier.ToString();
                // Turn off heat
                heatOn = false;
                lblHeatStatus.Text = "HEAT OFF";
                lblHeatStatus.ForeColor = Color.Black;
            }
            // If provided heat is greater than max heat
            else if (heatMultiplier > MAX_HEAT_MULTIPLIER)
            {
                // Set heat input and textbox text as max heat value
                heatMultiplier = MAX_HEAT_MULTIPLIER;
                txtHeatInput.Text = heatMultiplier.ToString();
            }
        }

        // When heat switch is pressed
        private void btnHeatSwitch_Click(object sender, EventArgs e)
        {
            // Invert current boolean value of heatOn, turning heat on or off
            heatOn = !heatOn;

            // If heat is on
            if (heatOn == true)
            {
                // Set heat status as on and enable textbox and buttons
                lblHeatStatus.Text = "HEAT ON";
                lblHeatStatus.ForeColor = Color.Red;
                txtHeatInput.Enabled = true;
                btnHeatSubmit.Enabled = true;
                btnEnableColors.Enabled = true;

                // If heat multiplier is at 0
                if (heatMultiplier == 0)
                {
                    // Set heat multiplier to 1 and update textbox
                    heatMultiplier = 1;
                    txtHeatInput.Text = heatMultiplier.ToString();
                }
            }

            // If heat is off
            else
            {
                // Set heat status as off and disable heat input
                lblHeatStatus.Text = "HEAT OFF";
                lblHeatStatus.ForeColor = Color.White;
                txtHeatInput.Text = MIN_HEAT_MULTIPLIER.ToString();
                txtHeatInput.Enabled = false;
            }
        }

        // When button is clicked
        private void btnEnableColors_Click(object sender, EventArgs e)
        {
            // Enable use of colors to display heat of molecules
            useHeatColors = !useHeatColors;
            
            // If on
            if (useHeatColors == true)
            {
                // Change text and color to on
                lblColorsStatus.Text = "IR ON";
                lblColorsStatus.ForeColor = Color.Gold;
            }
            // If off
            else
            {
                // Change to off
                lblColorsStatus.Text = "IR OFF";
                lblColorsStatus.ForeColor =  Color.White;
            }
        }

        // When button is clicked
        private void btnAddWater_Click(object sender, EventArgs e)
        {
            // If the current number of active molecules are less then the max molecules allowed minus number of molecules that can be added at once
            if (numOfActiveMolecules <= MAX_MOLECULES - NUMBER_OF_MOLECULES_ADDED)
            {
                // Resize all arrays

                // Resize locations
                PointF[] tempLocations = moleculeLocations;
                moleculeLocations = new PointF[numOfActiveMolecules + NUMBER_OF_MOLECULES_ADDED];
                // Resize rectangles
                RectangleF[] tempRectangles = moleculeHitboxes;
                moleculeHitboxes = new RectangleF[numOfActiveMolecules + NUMBER_OF_MOLECULES_ADDED];
                // Resize x speeds
                float[] tempFloats1 = moleculeXSpeeds;
                moleculeXSpeeds = new float[numOfActiveMolecules + NUMBER_OF_MOLECULES_ADDED];
                // Resize y speeds
                float[] tempFloats2 = moleculeYSpeeds;
                moleculeYSpeeds = new float[numOfActiveMolecules + NUMBER_OF_MOLECULES_ADDED];
                // Resize heat levels
                float[] tempFloats3 = moleculeHeatLevels;
                moleculeHeatLevels = new float[numOfActiveMolecules + NUMBER_OF_MOLECULES_ADDED];
                // Resize isgas state variables
                bool[] tempBools = moleculeIsGas;
                moleculeIsGas = new bool[numOfActiveMolecules + NUMBER_OF_MOLECULES_ADDED];

                // Run through all indices of current number of active molecules, setting the value in the arrays to their corresponding value in the temp arrays
                for (int currentMolecule = 0; currentMolecule < numOfActiveMolecules; currentMolecule++)
                {
                    moleculeLocations[currentMolecule] = tempLocations[currentMolecule];
                    moleculeHitboxes[currentMolecule] = tempRectangles[currentMolecule];
                    moleculeXSpeeds[currentMolecule] = tempFloats1[currentMolecule];
                    moleculeYSpeeds[currentMolecule] = tempFloats2[currentMolecule];
                    moleculeHeatLevels[currentMolecule] = tempFloats3[currentMolecule];
                    moleculeIsGas[currentMolecule] = moleculeIsGas[currentMolecule];
                } 

                // Setup and generate new molecule speeds using number of active molecules as minimum index and molecules added plus active molecules as the maximum index, set spawning level as 150 pixels above base air pressure level
                SetupMolecules(numOfActiveMolecules, numOfActiveMolecules + NUMBER_OF_MOLECULES_ADDED, baseAirPressureLevel - 150);
                GenerateNewMoleculeSpeeds(numOfActiveMolecules, numOfActiveMolecules + NUMBER_OF_MOLECULES_ADDED);

                // Increase number of active molecules by the number of molecules added at once
                numOfActiveMolecules += NUMBER_OF_MOLECULES_ADDED;
            }
        }

        // When program is being closed
        private void FelixA2WaterSimulator_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Turn off sim
            simState = SIM_OFF;
            Application.Exit();
        }
    }
}
