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
using DrOpen.DrCommon.DrData.Exceptions;
using DrTestExt.Schema;

namespace DrTestExt
{
    /// <summary>
    /// Extensions for DrData
    /// </summary>
    public static class DrTestActionExt
    {

        #region GetStubActionResultNode

        /// <summary>
        /// returns node type for DrTestAction
        /// </summary>
        /// <returns></returns>
        public static DDType GetTypeActionStatus()
        {
            return new DDType(SchemaDrTestAction.TYPE_ACTION);
        }
        /// <summary>
        /// verifies type of node. If have not equals DrTestActionType will throw '<typeparamref name="ValidateExpectedNodeType"/>'
        /// </summary>
        /// <param name="t">type of node for verification</param>
        /// <exception cref="ValidateExpectedNodeType"/>
        public static void IsThisNodeTypeActionOtherwiseThrow(this DDType t)
        {
            t.ValidateExpectedNodeType(SchemaDrTestAction.TYPE_ACTION);
        }
        /// <summary>
        /// returns out node for DrTestAction. 
        /// </summary>
        /// <returns></returns>
        public static DDNode GetStubActionResultNode()
        {
            var n = new DDNode(SchemaDrTestAction.NODE_ACTION_NAME, GetTypeActionStatus());
            n.Attributes.Add(SchemaDrTestAction.ATTR_STATUS_EXECUTE_STATUS, (int)SchemaDrTest.DrTestExecStatus.EXEC_STATUS_STARTED);
            n.Attributes.Add(SchemaDrTestAction.ATTR_STATUS_START_TIME, DateTime.Now);
            n.Attributes.Add(SchemaDrTestAction.ATTR_STATUS_END_TIME, null);
            n.Attributes.Add(SchemaDrTestAction.ATTR_STATUS_DESCRIPTION, String.Empty);
            return n;
        }
        #endregion GetStubActionResultNode

        #region SetActionStatus
        /// <summary>
        /// Sets value of attribute EndTime for DrTestAction node.
        /// </summary>
        /// <param name="n">node for out from action. Type of this node must be equls DrTestActionType otherwise sub will throw '<typeparamref name="ValidateExpectedNodeType"/>'</param>
        /// <exception cref="ValidateExpectedNodeType"/>
        public static void SetActionResultNodeEndTime(this DDNode n)
        {
            n.Type.IsThisNodeTypeActionOtherwiseThrow();
            n.Attributes.Add(SchemaDrTestAction.ATTR_STATUS_END_TIME, DateTime.Now, ResolveConflict.OVERWRITE);
        }

