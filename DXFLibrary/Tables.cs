using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DXFLibrary
{
    public class Tables
    {
        private List<AppIDRecord> appids = new List<AppIDRecord>();
        [Table("APPID",typeof(DXFAppIDParser))]
        public List<AppIDRecord> AppIDs { get { return appids; } }

        private List<BlockRecord> blocks = new List<BlockRecord>();
        [Table("BLOCK_RECORD", typeof(DXFBlockRecordParser))]
        public List<BlockRecord> Blocks { get { return blocks; } }

        private List<DimStyleRecord> dimstyles = new List<DimStyleRecord>();
        [Table("DIMSTYLE", typeof(DXFDimStyleRecordParser))]
        public List<DimStyleRecord> DimStyles { get { return dimstyles; } }

        private List<LayerRecord> layers = new List<LayerRecord>();
        [Table("LAYER", typeof(DXFLayerRecordParser))]
        public List<LayerRecord> Layers { get { return layers; } }

        private List<LineTypeRecord> linetypes = new List<LineTypeRecord>();
        [Table("LTYPE", typeof(DXFLineTypeRecordParser))]
        public List<LineTypeRecord> LineTypes { get { return linetypes; } }

        private List<StyleRecord> styles = new List<StyleRecord>();
        [Table("STYLE", typeof(DXFStyleRecordParser))]
        public List<StyleRecord> Styles { get { return styles; } }

        private List<UCSRecord> ucs = new List<UCSRecord>();
        [Table("UCS", typeof(DXFUCSRecordParser))]
        public List<UCSRecord> UCS { get { return ucs; } }

        private List<DXFViewRecord> views = new List<DXFViewRecord>();
        [Table("VIEW", typeof(DXFViewRecordParser))]
        public List<DXFViewRecord> Views { get { return views; } }

        private List<VPortRecord> vports = new List<VPortRecord>();
        [Table("VPORT", typeof(DXFVPortRecordParser))]
        public List<VPortRecord> VPorts { get { return vports; } }
    }
}
