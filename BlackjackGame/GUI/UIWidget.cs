using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace _2_Assignment_Blackjack.GUI
{
    abstract class UIWidget
    {
        public bool Visible { get; set; }
        public bool IsClicked { get; set; }
        public bool OnHover { get; set; }
        protected Rectangle rectangle;
        public Rectangle Rectangle
        {
            get { return rectangle; }
        }

        public UIWidget(Size size, Point location, bool visible) : this(location, visible)
        {
            rectangle.Size = size;
        }

        public UIWidget(Point location, bool visible)
        {
            Visible = visible;
            rectangle.Location = location;
        }

        public virtual void CheckHover(MouseEventArgs e)
        {
            if (Visible == true && rectangle.Contains(e.Location))
            {
                OnHover = true;
                return;
            }
            OnHover = false;
        }

        public virtual void CheckClick(MouseEventArgs e)
        {
            if (Visible == true && IsClicked == false && rectangle.Contains(e.Location))
            {
                IsClicked = true;
                return;
            }
            IsClicked = false;
        }

        public abstract void Paint(PaintEventArgs e);
    }
}
