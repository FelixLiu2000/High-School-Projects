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
    enum EnemyDirection { None, Up, Down, Left, Right };

    class Enemy
    {
        public HealthBar healthBar;

        public float x;
        public float y;
        public float speed;
        public RectangleF boundingBox;
        public int killReward;
        public int health;
        int section = 1;
        public float distanceTraveled;
        public EnemyDirection moveDirection = EnemyDirection.None;
        Bitmap upImage, downImage, leftImage, rightImage;
        public Bitmap paintImage;

        public Enemy(float startingXLocation, float startingYLocation, float speedInput, SizeF sizeInput, int healthInput, int reward, Bitmap imageInput)
        {
            x = startingXLocation;
            y = startingYLocation;
            speed = speedInput;
            health = healthInput;
            killReward = reward;

            boundingBox.Size = sizeInput;
            boundingBox.Location = new PointF(x, y);
            healthBar = new HealthBar(boundingBox, health);
            paintImage = imageInput;
            downImage = imageInput;
            leftImage = Graphic.RotateImage(imageInput, 90);
            upImage = Graphic.RotateImage(imageInput, 180);
            rightImage = Graphic.RotateImage(imageInput, 270);
        }

        void MoveDown()
        {
            y += speed * Main.deltaTime;
            if(y >= 0)
                distanceTraveled += speed * Main.deltaTime;
            boundingBox.Location = new PointF(x, y);
            moveDirection = EnemyDirection.Down;
        }
        void MoveUp()
        {
            y -= speed * Main.deltaTime;
            if (y >= 0)
                distanceTraveled += speed * Main.deltaTime;
            boundingBox.Location = new PointF(x, y);
            moveDirection = EnemyDirection.Up;
        }
        void MoveLeft()
        {
            x -= speed * Main.deltaTime;
            if (y >= 0)
                distanceTraveled += speed * Main.deltaTime;
            boundingBox.Location = new PointF(x, y);
            moveDirection = EnemyDirection.Left;
        }
        void MoveRight()
        {
            x += speed * Main.deltaTime;
            if (y >= 0)
                distanceTraveled += speed * Main.deltaTime;
            boundingBox.Location = new PointF(x, y);
            moveDirection = EnemyDirection.Right;
        }
        
        public void SetFacingDirection(EnemyDirection direction)
        {
            if (direction == EnemyDirection.Down)
                paintImage = downImage;
            else if (direction == EnemyDirection.Left)
            {
                paintImage = leftImage;
            }
            else if (direction == EnemyDirection.Up)
            {
                paintImage = upImage;
            }
            else if (direction == EnemyDirection.Right)
            {
                paintImage = rightImage;
            }
        }
        public void Move()
        {
            switch (moveDirection)
            {
                case EnemyDirection.Up:
                    if (y <= 135 && section == 5)
                    {
                        section++;
                        MoveLeft();
                    }
                    else
                        MoveUp();
                    break;
                case EnemyDirection.Down:
                    if (y >= 335 && section == 1)
                    {
                        section++;
                        MoveRight();
                    }
                    else if (y >= 630 && section == 3)
                    {
                        section++;
                        MoveLeft();
                    }
                    else
                        MoveDown();
                    break;
                case EnemyDirection.Left:
                    if (x <= 335 && section == 4)
                    {
                        section++;
                        MoveUp();
                    }
                    else if (x <= 135 && section == 6)
                    {
                        section++;
                        MoveDown();
                    }
                    else
                        MoveLeft();
                    break;
                case EnemyDirection.Right:
                    if (x >= 1130 && section == 2)
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
            healthBar.Refresh(boundingBox, health);
        }

        public void ReduceHealth(int amount)
        {
            health -= amount;
        }
        public void Paint(PaintEventArgs e, Image enemyImage)
        {
            healthBar.Paint(e, Brushes.Maroon, Brushes.Red);
            e.Graphics.DrawImage(enemyImage, boundingBox);           
        }
    }

    class EnemyList
    {
        public Enemy[] enemies;
        public EnemyList(int numberOfEnemies, float startingXLocation, float startingYLocation, float enemySpeed,
            float distanceBetweenEachEnemy, SizeF enemySize, int enemyHealth, int reward, Bitmap enemyImage)
        {
            enemies = new Enemy[numberOfEnemies];
            for (int i = 0; i < numberOfEnemies; i++)
                enemies[i] = new Enemy(startingXLocation, startingYLocation - (i * distanceBetweenEachEnemy), enemySpeed, enemySize, enemyHealth, reward, enemyImage);
        }
        void CheckEnemy(int index)
        {
            //where enemy dies
            if (enemies[index].health <= 0)
            {
                UI.user.MoneyGain(enemies[index].killReward);
                RemoveEnemy(index);
            }
            else if (enemies[index].y > Main.size.Height)
            {
                RemoveEnemy(index);
                UI.user.LivesLoss();
            }
        }
        void RemoveEnemy(int index)
        {
            for (int i = index, n = enemies.Length - 1; i < n; i++)
                enemies[i] = enemies[i + 1];
            //resize the list to one less of its length
            ResizeEnemy(enemies.Length - 1);
            
        }
        void ResizeEnemy(int newSize)
        {
            Enemy[] copy = new Enemy[newSize];
            if (enemies.Length > newSize)
                for (int i = 0; i < newSize; i++)
                    copy[i] = enemies[i];
            else
                for (int i = 0, n = enemies.Length; i < n; i++)
                    copy[i] = enemies[i];
            enemies = new Enemy[newSize];
            for (int i = 0; i < newSize; i++)
                enemies[i] = copy[i];
        }
        
        public void Update()
        {
            for(int i = 0; i < enemies.Length; i++)
            {
                enemies[i].Move();
                enemies[i].SetFacingDirection(enemies[i].moveDirection);
                CheckEnemy(i);
            }
        }

        public void Paint(PaintEventArgs e)
        {
            foreach (Enemy enem in enemies)
                enem.Paint(e, enem.paintImage);
        }
    }
}
