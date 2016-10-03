using DrTest.DrAction.DrTestActionSampleVM.Res;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrTest.DrAction.DrTestActionSampleVM.Exceptions
{
    internal class VMForbiddenPowerAction : Exception
    {
        public VMForbiddenPowerAction(string actionName)
            : base(string.Format(Msg.VM_POWER_UNKNOW_ACTION, actionName))
        {
            this.ActionName = actionName;
        }
        public string ActionName { get; private set; }


    }
}
