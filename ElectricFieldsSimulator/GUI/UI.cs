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
    /// Stores indices of simulator widget instances in the UI's sim Widget array.
    /// </summary>
    static class UiWidgets
    {
        // Total number of widgets
        public static int numOfWidgets = 0;
    }

    /// <summary>
    /// Renders, declares, and manages all user interface related instances .
    /// Controls state and layout of UI and provides methods for dependant classes to access events.
    /// </summary>
    class UI
    {
        // Properties, fields and object declarations

        public Widget[] chargeInfo = new Widget[UiWidgets.numOfWidgets];
        public Widget[] ruler = new Widget[(Main.size.Height / 10) - 1];
        // SolidBrush brush that lights up a widget when hovered using a translucent white overlay
        SolidBrush hoverBrush = new SolidBrush(Color.FromArgb(25, Color.White));

        // UI constructor, takes the current state of the UI as its parameter
        public UI()
        {
            // Initialize UI based on the current UI state
            InitializeUI();
        }

        // Initializes the UI by calling procedures based on the current UI state. No return, parameter for passing in the selected UI state of type UIState.
        void InitializeUI()
        {
            for (int i = 0; i < ruler.Length; i++)
            {
                Font font = new Font("Arial", 8);
                StringFormat stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Near;
                ruler[i] = new Container(false, true, Main.size.Width - 50, i * 10, 50, 20, "--- " + (i * 10).ToString(), font, Brushes.Black, stringFormat);
            }
        }

        // Determines the current function of the button that was pressed. No return, takes in the function of a button of type UIButtonFunctions
        void ButtonFunctions()
        {
        }


        // Timer update method, runs every program cycle. no return value.
        public void Update(PCharge[] charges)
        {
            for (int i = 0; i < charges.Length; i++)
            {
                Font font = new Font("Arial", 8);
                StringFormat stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Center;
                chargeInfo[i] = new Container(false, true, (int)charges[i].Bounds.Location.X, (int)charges[i].Bounds.Location.Y + (int)charges[i].Bounds.Height / 4, (int)charges[i].Bounds.Width, (int)charges[i].Bounds.Height, Math.Round(charges[i].NetForce.Length, 3).ToString() + " N", font, Brushes.Black, stringFormat);
            }
        }

        // Paints UI widgets, background, and placement towers on refresh. Takes the PaintEventArgs instance parameter of the Main class OnPaint method as its parameter, no return value.
        public void Paint(PaintEventArgs e)
        {
            // Loop through and paint all widgets in the startWidget array
            for (int currentWidget = 0; currentWidget < UiWidgets.numOfWidgets; currentWidget++)
            {
                if (chargeInfo[currentWidget] != null)
                {
                    chargeInfo[currentWidget].Paint(e);
                }
            }
            foreach (Widget increment in ruler)
            {
                increment.Paint(e);
            }
        }

        public void AddChargeInfoWidget()
        {
            Widget[] tempWidgets = new Widget[UiWidgets.numOfWidgets];
            tempWidgets = chargeInfo;
            UiWidgets.numOfWidgets++;
            chargeInfo = new Widget[UiWidgets.numOfWidgets];

            for (int i = 0; i < tempWidgets.Length; i++)
            {
                chargeInfo[i] = tempWidgets[i];
            }
        }

        public void RemoveChargeInfoWidget(int i)
        {
            Widget[] tempWidgets = new Widget[UiWidgets.numOfWidgets];
            tempWidgets = chargeInfo;
            UiWidgets.numOfWidgets--;
            chargeInfo = new Widget[UiWidgets.numOfWidgets];
            for (int j = 0; j < i; j++)
            {
                chargeInfo[j] = tempWidgets[j];
            }
            for (int j = i; j < chargeInfo.Length; j++)
            {
                chargeInfo[j] = tempWidgets[j + 1];
            }
        }

        // Mouse click method that triggers when the mouse is clicked, takes the MouseEventArgs instance from the MainClass mouse click event
        public void OnClick(MouseEventArgs e)
        {
            
        }
    }
}
