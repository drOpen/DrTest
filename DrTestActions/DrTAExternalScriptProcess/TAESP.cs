/*
  TAESP.cs -- DrTestAction - executor for external script process, October 7, 2017
  
  Copyright (c) 2013-2017 Kudryashov Andrey aka Dr
 
  This software is provided 'as-is', without any express or implied
  warranty. In no event will the authors be held liable for any damages
  arising from the use of this software.

  Permission is granted to anyone to use this software for any purpose,
  including commercial applications, and to alter it and redistribute it
  freely, subject to the following restrictions:

      1. The origin of this software must not be misrepresented; you must not
      claim that you wrote the original software. If you use this software
      in a product, an acknowledgment in the product documentation would be
      appreciated but is not required.

      2. Altered source versions must be plainly marked as such, and must not be
      misrepresented as being the original software.

      3. This notice may not be removed or altered from any source distribution.

      Kudryashov Andrey <kudryashov.andrey at gmail.com>

 */

using DrOpen.DrCommon.DrData;
using DrOpen.DrCommon.DrDataSx;
using DrOpen.DrTest.DrTAHelper;
using DrOpen.DrTest.DrTAProcess;
using System;
using System.IO;


namespace DrOpen.DrTest.DrTAExternalScriptProcess
{
    public class TAESP : DrTAProcess.TAProcess
    {
        public void Execute(DDNode n)
        {
            var sharedNodePath = Path.GetTempFileName(); //path to shared node 
            var v = n.Attributes.GetValue(TAProcessSchema.AttrArguments, TAProcessSchema.DefaultArguments).GetValueAsString();
            if (v.Length > 0) 
                v = sharedNodePath + " " + v;
            else
                v = sharedNodePath;
            n.Attributes.Add(TAProcessSchema.AttrArguments, v, ResolveConflict.OVERWRITE); // adds first argument. here is a path to shared node
            base.CreateProcess(n);
            var sharedNode  = Load(sharedNodePath);
            this.OutPut.Merge(sharedNode, DDNode.DDNODE_MERGE_OPTION.ALL, ResolveConflict.OVERWRITE);

            // test is failed
            if ((sharedNode.Attributes.GetValue(TASchema.DrTestLegacyStatusAttributeStatus, TASchema.TEST_STATUS.SKIPPED).GetValueAsInt() | (int)TASchema.TEST_STATUS.FAILED) == (int)TASchema.TEST_STATUS.FAILED)
            {
                var reason = sharedNode.Attributes.GetValue(TASchema.DrTestLegacyStatusAttributeMessage, String.Empty);
                throw new DrTAFailedException(reason);
            }
        }
        #region Load/Save
        /// <summary>
        /// returns data from xml file in the DDNode format 
        /// </summary>
        /// <param name="path">path to xml file</param>
        public DDNode Load(string path)
        {
            DDNode tmpNode;
            using (var sf = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                using (var sr = new StreamReader(sf))
                {
                    tmpNode = DDNodeSxe.Deserialize(sr); // gets data from xml file to temporary node
                }
            }
            return tmpNode;
            //this.tNode.Merge(tmpNode, DDNode.DDNODE_MERGE_OPTION.ALL, ResolveConflict.SKIP); // merge stub node with data from file
        }
        /// <summary>
        /// saves base node to specified xml file in the DDNode format
        /// </summary>
        /// <param name="path">path to xml file</param>
        public void Save(string path, DDNode n)
        {
            using (var sf = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                using (var sw = new StreamWriter(sf))
                {
                    n.Serialize(sw);
                }
            }
        }
        #endregion Load/Save

    }
}
