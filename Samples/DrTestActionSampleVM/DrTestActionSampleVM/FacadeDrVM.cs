using DrOpen.DrCommon.DrData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DrTestExt;
using DrTest.DrAction.DrTestActionSampleVM.Res;

namespace DrTest.DrAction.DrTestActionSampleVM
{
    public class FacadeDrVM
    {

        private delegate void vmDelegatePowerAction(string vmName);

        private enum vmPowerAction
        {
            VM_POWER_ON,
            VM_POWER_OFF,
            VM_POWER_RESET,
            VM_POWER_SUSPEND
        }

        private Dictionary<vmPowerAction, vmDelegatePowerAction> GetVMPowerActionDelegations(WrapperDrVM vm)
        {
            return new Dictionary<vmPowerAction, vmDelegatePowerAction>()
            {
                {vmPowerAction.VM_POWER_ON, vm.PowerOnVM},
                {vmPowerAction.VM_POWER_OFF, vm.PowerOffVM},
                {vmPowerAction.VM_POWER_RESET, vm.ResetVM},
                {vmPowerAction.VM_POWER_SUSPEND, vm.SuspendVM}
            };
        }

        /// <summary>
        /// Powers on this virtual machine. 
        /// If the virtual machine is suspended, this method resumes execution from the suspend point. 
        /// When powering on a virtual machine in a cluster, the system might implicitly or due to the host argument, do an implicit relocation of the virtual machine to another host. 
        /// Hence, errors related to this relocation can be thrown.
        /// </summary>
        /// <param name="nIn">Action node contains the mandatory attributes:
        /// <list type="bullet">
        /// <item><description><value>SchemaDrTestActionVM.ATTRIBUTE_NAME_SERVER_NAME</value></description></item> 
        /// <item><description><value>SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_NAME</value></description></item> 
        /// <item><description><value>SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_PWD</value></description></item> 
        /// <item><description><value>SchemaDrTestActionVM.ATTRIBUTE_NAME_VM_NAME</value></description></item>
        /// </list> 
        /// </param>
        /// <returns>Action result</returns>
        public DDNode VMPowerOn(DDNode nIn)
        {
            var nOut = DrTestActionExt.GetStubActionResultNode();
            try
            {
                nIn.Attributes.ContainsAttributesOtherwiseThrow(SchemaDrTestActionVM.ATTRIBUTE_NAME_SERVER_NAME,
                                                                SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_NAME,
                                                                SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_PWD,
                                                                SchemaDrTestActionVM.ATTRIBUTE_NAME_VM_NAME);

                var vm = new WrapperDrVM(nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_NAME_SERVER_NAME]);

                vm.LogIn(nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_NAME], nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_PWD]);
                vm.PowerOnVM(nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_NAME_VM_NAME]);
                vm.Logout();

                return nOut.SetActionResultStatusOK(string.Format(Msg.VM_SUCCESS_CHANGE_POWER_STATUS, Msg.VM_POWER_STATUS_ON, nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_NAME_VM_NAME]));
            }
            catch (Exception e) // should be set Failed status because it's global exception of this action
            {
                return nOut.SetActionResultStatusFailed(string.Format(Msg.VM_CANNOT_CHANGE_POWER_STATUS, Msg.VM_POWER_STATUS_ON, nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_NAME_VM_NAME]), e);
            }
            finally
            {
                nOut.SetActionResultNodeEndTime(); // sets EndTime attribute at the end
            }
        }

    }
}
