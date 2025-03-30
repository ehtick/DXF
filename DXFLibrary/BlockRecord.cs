using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DXFLibrary
{
    public class BlockRecord : Record
    {
        public string BlockName { get; set; }
    }

    class DXFBlockRecordParser : DXFRecordParser
    {
        private BlockRecord _currentRecord;
        protected override Record currentRecord
        {
            get { return _currentRecord; }
        }

        protected override void createRecord(Document doc)
        {
            _currentRecord = new BlockRecord();
            doc.Tables.Blocks.Add(_currentRecord);
        }

        public override void ParseGroupCode(Document doc, int groupcode, string value)
        {
            base.ParseGroupCode(doc, groupcode, value);
            if (groupcode == 2)
            {
                _currentRecord.BlockName = value;
            }
        }
    }

}
