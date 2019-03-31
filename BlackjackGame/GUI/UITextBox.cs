using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace _2_Assignment_Blackjack.GUI
{
    class UITextBox : UIWidget
    {
        public string Text { get; set; }
        Font font;
        SolidBrush textBrush;

        public UITextBox(Point location, string text, Font font, Color color, bool visible) : base(location, visible)
        {
            this.Text = text;
            this.font = font;
            textBrush = new SolidBrush(color);
        }

        public override void Paint(PaintEventArgs e)
        {
            if (Visible == true)
            {
                rectangle.Size = Size.Ceiling(e.Graphics.MeasureString(Text, font));
                e.Graphics.DrawString(Text, font, textBrush, rectangle);
            }
        }
    }
}
