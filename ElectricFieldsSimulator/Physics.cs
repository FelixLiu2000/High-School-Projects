using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PhysicsElectricFieldsSimulator
{
    class Physics
    {
        public int TimeStep { get; set; }
        public Physics(int constant)
        {
            TimeStep = constant;
        }

        public PCharge[] ApplyEForce(PCharge[] charges)
        {
            for (int a = 0; a < charges.Length; a++)
            {
                if (charges[a].Active == true)
                {
                    Vector netForce = new Vector(0, 0);
                    for (int b = 0; b < charges.Length; b++)
                    {
                        if (charges[a] != charges[b] && charges[b].Active == true)
                        {
                            float distanceSquared = (((charges[b].Bounds.Location.X - charges[a].Bounds.Location.X) * (charges[b].Bounds.Location.X - charges[a].Bounds.Location.X)) + ((charges[b].Bounds.Location.Y - charges[a].Bounds.Location.Y) * (charges[b].Bounds.Location.Y - charges[a].Bounds.Location.Y)));
                            float force = CalculateEForce(charges[a].Charge, charges[b].Charge, distanceSquared);
                            Vector forceDirection = new Vector((charges[b].Bounds.Location.X - charges[a].Bounds.Location.X), (charges[b].Bounds.Location.Y - charges[a].Bounds.Location.Y));
                            forceDirection.Normalize();
                            netForce += force * forceDirection;
                        }
                    }
                    charges[a].Speed += Acceleration(netForce, charges[a].Mass);
                    if (charges[a].Fixed == true)
                        charges[a].Speed = new Vector(0, 0);
                    Vector adjustedSpeed = charges[a].Speed * Main.deltaTime;
                    
                    charges[a].Bounds = new System.Drawing.RectangleF((float)(charges[a].Bounds.Location.X + adjustedSpeed.X), (float)(charges[a].Bounds.Location.Y + adjustedSpeed.Y), charges[a].Bounds.Width, charges[a].Bounds.Height);

                    charges[a] = ResolveCollisions(charges[a], charges, adjustedSpeed);

                    charges[a].UpdateNetForce(netForce);
                }
            }
            return charges;
        }

        PCharge ResolveCollisions(PCharge chargeA, PCharge[] charges, Vector previousSpeed)
        {
            for (int i = 0; i < charges.Length; i++)
            {
                if (chargeA != charges[i] && chargeA.Bounds.IntersectsWith(charges[i].Bounds) && charges[i].Active == true)
                {
                    chargeA.Speed = new Vector(0, 0);
                    chargeA.Bounds = new System.Drawing.RectangleF((float)(chargeA.Bounds.Location.X - previousSpeed.X), (float)(chargeA.Bounds.Location.Y - previousSpeed.Y), chargeA.Bounds.Width, chargeA.Bounds.Height);
                    break;
                }
            }
            return chargeA;
        }

        float CalculateEForce(float chargeA, float chargeB, float distance)
        {
            // Coulomb's law using a custom proportionality constant that controls the timestep, rather than Coulomb's constant
            return -((TimeStep * chargeA * chargeB) / distance);
        }
        Vector Acceleration(Vector force, float mass)
        {
            // Newton's second law
            return force / mass;
        }
    }
}
