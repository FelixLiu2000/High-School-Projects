using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace _2_Assignment_Blackjack.GUI
{
    class UIPictureBox : UIWidget
    {
        Image image;

        public UIPictureBox(Size size, Point location, Image image, bool visible) : base(size, location, visible)
        {
            this.image = image;
        }

        public override void Paint(PaintEventArgs e)
        {
            if (Visible == true)
            {
                e.Graphics.DrawImage(image, rectangle);
            }
        }
    }
}
