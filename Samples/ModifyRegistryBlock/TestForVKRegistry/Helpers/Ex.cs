using DrOpen.DrCommon.DrData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace TestForVKRegistry.Helpers
{
    public static class Ex
    {
        public static string XmlText(this DDNode node)
        {
            var formatter = new XmlSerializer(typeof(DDNode));

            using (var ms = new MemoryStream())
            {
                formatter.Serialize(ms, node);
                ms.Position = 0;
                return XDocument.Load(XmlReader.Create(ms)).ToString();
            }
        }
    }
}
