using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DXFLibrary
{
    [Entity("SOLID")]
    public class Solid : Entity
    {
        private Point extrusion = new Point();
        public Point ExtrusionDirection { get { return extrusion; } }
        private Point[] corners = new Point[] { new Point(), new Point(), new Point(),new Point() };
        public Point[] Corners { get { return corners; } }

        public override void ParseGroupCode(int groupcode, string value)
        {
            base.ParseGroupCode(groupcode, value);
            if (groupcode >= 10 && groupcode <= 33)
            {
                int idx = groupcode % 10;
                int component = groupcode / 10;
                switch (component)
                {
                    case 1:
                        Corners[idx].X = double.Parse(value);
                        break;
                    case 2:
                        Corners[idx].Y = double.Parse(value);
                        break;
                    case 3:
                        Corners[idx].Z = double.Parse(value);
                        break;
                }
            }

            switch (groupcode)
            {
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
