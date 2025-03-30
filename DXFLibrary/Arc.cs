using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DXFLibrary
{
    [Entity("ARC")]
    public class Arc : Entity
    {
        public double Thickness { get; set; }
        private Point center = new Point();
        public Point Center { get { return center; } }

        public double Radius { get; set; }

        public double StartAngle { get; set; }
        public double EndAngle { get; set; }

        private Point extrusion = new Point();
        public Point ExtrusionDirection { get { return extrusion; } }

        public override void ParseGroupCode(int groupcode, string value)
        {
            base.ParseGroupCode(groupcode, value);
            switch (groupcode)
            {
                case 39:
                    Thickness = double.Parse(value);
                    break;
                case 10:
                    Center.X = double.Parse(value);
                    break;
                case 20:
                    Center.Y = double.Parse(value);
                    break;
                case 30:
                    Center.Z = double.Parse(value);
                    break;
                case 40:
                    Radius = double.Parse(value);
                    break;
                case 50:
                    StartAngle = double.Parse(value);
                    break;
                case 51:
                    EndAngle = double.Parse(value);
                    break;
                case 210:
                    ExtrusionDirection.X = double.Parse(value);
                    break;
                case 220:
                    ExtrusionDirection.Y = double.Parse(value);
                    break;
                case 230:
                    ExtrusionDirection.Z = double.Parse(value);
                    break;
            }
        }
    }
}
