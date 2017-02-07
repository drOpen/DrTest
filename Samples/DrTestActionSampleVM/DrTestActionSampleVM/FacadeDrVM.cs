/*
  FacadeDrVM.cs -- facade for vm, September 11, 2016
  
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
        #region VMPowerAction
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
            return VMPowerAction(nIn, WrapperDrVM.vmPowerAction.VM_POWER_ON);
        }
        /// <summary>
        /// Powers off this virtual machine.
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
        public DDNode VMPowerOff(DDNode nIn)
        {
            return VMPowerAction(nIn, WrapperDrVM.vmPowerAction.VM_POWER_OFF);
        }
        /// <summary>
        /// Resets power on this virtual machine. If the current state is poweredOn, then this method first performs powerOff(hard). Once the power state is poweredOff, then this method performs powerOn(option).
        /// Although this method functions as a powerOff followed by a powerOn, the two operations are atomic with respect to other clients, meaning that other power operations cannot be performed until the reset method completes.
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
        public DDNode VMReset(DDNode nIn)
        {
            return VMPowerAction(nIn, WrapperDrVM.vmPowerAction.VM_POWER_RESET);
        }
        /// <summary>
        /// Suspends execution in this virtual machine.
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
        public DDNode VMSuspend(DDNode nIn)
        {
            return VMPowerAction(nIn, WrapperDrVM.vmPowerAction.VM_POWER_SUSPEND);
        }

        private DDNode VMPowerAction(DDNode nIn, WrapperDrVM.vmPowerAction vmPA)
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
                vm.VMChangePowerState(nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_NAME_VM_NAME], vmPA);
                vm.Logout();

                return nOut.SetActionResultStatusOK(string.Format(Msg.VM_SUCCESS_CHANGE_POWER_STATUS, Msg.VM_POWER_STATUS_ON, nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_NAME_VM_NAME]));
            }
            catch (Exception e) // should be set Failed status because it's global exception of this action
            {
                return nOut.SetActionResultStatusFailed(string.Format(Msg.VM_CANNOT_CHANGE_POWER_STATUS, WrapperDrVM.GetVMPowerActionDescription()[vmPA], nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_NAME_VM_NAME]), e);
            }
            finally
            {
                nOut.SetActionResultNodeEndTime(); // sets EndTime attribute at the end
            }
        }
        #endregion VMPowerAction
    }
}
