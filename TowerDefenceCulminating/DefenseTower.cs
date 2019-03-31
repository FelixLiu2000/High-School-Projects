/*
 * Bosen Qu
 * January 24, 2017
 * The following enumeration and class creates different types of enemy
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Windows;
using System.Resources;

namespace BosenQuFelixLiuFinalCulminatingProject
{
    /*
     * This enumeration creates different types of tower
     */
    enum DefenseTowerType
    {
        Normal, LongRange, Missile
    }
    /// <summary>
    /// This class provides different information to the each defense tower
    /// </summary>
    class DefenseTower
    {
        //set the cost for each defense tower
        public const int NORMAL_COST = 50;
        public const int LONG_COST = 200;
        public const int MISSILE_COST = 400;
        //set the type for the defense tower
        DefenseTowerType type;
        //set the bouding box for the range of the defense tower
        public RectangleF rangeBB;
        //set the length of the range of the defense tower
        public float range;
        //set the cost of the defense tower
        public int cost;
        //set the projectile for the defense tower
        public Projectile projectile;
        //set the bouding box for the defen tower
        public RectangleF boundingBox;
        //set the original image of the defense tower
        Bitmap defenseTowerImage;
        //set the actual image of the defense tower
        Bitmap paintDefenseTowerImage;
        //set the variable that checks if the tower is placed or not
        public bool towerPlaced;
        //set the variable that chekcs if the user is clicking the tower or not
        bool towerClicked = true;
        //set the angle of the tower that is currently rotating
        float currentAngle = 0;
        //store the range for each defense tower
        const int NORMAL_TOWER_RANGE = 150, LONG_RANGE_TOWER_RANGE = 300, MISSILE_TOWER_RANGE = 200;
        /*
         * This constructer initializes the defense tower. It takes the bouding box of the defense tower
         * that user wants to place and the type of the defense tower as its argument.
         */
        public DefenseTower(RectangleF boundingBoxInput, DefenseTowerType defenseTowerTypeInput)
        {        
            //store the type of the defense tower
            type = defenseTowerTypeInput;
            //initialize the projectile depending on the type of the projectile
            switch(type)
            {
                case DefenseTowerType.Normal:
                    range = NORMAL_TOWER_RANGE;
                    cost = NORMAL_COST;
                    defenseTowerImage = Properties.Resources.NormalTower;
                    break;
                case DefenseTowerType.LongRange:
                    range = LONG_RANGE_TOWER_RANGE;
                    cost = LONG_COST;
                    defenseTowerImage = Properties.Resources.LongRangeTower;
                    break;
                case DefenseTowerType.Missile:
                    range = MISSILE_TOWER_RANGE;
                    cost = MISSILE_COST;
                    defenseTowerImage = Properties.Resources.MissileTowerWithProjectile;
                    break;
                default:
                    break;
                    
            }
            //set the bouding box for the defense tower
            boundingBox = boundingBoxInput;
            //set the actual image of the defense tower
            paintDefenseTowerImage = defenseTowerImage;
        }
        /*
         * This function sets the projectile for the defense tower
         */
        public void SetProjectile()
        {
            //initialize the projectile
            projectile = new Projectile(boundingBox, range, type);
        }
        /*
         * This function rotates the defense tower based on enemy's location. It takes 
         * the rotate angle as its argument
         */
        public void Rotate(float angle)
        {
            //rotate the image of the defense tower and copy to the actual image that goes to on paint
            paintDefenseTowerImage = Graphic.RotateImage(defenseTowerImage, angle);
            //update the current angle
            currentAngle = angle;          
        }
        /*
         * This function paints the defense tower. It takes the paint event argument from main as its
         * argument.
         */
        public void Paint(PaintEventArgs e)
        {
            //draw the range circle
            if (towerPlaced == false || towerClicked == true)
            {
                Pen myPen = new Pen(Color.Black);
                e.Graphics.DrawEllipse(myPen, rangeBB);
            }
            //paint the defense tower
            if (towerPlaced== true &&type == DefenseTowerType.Missile && projectile.noMissile == true)
                e.Graphics.DrawImage(Properties.Resources.MissileTowerWithoutProjectile, boundingBox);
            else
                e.Graphics.DrawImage(paintDefenseTowerImage, boundingBox);
            //paint the projectile of the defense tower
            if(towerPlaced == true)
                projectile.Paint(e);
        }
        /*
         * This function shows the range of the defense tower if the user clicks on the defense tower
         */
        public void MouseClickSettings(MouseEventArgs e)
        {
            //if the user clicks on the defense tower, show the tower
            if (boundingBox.Contains(e.Location))
            {
                if (towerClicked == true)
                    towerClicked = false;
                else towerClicked = true;
            }
            //if not, do not show the tower
            else towerClicked = false;
        }
    }
    /// <summary>
    /// This class creates a list of defense tower that is placed on the map. If a defense tower is placed, it gets
    /// added to the list. If the defense tower is sold, it gets removed from the list
    /// </summary>
    class DefenseTowerList
    {
        //create a list of defense tower
        public static DefenseTower[] defenseTowers = new DefenseTower[0];
        //store the index of the head of the array
        const int HEAD = 0;
        /*
         * This function adds one projectile to the list. It takes the new projectile that user
         * wants to add as its argument
         */
        public void AddDefenseTower(DefenseTower toPlace)
        {
            //resize the list to one more of its length      
            ResizeDefenseTower(defenseTowers.Length + 1);
            //initialize the lastest elemnt
            defenseTowers[defenseTowers.Length - 1] = toPlace;
            //update the money the user has 
            GUI.UI.user.MoneyLoss(toPlace.cost);
        }
        /*
         * This fcuntion removes a certain projectile from the list. It takes the index of the 
         * element that needs to be removed as its argument.
         */
        public void RemoveDefenseTower(int index)
        {
            //shift each element one place left
            for (int i = index, n = defenseTowers.Length - 1; i < n; i++)
                defenseTowers[i] = defenseTowers[i + 1];
            //resize the list to one less of its length
            ResizeDefenseTower(defenseTowers.Length - 1);
        }
        /*
         * This function resizeds the array to a certain size. It takes the new size of the array
         * as its argument.
         */
        void ResizeDefenseTower(int newSize)
        {
            //create a copy for the current list
            DefenseTower[] copy = new DefenseTower[newSize];
            if (newSize > defenseTowers.Length)
                for (int i = HEAD, n = defenseTowers.Length; i < n; i++)
                    copy[i] = defenseTowers[i];
            else
                for (int i = HEAD; i < newSize; i++)
                    copy[i] = defenseTowers[i];
            //resize the original array
            defenseTowers = new DefenseTower[newSize];
            //transfer what's on the copy to the original array
            for (int i = HEAD; i < newSize; i++)
                defenseTowers[i] = copy[i];
        }
        /*
         * This function updates the projectile of the defense tower.
         */
        public void Update(ref Enemy[] targets)
        {
            //cheak each elements of the array
            for(int i = HEAD, n = defenseTowers.Length; i < n; i++)
            {
                //shoot the projectile and store the rotate angle
                float rotateAngle = defenseTowers[i].projectile.Shoot(ref targets);
                //rotate the projectile
                defenseTowers[i].Rotate(rotateAngle);
            }
        }
        /*
         * This function runs when the user clicks on the screen
         */
        public void MouseClickSettings(MouseEventArgs e)
        {
            //check each elements of the array and call the mouse click settings in each defense tower
            for (int i = HEAD, n = defenseTowers.Length; i < n; i++)
                defenseTowers[i].MouseClickSettings(e);
        }
        /*
         * This function checks if the new defense the user is putting intersects with the current defense
         * towers. It takes the location of the defense tower the user is putting. If it intersects with the
         * defense tower that is in the list, return false, else return ture.
         */
        public bool CheckPlacingItems(PointF targetLocation)
        {
            //check if the target location intersects with each projectile in the list
            for (int i = HEAD, n = defenseTowers.Length; i < n; i++)
                //here, there must be a defense tower on that point
                if (defenseTowers[i].boundingBox.Contains(targetLocation))
                    return false;
            //if intersects with no defense tower, return true
            return true;
        }
        /*
         * This function paints each defense tower in the list. It takes the paint event argument of main as
         * its argument.
         */
        public void Paint(PaintEventArgs e)
        {
            //paint each defense tower
            for (int i = HEAD, n = defenseTowers.Length; i < n; i++)
                defenseTowers[i].Paint(e);
        }
    }
}
