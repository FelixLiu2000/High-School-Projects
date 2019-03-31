using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace BosenQuFelixLiuFinalCulminatingProject
{
    static class Graphic
    {
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

            g.DrawImage(b, 0, 0, b.Width, b.Height);  //My Final Solution :3
            return returnBitmap;
        }
        public static PointF RotatePoint(PointF originalPoint, PointF centerPoint, float angle)
        {
            float x = 0, y = 0;
            x = (float)(Math.Cos(angle * Math.PI / 180) * (originalPoint.X - centerPoint.X) - Math.Sin(angle * Math.PI / 180) * (originalPoint.Y - centerPoint.Y) + originalPoint.X);
            y = (float)(Math.Sin(angle * Math.PI / 180) * (originalPoint.X - centerPoint.X) + Math.Cos(angle * Math.PI / 180) * (originalPoint.Y - centerPoint.Y) + originalPoint.Y);
            return new PointF(x, y);
        }
        public static PointF CalculateCenterPoint(RectangleF objectBB)
        {
            float x = objectBB.X + objectBB.Width / 2;
            float y = objectBB.Y + objectBB.Height / 2;
            return new PointF(x, y);
        }

        public static int CalculateQuadrant(PointF objectLocation, PointF targetLocation)
        {
            int result = 0;
            if (targetLocation.X < objectLocation.X)
                if (targetLocation.Y < objectLocation.Y)
                    result = 2;
                else result = 3;
            else
                if (targetLocation.Y < objectLocation.Y)
                result = 1;
            else result = 4;
            return result;
        }

        public static float CalculateRotateAngle(PointF objectLocation, PointF targetLocation)
        {
            float angle = 0;
            float x = Math.Abs(targetLocation.X - objectLocation.X);
            float y = Math.Abs(targetLocation.Y - objectLocation.Y);
            angle = (float)(Math.Atan(y / x) * (180 / Math.PI));
            switch (CalculateQuadrant(objectLocation, targetLocation))
            {
                case 1:
                    angle = 90 - angle;
                    break;
                case 2:
                    angle = 270 + angle;
                    break;
                case 3:
                    angle = 270 - angle;
                    break;
                case 4:
                    angle = 90 + angle;
                    break;
                default:
                    break;
            }
            return angle;
        }

        public static bool RectangleIntersectionDetection(RectangleF rectangleA, RectangleF rectangleB)
        {
            if (rectangleA.IntersectsWith(rectangleB))
                return true;
            return false;
        }
        public static bool RectangleContainsDection(RectangleF rectangleA, RectangleF rectangleB)
        {
            if (rectangleB.Contains(rectangleA))
                return true;
            return false;
        }
        public static bool CircleCollisionDetection(RectangleF targetBB, PointF centerPoint, float radious)
        {
            if (Math.Pow(targetBB.X + targetBB.Width / 2 - centerPoint.X, 2) + Math.Pow(targetBB.Y + targetBB.Height / 2 - centerPoint.Y, 2) <= Math.Pow(radious, 2))
                return true;
            return false;
        }

    }
}
