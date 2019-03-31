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
    /// This class allows user to place the different defense towers by creating a defense tower list
    /// </summary>
    class Placement
    {
        //create a list of defense towers
        DefenseTowerList defenseTowerList = new DefenseTowerList();
        //create the initial rectangle when the user clicks
        RectangleF initialRectangle;
        SizeF tileSize;
        DefenseTower tempTower;
        public bool movingDefenseTower = false;
        bool showCannotPlaceItem = false;

        public Placement(RectangleF initialRectangleInput, SizeF tileSizeInput)
        {
            initialRectangle = initialRectangleInput;
            tileSize = tileSizeInput;
        }

        public void MouseClickSettings(MouseEventArgs e, Widget button)
        {
            if (defenseTowerList.CheckPlacingItems(e.Location) == true)
            {
                MouseClickNew(e, button);
            }
            defenseTowerList.MouseClickSettings(e);
        }

        public void MouseClickSettings(MouseEventArgs e)
        {
            if (defenseTowerList.CheckPlacingItems(e.Location) == true)
            {
                MouseClickMoving(e);
            }
            if (movingDefenseTower == false)
                defenseTowerList.MouseClickSettings(e);
        }

        public void MouseClickNew(MouseEventArgs e, Widget button)
        {
            if (movingDefenseTower == false)
            {
                movingDefenseTower = true;
                if ((button as BosenQuFelixLiuFinalCulminatingProject.GUI.Button).ButtonFunction == UIButtonFunctions.Tower1)
                    tempTower = new DefenseTower(new RectangleF(e.Location, tileSize), DefenseTowerType.Normal);
                else if ((button as BosenQuFelixLiuFinalCulminatingProject.GUI.Button).ButtonFunction == UIButtonFunctions.Tower2)
                    tempTower = new DefenseTower(new RectangleF(e.Location, tileSize), DefenseTowerType.LongRange);
                else if ((button as BosenQuFelixLiuFinalCulminatingProject.GUI.Button).ButtonFunction == UIButtonFunctions.Tower3)
                    tempTower = new DefenseTower(new RectangleF(e.Location, tileSize), DefenseTowerType.Missile);
                Background.placingDefenseTower = true;
            }
        }

        public void MouseClickMoving(MouseEventArgs e)
        {
            if (movingDefenseTower == true)
            {
                if (defenseTowerList.CheckPlacingItems(initialRectangle.Location) == false)
                    return;
                if (UI.background.CheckPlacingItem(e.Location) == false)
                    return;
                // Check for user panel here
                Background.placingDefenseTower = false;
                tempTower.SetProjectile();
                tempTower.towerPlaced = true;
                defenseTowerList.AddDefenseTower(tempTower);
                tempTower = null;
                movingDefenseTower = false;
            }
        }
        public void MouseMoveSettings(MouseEventArgs e)
        {
            if (movingDefenseTower == true)
            {    
                tempTower.boundingBox.Location = CalculateCoordinate(e.X, e.Y);
                if (defenseTowerList.CheckPlacingItems(e.Location) == false || UI.background.CheckPlacingItem(e.Location) == false)
                    showCannotPlaceItem = true;
                else showCannotPlaceItem = false;
                tempTower.rangeBB = Projectile.CalculateRangeBoundingBox(tempTower.boundingBox, tempTower.range);
            }
        }

        PointF CalculateCoordinate(float x, float y)
        {
            float newX = (int)(x / tileSize.Width);
            float newY = (int)(y / tileSize.Height);
            return new PointF(newX * tileSize.Width, newY * tileSize.Height);
        }
        

        public void Update(ref Enemy[] targets)
        {
            defenseTowerList.Update(ref targets);
        }

        public void Paint(PaintEventArgs e, Image defenseTowerImage, Image projectileImage)
        {
            if (movingDefenseTower == true)
            {
                tempTower.Paint(e,projectileImage);
                if (showCannotPlaceItem == true)
                {
                    SolidBrush trnsRedBrush = new SolidBrush(Color.FromArgb(0x60FF0000));
                    e.Graphics.FillRectangle(trnsRedBrush, tempTower.boundingBox);
                }
            }       
            defenseTowerList.Paint(e, defenseTowerImage, projectileImage);
        }
    }
}
