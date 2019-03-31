/*
 * Felix Liu
 * January 16th, 2017
 * 
 * Contains methods and properties for button widgets; widgets with text or outlines used for completing a function when clicked.
 * 
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace BosenQuFelixLiuFinalCulminatingProject.GUI
{
    /// <summary>
    /// Subclass of Widget that creates and provides methods and properties for clickable rectangles with optional text, image, background color, and outline.
    /// </summary>
    class Button : Widget
    {
        // Properties

        // Gets/sets whether the button will change color when hovered over
        public bool AllowHover { get; set; }
        // Gets/sets the current color of the button when hovered over
        public Brush HoverColor { get; set; }
        // Gets/sets the pen used for drawing the button outline
        public Pen OutlinePen { get; set; }
        // Gets/sets the function of the button of type UIButtonFunctions
        public UIButtonFunctions ButtonFunction { get; set; }

        // Base constructor for button using an image, uses constructor from base class with same purpose. Constructor parameters have already been documented in the Widget parent class.
        public Button(UIButtonFunctions function, bool interact, bool visible, int startingX, int startingY, int width, int height, Image image, Brush hoverColor)
            : base(interact, visible, startingX, startingY, width, height, image)
        {
            // Set button function
            ButtonFunction = function;
            // Set the allow hover boolean to whether or not the hoverColor provided was null. If not null, true, if null, false
            AllowHover = (hoverColor != null) ? true : false;
            HoverColor = hoverColor;
        }

        // Base constructor for button using an image with text, uses constructor from base class with same purpose
        public Button(UIButtonFunctions function, bool interact, bool visible, int startingX, int startingY, int width, int height, Image image, string text, Font font, Brush textColor, StringFormat format, Brush hoverColor)
            : base(interact, visible, startingX, startingY, width, height, image, text, font, textColor, format)
        {
            // Already documented previously
            ButtonFunction = function;
            AllowHover = (hoverColor != null) ? true : false;
            HoverColor = hoverColor;
        }

        // Constructor for button using an image with text and an outline, derived from above constructor. Has additional parameters for drawing an outline around the button.
        // Parameters include outline color of type Brush and outline width of type float.
        public Button(UIButtonFunctions function, bool interact, bool visible, int startingX, int startingY, int width, int height, Image image, string text, Font font, Brush textColor, StringFormat format, Brush lineColor, float lineWidth, Brush hoverColor)
            : this(function, interact, visible, startingX, startingY, width, height, image, text, font, textColor, format, hoverColor)
        {
            // Instantiate the button's outline pen to the provided outline color and outline width.
            OutlinePen = new Pen(lineColor, lineWidth);
        }

        // Base constructor for button using a solid rectangle, uses constructor from base class with same purpose
        public Button(UIButtonFunctions function, bool interact, bool visible, int startingX, int startingY, int width, int height, Brush backColor, Brush hoverColor)
            : base(interact, visible, startingX, startingY, width, height, backColor)
        {
            // Already documented previously
            ButtonFunction = function;
            AllowHover = (hoverColor == null) ? true : false;
            HoverColor = hoverColor;
        }

        // Base constructor for button using an solid rectangle with text, uses constructor from base class with same purpose
        public Button(UIButtonFunctions function, bool interact, bool visible, int startingX, int startingY, int width, int height, Brush backColor, string text, Font font, Brush textColor, StringFormat format, Brush hoverColor)
            : base(interact, visible, startingX, startingY, width, height, backColor, text, font, textColor, format)
        {
            // Already documented previously
            ButtonFunction = function;
            AllowHover = (hoverColor == null) ? true : false;
            HoverColor = hoverColor;
        }

        // Base constructor for button using an solid rectangle with text and an outline, derived from above constructor
        public Button(UIButtonFunctions function, bool interact, bool visible, int startingX, int startingY, int width, int height, Brush backColor,
            string text, Font font, Brush textColor, StringFormat format, Brush lineColor, float lineWidth, Brush hoverColor)
            : this(function, interact, visible, startingX, startingY, width, height, backColor, text, font, textColor, format, hoverColor)
        {
            // Already documented previously
            OutlinePen = new Pen(lineColor, lineWidth);
        }

        // Already documented in Widget class, uses the base class's OnClick method
        public override void OnClick(MouseEventArgs e)
        {
            // Call base class's method
            base.OnClick(e);
        }

        // Already documented in Widget class, uses the base class's OnHover method, returns a bool value same of that of the base class
        public override bool OnHover(MouseEventArgs e)
        {
            // Call base class's method
            return base.OnHover(e);
        }

        // Already documented previously, uses the base class's OnHover method, returns a bool value same of that of the base class
        public override void Paint(PaintEventArgs e)
        {
            // If the button is visible
            if (Visible == true)
            {
                // If the widget needs an image (image was not provided as null)
                if (Image != null)
                    // Draw button image background with provided bounding box and image
                    e.Graphics.DrawImage(Image, BoundingBox);
                // If the widget needs a solid rectangle (rectangle was not provided as null)
                else if (BackColor != null)
                    // Draw button colored rectangle background with provided bounding box and color (brush)
                    e.Graphics.FillRectangle(BackColor, BoundingBox);
                // Render hover color over image or rectangle if mouse is being hovered over button
                if (Hover == true)
                    // Draw a translucent white overlay to lighten the button's surface when hovered, using the hover color brush and the button's bounding box.
                    e.Graphics.FillRectangle(HoverColor, BoundingBox);
                // If the widget needs text (text was not provided as null)
                if (Text != null)
                    // Draw the text using the provided text attributes
                    e.Graphics.DrawString(Text, TextFont, TextColor, BoundingBox, TextFormat);
                // If button is not interactable
                if (Interactable == false)
                    // Cover all previous elements with gray overlay
                    e.Graphics.FillRectangle(Brushes.Gray, BoundingBox);
            }

            // If the widget needs an outline (outline pen was not provided as null)
            if (OutlinePen != null)
                e.Graphics.DrawRectangle(OutlinePen, BoundingBox);
        }
    }
}