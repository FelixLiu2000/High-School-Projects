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
    /// This class provides different information to the 
    /// </summary>
    class DefenseTower
    {
        public const int NORMAL_COST = 50;
        public const int LONG_COST = 300;
        public const int MISSILE_COST = 500;
        DefenseTowerType type;
        public RectangleF rangeBB;
        public float range;
        public int cost;
        public Projectile projectile;
        public RectangleF boundingBox;
        public Bitmap defenseTowerImage;
        public Bitmap paintDefenseTowerImage;
        
        public bool towerPlaced;
        public bool towerClicked = true;
        public float currentAngle = 0;
        

        public DefenseTower(RectangleF boundingBoxInput, DefenseTowerType defenseTowerTypeInput)
        {
            type = defenseTowerTypeInput;
            if (type == DefenseTowerType.Normal)
            {
                range = 150;
                cost = NORMAL_COST;
                defenseTowerImage = Properties.Resources.NormalTower;
            }
            else if (type == DefenseTowerType.LongRange)
            {
                range = 300;
                cost = LONG_COST;
                defenseTowerImage = Properties.Resources.LongRangeTower;
            }
            else if (type == DefenseTowerType.Missile)
            {
                range = 200;
                cost = MISSILE_COST;
                defenseTowerImage = Properties.Resources.MissileTowerWithProjectile;
            }
            boundingBox = boundingBoxInput;
            paintDefenseTowerImage = defenseTowerImage;
        }
        public void SetProjectile()
        {
            projectile = new Projectile(boundingBox, range, type);
        }

        public void Rotate(float angle)
        {
            paintDefenseTowerImage = null;
            paintDefenseTowerImage = Graphic.RotateImage(defenseTowerImage, angle);
            currentAngle = angle;
            
        }
        public void Paint(PaintEventArgs e, Image projectileImage)
        {
            //draw the range circle
            if (towerPlaced == false || towerClicked == true)
            {
                Pen myPen = new Pen(Color.Black);
                e.Graphics.DrawEllipse(myPen, rangeBB);
            }
            if (towerPlaced== true &&type == DefenseTowerType.Missile && projectile.noMissile == true)
                e.Graphics.DrawImage(Properties.Resources.MissileTowerWithoutProjectile, boundingBox);
            else
                e.Graphics.DrawImage(paintDefenseTowerImage, boundingBox);
            
            if(towerPlaced == true)
                projectile.Paint(e, projectileImage);
            //e.Graphics.DrawImage(Properties.Resources.black, newInitialLocation);
        }

        public void MouseClickSettings(MouseEventArgs e)
        {
            if (boundingBox.Contains(e.Location))
            {
                if (towerClicked == true)
                    towerClicked = false;
                else towerClicked = true;
            }
            else towerClicked = false;
        }
    }

    class DefenseTowerList
    {
        public static DefenseTower[] defenseTowers = new DefenseTower[0];
        
        public void AddDefenseTower(DefenseTower toPlace)
        {
            ResizeDefenseTower(defenseTowers.Length + 1);         
            defenseTowers[defenseTowers.Length - 1] = toPlace;
            GUI.UI.user.MoneyLoss(toPlace.cost);
        }
        public void RemoveDefenseTower(int index)
        {
            for (int i = index, n = defenseTowers.Length - 1; i < n; i++)
                defenseTowers[i] = defenseTowers[i + 1];
            //resize the list to one less of its length
            ResizeDefenseTower(defenseTowers.Length - 1);
        }
        void ResizeDefenseTower(int newSize)
        {
            DefenseTower[] copy = new DefenseTower[newSize];
            if (newSize > defenseTowers.Length)
                for (int i = 0, n = defenseTowers.Length; i < n; i++)
                    copy[i] = defenseTowers[i];
            else
                for (int i = 0; i < newSize; i++)
                    copy[i] = defenseTowers[i];
            defenseTowers = new DefenseTower[newSize];
            for (int i = 0; i < newSize; i++)
                defenseTowers[i] = copy[i];
        }
        public void Update(ref Enemy[] targets)
        {
            foreach(DefenseTower def in defenseTowers)
            {
                float rotateAngle = def.projectile.Shoot(ref targets);
                def.Rotate(rotateAngle);
            }
        }
        public void MouseClickSettings(MouseEventArgs e)
        {
            foreach (DefenseTower def in defenseTowers)
                def.MouseClickSettings(e);
        }
        public bool CheckPlacingItems(PointF targetLocation)
        {
            foreach (DefenseTower def in defenseTowers)
                if (def.boundingBox.Contains(targetLocation))
                    return false;
            return true;
        }
        public void Paint(PaintEventArgs e, Image defenseTowerImage, Image projectileImage)
        {
            foreach (DefenseTower def in defenseTowers)
                def.Paint(e, projectileImage);
        }
    }
}
