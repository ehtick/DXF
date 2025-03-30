using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DXFLibrary
{
    [Entity("INSERT")]
    public class Insert : Entity
    {
        public string BlockName { get; set; }
        private Point insertionPoint = new Point();
        public Point InsertionPoint { get { return insertionPoint; } }
        private Point scaling = new Point();
        public Point Scaling { get { return scaling; } }
        public double? RotationAngle { get; set; }
        private Point extrusionDirection = new Point();
        public Point ExtrusionDirection { get { return extrusionDirection; } }

        public override void ParseGroupCode(int groupcode, string value)
        {
            base.ParseGroupCode(groupcode, value);
            switch (groupcode)
            {
                case 2:
                    BlockName = value;
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
                case 41:
                    Scaling.X = double.Parse(value);
                    break;
                case 42:
                    Scaling.Y = double.Parse(value);
                    break;
                case 43:
                    Scaling.Z = double.Parse(value);
                    break;
            }
        }
    }
}
