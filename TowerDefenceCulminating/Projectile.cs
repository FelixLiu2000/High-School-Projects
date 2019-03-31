/*
 * Bosen Qu
 * January 12, 2017
 * The following class and structure are responsible for defense tower shooting projectiles to the enemy 
 * if the enemy is in the range
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;

namespace BosenQuFelixLiuFinalCulminatingProject
{
    /// <summary>
    /// This data structer stores in the information a projectile needs
    /// </summary>
    struct ProjectileInformation
    {
        public float x;
        public float y;
        public float targetInstantX;
        public float targetInstantY;
        public float startingX;
        public float startingY;
        public SizeF targetSize;
        public RectangleF boundingBox;
        public Bitmap image;
        public int targetIndex;
    }
    /// <summary>
    /// This class is responsible for calculating and shooting the projectile from a starting point to the target
    /// </summary>
    class Projectile
    {
        //declear the variables for the range of the defense tower
        float rangeRadius;
        RectangleF rangeBB;
        //declear the variables for the initial location of the projectile
        float initialXLocation, initialYLocation;
        //make a list of projectiles
        ProjectileInformation[] projectiles = new ProjectileInformation[0];
        //declear the size of the projectile
        SizeF projectileSize;
        //declear the speed of the projectile
        float speed;
        //declear the gap time that a defense tower shoots the projectile
        float gapTime;
        //declear the current time
        //int time = 0;
        Stopwatch timeCounter;
        //declear the center point for the defensetower
        PointF centerPoint;
        //create the list that stores the index of the enemy to be shooted
        int[] shootingTargetIndex = new int[0];
        //create the list that stores if the enemy is added to th shooting list or not
        bool[] addedToShootingList;
        //create the variable that stores in if the shooting list is initialized or not
        bool initializeShootingList;
        //store the damage of the projectile
        int projectileDamage;
        //store the angle that the defense tower needs to rotate
        float returnAngle = 0;
        //store the type of the defense tower
        DefenseTowerType defenseTowerType;
        //store the initial image for the each kind of the projectile
        Bitmap normalProjectileImage = Properties.Resources.NormalProjectile, missileProjectileImage = Properties.Resources.MissileProjectile;
        //create the variable that tells on paint to show explosion image or not (for missile tower only)
        bool showExplosion = false;
        //create the graphics for the explosion (for missile tower only)
        RectangleF explosionBB;
        SizeF explosionSize = new SizeF(130, 130);
        //count the time for the explosion
        Stopwatch explosionCounter;
        //store the explosion image
        Bitmap explosionImage = Properties.Resources.Explosion;
        //create the variable that checks if there's missile in the tower or not
        public bool noMissile = false;
        /*
         * This is a constructer that record all the information for the projectile from the user. It takes the bounding box
         * of the defenseTower, the range of the defense tower, the type of the defense tower as its argument.
         */
        public Projectile(RectangleF defenseTowerBB, float rangeInput, DefenseTowerType defenseTowerTypeInput)
        {
            //create the constants for each proprotities of the defense TOWER
            const float NORMAL_PROJECTILE_SPEED = 1000, LONG_RANGE_PROJECTILE_SPEED = 2000, MISSILE_PROJECTILE_SPEED = 200;
            const int NORMAL_PROJECTILE_DAMAGE = 50, LONG_RANGE_PROJECTILE_DAMAGE = 150, MISSILE_PROJECTILE_DAMAGE = 150;
            const float NORMAL_PROJECTILE_GAPTIME = 0.3f, LONG_RANGE_PROJECTILE_GAPTIME = 1.0f, MISSILE_PROJECTILE_GAPTIME = 1.5f;    
            //assign values to all private variables
            centerPoint = Graphic.CalculateCenterPoint(defenseTowerBB);
            rangeRadius = rangeInput;
            defenseTowerType = defenseTowerTypeInput;
            rangeBB = CalculateRangeBoundingBox(defenseTowerBB, rangeInput);
            timeCounter = new Stopwatch();
            //set up the projectile depeding the type of the defenseTower
            switch (defenseTowerType)
            {
                case DefenseTowerType.Normal:
                    speed = NORMAL_PROJECTILE_SPEED;
                    projectileDamage = NORMAL_PROJECTILE_DAMAGE;
                    gapTime = NORMAL_PROJECTILE_GAPTIME;
                    projectileSize = new SizeF(15, 15);
                    //start the projectile from the center of the tower
                    initialXLocation = centerPoint.X - projectileSize.Width / 2;
                    initialYLocation = centerPoint.Y - projectileSize.Height / 2;
                    break;
                case DefenseTowerType.LongRange:
                    speed = LONG_RANGE_PROJECTILE_SPEED;
                    projectileDamage = LONG_RANGE_PROJECTILE_DAMAGE;
                    gapTime = LONG_RANGE_PROJECTILE_GAPTIME;      
                    projectileSize = new SizeF(20, 20);
                    //start the projectile from the center of the tower
                    initialXLocation = centerPoint.X - projectileSize.Width / 2;
                    initialYLocation = centerPoint.Y - projectileSize.Height / 2;
                    break;
                case DefenseTowerType.Missile:
                    speed = MISSILE_PROJECTILE_SPEED;
                    projectileDamage = MISSILE_PROJECTILE_DAMAGE;
                    gapTime = MISSILE_PROJECTILE_GAPTIME;      
                    projectileSize = new SizeF(50, 50);
                    initialXLocation = defenseTowerBB.X + defenseTowerBB.Width / 2;
                    initialYLocation = defenseTowerBB.Y;
                    break;
                default:
                    break;
            }        
        }
        /*
         * This function calculates the bounding box of a given range. It takes the defene tower's bounding box
         * and range as its argument. It returns the bouding box of the range.
         */
        public static RectangleF CalculateRangeBoundingBox(RectangleF defenseTowerBB, float range)
        {
            RectangleF rangeBB = new RectangleF();
            //determine the center point of the projectile
            PointF centerPoint = Graphic.CalculateCenterPoint(defenseTowerBB);
            //draw the circle for the range of the projectile
            rangeBB.Location = new PointF(centerPoint.X - range, centerPoint.Y - range);
            //determine the size of the range circle
            rangeBB.Size = new SizeF(2 * range, 2 * range);
            return rangeBB;
        }
        /*
         * This function allows the defense tower to shoot the projectile. It takes the a series of enemies as
         * its argument. It returns the angle that the defense tower needs to rotate.
         */  
        public float Shoot(ref Enemy[] targets)
        {
            //stores the index of the head of the array
            const int HEAD = 0;
            //if the shooting list is not initialized, initialize the shooting list by providing the capacity for
            //the array
            if(initializeShootingList == false)
            {
                addedToShootingList = new bool[targets.Length];
                initializeShootingList = true;  
            }
            //if the target is inside the range circle, add the target to the shooting list
            for (int i = HEAD, n = targets.Length; i < n; i++)
                if (Graphic.CircleCollisionDetection(targets[i].boundingBox, centerPoint, rangeRadius))
                    if (addedToShootingList[i] == false)
                    {
                        AddShootingTargetIndex(i);
                        addedToShootingList[i] = true;
                    }
            //if the target is outside the range circle, remove the target from the shooting list
            for (int i = HEAD; i < shootingTargetIndex.Length; i++)
                if (shootingTargetIndex[i] >= targets.Length || Graphic.CircleCollisionDetection(targets[i].boundingBox, centerPoint, rangeRadius) == false)
                    RemoveShootingTargetIndex(i);
            //if there are elements in shooting list, this means there are enemies in the range circle, so shoot the enemy
            if (shootingTargetIndex.Length > 0)
            {     
                //if the timer is reset to 0, add a projectile
                if (timeCounter.IsRunning == false)
                {
                    //if the defense tower is not missile tower, calculte the return angle
                    if (defenseTowerType != DefenseTowerType.Missile)
                        returnAngle = Graphic.CalculateRotateAngle(centerPoint, Graphic.CalculateCenterPoint(targets[shootingTargetIndex[0]].boundingBox));
                    //shoot the projectile at the first element of the shooting list
                    AddProjectile(targets[shootingTargetIndex[HEAD]].boundingBox, shootingTargetIndex[HEAD]);
                    //here, there must not be misslies in each defense tower
                    noMissile = true;
                    //restart the timer
                    timeCounter.Restart();
                }
                //if the timer runs more than gap timer, shoot another projectile
                if (timeCounter.Elapsed.TotalSeconds >= gapTime)
                {
                    timeCounter.Reset();
                    //the missile is now loaded
                    noMissile = false;
                }               
            }
            //here, there must be no enemy inside the range circle, so clear the shooting list
            else
            {
                initializeShootingList = false;
                ResizeShootingTargetIndex(0);
                //if the time reaches the loading time, show the missile in missile tower
                if (timeCounter.Elapsed.TotalSeconds >= gapTime)
                    noMissile = false;
            }
            //update the projectile motion regarding to the type of the defense tower
            if (defenseTowerType != DefenseTowerType.Missile)
                ProjectileMotionForNormalAndLongRangeTower(ref targets);
            else
                ProjectileMotionForMissleTower(ref targets);
           //show explosion if the missile hit the enemy
            ExplosionSettings();
            return returnAngle;
        }
        /*
         * This function determines whether show the explosion or not
         */
        void ExplosionSettings()
        {
            const float EXPLOSION_TIME = 0.3f;
            //when there is an explosion
            if (showExplosion == true)
            {
                //if the explosion lasts more than a certain time, stop it
                if (explosionCounter.Elapsed.TotalSeconds >= EXPLOSION_TIME)
                {       
                    showExplosion = false;
                    explosionCounter.Reset();                
                }
            }
            //if there's not an explosion, set the bouding box to null
            else
            {
                explosionBB.Location = new PointF(0, 0);
                explosionBB.Size = new SizeF(0, 0);
            }
        }
        /*
         * This function determines the projectile's motion by calculating the slope of the line 
         * between the starting point and the enemy for missle tower. It takes the 
         * information of the enemies as its argument.
         */
        void ProjectileMotionForMissleTower(ref Enemy[] targets)
        {
            //store the head of the array
            const int HEAD = 0;
            //check each projectile in the list
            for (int i = HEAD; i < projectiles.Length; i++)
            {
                float run = 0, rise = 0;
                //calculte three sides of the triange formed between the starting point and the target
                run = targets[projectiles[i].targetIndex].boundingBox.X - projectiles[i].x;
                rise = targets[projectiles[i].targetIndex].boundingBox.Y - projectiles[i].y;
                float hypotenuse = (float)Math.Sqrt(run * run + rise * rise);
                //update the enemy location
                projectiles[i].x += (run / hypotenuse) * (speed * Main.deltaTime);
                projectiles[i].y += (rise / hypotenuse) * (speed * Main.deltaTime);
                projectiles[i].boundingBox.Location = new PointF(projectiles[i].x, projectiles[i].y);
                projectiles[i].image = Graphic.RotateImage(missileProjectileImage, Graphic.CalculateRotateAngle(projectiles[i].boundingBox.Location, targets[projectiles[i].targetIndex].boundingBox.Location));
                //check if the missle hit the enemy
                for(int j = HEAD, k = targets.Length; j < k; j++)
                {
                    //if the missle hit the enemy, show the explosion and remove the projectile
                    if (Graphic.RectangleIntersectionDetection(targets[j].boundingBox, projectiles[i].boundingBox))
                    {
                        //set the explosion boundingbox to the corrent size and location
                        explosionBB.Size = explosionSize;
                        explosionBB.Location = new PointF(projectiles[i].x - explosionSize.Width / 4, projectiles[i].y - explosionSize.Height / 2);                
                        //show the image of the explosion                
                        showExplosion = true;             
                        //start the counter that stores the time for the explosion lasts            
                        explosionCounter = new Stopwatch();
                        explosionCounter.Start();
                        //update the health of the enemy if they are in the explosion zone
                        for (int m = HEAD; m < targets.Length; m++)
                        {
                            if (Graphic.RectangleIntersectionDetection(explosionBB, targets[m].boundingBox))
                                targets[m].health -= projectileDamage;
                        }
                        RemoveProjectile(i);
                        break;
                    }
                }
            }
        }
        /*
         * This function determines the projectile's motion by calculating the slope of the line 
         * between the starting point and the enemy for normal and long range tower. It takes the 
         * information of the enemys as its argument.
         */
        void ProjectileMotionForNormalAndLongRangeTower(ref Enemy[] targets)
        {
            //check each projectile in the list
            for (int i = 0; i < projectiles.Length; i++)
            {
                float run = 0, rise = 0;
                //calculte three sides of the triange formed between the starting point and the target
                run = projectiles[i].targetInstantX - projectiles[i].startingX;
                rise = projectiles[i].targetInstantY - projectiles[i].startingY;
                float hypotenuse = (float)Math.Sqrt(run * run + rise * rise);
                //update the enemy location
                projectiles[i].x += (run / hypotenuse) * (speed * Main.deltaTime);
                projectiles[i].y += (rise / hypotenuse) * (speed * Main.deltaTime);
                projectiles[i].boundingBox.Location = new PointF(projectiles[i].x, projectiles[i].y);
                //check if the enemy is out of bound, if so, terminate the loop and update the array length
                BoundaryCheck(ref targets, i);                              
            }
        }
        /*
         * This function checks if the projectile needs to be removed from the list. It takes the index of a certain
         * element in projectiles and the bouding box of the target as its argument. It returns true if it removed this
         * element, false if not.
         */
        void BoundaryCheck(ref Enemy[] targets, int index)
        {
            //if the projectile is out of boud, remove it and terminate the function
            if (projectiles[index].x + projectileSize.Width < 0 || projectiles[index].x > Main.size.Width ||
                projectiles[index].y + projectileSize.Height < 0 || projectiles[index].y > Main.size.Height)
            {
                RemoveProjectile(index);
                return;
            }
            for (int i = 0; i < targets.Length; i++)
            {
                //if the projectile intersects with the target, remove it and terminate the function
                if (Graphic.RectangleIntersectionDetection(projectiles[index].boundingBox, targets[i].boundingBox))
                {
                    targets[i].health -= projectileDamage;
                    RemoveProjectile(index);
                    return;
                }
        }
        }
        /*
         * This function resizes the projectile list to a certain size. It takes the new size as its argument.
         */
        void ResizeProjectile(int newSize)
        {
            //store the index of the head of the list
            const int HEAD = 0;
            //create a copy for the current list
            ProjectileInformation[] copy = new ProjectileInformation[newSize];
            if (newSize > projectiles.Length)
                for (int i = HEAD, n = projectiles.Length; i < n; i++)
                    copy[i] = projectiles[i];
            else
                for (int i = HEAD; i < newSize; i++)
                    copy[i] = projectiles[i];
            //resize the original array
            projectiles = new ProjectileInformation[newSize];
            //transfer what's on the copy to the original array
            for (int i = HEAD; i < newSize; i++)
                projectiles[i] = copy[i];
        }
        /*
         * This function adds one new projectile to the list. It takes the bounding box of the target as its
         * argument.
         */
        void AddProjectile(RectangleF targetBB, int targetIndex)
        {
            //resize the list to one more of its length
            ResizeProjectile(projectiles.Length + 1);
            //initialize the lastest elemnt
            projectiles[projectiles.Length - 1] = NewProjectile(targetBB, targetIndex);
        }
        /*
         * This function removes one projectile from the list. It takes the index of the element to be removed 
         * as its argument.
         */
        void RemoveProjectile(int index)
        {
            //shift each element one place left
            for (int i = index, n = projectiles.Length - 1; i < n; i++)
                projectiles[i] = projectiles[i + 1];
            //resize the list to one less of its length
            ResizeProjectile(projectiles.Length - 1);

        }
        /*
         * This function assigns new information to a new projectile. It takes the bouding box of the 
         * target as its argument.
         */
        ProjectileInformation NewProjectile(RectangleF targetBB, int targetIndex)
        {
            //creat a new projectile
            ProjectileInformation newProjectile = new ProjectileInformation();
            //assign all the information to the new projectile
            newProjectile.x = initialXLocation;
            newProjectile.y = initialYLocation;
            newProjectile.boundingBox.Location = new PointF(initialXLocation, initialYLocation);
            newProjectile.boundingBox.Size = projectileSize;
            newProjectile.targetInstantX = targetBB.X + targetBB.Width / 2;
            newProjectile.targetInstantY = targetBB.Y + targetBB.Height / 2;
            newProjectile.startingX = initialXLocation;
            newProjectile.startingY = initialYLocation;
            newProjectile.targetSize = targetBB.Size;
            if (defenseTowerType != DefenseTowerType.Missile)
                newProjectile.image = normalProjectileImage;
            else newProjectile.image = missileProjectileImage;
            newProjectile.targetIndex = targetIndex;
            //return the new proejctile
            return newProjectile;
        }
        /*
        * This function resizes the shooting target list to a certain size. It takes the new size as its argument.
        */
        void ResizeShootingTargetIndex(int newSize)
        {
            //store the index of the head of the array
            const int HEAD = 0;
            //create a copy for the current list
            int[] copy = new int[newSize];
            if (shootingTargetIndex.Length > newSize)
                for (int i = HEAD; i < newSize; i++)
                    copy[i] = shootingTargetIndex[i];
            else
                for (int i = HEAD, n = shootingTargetIndex.Length; i < n; i++)
                    copy[i] = shootingTargetIndex[i];
            //resize the original array
            shootingTargetIndex = new int[newSize];
            //transfer what's on the copy to the original array
            for (int i = HEAD; i < newSize; i++)
                shootingTargetIndex[i] = copy[i];
        }
        /*
         * This function adds one new shooting target to the list. It takes the index of the new target
         * argument.
         */
        void AddShootingTargetIndex(int newIndex)
        {
            //resize the list to one more of its length
            ResizeShootingTargetIndex(shootingTargetIndex.Length + 1);
            //set the new index to the end of the list
            shootingTargetIndex[shootingTargetIndex.Length - 1] = newIndex;
        }
        /*
         * This function removes one shooting target index from the list. It takes the index of the element to be removed 
         * as its argument.
         */
        void RemoveShootingTargetIndex(int indexToRemove)
        {
            for (int i = indexToRemove, n = shootingTargetIndex.Length - 1; i < n; i++)
                shootingTargetIndex[i] = shootingTargetIndex[i + 1];
            ResizeShootingTargetIndex(shootingTargetIndex.Length - 1);
        }
        /*
         * This function draws the projectile. It takes the paint event argument of main form and the image of
         * the projectile as its argument.
         */
        public void Paint(PaintEventArgs e, Image projectileImage)
        {
            //store in the head of the array
            const int HEAD = 0;
            //draw the projectile 
            for (int i = HEAD, n = projectiles.Length; i < n; i++)
                e.Graphics.DrawImage(projectiles[i].image, projectiles[i].boundingBox);
            //if there's an explosion, show the explosion image
            if (showExplosion == true)
                e.Graphics.DrawImage(explosionImage, explosionBB);
        }
    }
}
