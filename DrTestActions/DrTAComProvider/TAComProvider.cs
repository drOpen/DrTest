/*
  TAComProvider.cs -- DrTestAction - synchronization model via COM for external tests, October 7, 2017
  
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
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using DrOpen.DrCommon.DrData;
using DrOpen.DrCommon.DrDataSx;
using System.IO;
using DrOpen.DrTest.DrTAHelper;

namespace DrOpen.DrTest.DrTAComProvider
{



    /// <summary>
    /// interface of synchronization model via COM for external tests
    /// </summary>
    [Guid("1335CCD7-EE3D-4d84-BBB8-65CC800C3074")]
    public interface ITAComProvider
    {
        [DispId(1)]
        void Load([In, MarshalAs(UnmanagedType.BStr)]  string path);
        [DispId(2)]
        void Save([In, MarshalAs(UnmanagedType.BStr)]  string path);
        [DispId(3)]
        void SetStatus(TASchema.TEST_STATUS status, [In, MarshalAs(UnmanagedType.BStr), Optional]  string message);
    }

    /// <summary>
    /// com events of synchronization model via COM for external tests
    /// </summary>
    [Guid("EC39B55B-AE93-4afa-ACAC-9A0BD265A0D0")]
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface ITAComProviderEvents
    {
    }
    /// <summary>
    /// class of synchronization model via COM for external tests
    /// </summary>
    [Guid("8FD62AE4-924F-45a3-9D25-88F2FB95F327")]
    [ClassInterface(ClassInterfaceType.None)]
    [ComSourceInterfaces(typeof(ITAComProviderEvents))]
    [ProgId("DrTest.TAComProvider")]
    public class TAComProvider : TAHelper, ITAComProvider
    {
        /// <summary>
        /// Test node
        /// </summary>
        DDNode tNode;
        #region TAComProvider 
        public TAComProvider()
        {
            this.tNode = base.GetStubResultNode();
            base.legacyStatus.Attributes.Add(TASchema.DrTestLegacyStatusAttributeStatus, (int)TASchema.TEST_STATUS.SKIPPED);
            base.legacyStatus.Attributes.Add(TASchema.DrTestLegacyStatusAttributeMessage, String.Empty);
        }

        ~TAComProvider()
        { 
            
        }
        #endregion
        #region Load/Save
        /// <summary>
        /// Loads data from xml file in the DDNode format to temporary node and merge temporary data with base node
        /// </summary>
        /// <param name="path">path to xml file</param>
        public void Load(string path)
        {
            DDNode tmpNode;
            using (var sf = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                using (var sr = new StreamReader(sf))
                {
                    tmpNode = DDNodeSxe.Deserialize(sr); // gets data from xml file to temporary node
                }
            }
            this.tNode.Merge(tmpNode, DDNode.DDNODE_MERGE_OPTION.ALL, ResolveConflict.SKIP); // merge stub node with data from file
        }

        /// <summary>
        /// saves base node to specified xml file in the DDNode format
        /// </summary>
        /// <param name="path">path to xml file</param>
        public void Save(string path)
        {
            using (var sf = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                using (var sw = new StreamWriter(sf))
                {
                    tNode.Serialize(sw);
                }
            }
        }
        #endregion Load/Save
        #region SetStatus
        public void SetStatus(TASchema.TEST_STATUS status, string message)
        {
            base.legacyStatus.Attributes.Add(TASchema.DrTestLegacyStatusAttributeStatus, (int)status, ResolveConflict.OVERWRITE);
            base.legacyStatus.Attributes.Add(TASchema.DrTestLegacyStatusAttributeMessage, message, ResolveConflict.OVERWRITE);

        }
        #endregion SetStatus
    }
}
