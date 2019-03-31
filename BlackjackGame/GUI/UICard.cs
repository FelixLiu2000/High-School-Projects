using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace _2_Assignment_Blackjack.GUI
{
    class UICard : UIWidget
    {
        int face;
        int suit;
        string faceCharacter;
        SolidBrush faceBrush;
        Font faceFont = new Font("Arial", 30);
        Image suitImage;
        Rectangle suitRectangle;
        Rectangle faceRectangle;

        public UICard(Point location, int suit, int face, bool visible) : base(location, visible)
        {
            rectangle.Size = new Size(100, 75);
            this.face = face;
            this.suit = suit;
            suitRectangle.Size = new Size(50, 50);
            suitRectangle.Location = new Point(rectangle.X + rectangle.Width / 2 - suitRectangle.Width, rectangle.Y + rectangle.Height / 2 - suitRectangle.Height / 2);
            
            switch(suit)
            {
                case Card.CLUBS:
                    suitImage = Properties.Resources.Club;
                    break;
                case Card.DIAMONDS:
                    suitImage = Properties.Resources.Diamond;
                    break;
                case Card.HEARTS:
                    suitImage = Properties.Resources.Heart;
                    break;
                case Card.SPADE:
                    suitImage = Properties.Resources.Spade;
                    break;
                default:
                    break;
            }

            if (face == 1)
                faceCharacter = "A";
            else if (face > 1 && face <= 10)
                faceCharacter = face.ToString();
            else if (face == 11)
                faceCharacter = "J";
            else if (face == 12)
                faceCharacter = "Q";
            else if (face == 13)
                faceCharacter = "K";

            faceBrush = new SolidBrush(Color.Black);
        }

        public override void Paint(PaintEventArgs e)
        {
            if (Visible == true)
            {
                faceRectangle.Size = Size.Ceiling(e.Graphics.MeasureString(faceCharacter, faceFont));
                faceRectangle.Location = new Point(rectangle.X + rectangle.Width / 2, rectangle.Y + rectangle.Height / 2 - faceRectangle.Height / 2);
                e.Graphics.DrawRectangle(new Pen(Color.Black), rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
                e.Graphics.DrawImage(suitImage, suitRectangle);
                e.Graphics.DrawString(faceCharacter, faceFont, faceBrush, faceRectangle);
            }
        }
    }
}
