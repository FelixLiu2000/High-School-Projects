/*
 * Bosen Qu
 * January 24, 2017
 * The following program controls enemy's movement and paints each enemy
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
    /*
     * This enumeration checks the direction that an enemy is moving
     */
    enum EnemyDirection { None, Up, Down, Left, Right };
    /// <summary>
    /// This class creates, controls movement and paints each enemy
    /// </summary>
    class Enemy
    {
        //declear the information for the health bar for the enemy
        public HealthBar healthBar;
        //declear the location of the enemy
        public float x;
        public float y;
        //declear the variable that stores the speed of the enemy
        public float speed;
        //declear the variable that stores the bouding box of the rectangle
        public RectangleF boundingBox;
        //declear the variable that stores the money added after killing an enemy
        public int killReward;
        //declear the health of the enemy
        public int health;
        //declear the variable that stores the section on the map that the enemy is on
        int section = 1;
        //declear the variables that stores the distance has traveled by the enemy
        public float distanceTraveled;
        //declear and assign the vairbles that stores the direction the enemy is moving
        public EnemyDirection moveDirection = EnemyDirection.None;
        //declear the image that stoes the enemy facing differenct direction
        Bitmap upImage, downImage, leftImage, rightImage;
        //declear the image that stores the actual image of the enemy
        public Bitmap paintImage;
        //store the initial y location of the enemy
        const int STARTING_Y_LOCATION = 0;
        //store each section's number
        const int SECTION_ONE = 1, SECTION_TWO = 2, SECTION_THREE = 3, SECTION_FOUR = 4, SECTION_FIVE = 5, SECTION_SIX = 6;
        /*
         * This constructor initializes each propertities of the enemy. It takes the starting location 
         * of the enemy, the speed of the enemy, the size of the bouding box of the enemy, the health 
         * of the enemy, the amount of reward chash after killing the enemy, the image of the enemy as its
         * argument.
         */
        public Enemy(float startingXLocation, float startingYLocation, float speedInput, SizeF sizeInput, int healthInput, int reward, Bitmap imageInput)
        {
            //assign the location of the enemy       
            x = startingXLocation;
            y = startingYLocation;
            //assign the speed of the enemy
            speed = speedInput;
            //assign the health of the enemy
            health = healthInput;
            //assign the killing reward after killing the enemy
            killReward = reward;
            //assign the size of the bouding box of the enemy
            boundingBox.Size = sizeInput;
            //assign the location of the bouding box of the enemy
            boundingBox.Location = new PointF(x, y);
            //initialize the information needed for health bar
            healthBar = new HealthBar(boundingBox, health);
            //initialize the image that goes to on paint
            paintImage = imageInput;
            //store the image for different directions of the enemy
            downImage = imageInput;
            leftImage = Graphic.RotateImage(imageInput, 90);
            upImage = Graphic.RotateImage(imageInput, 180);
            rightImage = Graphic.RotateImage(imageInput, 270);
        }
        /*
         * This function allows the enemy to move down
         */
        void MoveDown()
        {
            //update the y location
            y += speed * Main.deltaTime;
            boundingBox.Location = new PointF(x, y);
            //update the distance enemy has traveled
            if (y >= STARTING_Y_LOCATION)
                distanceTraveled += speed * Main.deltaTime;       
            //update the enemy direction
            moveDirection = EnemyDirection.Down;
        }
        /*
         * This function allows the enemy to move up
         */
        void MoveUp()
        {
            //update the y location
            y -= speed * Main.deltaTime;
            boundingBox.Location = new PointF(x, y);
            //update the distance enemy has traveled
            if (y >= STARTING_Y_LOCATION)
                distanceTraveled += speed * Main.deltaTime;
           //update the enemy direction
            moveDirection = EnemyDirection.Up;
        }
        /*
         * This function allows the enemy to move left
         */
        void MoveLeft()
        {
            //update the x location
            x -= speed * Main.deltaTime;         
            boundingBox.Location = new PointF(x, y);
            //update the distance enemy has traveled
            if (y >= STARTING_Y_LOCATION)
                distanceTraveled += speed * Main.deltaTime;
            //update the enemy direction
            moveDirection = EnemyDirection.Left;
        }
        /*
         * This function allows thew enemy to move right
         */
        void MoveRight()
        {
            //update the x location
            x += speed * Main.deltaTime;       
            boundingBox.Location = new PointF(x, y);
            //update the distance enemy has traveled
            if (y >= STARTING_Y_LOCATION)
                distanceTraveled += speed * Main.deltaTime;
            //update the enemy direction
            moveDirection = EnemyDirection.Right;
        }
        /*
         * This function updates the enemy image depending on the direction that the enemy is move. It takes the enemy's
         * current facing direction as its argument.
         */
        public void SetFacingDirection(EnemyDirection direction)
        {      
            //update enemy's image based on its direction
            switch(direction)
            {
                case EnemyDirection.Down:
                    paintImage = downImage;
                    break;
                case EnemyDirection.Left:
                    paintImage = leftImage;
                    break;
                case EnemyDirection.Up:
                    paintImage = upImage;
                    break;
                case EnemyDirection.Right:
                    paintImage = rightImage;
                    break;
                default:
                    break;
            }
        }
        /*
         * This function moves the enemy and updates the location of the health bar.
         */
        public void Move()
        {
            //check enemy location and let the enemy to move at the corrent direction
            switch (moveDirection)
            {
                case EnemyDirection.Up:
                    if (y <= 135 && section == SECTION_FIVE)
                    {
                        section++;
                        MoveLeft();
                    }
                    else
                        MoveUp();
                    break;
                case EnemyDirection.Down:
                    if (y >= 335 && section == SECTION_ONE)
                    {
                        section++;
                        MoveRight();
                    }
                    else if (y >= 630 && section == SECTION_THREE)
                    {
                        section++;
                        MoveLeft();
                    }
                    else
                        MoveDown();
                    break;
                case EnemyDirection.Left:
                    if (x <= 335 && section == SECTION_FOUR)
                    {
                        section++;
                        MoveUp();
                    }
                    else if (x <= 135 && section == SECTION_SIX)
                    {
                        section++;
                        MoveDown();
                    }
                    else
                        MoveLeft();
                    break;
                case EnemyDirection.Right:
                    if (x >= 1130 && section == SECTION_TWO)
                    {
                        section++;
                        MoveDown();
                    }
                    else
                        MoveRight();
                    break;
                case EnemyDirection.None:
                    MoveDown();
                    break;
                default:
                    break;
            }
            //update the enemy's health bar location
            healthBar.Refresh(boundingBox, health);
        }
        /*
         * This function updates enemy's health if the enemy is hit by the fireball. It takes the amount of damage
         * of its argument.
         */
        public void ReduceHealth(int amount)
        {
            //update the health
            health -= amount;
        }
        /*
         * This function paints the enemy. It takes the paint event argument from main and the image of the enemy as
         * its argument.
         */
        public void Paint(PaintEventArgs e, Image enemyImage)
        {
            //paint the health bar
            healthBar.Paint(e, Brushes.Maroon, Brushes.Red);
            //paint the enemy
            e.Graphics.DrawImage(enemyImage, boundingBox);           
        }
    }
    /// <summary>
    /// This class creates a list of enemies. When the enemy appears it gets added to the list, if the enemy died or
    /// goes out of the bouding it gets removed from the array.
    /// </summary>
    class EnemyList
    {
        //create a list of enemies
        public Enemy[] enemies;
        //store the value of the enemy health when the enemy is dying
        const int NO_HEALTH = 0;
        //store the index of the head of the array
        const int HEAD = 0;
        /*
         * This constructor initializes the enemy list. It takes the number of enemies, the starting location of the enemy,
         * the speed of the enemy, the distance between each enemy, the size of the enemy, the full health of the enemy,
         * the cash reward after killing an enemy, and the image of the enemy
         */
        public EnemyList(int numberOfEnemies, float startingXLocation, float startingYLocation, float enemySpeed,
            float distanceBetweenEachEnemy, SizeF enemySize, int enemyHealth, int reward, Bitmap enemyImage)
        {
            //update the capacity of the list
            enemies = new Enemy[numberOfEnemies];
            //initialize each enemy
            for (int i = 0; i < numberOfEnemies; i++)
                enemies[i] = new Enemy(startingXLocation, startingYLocation - (i * distanceBetweenEachEnemy), enemySpeed, enemySize, enemyHealth, reward, enemyImage);
        }
        /*
         * This function checks the status of the enemy. It takes the index of the enemy as its argument
         */
        void CheckEnemy(int index)
        {
            //When an enemy dies, remove the enemy the enemy from the list and update the user's cash
            if (enemies[index].health <= NO_HEALTH)
            {
                UI.user.MoneyGain(enemies[index].killReward);
                RemoveEnemy(index);
            }
            //When an goes to the base, remove the enemy the enemy from the list and update the user's health
            else if (enemies[index].y > Main.size.Height)
            {
                RemoveEnemy(index);
                UI.user.LivesLoss();
            }
        }
        /*
         * This function removes a certian enemy from the list. It takes the index of the enemy that needs to be
         * removed as its argument.
         */
        void RemoveEnemy(int index)
        {
            //shift each element one place left
            for (int i = index, n = enemies.Length - 1; i < n; i++)
                enemies[i] = enemies[i + 1];
            //resize the list to one less of its length
            ResizeEnemy(enemies.Length - 1);       
        }
        /*
         * This function resizes the enemy list to a certain size. It takes the new size as its argument.
         */
        void ResizeEnemy(int newSize)
        {
            //create a copy of the list
            Enemy[] copy = new Enemy[newSize];
            if (enemies.Length > newSize)
                for (int i = HEAD; i < newSize; i++)
                    copy[i] = enemies[i];
            else
                for (int i = HEAD, n = enemies.Length; i < n; i++)
                    copy[i] = enemies[i];
            //resize the original array
            enemies = new Enemy[newSize];
            //copy what's on the copied list to the original list
            for (int i = HEAD; i < newSize; i++)
                enemies[i] = copy[i];
        }
        /*
         * This function updates each enemy as the timer runs
         */
        public void Update()
        {
            //check each enemy
            for(int i = HEAD; i < enemies.Length; i++)
            {
                //move the enemy
                enemies[i].Move();
                //update the facing direction of the enemy
                enemies[i].SetFacingDirection(enemies[i].moveDirection);
                //update each enemy's status
                CheckEnemy(i);
            }
        }
        /*
         * This function paints each enemy in the list. It takes the paint event argument from main as its argument.
         */
        public void Paint(PaintEventArgs e)
        {
            //paint each enemy
            for(int i = HEAD, n = enemies.Length; i < n; i++)
                enemies[i].Paint(e, enemies[i].paintImage);
        }
    }
}
