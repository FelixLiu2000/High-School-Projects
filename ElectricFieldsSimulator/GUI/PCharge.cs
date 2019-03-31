using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Windows;

namespace PhysicsElectricFieldsSimulator
{
    class PCharge
    {
        readonly Brush POSITIVE = Brushes.Red;
        readonly Brush NEGATIVE = Brushes.Blue;
        public Vector Speed { get; set; }
        public Vector NetForce { get; private set; }
        public float Charge { get; set; }
        public float Mass { get; set; }
        public RectangleF Bounds { get; set; }
        public Brush Color { get; set; }
        public bool Active { get; set; }
        public bool Fixed { get; set; }
        public bool Positive
        {
            get
            {
                if (Charge >= 0)
                    return true;
                else
                    return false;
            }
        }

        public PCharge(bool active, bool fix, float charge, float mass, int posX, int posY, int width, int height)
        {
            InitializePCharge(active, fix, charge, mass, posX, posY, width, height);
        }

        void InitializePCharge(bool active, bool fix, float charge, float mass, int posX, int posY, int width, int height)
        {
            Charge = charge;
            Mass = mass;
            Active = active;
            Fixed = fix;
            Bounds = new RectangleF(posX, posY, width, height);

            if (Positive == true)
                Color = POSITIVE;
            else
                Color = NEGATIVE;
        }

        public void UpdateNetForce(Vector force)
        {
            NetForce = force;
        }

        public void Paint(PaintEventArgs e)
        {
            e.Graphics.FillEllipse(Color, Bounds);
        }
    }
}
