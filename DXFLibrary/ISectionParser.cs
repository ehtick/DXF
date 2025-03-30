using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DXFLibrary
{
    interface ISectionParser
    {
        void ParseGroupCode(Document doc, int groupcode, string value);
    }
}
