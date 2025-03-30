using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DXFLibrary
{
    public class UCSRecord : Record
    {
        public string UCSName { get; set; }
        private Point origin = new Point();
        public Point Origin { get { return origin; } }
        private Point xaxis=new Point();
        public Point XAxis { get { return xaxis; } }
        private Point yaxis = new Point();
        public Point YAxis { get { return yaxis; } }
    }

    class DXFUCSRecordParser : DXFRecordParser
    {
        private UCSRecord _record;
        protected override Record currentRecord
        {
            get { return _record; }
        }

        protected override void createRecord(Document doc)
        {
            _record = new UCSRecord();
            doc.Tables.UCS.Add(_record);
        }

        public override void ParseGroupCode(Document doc, int groupcode, string value)
        {
            base.ParseGroupCode(doc, groupcode, value);
            switch (groupcode)
            {
                case 2:
                    _record.UCSName = value;
                    break;
                case 10:
                    _record.Origin.X = double.Parse(value);
                    break;
                case 20:
                    _record.Origin.Y = double.Parse(value);
                    break;
                case 30:
                    _record.Origin.Z = double.Parse(value);
                    break;
                case 11:
                    _record.XAxis.X = double.Parse(value);
                    break;
                case 21:
                    _record.XAxis.Y = double.Parse(value);
                    break;
                case 31:
                    _record.XAxis.Z = double.Parse(value);
                    break;
                case 12:
                    _record.YAxis.X = double.Parse(value);
                    break;
                case 22:
                    _record.YAxis.Y = double.Parse(value);
                    break;
                case 32:
                    _record.YAxis.Z = double.Parse(value);
                    break;
            }
        }
    }

}
