/*
 * Felix Liu
 * January 15th, 2017
 * 
 * Contains types, methods, properties for creating widget objects, declaring/managing user and background objects, 
 * defining start game and game over widgets and menus with static constants and enums,
 * and drawing all UI related elements.
 * 
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace BosenQuFelixLiuFinalCulminatingProject.GUI
{
    /// <summary>
    /// Enumeration that defines type/state of the UI.
    /// </summary>
    enum UIState { Start, Game, GameOver };
    /// <summary>
    /// Enumeration that defines the functions of each button used in the UI.
    /// </summary>
    enum UIButtonFunctions { StartGame, ViewTowers, Pause, Settings, CloseBars, Tower1, Tower2, Tower3, Restart };

    /// <summary>
    /// Stores indices of start page widget instances in the UI's start Widget array.
    /// </summary>
    static class UIStartWidgets
    {
        // Background container index
        public const int BACKGROUND = 0;
        // Start game button index
        public const int START_GAME = 1;

        // Total number of widgets
        public const int NUM_OF_START_WIDGETS = 2;
    }

    /// <summary>
    /// Stores indices of game widget instances in the UI's game Widget array.
    /// </summary>
    static class UIGameWidgets
    {
        // Toolbar container index
        public const int TOOL_BAR = 0;
        // User stats bar container index
        public const int USER_STATS_BAR = 1;
        // User lives container index
        public const int USER_LIVES = 2;
        // User money container index
        public const int USER_MONEY = 3;
        // User waves container index
        public const int USER_WAVE = 4;
        // User stat panel container index (outlines unplacable tower area)
        public const int USER_PANEL = 5;
        // View towers button index
        public const int VIEW_TOWERS = 6;
        // Pause button index
        public const int PAUSE = 7;
        // Settings button index
        public const int SETTINGS = 8;
        // Close tower placements button index
        public const int CLOSE_BARS = 9;
        // Place tower 1-3 button indices
        public const int TOWER_1 = 10;
        public const int TOWER_2 = 11;
        public const int TOWER_3 = 12;

        // Total number of widgets
        public const int NUM_OF_GAME_WIDGETS = 13;
    }

    /// <summary>
    /// Stores indices of game over widget instances in the UI's game over Widget array.
    /// </summary>
    static class UIGameOverWidgets
    {
        // Background container index
        public const int BACKGROUND = 0;
        // Game over container index
        public const int GAME_OVER = 1;
        // Waves survived container index
        public const int WAVES_SURVIVED = 2;
        // Game restart button index
        public const int RESTART = 3;

        // Total number of widgets
        public const int NUM_OF_GAME_OVER_WIDGETS = 4;
    }

    /// <summary>
    /// Renders, declares, and manages all user interface related instances .
    /// Controls state and layout of UI and provides methods for dependant classes to access events.
    /// </summary>
    class UI
    {
        // Properties, fields and object declarations

        // Boolean property, whether or not the game has started
        public bool GameStart { get; set; }
        // UIState property, enum variable that stores the current UI menu/state
        public UIState CurrentState { get; set; }
        // Widget instance arrays (widgets for start, game and game over states), used for storing the instances of the subclasses of the abstract class Widget
        public Widget[] startWidgets = new Widget[UIStartWidgets.NUM_OF_START_WIDGETS];
        public Widget[] gameWidgets = new Widget[UIGameWidgets.NUM_OF_GAME_WIDGETS];
        public Widget[] gameOverWidgets = new Widget[UIGameOverWidgets.NUM_OF_GAME_OVER_WIDGETS];
        // Instance of placement that places/generates a tower
        Placement placeTower;
        // Static instance of the background and the user
        public static Background background;
        public static User user = new User();
        // UIButtonFunction property, enum variable that allows the UI class to track which tower is being placed based on which tower button was pressed
        UIButtonFunctions currentTowerPlacement;
        // SolidBrush brush that lights up a widget when hovered using a translucent white overlay
        SolidBrush hoverBrush = new SolidBrush(Color.FromArgb(25, Color.White));

        // UI constructor, takes the current state of the UI as its parameter
        public UI(UIState startingMenuState)
        {
            // Initialize UI based on the current UI state
            InitializeUI(startingMenuState);
        }

        // Initializes the UI by calling procedures based on the current UI state. No return, parameter for passing in the selected UI state of type UIState.
        void InitializeUI(UIState startingMenuType)
        {
            // Set current state to the passed UIState
            CurrentState = startingMenuType;

            // If the current state is the start state, initialize start menus, otherwise initialize game menus
            if (CurrentState == UIState.Start)
                StartMenus();
            else
                GameMenus();
        }

        // Initialize start menu widgets by assigning array elements instances of widget objects, no return or parameters
        void StartMenus()
        {
            // Set button fonts and locations
            int startButtonsXLocation =  (Main.size.Width / 2) - (Main.size.Width / 4);
            Font btnFont = new Font("Trebuchet MS", 52);
            StringFormat btnTxtFormat = new StringFormat();
            btnTxtFormat.Alignment = StringAlignment.Center;
            btnTxtFormat.LineAlignment = StringAlignment.Center;

            // Assign widget array elements with corresponding widget instances, pass in widget attributes to the constructor
            startWidgets[UIStartWidgets.BACKGROUND] = new Container(false, true, 0, 0, Main.size.Width, Main.size.Height, Brushes.White);
            startWidgets[UIStartWidgets.START_GAME] = new Button(UIButtonFunctions.StartGame, true, true, startButtonsXLocation, Main.size.Height / 2,
                Main.size.Width / 2, Main.size.Height / 6, Brushes.DarkGreen, "Start Game", btnFont, Brushes.White, btnTxtFormat, Brushes.Black, 3, hoverBrush);

            //startWidgets[UIStartWidgets.INSTRUCTIONS] = new Button(UIButtonFunctions.Instructions, true, true, startButtonsXLocation, (int)(Main.size.Height * 0.75f),
            //    Main.size.Width / 2, Main.size.Height / 6, Brushes.Black, "Instructions", btnFont, Brushes.White, btnTxtFormat, Brushes.Black, 3, hoverBrush);
        }

        // Initialize game menu widgets by assigning array elements instances of widget objects, no return or parameters
        void GameMenus()
        {
            // Set button fonts and locations
            Font buttonFont = new Font("Arial", 14);
            Font userStatFont = new Font("Century Gothic", 18);
            StringFormat buttonTxtFormat = new StringFormat();
            buttonTxtFormat.Alignment = StringAlignment.Center;
            buttonTxtFormat.LineAlignment = StringAlignment.Center;

            // Set button size, distance between each button, and locations + sizes of toolbars and user spaces
            Size gameButtonSize = new Size(64, 64);
            int gameButtonMargin = (int)(gameButtonSize.Width * 0.45f);
            Size toolBarSize = new Size(4 * gameButtonMargin + 3 * gameButtonSize.Width, gameButtonSize.Height + gameButtonMargin);
            Point toolBarLocation = new Point(Main.size.Width - toolBarSize.Width, 0);
 
            Size userStatsSize = new Size(toolBarSize.Width, (int)(toolBarSize.Height * (3f/4f)));
            Point userStatsLocation = new Point(Main.size.Width - userStatsSize.Width, toolBarSize.Height);
            int userStatsStatSpace = userStatsSize.Width / 3;

            // Assign widget array elements with corresponding widget instances, pass in widget attributes to the constructor

            // Container widgets, toolbars and stats
            gameWidgets[UIGameWidgets.TOOL_BAR] = new Container(false, true, toolBarLocation.X, toolBarLocation.Y, toolBarSize.Width, toolBarSize.Height, Brushes.Black);
            gameWidgets[UIGameWidgets.USER_STATS_BAR] = new Container(false, true, userStatsLocation.X, userStatsLocation.Y, userStatsSize.Width, userStatsSize.Height, Brushes.Black);
            gameWidgets[UIGameWidgets.USER_LIVES] = new Container(false, true, userStatsLocation.X, userStatsLocation.Y, userStatsStatSpace, userStatsSize.Height, new StringBuilder().Insert(0, "\u2665", user.Lives).ToString(), userStatFont, Brushes.White, buttonTxtFormat);
            gameWidgets[UIGameWidgets.USER_MONEY] = new Container(false, true, gameWidgets[UIGameWidgets.USER_LIVES].BoundingBox.Right, userStatsLocation.Y, userStatsStatSpace, userStatsSize.Height, "$" + user.Money.ToString(), userStatFont, Brushes.White, buttonTxtFormat);
            gameWidgets[UIGameWidgets.USER_WAVE] = new Container(false, true, gameWidgets[UIGameWidgets.USER_MONEY].BoundingBox.Right, userStatsLocation.Y, userStatsStatSpace, userStatsSize.Height, "WAVE " + user.WavesSurvived.ToString(), userStatFont, Brushes.White, buttonTxtFormat);
            gameWidgets[UIGameWidgets.USER_PANEL] = new Container(false, false, toolBarLocation.X, toolBarLocation.Y, toolBarSize.Width, toolBarSize.Height + userStatsSize.Height * 2, new SolidBrush(Color.Empty));
            gameWidgets[UIGameWidgets.CLOSE_BARS] = new Button(UIButtonFunctions.CloseBars, false, false, 0, 0, 20, 20, Brushes.Red, "X", buttonFont, Brushes.Black, buttonTxtFormat, hoverBrush);

            // First layer of selection buttons
            gameWidgets[UIGameWidgets.VIEW_TOWERS] = new Button(UIButtonFunctions.ViewTowers, true, true, toolBarLocation.X + gameButtonMargin,
                gameWidgets[UIGameWidgets.TOOL_BAR].BoundingBox.Y + gameButtonMargin / 2, gameButtonSize.Width, gameButtonSize.Height, Brushes.Blue, hoverBrush);
            gameWidgets[UIGameWidgets.PAUSE] = new Button(UIButtonFunctions.Pause, true, true, gameWidgets[UIGameWidgets.VIEW_TOWERS].BoundingBox.Right + gameButtonMargin,
                gameWidgets[UIGameWidgets.VIEW_TOWERS].BoundingBox.Y, gameButtonSize.Width, gameButtonSize.Height, Brushes.Blue, hoverBrush);
            gameWidgets[UIGameWidgets.SETTINGS] = new Button(UIButtonFunctions.Settings, true, true, gameWidgets[UIGameWidgets.PAUSE].BoundingBox.Right + gameButtonMargin,
                gameWidgets[UIGameWidgets.VIEW_TOWERS].BoundingBox.Y, gameButtonSize.Width, gameButtonSize.Height, Brushes.Blue, hoverBrush);

            // Second layer of selection buttons, places towers
            gameWidgets[UIGameWidgets.TOWER_1] = new Button(UIButtonFunctions.Tower1, false, false, toolBarLocation.X + gameButtonMargin,
                gameWidgets[UIGameWidgets.TOOL_BAR].BoundingBox.Y + gameButtonMargin / 2, gameButtonSize.Width, gameButtonSize.Height, Properties.Resources.NormalTower, hoverBrush);
            gameWidgets[UIGameWidgets.TOWER_2] = new Button(UIButtonFunctions.Tower2, false, false, gameWidgets[UIGameWidgets.TOWER_1].BoundingBox.Right + gameButtonMargin,
                gameWidgets[UIGameWidgets.TOWER_1].BoundingBox.Y, gameButtonSize.Width, gameButtonSize.Height, Properties.Resources.LongRangeTower, hoverBrush);
            gameWidgets[UIGameWidgets.TOWER_3] = new Button(UIButtonFunctions.Tower3, false, false, gameWidgets[UIGameWidgets.TOWER_2].BoundingBox.Right + gameButtonMargin,
                gameWidgets[UIGameWidgets.TOWER_1].BoundingBox.Y, gameButtonSize.Width, gameButtonSize.Height, Properties.Resources.MissileTowerWithProjectile, hoverBrush);

            // Initialize background, pass in the user panel area to be excluded from the play area
            background = new Background(gameWidgets[UIGameWidgets.USER_PANEL].BoundingBox);
        }

        // Initialize game over widgets by assigning array elements instances of widget objects, no return or parameters
        void GameOver()
        {
            // Set font and margins
            Font gameOverFont = new Font("Century Gothic", 72);
            Font wavesSurvivedFont = new Font("Century Gothic", 16);
            StringFormat gameOverFormat = new StringFormat();
            gameOverFormat.Alignment = StringAlignment.Center;
            gameOverFormat.LineAlignment = StringAlignment.Center;
            int gameOverMargin = 32;

            // Assign widget array elements with corresponding widget instances, pass in widget attributes to the constructor
            
            // Background and game over info/prompts 
            gameOverWidgets[UIGameOverWidgets.BACKGROUND] = new Container(false, true, 0, 0, Main.size.Width, Main.size.Height, Brushes.Black);
            gameOverWidgets[UIGameOverWidgets.GAME_OVER] = new Container(false, true, (Main.size.Width / 2) - (Main.size.Width / 4),
                Main.size.Height / 3, Main.size.Width / 2, Main.size.Height / 5, "GAME OVER", gameOverFont, Brushes.Red, gameOverFormat);
            gameOverWidgets[UIGameOverWidgets.WAVES_SURVIVED] = new Container(false, true, gameOverWidgets[UIGameOverWidgets.GAME_OVER].BoundingBox.X,
                gameOverWidgets[UIGameOverWidgets.GAME_OVER].BoundingBox.Bottom + gameOverMargin,
                Main.size.Width / 2, Main.size.Height / 5, "Waves Survived: " + user.WavesSurvived.ToString(), wavesSurvivedFont, Brushes.White, gameOverFormat);
            // Restart button
            gameOverWidgets[UIGameOverWidgets.RESTART] = new Button(UIButtonFunctions.Restart, true, true, (gameOverWidgets[UIGameOverWidgets.GAME_OVER].BoundingBox.X + gameOverWidgets[UIGameOverWidgets.GAME_OVER].BoundingBox.Width / 2) - gameWidgets[UIGameOverWidgets.GAME_OVER].BoundingBox.Width / 4,
                gameOverWidgets[UIGameOverWidgets.WAVES_SURVIVED].BoundingBox.Bottom + gameOverMargin, gameWidgets[UIGameOverWidgets.GAME_OVER].BoundingBox.Width / 2,
                gameWidgets[UIGameOverWidgets.GAME_OVER].BoundingBox.Height / 2, Brushes.LightGray, "RESTART", wavesSurvivedFont, Brushes.Black, gameOverFormat, hoverBrush);
        }

        // Determines the current function of the button that was pressed. No return, takes in the function of a button of type UIButtonFunctions
        void ButtonFunctions(UIButtonFunctions btnFunction)
        {
            // Switch statement for determining actions for a given button function e.g. StartGame (case statements are self explanatory)
            switch (btnFunction)
            {
                case UIButtonFunctions.StartGame:
                    // Initialize the UI's game menu
                    InitializeUI(UIState.Game);
                    // Set the game as started
                    GameStart = true;
                    break;
                case UIButtonFunctions.ViewTowers:
                    // Hide first layer buttons
                    gameWidgets[UIGameWidgets.VIEW_TOWERS].Visible = false;
                    gameWidgets[UIGameWidgets.VIEW_TOWERS].Interactable = false;
                    gameWidgets[UIGameWidgets.PAUSE].Visible = false;
                    gameWidgets[UIGameWidgets.PAUSE].Interactable = false;
                    gameWidgets[UIGameWidgets.SETTINGS].Visible = false;
                    gameWidgets[UIGameWidgets.SETTINGS].Interactable = false;

                    // Show second layer place tower buttons
                    gameWidgets[UIGameWidgets.TOWER_1].Visible = true;
                    gameWidgets[UIGameWidgets.TOWER_1].Interactable = true;
                    gameWidgets[UIGameWidgets.TOWER_2].Visible = true;
                    gameWidgets[UIGameWidgets.TOWER_2].Interactable = true;
                    gameWidgets[UIGameWidgets.TOWER_3].Visible = true;
                    gameWidgets[UIGameWidgets.TOWER_3].Interactable = true;
                    // Show and instantiate close button for closing the tower select bar
                    gameWidgets[UIGameWidgets.CLOSE_BARS].BoundingBox = new Rectangle(gameWidgets[UIGameWidgets.TOOL_BAR].BoundingBox.Location,
                        gameWidgets[UIGameWidgets.CLOSE_BARS].BoundingBox.Size);
                    gameWidgets[UIGameWidgets.CLOSE_BARS].Visible = true;
                    gameWidgets[UIGameWidgets.CLOSE_BARS].Interactable = true;
                    break;
                case UIButtonFunctions.Pause:
                    break;
                case UIButtonFunctions.CloseBars:
                    // Show first layer buttons
                    gameWidgets[UIGameWidgets.VIEW_TOWERS].Visible = true;
                    gameWidgets[UIGameWidgets.VIEW_TOWERS].Interactable = true;
                    gameWidgets[UIGameWidgets.PAUSE].Visible = true;
                    gameWidgets[UIGameWidgets.PAUSE].Interactable = true;
                    gameWidgets[UIGameWidgets.SETTINGS].Visible = true;
                    gameWidgets[UIGameWidgets.SETTINGS].Interactable = true;

                    // Hide tower placement buttons
                    gameWidgets[UIGameWidgets.TOWER_1].Visible = false;
                    gameWidgets[UIGameWidgets.TOWER_1].Interactable = false;
                    gameWidgets[UIGameWidgets.TOWER_2].Visible = false;
                    gameWidgets[UIGameWidgets.TOWER_2].Interactable = false;
                    gameWidgets[UIGameWidgets.TOWER_3].Visible = false;
                    gameWidgets[UIGameWidgets.TOWER_3].Interactable = false;
                    gameWidgets[UIGameWidgets.CLOSE_BARS].Visible = false;
                    gameWidgets[UIGameWidgets.CLOSE_BARS].Interactable = false;
                    break;
                case UIButtonFunctions.Settings:
                    break;
                case UIButtonFunctions.Tower1:
                    // Create new instance of placement using the current bounding box of the tower button and the size of a background tile
                    placeTower = new Placement(gameWidgets[UIGameWidgets.TOWER_1].BoundingBox, background.tileSize);
                    // Set the current tower being placed as the function of the tower button just clicked, allows UI to keep track of the current tower being placed
                    currentTowerPlacement = (gameWidgets[UIGameWidgets.TOWER_1] as Button).ButtonFunction;
                    break;
                case UIButtonFunctions.Tower2:
                    // Same as tower 1, but for tower 2

                    placeTower = new Placement(gameWidgets[UIGameWidgets.TOWER_2].BoundingBox, background.tileSize);
                    currentTowerPlacement = (gameWidgets[UIGameWidgets.TOWER_2] as Button).ButtonFunction;
                    break;
                case UIButtonFunctions.Tower3:
                    // Same as tower 1, but for tower 3

                    placeTower = new Placement(gameWidgets[UIGameWidgets.TOWER_3].BoundingBox, background.tileSize);
                    currentTowerPlacement = (gameWidgets[UIGameWidgets.TOWER_3] as Button).ButtonFunction;
                    break;
                case UIButtonFunctions.Restart:
                    // Call the restart method from the Main class to restart the game
                    Main.Restart();
                    break;
                default:
                    break;
            }
        }

        // Assists in restarting the game by reinstantiating the user and game menu objects, no return or parameters
        public void RestartGame()
        {
            user = new User();
            CurrentState = UIState.Game;
            GameMenus();
        }

        // Timer update method, runs every program cycle. Takes the current enemy list's array reference as its parameter, no return value.
        public void Update(ref Enemy[] targets)
        {
            // If the instance of Placement has been instantiated, update the current defence towers
            if (placeTower != null)
                placeTower.Update(ref targets);

            // Update the user stats being displayed
            UpdateUserStats();
        }

        // Updates the user stats. Prevents towers from being interactable if the user lacks the money, and detects whether the user health drops to 0.
        public void UpdateUserStats()
        {
            // Set the interactability of the tower buttons to the boolean value of whether or not the user can afford the corresponding tower cost
            gameWidgets[UIGameWidgets.TOWER_1].Interactable = user.Buyable(DefenseTower.NORMAL_COST) ? true : false;
            gameWidgets[UIGameWidgets.TOWER_2].Interactable = user.Buyable(DefenseTower.LONG_COST) ? true : false;
            gameWidgets[UIGameWidgets.TOWER_3].Interactable = user.Buyable(DefenseTower.MISSILE_COST) ? true : false;

            // Update the text of user stat widgets
            gameWidgets[UIGameWidgets.USER_MONEY].Text = "$" + user.Money.ToString();
            gameWidgets[UIGameWidgets.USER_LIVES].Text = new StringBuilder().Insert(0, "\u2665", user.Lives).ToString();
            gameWidgets[UIGameWidgets.USER_WAVE].Text = "Wave  " + (user.WavesSurvived + 1).ToString();

            // If the user is no longer alive
            if (user.Alive() == false)
            {
                // Set the current state of the game as game over
                CurrentState = UIState.GameOver;
                // Instantiate game over widgets
                GameOver();
            }
        }

        // Paints UI widgets, background, and placement towers on refresh. Takes the PaintEventArgs instance parameter of the Main class OnPaint method as its parameter, no return value.
        public void Paint(PaintEventArgs e)
        {
            // If the UI is currently in the start state
            if (CurrentState == UIState.Start)
            {
                // Loop through and paint all widgets in the startWidget array
                for (int currentWidget = 0; currentWidget < UIStartWidgets.NUM_OF_START_WIDGETS; currentWidget++)
                {
                    startWidgets[currentWidget].Paint(e);
                }
            }
            // If the UI is currently in the game state
            else if (CurrentState == UIState.Game)
            {
                // Draw the game background
                background.Paint(e);
                // Loop through and paint all widgets in the gameWidget array
                for (int currentWidget = 0; currentWidget < UIGameWidgets.NUM_OF_GAME_WIDGETS; currentWidget++)
                {
                    gameWidgets[currentWidget].Paint(e);
                }
                // Switch statement that determines which tower image to draw while the tower is being placed
                switch (currentTowerPlacement)
                {
                    case UIButtonFunctions.Tower1:
                        // Draw tower 1
                        placeTower.Paint(e, Properties.Resources.NormalTower, Properties.Resources.NormalProjectile);
                        break;
                    case UIButtonFunctions.Tower2:
                        // Draw tower 2
                        placeTower.Paint(e, Properties.Resources.LongRangeTower, Properties.Resources.NormalProjectile);
                        break;
                    case UIButtonFunctions.Tower3:
                        // Draw tower 3
                        placeTower.Paint(e, Properties.Resources.MissileTowerWithProjectile, Properties.Resources.MissileProjectile);
                        break;
                    default:
                        break;
                }
            }
            // If the UI is currently in the game over state
            else
            {
                // Loop through and paint all widgets in the gameOverWidget array
                for (int currentWidget = 0; currentWidget < UIGameOverWidgets.NUM_OF_GAME_OVER_WIDGETS; currentWidget++)
                {
                    gameOverWidgets[currentWidget].Paint(e);
                }
            }
        }

        // Mouse move method that triggers when the mouse is moved, takes the MouseEventArgs instance from the MainClass mouse move event and the form that called it (used for forcibly refreshing the form)
        public void OnMove(MouseEventArgs e, Form sender)
        {
            // If the UI is currently in the start state
            if (CurrentState == UIState.Start)
            {
                // Loop through all startWidgets checking whether or not the mouse is hovering on the widget
                for (int currentWidget = 0; currentWidget < UIStartWidgets.NUM_OF_START_WIDGETS; currentWidget++)
                {
                    // If the widget's on hover method returns true (meaning the hover property has changed), refresh the form to reflect the change in appearance
                    if (startWidgets[currentWidget].OnHover(e) == true)
                    {
                        sender.Refresh();
                        // Break out of the for loop (no need to keep searching)
                        break;
                    }
                }
            }

            // If the UI is currently in the game state
            else if (CurrentState == UIState.Game)
            {
                // Loop through all gameWidgets checking whether or not the mouse is hovering on the widget 
                for (int currentWidget = 0; currentWidget < UIGameWidgets.NUM_OF_GAME_WIDGETS; currentWidget++)
                {
                    gameWidgets[currentWidget].OnHover(e);
                }
                // If the instance of Placement has been instantiated, call its MouseMove method using this method's MouseEventArgs instance
                if (placeTower != null)
                    placeTower.MouseMoveSettings(e);
            }
        }

        // Mouse click method that triggers when the mouse is clicked, takes the MouseEventArgs instance from the MainClass mouse click event
        public void OnClick(MouseEventArgs e)
        {
            // If the UI is currently in the start state
            if (CurrentState == UIState.Start)
            {
                // Loop through all startWidgets checking whether or not the mouse clicked the widget
                for (int i = 0; i < UIStartWidgets.NUM_OF_START_WIDGETS; i++)
                {
                    startWidgets[i].OnClick(e);
                    // If the widget was clicked
                    if (startWidgets[i].Clicked == true)
                    {
                        // Pass the button's function to the ButtonFunctions method to perform its function
                        ButtonFunctions((startWidgets[i] as Button).ButtonFunction);
                        // Break out of the for loop (no need to keep searching)
                        break;
                    }
                }
            }
            // If the UI is currently in the game state
            else if (CurrentState == UIState.Game)
            {
                // If the instance of Placement has been instantiated, call its MouseClick method using this method's MouseEventArgs instance (for moving towers)
                if (placeTower != null)
                    placeTower.MouseClickSettings(e);

                // Loop through all gameWidgets checking whether or not the mouse clicked the widget
                for (int i = 0; i < UIGameWidgets.NUM_OF_GAME_WIDGETS; i++)
                {
                    gameWidgets[i].OnClick(e);
                    // If the widget was clicked
                    if (gameWidgets[i].Clicked == true)
                    {
                        // If widget that was clicked was a tower placement button
                        if ((gameWidgets[i] as Button).ButtonFunction == UIButtonFunctions.Tower1 || (gameWidgets[i] as Button).ButtonFunction == UIButtonFunctions.Tower2
                            || (gameWidgets[i] as Button).ButtonFunction == UIButtonFunctions.Tower3)
                        {
                            // Pass the button's function to the ButtonFunction method to perform its function
                            ButtonFunctions((gameWidgets[i] as Button).ButtonFunction);
                            // Call placement's MouseClick method using this method's MouseEventArgs instance (for creating new towers)
                            placeTower.MouseClickSettings(e, gameWidgets[i]);
                        }
                        // For other buttons
                        else
                            // Pass the button's function to the ButtonFunction method to perform its function
                            ButtonFunctions((gameWidgets[i] as Button).ButtonFunction);
                        // Break out of the for loop (no need to keep searching)
                        break;
                    }
                }
            }
            // If the UI is currently in the game over state
            else
            {
                // Loop through all gameOverWidgets checking whether or not the mouse clicked the widget
                for (int i = 0; i < UIGameOverWidgets.NUM_OF_GAME_OVER_WIDGETS; i++)
                {
                    gameOverWidgets[i].OnClick(e);
                    // If the widget was clicked
                    if (gameOverWidgets[i].Clicked == true)
                    {
                        // Pass the button's function to the ButtonFunction method to perform its function
                        ButtonFunctions((gameOverWidgets[i] as Button).ButtonFunction);
                        // Break out of the for loop (no need to keep searching)
                        break;
                    }
                }
            }
        }
    }
}
