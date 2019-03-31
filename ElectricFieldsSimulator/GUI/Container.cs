/*
 * Contains methods and properties for container widgets; widgets with text and images or filled rectangles for design/information purposes.
 * 
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace PhysicsElectricFieldsSimulator
{
    /// <summary>
    /// Subclass of Widget that creates and provides methods and properties for rectangles with no function other than information or design.
    /// </summary>
    class Container : Widget
    {
        // Constructor for an image, uses base constructor with same purpose, already documented in widget class
        public Container(bool interact, bool visible, int startingX, int startingY, int width, int height, Image image)
            : base(interact, visible, startingX, startingY, width, height, image) { }

        // Constructor for a solid rectangle, uses base constructor with same purpose, already documented in widget class
        public Container(bool interact, bool visible, int startingX, int startingY, int width, int height, Brush backColor)
            : base(interact, visible, startingX, startingY, width, height, backColor) { }

        // Constructor for text, uses base constructor with same purpose, already documented in widget class
        public Container(bool interact, bool visible, int startingX, int startingY, int width, int height, string text, Font font, Brush textColor, StringFormat format)
            : base(interact, visible, startingX, startingY, width, height, text, font, textColor, format) { }
    }
}
