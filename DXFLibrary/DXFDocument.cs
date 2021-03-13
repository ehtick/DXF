﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Globalization;

namespace DXFLib
{
    public class DXFDocument
    {
        #region Variables

        private DXFHeader header = new DXFHeader();
        private List<DXFClass> classes = new List<DXFClass>();
        private List<DXFBlock> blocks = new List<DXFBlock>();
        private DXFTables tables = new DXFTables();
        private List<DXFEntity> entities = new List<DXFEntity>();

        #endregion
        #region Constructors

        public DXFDocument()
        {

        }
        #endregion
        #region Properties
        public DXFHeader Header { get { return header; } }
        public List<DXFClass> Classes { get { return classes; } }
        public List<DXFBlock> Blocks { get { return blocks; } }
        public DXFTables Tables { get { return tables; } }
        public List<DXFEntity> Entities { get { return entities; } }
        private enum LoadState
        {
            OutsideSection,
            InSection
        }
        #endregion
        #region Envents
        public event EventHandler OnFileVersionIncompatible;
        #endregion
        #region Methods

        public void Load(string filename)
        {
            FileStream stream = new FileStream(filename, FileMode.Open);
            Load(stream);
            stream.Close();
        }

        public void Load(Stream file)
        {
            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
            bool versionwarningsent = false;

            Dictionary<string, ISectionParser> sectionparsers = new Dictionary<string, ISectionParser>();

            sectionparsers["HEADER"] = new HeaderParser();
            sectionparsers["CLASSES"] = new ClassParser();
            sectionparsers["TABLES"] = new TableParser();
            sectionparsers["ENTITIES"] = new EntityParser();
            sectionparsers["BLOCKS"] = new BlockParser();
            ISectionParser currentParser = null;

            TextReader reader = new StreamReader(file);
            LoadState state = LoadState.OutsideSection;
            int? groupcode;
            string value;
            reader.ReadDXFEntry(out groupcode, out value);
            while (groupcode != null)
            {
                switch (state)
                {
                    case LoadState.OutsideSection:
                        if (groupcode == 0 && value.Trim() == "SECTION")
                        {
                            state = LoadState.InSection;
                            reader.ReadDXFEntry(out groupcode, out value);
                            if (groupcode != 2)
                                throw new Exception("Sektion gefunden aber keinen Namen zur Sektion");
                            value = value.Trim();
                            if (sectionparsers.ContainsKey(value))
                                currentParser = sectionparsers[value];
                        }
                        break;
                    case LoadState.InSection:
                        if (groupcode == 0 && value.Trim() == "ENDSEC")
                        {
                            state = LoadState.OutsideSection;
                            //after each section check wether the File Version is set
                            if (Header.AutoCADVersion != null &&
                                Header.AutoCADVersion != "AC1014")
                            {
                                if (!versionwarningsent)
                                {
                                    try
                                    {
                                        if (OnFileVersionIncompatible != null)
                                            OnFileVersionIncompatible(this, EventArgs.Empty);

                                    }
                                    catch (Exception)
                                    {

                                    } 
                                    versionwarningsent = true;
                                }
                            }
                        }
                        else
                        {
                            if (currentParser != null)
                            {
                                currentParser.ParseGroupCode(this, (int)groupcode, value);
                            }
                        }
                        break;
                    default:
                        break;
                }
                reader.ReadDXFEntry(out groupcode, out value);
            }
            if (state == LoadState.InSection)
                throw new Exception("Dateiende erreicht aber immer noch offene Sektion vorhanden");
            Thread.CurrentThread.CurrentCulture = currentCulture;
        }
        #endregion
    }
}
