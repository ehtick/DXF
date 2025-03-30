using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DXFLibrary
{
    public class LayerRecord : Record
    {
        public string LayerName { get; set; }
        public int Color { get; set; }
        public string LineType { get; set; }
    }

    class DXFLayerRecordParser : DXFRecordParser
    {
        private LayerRecord _record;
        protected override Record currentRecord
        {
            get { return _record; }
        }

        protected override void createRecord(Document doc)
        {
            _record = new LayerRecord();
            doc.Tables.Layers.Add(_record);
        }

        public override void ParseGroupCode(Document doc, int groupcode, string value)
        {
            base.ParseGroupCode(doc, groupcode, value);
            switch (groupcode)
            {
                case 2:
                    _record.LayerName = value;
                    break;
                case 62:
                    _record.Color = int.Parse(value);
                    break;
                case 6:
                    _record.LineType = value;
                    break;
            }
        }
    }



}
