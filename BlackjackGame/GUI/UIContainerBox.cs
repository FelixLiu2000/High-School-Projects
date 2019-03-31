using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace _2_Assignment_Blackjack.GUI
{
    class UIContainerBox : UIWidget
    {
        Pen borderPen;
        Brush fillColour;
        public UIContainerBox(Size size, Point location, Brush fillColour, bool visible) : base(size, location, visible)
        {
            this.fillColour = fillColour;
        }

        public UIContainerBox(Size size, Point location, Brush fillColour, Brush borderColour, float borderWidth, bool visible) : this(size, location, fillColour, visible)
        {
            borderPen = new Pen(borderColour, borderWidth);
        }

        public override void Paint(PaintEventArgs e)
        {
            if (Visible == true)
            {
                if (borderPen != null)
                    e.Graphics.DrawRectangle(borderPen, rectangle);
                else
                    e.Graphics.FillRectangle(fillColour, rectangle);
            }
        }
    }
}
