/*
  DrTestActionExt.cs -- extensions for DrData, August  28, 2016
  
  Copyright (c) 2013-2016 Kudryashov Andrey aka Dr
 
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
using System.Text;
using DrOpen.DrCommon.DrData;
using DrTestExt.DrTestExceptions;

namespace DrTestExt
{
    /// <summary>
    /// Extensions for DrData
    /// </summary>
    public static class DrTestActionExt
    {

        #region GetStubActionResultNode





        public static DDType GetTypeActionStatus()
        {
            return new DDType(SchemaDrTestAction.TYPE_ACTION);
        }

        public static void IsThisNodeTypeActionOtherwiseThrow(this DDType t)
        {
            t.ValidateExpectedNodeType(SchemaDrTestAction.TYPE_ACTION);
        }

        public static DDNode GetStubActionResultNode()
        {
            var n = new DDNode(SchemaDrTestAction.NODE_ACTION_NAME, GetTypeActionStatus());

            n.Attributes.Add(SchemaDrTestAction.ATTR_STATUS_EXECUTE_STATUS, 0);
            n.Attributes.Add(SchemaDrTestAction.ATTR_STATUS_START_TIME, DateTime.Now);

            n.Attributes.Add(SchemaDrTestAction.ATTR_STATUS_END_TIME, null);
            n.Attributes.Add(SchemaDrTestAction.ATTR_STATUS_DESCRIPTION, String.Empty);

            return n;

        }


        public static void SetActionResultNodeEndTime(this DDNode n)
        {
            n.Type.IsThisNodeTypeActionOtherwiseThrow();
            n.Attributes.Add(SchemaDrTestAction.ATTR_STATUS_END_TIME, DateTime.Now, ResolveConflict.OVERWRITE);
        }


        public static DDNode SetActionResultStatusOK(this DDNode n)
        {
            n.Type.IsThisNodeTypeActionOtherwiseThrow();
            n.Attributes.Add(SchemaDrTestAction.ATTR_STATUS_EXECUTE_STATUS, 1, ResolveConflict.OVERWRITE);
            return n;
        }


        public static DDNode SetActionResultStatusFailed(this DDNode n)
        {
            n.Type.IsThisNodeTypeActionOtherwiseThrow();
            n.Attributes.Add(SchemaDrTestAction.ATTR_STATUS_EXECUTE_STATUS, 2, ResolveConflict.OVERWRITE);
            return n;
        }

        public static DDNode SetActionResultStatusFailed(this DDNode n, Exception e)
        {
            n.Type.IsThisNodeTypeActionOtherwiseThrow();
            n.Add(e);
            n.Attributes.Add(SchemaDrTestAction.ATTR_STATUS_EXECUTE_STATUS, 2, ResolveConflict.OVERWRITE);
            return n;
        }


        #endregion GetStubActionResultNode


        #region ContainsAttributesOtherwiseThrow
        /// <summary>
        /// Determines whether the Attribute Collection contains an elements with the specified names and will be throw new <exception cref="ContainsAttributesException"/> if one of these attributes was not found
        /// </summary>
        /// <param name="attr">collection of DDValue that can be accessed by name.</param>
        /// <param name="names">list of names of mandatory attributes</param>
        public static void ContainsAttributesOtherwiseThrow(this DDAttributesCollection attr, params Enum[] names)
        {
            ContainsAttributesOtherwiseThrow(attr, names);
        }
        /// <summary>
        /// Determines whether the Attribute Collection contains an elements with the specified names and will be throw new <exception cref="ContainsAttributesException"/> if one of these attributes was not found
        /// </summary>
        /// <param name="attr">collection of DDValue that can be accessed by name.</param>
        /// <param name="names">list of names of mandatory attributes</param>
        public static void ContainsAttributesOtherwiseThrow(this DDAttributesCollection attr, params string[] names)
        {
            containsAttributesOtherwiseThrow(attr, names);
        }
        /// <summary>
        /// Determines whether the Attribute Collection contains an elements with the specified names and will be throw new <exception cref="ContainsAttributesException"/> if one of these attributes was not found
        /// </summary>
        /// <param name="attr">collection of DDValue that can be accessed by name.</param>
        /// <param name="names">list of names of mandatory attributes</param>
        private static void containsAttributesOtherwiseThrow(this DDAttributesCollection attr, IEnumerable<string> names)
        {
            var nel = new List<string>();
            foreach (var name in names)
            {
                if (!attr.Contains(name)) nel.Add(name);
            }
            if (nel.Count > 0) throw new ContainsAttributesException(nel);
        }
        #endregion ContainsAttributesOtherwiseThrow
    }
}
