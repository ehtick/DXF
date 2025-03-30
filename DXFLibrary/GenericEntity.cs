using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DXFLibrary
{
    public class GenericEntity : Entity
    {
        public class Entry
        {
            public int GroupCode { get; set; }
            public string Value { get; set; }
            public Entry()
            {
            }

            public Entry(int g, string v)
            {
                GroupCode = g;
                Value = v;
            }
        }

        private List<Entry> entries = new List<Entry>();
        public List<Entry> Entries { get { return entries; } }

        public override void ParseGroupCode(int groupcode, string value)
        {
            base.ParseGroupCode(groupcode, value);
            Entries.Add(new Entry(groupcode, value));
        }
    }

    [Entity("3DSOLID")]
    public class DXF3DSolid : GenericEntity
    {
    }

    [Entity("ACAD_PROXY_ENTITY")]
    public class DXF3DAcadProxy : GenericEntity
    {
    }

    [Entity("ATTDEF")]
    public class DXFAttributeDefinition : GenericEntity
    {
    }

    [Entity("ATTRIB")]
    public class DXFAttribute : GenericEntity
    {
    }

    [Entity("BODY")]
    public class DXFBody : GenericEntity
    {
    }

    [Entity("DIMENSION")]
    public class DXFDimension : GenericEntity
    {
    }

    [Entity("HATCH")]
    public class DXFHatch : GenericEntity
    {
    }

    [Entity("IMAGE")]
    public class DXFImage : GenericEntity
    {
    }

    [Entity("LEADER")]
    public class DXFLeader : GenericEntity
    {
    }

    [Entity("MLINE")]
    public class DXFMLine : GenericEntity
    {
    }

    [Entity("MTEXT")]
    public class DXFMText : GenericEntity
    {
    }

    [Entity("OLEFRAME")]
    public class DXFOleFrame : GenericEntity
    {
    }

    [Entity("OLE2FRAME")]
    public class DXFOle2Frame : GenericEntity
    {
    }

    [Entity("REGION")]
    public class DXFRegion : GenericEntity
    {
    }

    [Entity("TEXT")]
    public class DXFText : GenericEntity
    {
    }

    [Entity("VIEWPORT")]
    public class DXFViewPort : GenericEntity
    {
    }

}
