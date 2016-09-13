using DrOpen.DrCommon.DrData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DrTestExt;

namespace DrTestActionSampleVM.DrAction.DrTest
{
    public class FacadeDrVM
    {

        public DDNode VMPowerOn(DDNode n)
        {
            n.Attributes.ContainsAttributesOtherwiseThrow(SchemaDrVM.ATTRIBUTE_NAME_SERVER_NAME, SchemaDrVM.ATTRIBUTE_NAME_USER_NAME, SchemaDrVM.ATTRIBUTE_NAME_USER_PWD, SchemaDrVM.ATTRIBUTE_NAME_VM_NAME);
            var vm = new WrapperDrVM(n.Attributes[SchemaDrVM.ATTRIBUTE_NAME_SERVER_NAME]);
            vm.LogIn(n.Attributes[SchemaDrVM.ATTRIBUTE_NAME_USER_NAME], n.Attributes[SchemaDrVM.ATTRIBUTE_NAME_USER_PWD]);
            vm.PowerOnVM(n.Attributes[SchemaDrVM.ATTRIBUTE_NAME_VM_NAME]);
            vm.Logout();
            return new DDNode();

        }

    }
}
