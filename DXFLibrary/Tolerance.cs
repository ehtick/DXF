using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DXFLibrary
{
    [Entity("TOLERANCE")]
    public class Tolerance : Entity
    {
        public string DimensionStyle { get; set; }
        private Point insertion = new Point();
        public Point InsertionPoint { get { return insertion; } }
        private Point extrusion = new Point();
        public Point ExtrusionDirection { get { return extrusion; } }
        private Point direction = new Point();
        public Point Direction { get { return direction; } }

        public override void ParseGroupCode(int groupcode, string value)
        {
            base.ParseGroupCode(groupcode, value);
            switch (groupcode)
            {
                case 3:
                    DimensionStyle = value;
                    break;
                case 10:
                    InsertionPoint.X = double.Parse(value);
                    break;
                case 20:
                    InsertionPoint.Y = double.Parse(value);
                    break;
                case 30:
                    InsertionPoint.Z = double.Parse(value);
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
