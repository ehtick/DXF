using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DXFLibrary
{
    public class AppIDRecord : Record
    {
        public string ApplicationName { get; set; }

    }

    class DXFAppIDParser : DXFRecordParser
    {
        #region ISectionParser Member

        public override void ParseGroupCode(Document doc, int groupcode, string value)
        {
            base.ParseGroupCode(doc, groupcode, value);
            switch (groupcode)
            {
                case 2:
                    _currentRecord.ApplicationName = value;
                    break;
            }
        }

        #endregion

        AppIDRecord _currentRecord;
        protected override Record currentRecord
        {
            get { return _currentRecord; }
        }

        protected override void createRecord(Document doc)
        {
            _currentRecord = new AppIDRecord();
            doc.Tables.AppIDs.Add(_currentRecord);
        }
    }

}
