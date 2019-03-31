/*
 * Bosen Qu
 * January 24, 2017
 * The following program allows the user to process graphic, for example the user
 * can rotate an image, find out the center point of a image, etc.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace BosenQuFelixLiuFinalCulminatingProject
{
    /// <summary>
    /// This class allows user to process graph
    /// </summary>
    static class Graphic
    {
        //store the number of each quadrant
        const int NOT_ASSIGNED = 0, FIRST_QUADRANT = 1, SECOND_QUADRANT = 2, THIRD_QUARDANT = 3, FORTH_QUARDANT = 4;
        /*
         * This function rotates an image by certian degrees from its center point. It takes the image and 
         * the angle that need to be rotated as its argument. It returns the rotated image.
         */
        public static Bitmap RotateImage(Bitmap b, float angle)
        {
            //Create a new empty bitmap to hold rotated image.
            Bitmap returnBitmap = new Bitmap(b.Width, b.Height);
            //Make a graphics object from the empty bitmap.
            Graphics g = Graphics.FromImage(returnBitmap);
            //move rotation point to center of image.
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            //g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.TranslateTransform((float)b.Width / 2, (float)b.Height / 2);
            //Rotate.        
            g.RotateTransform(angle);
            //Move image back.
            g.TranslateTransform(-(float)b.Width / 2, -(float)b.Height / 2);
            //draw the image
            g.DrawImage(b, 0, 0, b.Width, b.Height);  
            return returnBitmap;
        }
        /*
         * This function calculates the center point of an rectangle. It takes the rectangle as its argument.
         * It returns the location of the center point.
         */
        public static PointF CalculateCenterPoint(RectangleF objectBB)
        {
            //calculate the x and y location of the center point
            float x = objectBB.X + objectBB.Width / 2;
            float y = objectBB.Y + objectBB.Height / 2;
            return new PointF(x, y);
        }
        /*
         * This function calcualtes what quadrant of target is when assuming the location of the object
         * as the orgin point. It takes the location of the object and the location of the target as
         * its argument. It returns the quadrant number.
         */
        public static int CalculateQuadrant(PointF objectLocation, PointF targetLocation)
        {
            //declear and assign result with initialize value
            int result = NOT_ASSIGNED;
            //find out the what quadrant of the target by comparing its location to the object location
            if (targetLocation.X < objectLocation.X)
                if (targetLocation.Y < objectLocation.Y)
                    result = SECOND_QUADRANT;
                else result = THIRD_QUARDANT;
            else
                if (targetLocation.Y < objectLocation.Y)
                result = FIRST_QUADRANT;
            else result = FORTH_QUARDANT;
            return result;
        }
        /*
         * This function calculates the angle that the object needs to rotate in order to face the 
         * target. It takes the location of the object and the location of the target as its argument. 
         * It returns the angle that needs to be rotated.
         */
        public static float CalculateRotateAngle(PointF objectLocation, PointF targetLocation)
        {
            //declear and assign angle with initialize value
            float angle = 0;
            //calculate the reference angle by applying trig
            float x = Math.Abs(targetLocation.X - objectLocation.X);
            float y = Math.Abs(targetLocation.Y - objectLocation.Y);
            angle = (float)(Math.Atan(y / x) * (180 / Math.PI));
            //calculte the actual based on which quadrant the target is in
            switch (CalculateQuadrant(objectLocation, targetLocation))
            {
                case FIRST_QUADRANT:
                    angle = 90 - angle;
                    break;
                case SECOND_QUADRANT:
                    angle = 270 + angle;
                    break;
                case THIRD_QUARDANT:
                    angle = 270 - angle;
                    break;
                case FORTH_QUARDANT:
                    angle = 90 + angle;
                    break;
                default:
                    break;
            }
            return angle;
        }
        /*
         * This function determines if two rectangles are intersecting with each other. It takes two rectangles
         * as its argument. It retusn true if two are colliding with each other, false if not.
         */
        public static bool RectangleIntersectionDetection(RectangleF rectangleA, RectangleF rectangleB)
        {
            //determing if two rectangles are intersecting
            if (rectangleA.IntersectsWith(rectangleB))
                return true;
            return false;
        }
        /*
        * This function determines if ont rectangle contains another. It takes two rectangles
        * as its argument. It retusn true if one is contained by another, false if not.
        */
        public static bool RectangleContainsDection(RectangleF rectangleA, RectangleF rectangleB)
        {
            //determing if one rectangle contains another
            if (rectangleB.Contains(rectangleA))
                return true;
            return false;
        }
        /*
         * This function does collision detection to a circle and a recangle. It takes the bouding box of the target,
         * the center point of the circle, and the radious of the circle. It retusn true if two are colliding with each other, false if not.
         */
        public static bool CircleCollisionDetection(RectangleF targetBB, PointF centerPoint, float radious)
        {
            //if distance between two objects is less than radious, return true. If not, returns false.
            if (Math.Pow(targetBB.X + targetBB.Width / 2 - centerPoint.X, 2) + Math.Pow(targetBB.Y + targetBB.Height / 2 - centerPoint.Y, 2) <= Math.Pow(radious, 2))
                return true;
            return false;
        }

    }
}
