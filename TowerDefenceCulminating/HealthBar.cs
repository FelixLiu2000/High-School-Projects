/*
 * Felix Liu
 * January 12, 2017
 * 
 * Responsible for generating and managing enemy health bars, provides methods to do so.
 * 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace BosenQuFelixLiuFinalCulminatingProject
{
    // Class for providing enemies with healthbars, with methods for refreshing/showing healthbars upon health refresh.
    class HealthBar
    {
        // Stores the current size, location and bounding box of the health bar (when at full health)
        SizeF fullBarSize;
        PointF fullBarLocation;
        RectangleF fullBarBB;
        // Bounding box for the health bar representing the enemy's current health
        RectangleF currentBarBB;
        // Integer to store the enemy's max health (upon creation of the object)
        int enemyMaxHealth;
        // Constant for height between top of enemy and bottom of health bar
        const int ENEMY_BAR_OFFSET = 5;

        // Constructor for HealthBar class, parameter is the the enemy's rectangle bounding box and its integer current health
        public HealthBar(RectangleF enemyBB, int currentHealth)
        {
            // Set the size of the full health bar as the width of the enemy bb and the height of one tenth of the enemy's bb height
            fullBarSize = new SizeF(enemyBB.Width, enemyBB.Height * 0.1f);
            // Centre the health bar directly over the enemy by setting its x location to the middle of the enemy minus half of the bar's width, set the health bar y location as the height of the bar above the enemy's y
            fullBarLocation = new PointF((enemyBB.X + enemyBB.Width / 2) - (fullBarSize.Width / 2), enemyBB.Y - fullBarSize.Height - ENEMY_BAR_OFFSET);
            // Refresh full health bounding box with new location and size
            fullBarBB = new RectangleF(fullBarLocation, fullBarSize);
            // Refresh current health bounding box with new location and size
            currentBarBB = new RectangleF(fullBarLocation.X, fullBarLocation.Y, fullBarSize.Width, fullBarSize.Height);
            // Set the enemy's max health as the current health of the enemy
            enemyMaxHealth = currentHealth;
        }

        // Public method that displays/refreshes the health bar for the enemy, takes Enemy parameter of the enemy bounding box it corresponds to and its integer health
        public void Refresh(RectangleF enemyBB, int currentHealth)
        {
            // If the game is running
            if (Main.gameRunning == true)
            {
                // Centre the health bar directly over the enemy by setting its x location to the middle of the enemy minus half of the bar's width, set the health bar y location as the height of the bar above the enemy's y
                fullBarLocation = new PointF((enemyBB.X + enemyBB.Width / 2) - (fullBarSize.Width / 2), enemyBB.Y - fullBarSize.Height - ENEMY_BAR_OFFSET);
                // Refresh the full health bounding box 
                fullBarBB = new RectangleF(fullBarLocation, fullBarSize);

                // Refresh the current health bar bounding box with the location and height of the full bar and the width of the health percentage.
                currentBarBB = new RectangleF(fullBarLocation.X, fullBarLocation.Y, fullBarSize.Width * ((float)currentHealth / (float)enemyMaxHealth), fullBarSize.Height);
            }
        }

        // Public method with no return value that uses the OnPaint function to display the health bar. The parameters are the OnPaint PaintEventArgs parameter from the main class,
        // the color of the full health bar and the color of the current health bar
        public void Paint(PaintEventArgs e, Brush fullHealthColour, Brush currentHealthColour)
        {
            // Draw rectangles with their bounding boxes and colors
            e.Graphics.FillRectangle(fullHealthColour, fullBarBB);
            e.Graphics.FillRectangle(currentHealthColour, currentBarBB);
        }
    }
}
