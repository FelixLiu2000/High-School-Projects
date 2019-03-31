/*
 * Felix Liu
 * January 14th, 2017
 * 
 * Contains methods and properties for widgets; rectangles with information or for design purposes.
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
    /// Abstract type that provides methods and properties for UI design element types to inherit.
    /// Subclasses include Button and Container.
    /// </summary>
    abstract class Widget
    {
        // Properties

        // Gets/sets widget bounding box
        public Rectangle BoundingBox { get; set; }
        // Gets/sets whether widget is interactable (can be clicked)
        public bool Interactable { get; set; }
        // Gets/sets whether widget is visible (can be seen)
        public bool Visible { get; set; }
        // Gets/sets whether widget is currently being clicked
        public bool Clicked { get; set; }
        // Gets/sets whether widget is currently being hovered over
        public bool Hover { get; set; }
        // Gets/sets widget text
        public string Text { get; set; }
        // Gets/sets widget text font
        public Font TextFont { get; set; }
        // Gets/sets widget text color
        public Brush TextColor { get; set; }
        // Gets/sets widget text formatting
        public StringFormat TextFormat { get; set; }
        // Gets/sets widget background color
        public Brush BackColor { get; set; }
        // Gets/sets widget background image
        public Image Image { get; set; }

        // Base constructor for widgets with solid rectangles. Takes its interactability (bool), visibility (bool), x and y location (int), width and height (int) as parameters.
        public Widget(bool interact, bool visible, int startingX, int startingY, int width, int height, Brush backColor)
        {
            // Store/initialize physical widget attribute parameters in their respective properties
            InitializeWidget(interact, visible, startingX, startingY, width, height, backColor);
        }

        // Base constructor for widgets with images, same parameters as above but with background image (image) as a parameter instead of color.
        public Widget(bool interact, bool visible, int startingX, int startingY, int width, int height, Image image)
        {
            // Store/initialize physical attribute parameters in their respective properties
            InitializeWidget(interact, visible, startingX, startingY, width, height, image);
        }

        // Base constructor for widgets with just text (no background). Same parameters as above but without image or color, and text properties instead.
        // Text property parameters: text font (font), text color (brush), and text format (stringformat).
        public Widget(bool interact, bool visible, int startingX, int startingY, int width, int height, string text, Font font, Brush textColor, StringFormat format)
        {
            // Store/initialize physical and text widget attribute parameters in their respective properties
            InitializeWidget(interact, visible, startingX, startingY, width, height);
            InitializeText(text, font, textColor, format);
        }

        // Constructor for widgets with images and text, derived from image base constructor. Same parameters as derived constructor,
        // but with additional text properties identical to the text only constructor.
        public Widget(bool interact, bool visible, int startingX, int startingY, int width, int height, Image image, string text, Font font, Brush textColor, StringFormat format)
            : this(interact, visible, startingX, startingY, width, height, image)
        {
            // Store/initialize text widget attribute parameters in their respective properties
            InitializeText(text, font, textColor, format);
        }

        // Constructor for widgets with solid rectangles and text, derived from solid rectangle base constructor. Same parameters as derived constructor,
        // but with additional text properties identical to the text only constructor.
        public Widget(bool interact, bool visible, int startingX, int startingY, int width, int height, Brush backColor, string text, Font font, Brush textColor, StringFormat format)
            : this(interact, visible, startingX, startingY, width, height, backColor)
        {
            // Store/initialize text widget attribute parameters in their respective properties
            InitializeText(text, font, textColor, format);
        }

        // Overridable method to initialize widget, overload for a widget with no solid rectangle or image, takes physical attributes from a constructor as its parameters (parameter functions outlined in constructors), no return value
        public virtual void InitializeWidget(bool interact, bool visible, int startingX, int startingY, int width, int height)
        {
            // Instantiate the bounding box with the attribute parameters
            BoundingBox = new Rectangle(startingX, startingY, width, height);
            // Set the visibility and interactability as their respective parameters
            Interactable = interact;
            Visible = visible;
        }

        // Overridable method to initialize widget, overload for solid rectangles, takes physical attributes from a constructor as its parameters (parameter functions outlined in constructors), no return value
        public virtual void InitializeWidget(bool interact, bool visible, int startingX, int startingY, int width, int height, Brush backColor)
        {
            // Instantiate the bounding box with the attribute parameters
            BoundingBox = new Rectangle(startingX, startingY, width, height);
            // Set the visibility, interactability, and background color as their respective parameters
            BackColor = backColor;
            Interactable = interact;
            Visible = visible;
        }

        // Overridable method to initialize widget, overload for images, takes physical attributes from a constructor as its parameters (parameter functions outlined in constructors), no return value.
        public virtual void InitializeWidget(bool interact, bool visible, int startingX, int startingY, int width, int height, Image image)
        {
            // Instantiate the bounding box with the attribute parameters
            BoundingBox = new Rectangle(startingX, startingY, width, height);
            // Set the visibility, interactability, and background image as their respective parameters
            Image = image;
            Interactable = interact;
            Visible = visible;
        }

        // Overridable method to initialize widget text, takes text attributes from a constructor as its parameters (parameter functions outlined in constructors), no return value.
        public virtual void InitializeText(string text, Font font, Brush textColor, StringFormat format)
        {
            // Set the widget text, font, format, and color as their respective parameters
            Text = text;
            TextFont = font;
            TextFormat = format;
            TextColor = textColor;
        }

        // Overridable method for setting whether or not the widget was clicked when the mouse is clicked. Takes the main class MouseClick parameter of type MouseEventArgs
        // provided by the UI class as its parameter, no return value.
        public virtual void OnClick(MouseEventArgs e)
        {
            // If the widget is interactable
            if (Interactable == true)
                // Set the boolean as true or false based on if the mouse is within the widget
                Clicked = (BoundingBox.Contains(e.Location)) ? true : false;
            // Not interactable
            else
                // Ensure clicked is false
                Clicked = false;
        }

        // Overridable method for setting/checking whether the mouse is hovering over the widget, triggered by mouse movement. Takes the main class MouseMove parameter of type MouseEventArgs
        // provided by the UI class as its parameter. Boolean return value returns true to call a refresh of the form when the hover status of the widget has changed.
        public virtual bool OnHover(MouseEventArgs e)
        {
            // If the mouse is within the widget
            if (BoundingBox.Contains(e.Location))
            {
                // If the widget is already being hovered over
                if (Hover == true)
                    // Return false, no refresh
                    return false;
                // Was not being hovered over previously
                else
                {
                    // Set the hover property as true
                    Hover = true;
                    // Return true to trigger a refresh of the form
                    return true;
                }
            }
            // If the mouse is not hovering over the widget
            else
            {
                // If the hover property is already false
                if (Hover == false)
                    // Return false, no refresh
                    return false;
                // If the hover property was not previously false
                else
                {
                    // Set the property to false
                    Hover = false;
                    // Return true to trigger a refresh
                    return true;
                }
            }
        }

        // Overridable method for painting/drawing widgets, takes the main class OnPaint parameter of type PaintEventArgs provided by the UI class as its parameter, no return value
        public virtual void Paint(PaintEventArgs e)
        {
            // If the widget is visible
            if (Visible == true)
            {
                // If the widget needs an image
                if (Image != null)
                    // Draw the widgets image within its bounding box
                    e.Graphics.DrawImage(Image, BoundingBox);
                // Otherwise if the widget needs a solid rectangle
                else if (BackColor != null)
                    // Draw the widgets background colored rectangle within its bounding box
                    e.Graphics.FillRectangle(BackColor, BoundingBox);

                // If the widget needs text
                if (Text != null)
                    // Draw the widgets text with its text attributes
                    e.Graphics.DrawString(Text, TextFont, TextColor, BoundingBox, TextFormat);
            }
        }
    }
}
