/*
 * Bosen Qu
 * January 24, 2015
 * This following program creates and paints the background 
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
     * This enumeration classifies the type of each tile
     */
    enum Tile
    {
        Road, Grass, Barrier, WidgetBar
    }
    /// <summary>
    /// This class divieds the background to different tiles with various propertities. It also paints
    /// the background
    /// </summary>
    class Background
    {
        //declear the variables needed for background tiles
        const int BACKGROUND_TILE_WIDTH = 26, BACKGROUND_TILE_HEIGHT = 16;
        Image[,] tileImages = new Image[BACKGROUND_TILE_WIDTH, BACKGROUND_TILE_HEIGHT];
        public RectangleF[,] tileBB = new RectangleF[BACKGROUND_TILE_WIDTH, BACKGROUND_TILE_HEIGHT];
        public SizeF tileSize = new SizeF(50, 50);
        public Tile[,] tileProperties = new Tile[BACKGROUND_TILE_WIDTH, BACKGROUND_TILE_HEIGHT];
        //declear the variables needed for bushes
        const int NUMBER_OF_BUSHES = 6;
        RectangleF[] bushBB = new RectangleF[NUMBER_OF_BUSHES];
        SizeF bushSize = new SizeF(100, 100);
        Image[] bushImages = new Image[NUMBER_OF_BUSHES];
        //declear the variables needed for stones
        const int NUMBER_OF_STONES = 4;
        RectangleF[] stoneBB = new RectangleF[NUMBER_OF_STONES];
        SizeF stoneSize = new SizeF(80, 80);
        Image[] stoneImages = new Image[NUMBER_OF_STONES];
        //declear the image that shows wrench when the user is placing item
        Image placingItemImage = Properties.Resources.PlacingItem;
        //declear the vairbles to check if the user user is placing items or not
        public static bool placingDefenseTower;
        //declear the bouding box of the widget bar on the top right of the screen
        Rectangle widgetBar;
        //store the index of the head of the array
        const int HEAD = 0;
        /*
         * This constructor initializeds the background. It takes the recatangle of the widget bar
         * as its argument
         */
        public Background(Rectangle bar)
        {
            //copy the input rectnagle to the widget ba
            widgetBar = bar;
            //initialize the background
            InitialSetting();
        }
        /*
         * This function calculates the actual coordinate of the tile. It takes the index of
         * a two dementional array as its argument. It returns the actual point on the screen.
         */
        PointF TileToCoordinate(int xTile, int yTile)
        {
            //calculate the x and y coordinate
            float x = xTile * tileSize.Width;
            float y = yTile * tileSize.Height;
            //return the new point
            return new PointF(x, y);
        }
        /*
         * This function calculates the width index of the a certain element of the array. It takes an acutal
         * x location as its argument. It returns the width index of the array as its argument.
         */
        public int XCoordinateToXTile(int x)
        {
            return (int)(x / tileSize.Width);
        }
        /*
         * This function calculates the height index of the a certain element of the array. It takes an acutal
         * y location as its argument. It returns the height index of the array as its argument.
         */
        public int YCoordinateToYTile(int y)
        {
            return (int)(y / tileSize.Height);
        }
       /*
        * This function initializeds the background by assigning propertities to each tiles of the background
        */
        void InitialSetting()
        {
            //assignment propertities to each background tile
            for (int i = HEAD; i < BACKGROUND_TILE_WIDTH; i++)
                for (int j = HEAD; j < BACKGROUND_TILE_HEIGHT; j++)
                {
                    tileBB[i, j].Location = TileToCoordinate(i, j);
                    tileBB[i, j].Size = tileSize;
                    tileImages[i, j] = Properties.Resources.Grass;
                    tileProperties[i, j] = Tile.Grass;
                }
            //settings for grass
            for (int i = HEAD; i <= 5; i++)
            {
                tileImages[14, i] = Properties.Resources.RoadLeft;
                tileImages[15, i] = Properties.Resources.RoadRight;
                tileProperties[15, i] = Tile.Road;
                tileProperties[14, i] = Tile.Road;
            }
            tileImages[15, 6] = Properties.Resources.TurnUp;
            tileProperties[15, 6] = Tile.Road;
            tileImages[14, 6] = Properties.Resources.RoadLeft;
            tileProperties[14, 6] = Tile.Road;
            tileImages[14, 7] = Properties.Resources.TurnDown;
            tileProperties[14, 7] = Tile.Road;
            tileImages[15, 7] = Properties.Resources.RoadDown;
            tileProperties[15, 7] = Tile.Road;

            for (int i = 16; i <= 21; i++)
            {
                tileImages[i, 6] = Properties.Resources.RoadUp;
                tileImages[i, 7] = Properties.Resources.RoadDown;
                tileProperties[i, 6] = Tile.Road;
                tileProperties[i, 7] = Tile.Road;
            }
            tileImages[22, 6] = Properties.Resources.RoadUp;
            tileImages[23, 6] = Properties.Resources.TurnDown;
            tileImages[23, 6].RotateFlip(RotateFlipType.Rotate90FlipX);
            tileImages[22, 7] = Properties.Resources.TurnUp;
            tileImages[22, 7].RotateFlip(RotateFlipType.Rotate90FlipX);
            tileImages[23, 7] = Properties.Resources.RoadRight;
            tileProperties[22, 6] = Tile.Road;
            tileProperties[22, 7] = Tile.Road;
            tileProperties[23, 6] = Tile.Road;
            tileProperties[23, 7] = Tile.Road;
            for (int i = 8; i <= 11; i++)
            {
                tileImages[22, i] = Properties.Resources.RoadLeft;
                tileImages[23, i] = Properties.Resources.RoadRight;
                tileProperties[22, i] = Tile.Road;
                tileProperties[23, i] = Tile.Road;
            }
            tileImages[22, 12] = Properties.Resources.TurnUp;
            tileImages[22, 12].RotateFlip(RotateFlipType.Rotate270FlipNone);
            tileProperties[22, 12] = Tile.Road;
            tileImages[23, 12] = Properties.Resources.RoadRight;
            tileProperties[23, 12] = Tile.Road;
            tileImages[23, 13] = Properties.Resources.TurnDown;
            tileImages[23, 13].RotateFlip(RotateFlipType.Rotate270FlipNone);
            tileProperties[23, 13] = Tile.Road;
            tileImages[22, 13] = Properties.Resources.RoadDown;
            tileProperties[22, 13] = Tile.Road;
            for (int i = 21; i >= 8; i--)
            {
                tileImages[i, 12] = Properties.Resources.RoadUp;
                tileImages[i, 13] = Properties.Resources.RoadDown;
                tileProperties[i, 12] = Tile.Road;
                tileProperties[i, 13] = Tile.Road;
            }
            tileImages[7, 12] = Properties.Resources.TurnUp;
            tileProperties[7, 12] = Tile.Road;
            tileImages[6, 13] = Properties.Resources.TurnDown;
            tileProperties[6, 13] = Tile.Road;
            tileImages[6, 12] = Properties.Resources.RoadLeft;
            tileProperties[6, 12] = Tile.Road;
            tileImages[7, 13] = Properties.Resources.RoadDown;
            tileProperties[7, 13] = Tile.Road;
            for (int i = 11; i >= 4; i--)
            {
                tileImages[7, i] = Properties.Resources.RoadRight;
                tileImages[6, i] = Properties.Resources.RoadLeft;
                tileProperties[7, i] = Tile.Road;
                tileProperties[6, i] = Tile.Road;
            }
            tileImages[7, 2] = Properties.Resources.TurnDown;
            tileImages[7, 2].RotateFlip(RotateFlipType.Rotate90FlipX);
            tileProperties[7, 2] = Tile.Road;
            tileImages[6, 3] = Properties.Resources.TurnUp;
            tileImages[6, 3].RotateFlip(RotateFlipType.Rotate90FlipX);
            tileProperties[6, 3] = Tile.Road;
            tileImages[7, 3] = Properties.Resources.RoadRight;
            tileProperties[7, 3] = Tile.Road;
            tileImages[6, 2] = Properties.Resources.RoadUp;
            tileProperties[6, 2] = Tile.Road;
            for (int i = 5; i >= 4; i--)
            {
                tileImages[i, 2] = Properties.Resources.RoadUp;
                tileImages[i, 3] = Properties.Resources.RoadDown;
                tileProperties[i, 2] = Tile.Road;
                tileProperties[i, 3] = Tile.Road;
            }
            tileImages[2, 2] = Properties.Resources.TurnDown;
            tileImages[2, 2].RotateFlip(RotateFlipType.Rotate90FlipNone);
            tileProperties[2, 2] = Tile.Road;
            tileImages[3, 3] = Properties.Resources.TurnUp;
            tileImages[3, 3].RotateFlip(RotateFlipType.Rotate90FlipNone);
            tileProperties[3, 3] = Tile.Road;
            tileImages[3, 2] = Properties.Resources.RoadUp;
            tileProperties[3, 2] = Tile.Road;
            tileImages[2, 3] = Properties.Resources.RoadLeft;
            tileProperties[2, 3] = Tile.Road;
            for (int i = 4; i <= 15; i++)
            {
                tileImages[3, i] = Properties.Resources.RoadRight;
                tileImages[2, i] = Properties.Resources.RoadLeft;
                tileProperties[3, i] = Tile.Road;
                tileProperties[2, i] = Tile.Road;
            }
            //settings for bushes
            for (int i = HEAD; i < NUMBER_OF_BUSHES; i++)
                bushBB[i].Size = bushSize;
            bushBB[0].Location = new PointF(500, 100);
            bushImages[0] = Properties.Resources.Bush1;
            bushBB[1].Location = new PointF(550, 180);
            bushImages[1] = Properties.Resources.Bush2;
            bushBB[2].Location = new PointF(650, 500);
            bushImages[2] = Properties.Resources.Bush3;
            bushBB[3].Location = new PointF(625, 430);
            bushImages[3] = Properties.Resources.Bush3;
            bushImages[3].RotateFlip(RotateFlipType.RotateNoneFlipX);
            bushBB[4].Location = new PointF(20, 20);
            bushImages[4] = Properties.Resources.Bush1;
            bushBB[5].Location = new PointF(100, 38);
            bushImages[5] = Properties.Resources.Bush2;
            //settings for stones
            for (int i = 0; i < NUMBER_OF_STONES; i++)
                stoneBB[i].Size = stoneSize;
            stoneBB[0].Location = new PointF(211, 576);
            stoneImages[0] = Properties.Resources.Stone1;
            stoneBB[1].Location = new PointF(450, 360);
            stoneImages[1] = Properties.Resources.Stone2;
            stoneBB[2].Location = new PointF(1210, 490);
            stoneImages[2] = Properties.Resources.Stone3;
            stoneBB[3].Location = new PointF(1180, 435);
            stoneImages[3] = Properties.Resources.Stone2;

            for (int i = HEAD; i < BACKGROUND_TILE_WIDTH; i++)
                for (int j = HEAD; j < BACKGROUND_TILE_HEIGHT; j++)
                {
                    for (int k = HEAD; k < NUMBER_OF_BUSHES; k++)
                        if (tileBB[i, j].IntersectsWith(bushBB[k]))
                            tileProperties[i, j] = Tile.Barrier;
                    for (int k = HEAD; k < NUMBER_OF_STONES; k++)
                        if (tileBB[i, j].IntersectsWith(stoneBB[k]))
                            tileProperties[i, j] = Tile.Barrier;
                }

            for (int i = (int)(BACKGROUND_TILE_WIDTH - Math.Ceiling((float)widgetBar.Width / (float)tileSize.Width)); i < BACKGROUND_TILE_WIDTH; i++)
                for (int j = HEAD; j < Math.Ceiling((float)widgetBar.Height / (float)tileSize.Height); j++)
                {
                    tileProperties[i, j] = Tile.WidgetBar;
                }                 
        }
        /*
         * This function checks if the user can place item on a certain location. This function takes the location of 
         * the target that user wants to place as its argument. If there is barrier on that location, it returns false, 
         * else it return true.
         */
        public bool CheckPlacingItem(PointF targetLocation)
        {
            //calculate the index of the element
            int widthIndex = XCoordinateToXTile((int)targetLocation.X);
            int heightIndex = YCoordinateToYTile((int)targetLocation.Y);
            //return false if there are items placing on the element, else return false
            if (tileProperties[widthIndex, heightIndex] == Tile.Road ||
                tileProperties[widthIndex, heightIndex] == Tile.Barrier || tileProperties[widthIndex, heightIndex] == Tile.WidgetBar)
                return false;
            return true;
        }
        /*
         * This function paints the background. It takes the paint event argument from main as its argument.
         */
        public void Paint(PaintEventArgs e)
        {
            //paint each tile
            for (int i = HEAD; i < BACKGROUND_TILE_WIDTH; i++)
                for (int j = HEAD; j < BACKGROUND_TILE_HEIGHT; j++)
                {
                    if (placingDefenseTower == true && tileProperties[i, j] == Tile.Grass)
                        e.Graphics.DrawImage(placingItemImage, tileBB[i, j]);
                    else
                        e.Graphics.DrawImage(tileImages[i, j], tileBB[i, j]);
                }
            //paint each bush
            for (int i = HEAD; i < NUMBER_OF_BUSHES; i++)
                e.Graphics.DrawImage(bushImages[i], bushBB[i]);
            //paint each stone
            for (int i = HEAD; i < NUMBER_OF_STONES; i++)
                e.Graphics.DrawImage(stoneImages[i], stoneBB[i]);          
        }
    }
}
