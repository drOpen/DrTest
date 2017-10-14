/*
License.txt -- License file for 'DrVarManager' general purpose Builder variables 1.0, August 19, 2017
 
Copyright (c) 2013-2017 Kudryashov Andrey aka dr
 
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

namespace DrOpen.DrVariable
{
    public class DVManager
    {

        public DVManager()
        {
            shiftStack();
        }

        public const string SCHEMA_VAR_RAW = "raw";
        public const string SCHEMA_VAR_READY = "ready";

        Stack<DDNode> stack = new Stack<DDNode>();

        private DDNode currentStack = null;
        private DDNode currentStackRaw = null;
        private DDNode currentStackReady = null;

        private bool bNeedRebuild = false;

        #region Set

        public void Set(DDNode node)
        {
            foreach (var n in node.Traverse(true))
            {
                currentStackRaw.Attributes.Merge(n.Attributes, ResolveConflict.OVERWRITE);
            }
            if (bNeedRebuild == false) bNeedRebuild = true;
        }

        public void Set(DDAttributesCollection attr)
        {
            currentStackRaw.Attributes.Merge(attr, ResolveConflict.OVERWRITE);
            if (bNeedRebuild == false) bNeedRebuild = true;
        }

        public void Set(KeyValuePair<string, DDValue> at)
        {
            currentStackRaw.Attributes.Add(at.Key, at.Value, ResolveConflict.OVERWRITE);
            if (bNeedRebuild == false) bNeedRebuild = true;
        }

        #endregion Set

        #region Resolve

        public void Resolve(DDValue v)
        {
            if (bNeedRebuild) Rebuild();
        }

        #endregion Resolve

        #region Rebuild

        public bool Rebuild()
        {


            bNeedRebuild = false;
            return true;
        }

        #endregion Rebuild

        private void shiftStack()
        {
            if (currentStack != null) stack.Push(currentStack);
            currentStack = getStackNode();
            currentStackRaw = currentStack[SCHEMA_VAR_RAW];
            currentStackReady = currentStack[SCHEMA_VAR_READY];
        }

        private DDNode getStackNode()
        {
            var r = new DDNode();
            r.Add(SCHEMA_VAR_RAW);
            r.Add(SCHEMA_VAR_READY);
            return r;
        }

    }
}
