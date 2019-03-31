using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace _2_Assignment_Blackjack.GUI
{
    class UIButton : UIWidget
    {
        string text;
        Font font;
        SolidBrush textBrush;
        SolidBrush backgroundUnhoverBrush;
        SolidBrush backgrounHoverBrush;
        Pen outlinePen;
        public UIButton(Point location, string text, Font font, Color textColor, Color backgroundUnhoverColor, Color backgroundHoverColor, Color outlineColor, bool visible) : base(location, visible)
        {
            this.text = text;
            this.font = font;        
            textBrush = new SolidBrush(textColor);
            backgroundUnhoverBrush = new SolidBrush(backgroundUnhoverColor);
            backgrounHoverBrush = new SolidBrush(backgroundHoverColor);
            outlinePen = new Pen(outlineColor);
        }

        public override void Paint(PaintEventArgs e)
        {        
            if (Visible == true)
            {
                rectangle.Size = Size.Ceiling(e.Graphics.MeasureString(text, font));
                if (OnHover == true)
                    e.Graphics.FillRectangle(backgrounHoverBrush, rectangle);
                else
                    e.Graphics.FillRectangle(backgroundUnhoverBrush, rectangle);
                e.Graphics.DrawRectangle(outlinePen, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
                e.Graphics.DrawString(text, font, textBrush, rectangle);                           
            }
        }
    }
}
