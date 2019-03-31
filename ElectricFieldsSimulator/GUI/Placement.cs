using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PhysicsElectricFieldsSimulator.GUI
{
    class Placement
    {
        public bool Active { get; set; }
        public PCharge Charge { get; set; }

        public Placement(bool active, PCharge charge)
        {
            Charge = charge;
            Active = active;
        }

        public PCharge MouseMovement(MouseEventArgs e)
        {
            Charge.Bounds = new System.Drawing.RectangleF(e.Location.X - Charge.Bounds.Width / 2, e.Location.Y - Charge.Bounds.Height / 2, Charge.Bounds.Width, Charge.Bounds.Height);
            return Charge;
        }

        public PCharge MouseClick(MouseEventArgs e, bool illegalLocation)
        {
            if (illegalLocation == false)
            {
                Charge.Active = true;
                Active = false;
            }
            return Charge;
        }
    }
}
