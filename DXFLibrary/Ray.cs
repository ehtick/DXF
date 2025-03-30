using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DXFLibrary
{
    [Entity("RAY")]
    public class Ray : Entity
    {
        private Point startpoint = new Point();
        public Point Start { get { return startpoint; } }

        private Point direction = new Point();
        public Point Direction { get { return direction; } }

        public override void ParseGroupCode(int groupcode, string value)
        {
            base.ParseGroupCode(groupcode, value);
            switch (groupcode)
            {
                case 10:
                    Start.X = double.Parse(value);
                    break;
                case 20:
                    Start.Y = double.Parse(value);
                    break;
                case 30:
                    Start.Z = double.Parse(value);
                    break;
                case 11:
                    Direction.X = double.Parse(value);
                    break;
                case 21:
                    Direction.Y = double.Parse(value);
                    break;
                case 31:
                    Direction.Z = double.Parse(value);
                    break;
            }
        }
    }

    [Entity("XLINE")]
    public class DXFXLine : Ray
    {
    }
}
