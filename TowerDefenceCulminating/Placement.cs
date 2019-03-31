/*
 * Bosen Qu, Felix Liu
 * January 23, 2017
 * This class allows user to place defense towers on the map
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using BosenQuFelixLiuFinalCulminatingProject.GUI;
namespace BosenQuFelixLiuFinalCulminatingProject
{
    /// <summary>
    /// This class allows user to place the different defense towers on the map
    /// </summary>
    class Placement
    {
        //declear a list of defense towers
        DefenseTowerList defenseTowerList = new DefenseTowerList();
        //declear tha rectangle that stores in the initial rectangle when the user clicks
        RectangleF initialRectangle;
        //declear the variable that stores in the size of the tile
        SizeF tileSize;
        //declear the a temp tower for when the user is moving the tower
        DefenseTower tempTower;
        //declear the boolean that shows if the user is moving the tower or not
        public bool movingDefenseTower = false;
        //declear the boolearn that shows if the user can place item at a certain location or not
        bool showCannotPlaceItem = false;
        /*
         * This constructor initializes the placement. It takes the rectangle of the button and the size of
         * each tile on the map
         */
        public Placement(RectangleF initialRectangleInput, SizeF tileSizeInput)
        {
            //assign the button's rectangle
            initialRectangle = initialRectangleInput;
            //assign the size of the background tile
            tileSize = tileSizeInput;
        }
        /*
         * This function allows the program to respond after the user clicks on the screen. It takes the
         * mouse event argument from the main, and the information of the button as its argument. Calls
         * mouse clicked new to place new tower when the tower is not being moved.
         */
        public void MouseClickSettings(MouseEventArgs e, Widget button)
        {
            //when the user clicks on the screen
            if (defenseTowerList.CheckPlacingItems(e.Location) == true)
                //response depend on the type of defense tower user wants to use
                MouseClickNew(e, button);
            //check if we should move the tower or not
            defenseTowerList.MouseClickSettings(e);
        }
        /*
         * This function allows the user to move defense tower after clicking the button. It takes the
         * mouse event argument from the main as its argument. Calls mouse click moving to perform activities
         * while the tower is currently being moved.
         */
        public void MouseClickSettings(MouseEventArgs e)
        {
            // If the current location does not have a defense tower
            if (defenseTowerList.CheckPlacingItems(e.Location))
                // Call the mouse click for moving towers
                MouseClickMoving(e);
            // If the current location does have a defense tower
            if (movingDefenseTower == false)
                // Call the defense tower's mouse click method
                defenseTowerList.MouseClickSettings(e);
        }
        /*
         * This function allows user to click for a new defense tower
         */
        public void MouseClickNew(MouseEventArgs e, Widget button)
        {
            //if the defense tower is not moving
            if (movingDefenseTower == false)
            {
                //let the defense tower to move
                movingDefenseTower = true;
                //initialize the temperory tower depending on the type user clicks
                if ((button as BosenQuFelixLiuFinalCulminatingProject.GUI.Button).ButtonFunction == UIButtonFunctions.Tower1)
                    tempTower = new DefenseTower(new RectangleF(e.Location, tileSize), DefenseTowerType.Normal);
                else if ((button as BosenQuFelixLiuFinalCulminatingProject.GUI.Button).ButtonFunction == UIButtonFunctions.Tower2)
                    tempTower = new DefenseTower(new RectangleF(e.Location, tileSize), DefenseTowerType.LongRange);
                else if ((button as BosenQuFelixLiuFinalCulminatingProject.GUI.Button).ButtonFunction == UIButtonFunctions.Tower3)
                    tempTower = new DefenseTower(new RectangleF(e.Location, tileSize), DefenseTowerType.Missile);
                //tell the background class the user is placing tower
                Background.placingDefenseTower = true;
            }
        }
        /*
         * This function does the responds when the user clicks on the from after moving the mouse. It takes 
         * the mouse evnet argument from main as its argument.
         */
        public void MouseClickMoving(MouseEventArgs e)
        {
            //here, the user must been moving the defense tower
            if (movingDefenseTower == true)
            {
                //if the user clicks on the place that cannot place items, terminte the function
                if (defenseTowerList.CheckPlacingItems(initialRectangle.Location) == false)
                    return;
                if (UI.background.CheckPlacingItem(e.Location) == false)
                    return;
                //if the user places item on an approprate position, set the tower, so the tower won't be 
                //moving anymore
                Background.placingDefenseTower = false;
                //set the projectile information of the temporary tower
                tempTower.SetProjectile();
                //set the temporary tower to placed
                tempTower.towerPlaced = true;
                //copy the temporary tower to an element of the defense tower list
                defenseTowerList.AddDefenseTower(tempTower);
                //clear the temporary tower
                tempTower = null;
                //now, the user must not be moving the defense tower
                movingDefenseTower = false;
            }
        }
        /*
         * This function does responses when the user is moving the mouse. It takes the mouse event argument
         * from main as its argument.
         */
        public void MouseMoveSettings(MouseEventArgs e)
        {
            //if the user is moving the defense tower
            if (movingDefenseTower == true)
            {    
                //update the location of the defense tower
                tempTower.boundingBox.Location = CalculateLocationOfMovingTower(e.X, e.Y);
                //if it is inapproprate to place items, tell the user not to place items there
                if (defenseTowerList.CheckPlacingItems(e.Location) == false || UI.background.CheckPlacingItem(e.Location) == false)
                    showCannotPlaceItem = true;
                //else, don't show the message
                else showCannotPlaceItem = false;
                //update the range bounding box of the defense tower
                tempTower.rangeBB = Projectile.CalculateRangeBoundingBox(tempTower.boundingBox, tempTower.range);
            }
        }
        /*
         * This function calculates the location of the moving tower. It takes the cursor's x and y location
         * as its argument. It returns the point where the defense tower should be put
         */
        PointF CalculateLocationOfMovingTower(float x, float y)
        {
            //calculate the new x and y location by rounding off to the nearest divisor of tile's size
            float newX = (int)(x / tileSize.Width);
            float newY = (int)(y / tileSize.Height);
            return new PointF(newX * tileSize.Width, newY * tileSize.Height);
        }
        /*
         * This function updates the defense tower list. It takes an array of targets as its argument
         */
        public void Update(ref Enemy[] targets)
        {
            //update the defense tower list
            defenseTowerList.Update(ref targets);
        }
        /*
         * This function draws the placment. It takes the paint event argument from main as its argument.
         */
        public void Paint(PaintEventArgs e)
        {
            //if user is moving the defense tower, draw the temperory tower that is moving
            if (movingDefenseTower == true)
            {
                tempTower.Paint(e);
                //if the user cannot place items at a certain location, draw a red lay above the moving
                //tower to indicate it cannot be placed here
                if (showCannotPlaceItem == true)
                {
                    SolidBrush trnsRedBrush = new SolidBrush(Color.FromArgb(0x60FF0000));
                    e.Graphics.FillRectangle(trnsRedBrush, tempTower.boundingBox);
                }
            } 
            //draw the defense tower list      
            defenseTowerList.Paint(e);
        }
    }
}
