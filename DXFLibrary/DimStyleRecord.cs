using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DXFLibrary
{
    public class DimStyleRecord : Record
    {
        public string StyleName { get; set; }
        //TODO: weitere Felder unterstützen
    }

    class DXFDimStyleRecordParser : DXFRecordParser
    {
        private DimStyleRecord _record;
        protected override Record currentRecord
        {
            get { return _record; }
        }

        protected override void createRecord(Document doc)
        {
            _record = new DimStyleRecord();
            doc.Tables.DimStyles.Add(_record);
        }

        public override void ParseGroupCode(Document doc, int groupcode, string value)
        {
            base.ParseGroupCode(doc, groupcode, value);
            if (groupcode == 2)
                _record.StyleName = value;
        }
    }

}
