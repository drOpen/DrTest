using DrOpen.DrCommon.DrData;
using DrOpen.DrCommon.DrLog.DrLogClient;
using DrOpen.DrTest.DrTAHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace DrTAHelperSample
{
    public class TASample : TAHelper
    {

        public void CheckFiles(DDNode n)
        {
            int iFail = 0;

            foreach (var nFile in n)
            {
                if (checkFile(nFile.Value) == false) iFail ++;
            }
            if (iFail > 0) throw new DrTAFailedException("There are '{0}' failed files.", iFail.ToString());
        }

        public void CheckFile(DDNode n)
        {
            if (checkFile(n) == false) throw new DrTAFailedException ("File '{0}' is filed.", n.Attributes[TASampleSchema.AttrFile]);
        }

        private bool checkFile(DDNode n)
        {
            n.Attributes.ContainsAttributesOtherwiseThrow(TASampleSchema.AttrFile);

            var fileName = n.Attributes[TASampleSchema.AttrFile];
            var exp = n.Attributes.GetValue(TASampleSchema.AttrExpected, true);

            log.WriteTrace("Does file '{0}' exist?. Expected '{1}'.", fileName, exp);
            var result = File.Exists(fileName);
            log.Write((result == exp ? LogLevel.INF : LogLevel.ERR), "File '{0}' {1}. Expected '{2}'.", fileName,  (result ? "exists" : "doesn't exist")  , exp);

            return (result == exp);
        }

    }
}
