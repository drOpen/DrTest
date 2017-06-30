using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using DrOpen.DrCommon.DrData;

namespace DrOpen.DrVariable
{
    public class DVManager
    {

        public DVManager()
        {
            ShiftStack();
        }

        public const string SCHEMA_VAR_ROW = "raw";
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

        private void ShiftStack()
        {
            if (currentStack != null) stack.Push(currentStack);
            currentStack = GetStackNode();
            currentStackRaw = currentStack[SCHEMA_VAR_ROW];
            currentStackReady = currentStack[SCHEMA_VAR_READY];
        }

        private DDNode GetStackNode()
        {
            var r = new DDNode();
            r.Add(SCHEMA_VAR_ROW);
            r.Add(SCHEMA_VAR_READY);
            return r;
        }

    }
}
