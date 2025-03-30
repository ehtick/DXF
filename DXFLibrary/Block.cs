using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DXFLibrary
{
    public class Block : Entity
    {
        public string BlockName { get; set; }
        public int BlockFlags { get; set; }
        private Point basePoint = new Point();
        public Point BasePoint { get { return basePoint; } }
        public string XRef { get; set; }

        public override void ParseGroupCode(int groupcode, string value)
        {
            base.ParseGroupCode(groupcode, value);
            switch (groupcode)
            {
                case 2:
                case 3:
                    BlockName = value;
                    break;
                case 70:
                    BlockFlags = int.Parse(value);
                    break;
                case 1:
                    XRef = value;
                    break;
                case 10:
                    BasePoint.X = double.Parse(value);
                    break;
                case 20:
                    basePoint.Y = double.Parse(value);
                    break;
                case 30:
                    basePoint.Z = double.Parse(value);
                    break;
            }
        }
    }
}
