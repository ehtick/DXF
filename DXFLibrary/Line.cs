using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DXFLibrary
{
    [Entity("LINE")]
    public class Line : Entity
    {
        private Point start = new Point();
        public Point Start { get { return start; } }

        private Point end = new Point();
        public Point End { get { return end; } }

        public double Thickness { get; set; }

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
                    Start.X = double.Parse(value);
                    break;
                case 20:
                    Start.Y = double.Parse(value);
                    break;
                case 30:
                    Start.Z = double.Parse(value);
                    break;
                case 11:
                    End.X = double.Parse(value);
                    break;
                case 21:
                    End.Y = double.Parse(value);
                    break;
                case 31:
                    End.Z = double.Parse(value);
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
