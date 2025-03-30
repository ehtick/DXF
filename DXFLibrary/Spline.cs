using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DXFLibrary
{
    [Entity("SPLINE")]
    public class Spline : Entity
    {
        private Point normal = new Point();
        public Point Normal { get { return normal; } }

        [Flags]
        public enum FlagsEnum
        {
            Closed = 1,
            Periodic = 2,
            Rational = 4,
            Planar = 8,
            Linear = 16
        }
        public FlagsEnum Flags { get; set; }

        public int Degree { get; set; }
        public int KnotCount { get; set; }
        public int ControlPointCount { get; set; }
        public int FitPointCount { get; set; }

        public double KnotTolerance { get; set; }
        public double ControlPointTolerance { get; set; }
        public double FitPointTolerance { get; set; }

        private Point starttangent = new Point();
        public Point StartTangent { get { return starttangent; } }

        private Point endtangent = new Point();
        public Point EndTangent { get { return endtangent; } }

        private List<double> knotvalues = new List<double>();
        public List<double> KnotValues { get { return knotvalues; } }

        public double Weight { get; set; }

        private List<Point> controlpoints = new List<Point>();
        public List<Point> ControlPoints { get { return controlpoints; } }

        private List<Point> fitpoints = new List<Point>();
        public List<Point> FitPoints { get { return fitpoints; } }

        private Point LastControlPoint
        {
            get
            {
                if (ControlPoints.Count == 0) return null;
                return ControlPoints[ControlPoints.Count - 1];
            }
            set
            {
                ControlPoints.Add(value);
            }
        }

        private Point LastFitPoint
        {
            get
            {
                if (FitPoints.Count == 0) return null;
                return FitPoints[FitPoints.Count - 1];
            }
            set
            {
                FitPoints.Add(value);
            }
        }

        public override void ParseGroupCode(int groupcode, string value)
        {
            base.ParseGroupCode(groupcode, value);
            switch (groupcode)
            {
                case 210:
                    Normal.X = double.Parse(value);
                    break;
                case 220:
                    Normal.Y = double.Parse(value);
                    break;
                case 230:
                    Normal.Z = double.Parse(value);
                    break;
                case 70:
                    Flags = (FlagsEnum)Enum.Parse(typeof(FlagsEnum), value);
                    break;
                case 71:
                    Degree = int.Parse(value);
                    break;
                case 72:
                    KnotCount = int.Parse(value);
                    break;
                case 73:
                    ControlPointCount = int.Parse(value);
                    break;
                case 74:
                    FitPointCount = int.Parse(value);
                    break;
                case 42:
                    KnotTolerance = double.Parse(value);
                    break;
                case 43:
                    ControlPointTolerance = double.Parse(value);
                    break;
                case 44:
                    FitPointTolerance = double.Parse(value);
                    break;
                case 12:
                    StartTangent.X = double.Parse(value);
                    break;
                case 22:
                    StartTangent.Y = double.Parse(value);
                    break;
                case 32:
                    StartTangent.Z = double.Parse(value);
                    break;
                case 13:
                    EndTangent.X = double.Parse(value);
                    break;
                case 23:
                    EndTangent.Y = double.Parse(value);
                    break;
                case 33:
                    EndTangent.Z = double.Parse(value);
                    break;
                case 40:
                    KnotValues.Add(double.Parse(value));
                    break;
                case 41:
                    Weight = double.Parse(value);
                    break;
                case 10:
                    if (LastControlPoint==null || LastControlPoint.X != null)
                        LastControlPoint = new Point();
                    LastControlPoint.X = double.Parse(value);
                    break;
                case 20:
                    if (LastControlPoint == null || LastControlPoint.Y != null)
                        LastControlPoint = new Point();
                    LastControlPoint.Y = double.Parse(value);
                    break;
                case 30:
                    if (LastControlPoint == null || LastControlPoint.Z != null)
                        LastControlPoint = new Point();
                    LastControlPoint.Z = double.Parse(value);
                    break;
                case 11:
                    if (LastFitPoint == null || LastFitPoint.X != null)
                        LastFitPoint = new Point();
                    LastFitPoint.X = double.Parse(value);
                    break;
                case 21:
                    if (LastFitPoint == null || LastFitPoint.Y != null)
                        LastFitPoint = new Point();
                    LastFitPoint.Y = double.Parse(value);
                    break;
                case 31:
                    if (LastFitPoint == null || LastFitPoint.Z != null)
                        LastFitPoint = new Point();
                    LastFitPoint.Z = double.Parse(value);
                    break;
            }
        }
    }
}