        /// <summary>
        /// Sets OK value of attribute ExecutionStatus for DrTestAction node.
        /// </summary>
        /// <param name="n">node for out from action. Type of this node must be equls DrTestActionType otherwise sub will throw '<typeparamref name="ValidateExpectedNodeType"/>'</param>
        /// <returns>returns specified node</returns>
        /// <exception cref="ValidateExpectedNodeType"/>
        public static DDNode SetActionResultStatusOK(this DDNode n)
        {
            n.Type.IsThisNodeTypeActionOtherwiseThrow();
            n.Attributes.Add(SchemaDrTestAction.ATTR_STATUS_EXECUTE_STATUS, (int)SchemaDrTest.DrTestExecStatus.EXEC_STATUS_OK, ResolveConflict.OVERWRITE);
            return n;
        }
        /// <summary>
        /// Sets OK value of attribute ExecutionStatus and specified description for DrTestAction node.
        /// </summary>
        /// <param name="n">node for out from action. Type of this node must be equls DrTestActionType otherwise sub will throw '<typeparamref name="ValidateExpectedNodeType"/>'</param>
        /// <param name="description">add specified </param>
        /// <returns>description of describes action result</returns>
        /// <exception cref="ValidateExpectedNodeType"/>
        public static DDNode SetActionResultStatusOK(this DDNode n, string description)
        {
            SetActionResultStatusOK(n);
            n.Attributes.Add(SchemaDrTestAction.ATTR_STATUS_DESCRIPTION, description, ResolveConflict.OVERWRITE);
            return n;
        }
        /// <summary>
        /// Sets Failed value of attribute ExecutionStatus for DrTestAction node.
        /// </summary>
        /// <param name="n">node for out from action. Type of this node must be equls DrTestActionType otherwise sub will throw '<typeparamref name="ValidateExpectedNodeType"/>'</param>
        /// <returns>returns specified node</returns>
        /// <exception cref="ValidateExpectedNodeType"/>
        public static DDNode SetActionResultStatusFailed(this DDNode n)
        {
            n.Type.IsThisNodeTypeActionOtherwiseThrow();
            n.Attributes.Add(SchemaDrTestAction.ATTR_STATUS_EXECUTE_STATUS, (int)SchemaDrTest.DrTestExecStatus.EXEC_STATUS_FAILED, ResolveConflict.OVERWRITE);
            return n;
        }
        /// <summary>
        /// Sets Failed value of attribute ExecutionStatus and specified description for DrTestAction node.
        /// </summary>
        /// <param name="n">node for out from action. Type of this node must be equls DrTestActionType otherwise sub will throw '<typeparamref name="ValidateExpectedNodeType"/>'</param>
        /// <param name="description">add specified </param>
        /// <returns>returns specified node</returns>
        /// <exception cref="ValidateExpectedNodeType"/>
        public static DDNode SetActionResultStatusFailed(this DDNode n, string description)
        {
            SetActionResultStatusFailed(n);
            n.Attributes.Add(SchemaDrTestAction.ATTR_STATUS_DESCRIPTION, description, ResolveConflict.OVERWRITE);
            return n;
        }
        /// <summary>
        /// Sets Failed value of attribute ExecutionStatus and adds specified exception to DrTestAction node
        /// </summary>
        /// <param name="n">node for out from action. Type of this node must be equls DrTestActionType otherwise sub will throw '<typeparamref name="ValidateExpectedNodeType"/>'</param>
        /// <param name="e">Exception describes issues</param>
        /// <returns>returns specified node</returns>
        /// <exception cref="ValidateExpectedNodeType"/>
        public static DDNode SetActionResultStatusFailed(this DDNode n, Exception e)
        {
            n.Type.IsThisNodeTypeActionOtherwiseThrow();
            n.Add(e);
            n.Attributes.Add(SchemaDrTestAction.ATTR_STATUS_EXECUTE_STATUS, (int)SchemaDrTest.DrTestExecStatus.EXEC_STATUS_FAILED, ResolveConflict.OVERWRITE);
            return n;
        }
        /// <summary>
        /// Sets Failed value of attribute ExecutionStatus and specified description for DrTestAction node. Also, adds specified exception too.
        /// </summary>
        /// <param name="n">node for out from action. Type of this node must be equls DrTestActionType otherwise sub will throw '<typeparamref name="ValidateExpectedNodeType"/>'</param>
        /// <param name="e">Exception describes issues</param>
        /// <param name="description">add specified</param>
        /// <returns>returns specified node</returns>
        /// <exception cref="ValidateExpectedNodeType"/>
        public static DDNode SetActionResultStatusFailed(this DDNode n, string description, Exception e)
        {
            SetActionResultStatusFailed(n, e);
            n.Attributes.Add(SchemaDrTestAction.ATTR_STATUS_DESCRIPTION,  description, ResolveConflict.OVERWRITE);
            return n;
        }
        #endregion SetActionStatus



        #region ContainsAttributesOtherwiseThrow

        /// <summary>
        /// Determines whether the Attribute Collection contains an elements with the specified names and will be throw new <exception cref="ContainsAttributesException"/> if one of these attributes was not found
        /// </summary>
        /// <param name="attr">collection of DDValue that can be accessed by name.</param>
        /// <param name="names">list of names of mandatory attributes</param>
        /// <exception cref="DDMissingSomeOfAttributesException"/>
        public static void ContainsAttributesOtherwiseThrow(this DDAttributesCollection attr, params string[] names)
        {
            containsAttributesOtherwiseThrow(attr, names);
        }
        /// <summary>
        /// Determines whether the Attribute Collection contains an elements with the specified names and will be throw new <exception cref="ContainsAttributesException"/> if one of these attributes was not found
        /// </summary>
        /// <param name="attr">collection of DDValue that can be accessed by name.</param>
        /// <param name="names">list of names of mandatory attributes</param>
        /// <exception cref="DDMissingSomeOfAttributesException"/>
        private static void containsAttributesOtherwiseThrow(this DDAttributesCollection attr, IEnumerable<string> names)
        {
            var nel = new List<string>();
            foreach (var name in names)
            {
                if (!attr.Contains(name)) nel.Add(name);
            }
            if (nel.Count > 0) throw new DDMissingSomeOfAttributesException(nel.ToArray());
        }
        #endregion ContainsAttributesOtherwiseThrow
    }
}
