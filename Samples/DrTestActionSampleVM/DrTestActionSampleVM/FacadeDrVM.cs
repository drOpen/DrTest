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

        public DDNode VMPowerOn(DDNode nIn)
        {
            var nOut = DrTestActionExt.GetStubActionResultNode();
            //bool appositionResult;
            try
            {
                nIn.Attributes.ContainsAttributesOtherwiseThrow(SchemaDrTestActionVM.ATTRIBUTE_NAME_SERVER_NAME, 
                                                                SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_NAME, 
                                                                SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_PWD, 
                                                                SchemaDrTestActionVM.ATTRIBUTE_NAME_VM_NAME);

                var vm = new WrapperDrVM(nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_NAME_SERVER_NAME]);

                var expected = nIn.Attributes.GetValue(SchemaDrTestAction.ATTR_STATUS_EXPECTED_VALUE, true);
                try
                {
                    vm.LogIn(nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_NAME], nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_PWD]);
                    vm.Logout();
                    vm.PowerOnVM(nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_NAME_VM_NAME]);
                    vm.Logout();
                    //appositionResult = (expected == true);
                }
                catch (Exception e)
                { 

                }
                return nOut.SetActionResultStatusOK();
            }
            catch( Exception e)
            {
                return nOut.SetActionResultStatusFailed(e);
            }
            finally
            {
                nOut.SetActionResultNodeEndTime();
            }
        }

    }
}
