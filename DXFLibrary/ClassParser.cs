﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DXFLibrary
{
    class ClassParser:ISectionParser
    {
        private Class currentClass;
        #region ISectionParser Member

        public void ParseGroupCode(Document doc, int groupcode, string value)
        {
            switch (groupcode)
            {
                case 0:
                    currentClass = new Class();
                    doc.Classes.Add(currentClass);
                    break;
                case 1:
                    currentClass.DXFRecord = value;
                    break;
                case 2:
                    currentClass.ClassName = value;
                    break;
                case 3:
                    currentClass.AppName = value;
                    break;
                case 90:
                    currentClass.ClassProxyCapabilities = (Class.Caps)Enum.Parse(typeof(Class.Caps), value);
                    break;
                case 280:
                    if (int.Parse(value) != 0)
                        currentClass.WasProxy = true;
                    else
                        currentClass.WasProxy = false;
                    break;
                case 281:
                    if (int.Parse(value) != 0)
                        currentClass.IsEntity = true;
                    else
                        currentClass.IsEntity = false;
                    break;
            }
        }

        #endregion
    }
}
