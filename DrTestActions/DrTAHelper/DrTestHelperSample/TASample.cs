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

        public DDNode CheckFiles(DDNode n)
        {
            int iFail = 0;

            foreach (var nFile in n)
            {
                if (checkFile(nFile.Value) == false) iFail ++;
            }
            if (iFail > 0) SetTestFailed("There are '{0}' failed files.", iFail.ToString());
            return this.OutResult;

        }

        public DDNode CheckFile(DDNode n)
        {
            if (checkFile(n) == false) SetTestFailed("There is failed file.");
            return this.OutResult;
        }

        private bool checkFile(DDNode n)
        {
            n.Attributes.ContainsAttributesOtherwiseThrow(TASampleSchema.AttrFile);

            var fileName = n.Attributes[TASampleSchema.AttrFile];
            var exists = n.Attributes.GetValue(TASampleSchema.AttrExists, true);

            log.WriteTrace("Does file '{0}' exist?. Expected '{1}'.", fileName, exists);
            var result = File.Exists(fileName);
            log.Write((result == exists ? LogLevel.INF : LogLevel.ERR), "File '{0}' {1}. Expected '{2}'.", fileName,  (result ? "exists" : "doesn't exist")  , exists);

            return (result == exists);
        }

    }
}
