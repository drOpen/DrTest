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


        /// <summary>
        /// Clone excesting VM.
        /// </summary>
        /// <param name="nIn">Action node contains the mandatory attributes:
        /// <list type="bullet">
        /// <item><description><value>SchemaDrTestActionVM.ATTRIBUTE_NAME_SERVER_NAME</value></description></item> 
        /// <item><description><value>SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_NAME</value></description></item> 
        /// <item><description><value>SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_PWD</value></description></item> 
        /// <item><description><value>SchemaDrTestActionVM.ATTRIBUTE_NAME_VM_NAME</value></description></item>
        /// <item><description><value>SchemaDrTestActionVM.ATTRIBUTE_NEW_NAME_VM_NAME</value></description></item>
        /// </list> 
        /// </param>
        /// <returns>Action result</returns>
        public DDNode VMCloneVM(DDNode nIn)
        {
            var nOut = DrTestActionExt.GetStubActionResultNode();

            try
            {
                nIn.Attributes.ContainsAttributesOtherwiseThrow(SchemaDrTestActionVM.ATTRIBUTE_NAME_SERVER_NAME,
                                                                    SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_NAME,
                                                                    SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_PWD,
                                                                    SchemaDrTestActionVM.ATTRIBUTE_NAME_VM_NAME,
                                                                    SchemaDrTestActionVM.ATTRIBUTE_OLD_NAME_VM_NAME,
                                                                    SchemaDrTestActionVM.ATTRIBUTE_HOST_NAME,
                                                                    SchemaDrTestActionVM.ATTRIBUTE_VM_SNAPSHOT_NAME);
              
                string resourcePool = nIn.Attributes.GetValue(SchemaDrTestActionVM.ATTRIBUTE_RESOURCE_POOL_NAME, SchemaDrTestActionVM.ATTRIBUTE_RESOURCE_POOL_NAME_VALUE);

                var vm = new WrapperDrVM(nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_NAME_SERVER_NAME]);
                vm.LogIn(nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_NAME], nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_PWD]);
                vm.CloneVM(nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_OLD_NAME_VM_NAME],
                           nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_NAME_VM_NAME], 
                           nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_HOST_NAME], 
                           nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_VM_SNAPSHOT_NAME], 
                           resourcePool);

                vm.Logout();
                return nOut.SetActionResultStatusOK(string.Format(Msg.VM_CLONE_VM_SUCCES, nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_OLD_NAME_VM_NAME], nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_NAME_VM_NAME]));
            }
                catch (Exception e) 
            {
    
                return nOut.SetActionResultStatusFailed(string.Format(Msg.VM_CLONE_VM_FAILED, nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_OLD_NAME_VM_NAME], nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_NAME_VM_NAME], e.Message));
            }

            finally
            {
                nOut.SetActionResultNodeEndTime(); 
            }
        }




        /// <summary>
        /// Check Started Application
        /// </summary>
        /// <param name="nIn">Action node contains the mandatory attributes:
        /// <list type="bullet">
        /// <item><description><value>SchemaDrTestActionVM.ATTRIBUTE_NAME_SERVER_NAME</value></description></item> 
        /// <item><description><value>SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_NAME</value></description></item> 
        /// <item><description><value>SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_PWD</value></description></item> 
        /// <item><description><value>SchemaDrTestActionVM.ATTRIBUTE_NAME_VM_NAME</value></description></item>
        /// <item><description><value>SchemaDrTestActionVM.ATTRIBUTE_VM_GUEST_LOGIN_NAME</value></description></item>
        /// <item><description><value>SchemaDrTestActionVM.ATTRIBUTE_VM_GUEST_LOGIN_PWD</value></description></item>
        /// <item><description><value>SchemaDrTestActionVM.ATTRIBUTE_VM_APPLICATION_NAME</value></description></item>
        /// </list> 
        /// </param>
        /// <returns></returns>
        public DDNode VMCheckProcess(DDNode nIn)
        {
            var nOut = DrTestActionExt.GetStubActionResultNode();
            try
            {
                nIn.Attributes.ContainsAttributesOtherwiseThrow(SchemaDrTestActionVM.ATTRIBUTE_NAME_SERVER_NAME,
                                                                     SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_NAME,
                                                                     SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_PWD,
                                                                     SchemaDrTestActionVM.ATTRIBUTE_VM_GUEST_LOGIN_NAME,
                                                                     SchemaDrTestActionVM.ATTRIBUTE_VM_GUEST_LOGIN_PWD,
                                                                     SchemaDrTestActionVM.ATTRIBUTE_VM_APPLICATION_NAME,
                                                                     SchemaDrTestActionVM.ATTRIBUTE_NAME_VM_NAME);

                int retry = nIn.Attributes.GetValue(SchemaDrTestActionVM.ATTRIBUTE_VM_ATTEMPTS_RETRY, SchemaDrTestActionVM.ATTRIBUTE_VM_ATTEMPTS_RETRY_VALUE);
                int timeOut = nIn.Attributes.GetValue(SchemaDrTestActionVM.ATTRIBUTE_VM_ATTEMPTS_TIMEOUT, SchemaDrTestActionVM.ATTRIBUTE_VM_ATTEMPTS_TIMEOUT_VALUE);

                var vm = new WrapperDrVM(nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_NAME_SERVER_NAME]);
                vm.LogIn(nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_NAME], nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_PWD]);
                vm.CheckVMState(nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_NAME_VM_NAME],
                                nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_VM_GUEST_LOGIN_NAME],
                                nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_VM_GUEST_LOGIN_PWD],
                                nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_VM_APPLICATION_NAME],
                                retry,
                                timeOut);

                vm.Logout();
                return nOut.SetActionResultStatusOK(string.Format(Msg.VM_CHECK_PROCESS_SUCCESS, nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_VM_APPLICATION_NAME], nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_NAME_VM_NAME]));

            }
            catch (Exception e)
            {

                return nOut.SetActionResultStatusFailed(string.Format(Msg.VM_CHECK_PROCESS_FAILED, nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_VM_APPLICATION_NAME], nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_NAME_VM_NAME], e.Message));
            }

            finally
            {
                nOut.SetActionResultNodeEndTime();
            }
        }




   /// <summary>
   /// 
   /// </summary>
   /// <param name="nIn"></param>
   /// <returns></returns>
        public DDNode HostCreateResourcePool(DDNode nIn)
        {
            var nOut = DrTestActionExt.GetStubActionResultNode();
            try
            {
                nIn.Attributes.ContainsAttributesOtherwiseThrow(SchemaDrTestActionVM.ATTRIBUTE_NAME_SERVER_NAME,
                                                                     SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_NAME,
                                                                     SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_PWD,
                                                                       SchemaDrTestActionVM.ATTRIBUTE_RESOURCE_POOL_NAME,
                                                                       SchemaDrTestActionVM.ATTRIBUTE_RESOURCE_POOL_SOURCE,
                                                                       SchemaDrTestActionVM.ATTRIBUTE_HOST_NAME);



                var vm = new WrapperDrVM(nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_NAME_SERVER_NAME]);
                vm.LogIn(nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_NAME], nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_PWD]);
                vm.HostCreateResourcePool(nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_RESOURCE_POOL_SOURCE], nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_RESOURCE_POOL_NAME], nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_HOST_NAME]);
                vm.Logout();
                return nOut.SetActionResultStatusOK(string.Format(Msg.VM_SUCCESS_CREAT_RESOURCE_POOL, nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_RESOURCE_POOL_NAME], nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_RESOURCE_POOL_SOURCE]));

            }
            catch (Exception e)
            {

                return nOut.SetActionResultStatusFailed(string.Format(Msg.VM_FILED_CREAT_RESOURCE_POOL, nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_RESOURCE_POOL_NAME], nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_RESOURCE_POOL_SOURCE], e.Message));
            }

            finally
            {
                nOut.SetActionResultNodeEndTime();
            }
        }



        /// <summary>
        /// Function to start required procces on requiered VM
        /// </summary>
        /// <param name="nIn">Action node contains the mandatory attributes:
        /// <list type="bullet">
        /// <item><description><value>SchemaDrTestActionVM.ATTRIBUTE_NAME_SERVER_NAME</value></description></item> 
        /// <item><description><value>SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_NAME</value></description></item> 
        /// <item><description><value>SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_PWD</value></description></item> 
        /// <item><description><value>SchemaDrTestActionVM.ATTRIBUTE_NAME_VM_NAME</value></description></item>
        /// <item><description><value>SchemaDrTestActionVM.ATTRIBUTE_VM_GUEST_LOGIN_NAME</value></description></item>
        /// <item><description><value>SchemaDrTestActionVM.ATTRIBUTE_VM_GUEST_LOGIN_PWD</value></description></item>
        /// <item><description><value>SchemaDrTestActionVM.ATTRIBUTE_VM_APPLICATION_PATH</value></description></item>
        /// </list> 
        /// </param>
        /// <returns></returns>
        public DDNode VMStartProcess(DDNode nIn)
        {
            var nOut = DrTestActionExt.GetStubActionResultNode();
            try
            {
                nIn.Attributes.ContainsAttributesOtherwiseThrow(SchemaDrTestActionVM.ATTRIBUTE_NAME_SERVER_NAME,
                                                                     SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_NAME,
                                                                     SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_PWD,
                                                                     SchemaDrTestActionVM.ATTRIBUTE_VM_GUEST_LOGIN_NAME,
                                                                     SchemaDrTestActionVM.ATTRIBUTE_VM_GUEST_LOGIN_PWD,
                                                                     SchemaDrTestActionVM.ATTRIBUTE_VM_APPLICATION_PATH,
                                                                     SchemaDrTestActionVM.ATTRIBUTE_NAME_VM_NAME);

                int retry = nIn.Attributes.GetValue(SchemaDrTestActionVM.ATTRIBUTE_VM_ATTEMPTS_RETRY, SchemaDrTestActionVM.ATTRIBUTE_VM_ATTEMPTS_RETRY_VALUE);
                int timeOut = nIn.Attributes.GetValue(SchemaDrTestActionVM.ATTRIBUTE_VM_ATTEMPTS_TIMEOUT, SchemaDrTestActionVM.ATTRIBUTE_VM_ATTEMPTS_TIMEOUT_VALUE);
                string arguments = nIn.Attributes.GetValue(SchemaDrTestActionVM.ATTRIBUTE_VM_APPLICATION_ARGUMENTS, SchemaDrTestActionVM.ATTRIBUTE_VM_APPLICATION_ARGUMENTS_VALUE);

                var vm = new WrapperDrVM(nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_NAME_SERVER_NAME]);
                vm.LogIn(nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_NAME], nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_PWD]);
                vm.StartVMApplication(nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_NAME_VM_NAME],
                                nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_VM_GUEST_LOGIN_NAME],
                                nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_VM_GUEST_LOGIN_PWD],
                                nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_VM_APPLICATION_PATH],        
                                retry,
                                timeOut,
                                arguments);
                vm.Logout();
                return nOut.SetActionResultStatusOK(string.Format(Msg.VM_START_PROCESS_SUCCES, nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_VM_APPLICATION_PATH], nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_NAME_VM_NAME]));

            }
            catch (Exception e)
            {

                return nOut.SetActionResultStatusFailed(string.Format(Msg.VM_START_PROCESS_FAILED, nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_VM_APPLICATION_PATH], nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_NAME_VM_NAME], e.Message));
            }

            finally
            {
                nOut.SetActionResultNodeEndTime();
            }
        }



        /// <summary>
        /// Funbction to copy file from source to destination on required VM
        /// </summary>
        /// <param name="nIn">Action node contains the mandatory attributes:
        /// <list type="bullet">
        /// <item><description><value>SchemaDrTestActionVM.ATTRIBUTE_NAME_SERVER_NAME</value></description></item> 
        /// <item><description><value>SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_NAME</value></description></item> 
        /// <item><description><value>SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_PWD</value></description></item> 
        /// <item><description><value>SchemaDrTestActionVM.ATTRIBUTE_NAME_VM_NAME</value></description></item>
        /// <item><description><value>SchemaDrTestActionVM.ATTRIBUTE_VM_GUEST_LOGIN_NAME</value></description></item>
        /// <item><description><value>SchemaDrTestActionVM.ATTRIBUTE_VM_GUEST_LOGIN_PWD</value></description></item>
        /// <item><description><value>SchemaDrTestActionVM.ATTRIBUTE_VM_COPY_FILE_SOURCE</value></description></item>
        /// <item><description><value>SchemaDrTestActionVM.ATTRIBUTE_VM_COPY_FILE_DESTINATION</value></description></item>
        /// </list> 
        /// </param>
        /// <returns></returns>
        public DDNode VMCopyFile(DDNode nIn)
        {
            var nOut = DrTestActionExt.GetStubActionResultNode();
            try
            {
                nIn.Attributes.ContainsAttributesOtherwiseThrow(SchemaDrTestActionVM.ATTRIBUTE_NAME_SERVER_NAME,
                                                                     SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_NAME,
                                                                     SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_PWD,
                                                                     SchemaDrTestActionVM.ATTRIBUTE_VM_GUEST_LOGIN_NAME,
                                                                     SchemaDrTestActionVM.ATTRIBUTE_VM_GUEST_LOGIN_PWD,
                                                                     SchemaDrTestActionVM.ATTRIBUTE_VM_COPY_FILE_SOURCE,
                                                                     SchemaDrTestActionVM.ATTRIBUTE_VM_COPY_FILE_DESTINATION,
                                                                     SchemaDrTestActionVM.ATTRIBUTE_HOST_NAME,
                                                                     SchemaDrTestActionVM.ATTRIBUTE_NAME_VM_NAME);



                int retry = nIn.Attributes.GetValue(SchemaDrTestActionVM.ATTRIBUTE_VM_ATTEMPTS_RETRY, SchemaDrTestActionVM.ATTRIBUTE_VM_ATTEMPTS_RETRY_VALUE);
                int timeOut = nIn.Attributes.GetValue(SchemaDrTestActionVM.ATTRIBUTE_VM_ATTEMPTS_TIMEOUT, SchemaDrTestActionVM.ATTRIBUTE_VM_ATTEMPTS_TIMEOUT_VALUE);

                var vm = new WrapperDrVM(nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_NAME_SERVER_NAME]);
                vm.LogIn(nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_NAME], nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_PWD]);
                vm.CopyFileToGuestVM(nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_NAME_VM_NAME],
                                nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_VM_GUEST_LOGIN_NAME],
                                nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_VM_GUEST_LOGIN_PWD],
                                nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_VM_COPY_FILE_SOURCE],
                                nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_VM_COPY_FILE_DESTINATION],
                                nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_HOST_NAME],
                                retry,
                                timeOut);
                vm.Logout();
                return nOut.SetActionResultStatusOK(string.Format(Msg.VM_COPY_FILE_SUCCES, nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_VM_COPY_FILE_SOURCE], nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_VM_COPY_FILE_DESTINATION], nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_NAME_VM_NAME]));

            }
            catch (Exception e)
            {

                return nOut.SetActionResultStatusFailed(string.Format(Msg.VM_COPY_FILE_FAILED, nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_VM_COPY_FILE_SOURCE], nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_VM_COPY_FILE_DESTINATION], nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_NAME_VM_NAME], e.Message));
            }

            finally
            {
                nOut.SetActionResultNodeEndTime();
            }
        }



        /// <summary>
        /// Change Custom Action 
        /// </summary>
        /// <param name="nIn">Action node contains the mandatory attributes:
        /// <list type="bullet">
        /// <item><description><value>SchemaDrTestActionVM.ATTRIBUTE_NAME_SERVER_NAME</value></description></item> 
        /// <item><description><value>SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_NAME</value></description></item> 
        /// <item><description><value>SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_PWD</value></description></item> 
        /// <item><description><value>SchemaDrTestActionVM.ATTRIBUTE_VM_CUSTOM_ACTION_KEY</value></description></item>
        /// <item><description><value>SchemaDrTestActionVM.ATTRIBUTE_VM_CUSTOM_ACTION_VALUE</value></description></item>
        /// <item><description><value>SchemaDrTestActionVM.ATTRIBUTE_HOST_NAME</value></description></item>
        /// </list> 
        /// </param>
        /// <returns></returns>
        public DDNode ChangeSomeCustomAction(DDNode nIn)
        {
            var nOut = DrTestActionExt.GetStubActionResultNode();

            try
            {
                nIn.Attributes.ContainsAttributesOtherwiseThrow(SchemaDrTestActionVM.ATTRIBUTE_NAME_SERVER_NAME,
                                                                      SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_NAME,
                                                                      SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_PWD,
                                                                      SchemaDrTestActionVM.ATTRIBUTE_NAME_VM_NAME,
                                                                      SchemaDrTestActionVM.ATTRIBUTE_VM_CUSTOM_ACTION_KEY,
                                                                      SchemaDrTestActionVM.ATTRIBUTE_VM_CUSTOM_ACTION_VALUE,
                                                                      SchemaDrTestActionVM.ATTRIBUTE_HOST_NAME);

                var vm = new WrapperDrVM(nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_NAME_SERVER_NAME]);
                vm.LogIn(nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_NAME], nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_PWD]);
                vm.ChangeSomeCustomActions(nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_NAME_VM_NAME], nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_HOST_NAME], nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_VM_CUSTOM_ACTION_KEY], nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_VM_CUSTOM_ACTION_VALUE]);
                vm.Logout();
                return nOut.SetActionResultStatusOK(string.Format(Msg.VM_SUCCESS_CHANGE_CUSTOM_ACTION, nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_VM_CUSTOM_ACTION_KEY], nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_VM_CUSTOM_ACTION_VALUE], nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_NAME_VM_NAME] ));
            }
            catch (Exception e)
            {

                return nOut.SetActionResultStatusFailed(string.Format(Msg.VM_FAILED_CHANGE_CUSTOM_ACTION, nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_VM_CUSTOM_ACTION_KEY], nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_VM_CUSTOM_ACTION_VALUE], nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_NAME_VM_NAME], e.Message));
            }

            finally
            {
                nOut.SetActionResultNodeEndTime();
            }
        }



        /// <summary>
        /// Function to download file from vm using vm tool
        /// </summary>
        /// <param name="nIn">Action node contains the mandatory attributes:
        /// <list type="bullet">
        /// <item><description><value>SchemaDrTestActionVM.ATTRIBUTE_NAME_SERVER_NAME</value></description></item> 
        /// <item><description><value>SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_NAME</value></description></item> 
        /// <item><description><value>SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_PWD</value></description></item> 
        /// <item><description><value>SchemaDrTestActionVM.ATTRIBUTE_NAME_VM_NAME</value></description></item>
        /// <item><description><value>SchemaDrTestActionVM.ATTRIBUTE_VM_GUEST_LOGIN_NAME</value></description></item>
        /// <item><description><value>SchemaDrTestActionVM.ATTRIBUTE_VM_GUEST_LOGIN_PWD</value></description></item>
        /// <item><description><value>SchemaDrTestActionVM.ATTRIBUTE_HOST_DOWNLOAD_FILE_PATH</value></description></item>
        /// <item><description><value>SchemaDrTestActionVM.ATTRIBUTE_VM_DOWNLOAD_FILE_SOURCE</value></description></item>
        /// </list> 
        /// </param>
        /// <returns></returns>
        public DDNode VMDownloadFile(DDNode nIn)
        {
            var nOut = DrTestActionExt.GetStubActionResultNode();
            try
            {
                nIn.Attributes.ContainsAttributesOtherwiseThrow(SchemaDrTestActionVM.ATTRIBUTE_NAME_SERVER_NAME,
                                                                     SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_NAME,
                                                                     SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_PWD,
                                                                     SchemaDrTestActionVM.ATTRIBUTE_VM_GUEST_LOGIN_NAME,
                                                                     SchemaDrTestActionVM.ATTRIBUTE_VM_GUEST_LOGIN_PWD,
                                                                     SchemaDrTestActionVM.ATTRIBUTE_VM_DOWNLOAD_FILE_SOURCE,
                                                                     SchemaDrTestActionVM.ATTRIBUTE_NAME_VM_NAME,
                                                                     SchemaDrTestActionVM.ATTRIBUTE_HOST_DOWNLOAD_FILE_PATH
                                                                     );



                int retry = nIn.Attributes.GetValue(SchemaDrTestActionVM.ATTRIBUTE_VM_ATTEMPTS_RETRY, SchemaDrTestActionVM.ATTRIBUTE_VM_ATTEMPTS_RETRY_VALUE);
                int timeOut = nIn.Attributes.GetValue(SchemaDrTestActionVM.ATTRIBUTE_VM_ATTEMPTS_TIMEOUT, SchemaDrTestActionVM.ATTRIBUTE_VM_ATTEMPTS_TIMEOUT_VALUE);

                var vm = new WrapperDrVM(nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_NAME_SERVER_NAME]);
                vm.LogIn(nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_NAME], nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_PWD]);
                vm.CopyFileFromGuestVM(nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_NAME_VM_NAME],
                                nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_VM_GUEST_LOGIN_NAME],
                                nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_VM_GUEST_LOGIN_PWD],
                                nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_VM_DOWNLOAD_FILE_SOURCE],
                                retry,
                                timeOut,
                               nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_HOST_DOWNLOAD_FILE_PATH]);
                vm.Logout();
                return nOut.SetActionResultStatusOK(string.Format(Msg.VM_DOWNLOAD_FILE_SUCCESS, nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_VM_DOWNLOAD_FILE_SOURCE], nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_NAME_VM_NAME]));

            }
            catch (Exception e)
            {

                return nOut.SetActionResultStatusFailed(string.Format(Msg.VM_DOWNLOAD_FILE_FAIL, nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_VM_DOWNLOAD_FILE_SOURCE], nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_NAME_VM_NAME], e.Message));
            }

            finally
            {
                nOut.SetActionResultNodeEndTime();
            }
        }







        /// <summary>
        /// Function to create Virtual Switch on host
        /// </summary>
        /// <param name="nIn">Action node contains the mandatory attributes:
        /// <list type="bullet">
        /// <item><description><value>SchemaDrTestActionVM.ATTRIBUTE_NAME_SERVER_NAME</value></description></item> 
        /// <item><description><value>SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_NAME</value></description></item> 
        /// <item><description><value>SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_PWD</value></description></item> 
        /// <item><description><value>SchemaDrTestActionVM.ATTRIBUTE_SWITCH_NAME</value></description></item>
        /// <item><description><value>SchemaDrTestActionVM.ATTRIBUTE_SWITCH_PORT_NUM</value></description></item>
        /// <item><description><value>SchemaDrTestActionVM.ATTRIBUTE_HOST_NAME</value></description></item>
        /// </list> 
        /// </param>
        /// <returns></returns>
        public DDNode CreateVirtualSwitchOnHost(DDNode nIn)
        {
            var nOut = DrTestActionExt.GetStubActionResultNode();

            try
            {
                nIn.Attributes.ContainsAttributesOtherwiseThrow(SchemaDrTestActionVM.ATTRIBUTE_NAME_SERVER_NAME,
                                                                     SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_NAME,
                                                                     SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_PWD,
                                                                     SchemaDrTestActionVM.ATTRIBUTE_SWITCH_NAME,
                                                                     SchemaDrTestActionVM.ATTRIBUTE_SWITCH_PORT_NUM,
                                                                     SchemaDrTestActionVM.ATTRIBUTE_HOST_NAME);

                var vm = new WrapperDrVM(nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_NAME_SERVER_NAME]);
                vm.LogIn(nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_NAME], nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_PWD]);
                vm.CreateVirtualSwitch(nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_HOST_NAME], nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_SWITCH_NAME], nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_SWITCH_PORT_NUM]);
                vm.Logout();
                return nOut.SetActionResultStatusOK(string.Format(Msg.CREATE_VIRTUAL_SWITCH_SUCCESS, nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_SWITCH_NAME], nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_HOST_NAME]));
            }
            catch (Exception e)
            {

                return nOut.SetActionResultStatusFailed(string.Format(Msg.CREATE_VIRTUAL_SWITCH_FAILED, nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_SWITCH_NAME], nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_HOST_NAME], e.Message));
            }

            finally
            {
                nOut.SetActionResultNodeEndTime();
            }
        }

        /// <summary>
        /// Remove Virtual switch on host
        /// </summary>
        /// <param name="nIn">Action node contains the mandatory attributes:
        /// <list type="bullet">
        /// <item><description><value>SchemaDrTestActionVM.ATTRIBUTE_NAME_SERVER_NAME</value></description></item> 
        /// <item><description><value>SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_NAME</value></description></item> 
        /// <item><description><value>SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_PWD</value></description></item> 
        /// <item><description><value>SchemaDrTestActionVM.ATTRIBUTE_SWITCH_NAME</value></description></item>
        /// <item><description><value>SchemaDrTestActionVM.ATTRIBUTE_SWITCH_PORT_NUM</value></description></item>
        /// <item><description><value>SchemaDrTestActionVM.ATTRIBUTE_HOST_NAME</value></description></item>
        /// </list> 
        /// </param>
        /// <returns></returns>
        public DDNode RemoveVirtualSwitchOnHost(DDNode nIn)
        {
            var nOut = DrTestActionExt.GetStubActionResultNode();

            try
            {
                nIn.Attributes.ContainsAttributesOtherwiseThrow(SchemaDrTestActionVM.ATTRIBUTE_NAME_SERVER_NAME,
                                                                     SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_NAME,
                                                                     SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_PWD,
                                                                     SchemaDrTestActionVM.ATTRIBUTE_SWITCH_NAME,
                                                                     SchemaDrTestActionVM.ATTRIBUTE_SWITCH_PORT_NUM,
                                                                     SchemaDrTestActionVM.ATTRIBUTE_HOST_NAME);

                var vm = new WrapperDrVM(nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_NAME_SERVER_NAME]);
                vm.LogIn(nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_NAME], nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_PWD]);
                vm.RemoveVirtualSwitch(nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_HOST_NAME], nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_SWITCH_NAME]);
                vm.Logout();
                return nOut.SetActionResultStatusOK(string.Format(Msg.REMOVE_VIRTUAL_SWITCH_SUCCESS, nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_SWITCH_NAME], nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_HOST_NAME]));
            }
            catch (Exception e)
            {

                return nOut.SetActionResultStatusFailed(string.Format(Msg.REMOVE_VIRTUAL_SWITCH_FAILED, nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_SWITCH_NAME], nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_HOST_NAME], e.Message));
            }

            finally
            {
                nOut.SetActionResultNodeEndTime();
            }
        }


        /// <summary>
        /// Port group wich will be used on switch
        /// </summary>
        /// <param name="nIn">Action node contains the mandatory attributes:
        /// <list type="bullet">
        /// <item><description><value>SchemaDrTestActionVM.ATTRIBUTE_NAME_SERVER_NAME</value></description></item> 
        /// <item><description><value>SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_NAME</value></description></item> 
        /// <item><description><value>SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_PWD</value></description></item> 
        /// <item><description><value>SchemaDrTestActionVM.ATTRIBUTE_SWITCH_NAME</value></description></item>
        /// <item><description><value>SchemaDrTestActionVM.ATTRIBUTE_PORT_GROUP_NAME</value></description></item>
        /// <item><description><value>SchemaDrTestActionVM.ATTRIBUTE_HOST_NAME</value></description></item>
        /// </list> 
        /// </param>
        /// <returns></returns>
        public DDNode CreatePortGrpOnSwitch(DDNode nIn)
        {
            var nOut = DrTestActionExt.GetStubActionResultNode();

            try
            {
                nIn.Attributes.ContainsAttributesOtherwiseThrow(SchemaDrTestActionVM.ATTRIBUTE_NAME_SERVER_NAME,
                                                                       SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_NAME,
                                                                       SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_PWD,
                                                                       SchemaDrTestActionVM.ATTRIBUTE_SWITCH_NAME,
                                                                       SchemaDrTestActionVM.ATTRIBUTE_PORT_GROUP_NAME,
                                                                       SchemaDrTestActionVM.ATTRIBUTE_HOST_NAME);

                var vm = new WrapperDrVM(nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_NAME_SERVER_NAME]);
                vm.LogIn(nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_NAME], nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_PWD]);
                vm.CreatePortGrp(nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_HOST_NAME], nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_SWITCH_NAME], nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_PORT_GROUP_NAME]);
                vm.Logout();
                return nOut.SetActionResultStatusOK(string.Format(Msg.CREATE_PORT_GROUP_SUCCESS, nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_PORT_GROUP_NAME], nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_HOST_NAME], nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_SWITCH_NAME]));
            }
            catch (Exception e)
            {

                return nOut.SetActionResultStatusFailed(string.Format(Msg.CREATE_PORT_GROUP_FAILED, nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_PORT_GROUP_NAME], nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_HOST_NAME], nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_SWITCH_NAME], e.Message));
            }

            finally
            {
                nOut.SetActionResultNodeEndTime();
            }
        }

        /// <summary>
        /// Remove port group on switch
        /// </summary>
        /// <param name="nIn">Action node contains the mandatory attributes:
        /// <list type="bullet">
        /// <item><description><value>SchemaDrTestActionVM.ATTRIBUTE_NAME_SERVER_NAME</value></description></item> 
        /// <item><description><value>SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_NAME</value></description></item> 
        /// <item><description><value>SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_PWD</value></description></item> 
        /// <item><description><value>SchemaDrTestActionVM.ATTRIBUTE_SWITCH_NAME</value></description></item>
        /// <item><description><value>SchemaDrTestActionVM.ATTRIBUTE_PORT_GROUP_NAME</value></description></item>
        /// <item><description><value>SchemaDrTestActionVM.ATTRIBUTE_HOST_NAME</value></description></item>
        /// </list> 
        /// </param>
        /// <returns></returns>
        public DDNode RemovePortGrpOnSwitch(DDNode nIn)
        {
            var nOut = DrTestActionExt.GetStubActionResultNode();

            try
            {
                nIn.Attributes.ContainsAttributesOtherwiseThrow(SchemaDrTestActionVM.ATTRIBUTE_NAME_SERVER_NAME,
                                                                       SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_NAME,
                                                                       SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_PWD,
                                                                       SchemaDrTestActionVM.ATTRIBUTE_SWITCH_NAME,
                                                                       SchemaDrTestActionVM.ATTRIBUTE_PORT_GROUP_NAME,
                                                                       SchemaDrTestActionVM.ATTRIBUTE_HOST_NAME);

                var vm = new WrapperDrVM(nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_NAME_SERVER_NAME]);
                vm.LogIn(nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_NAME], nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_PWD]);
                vm.RemovePortGrp(nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_HOST_NAME], nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_SWITCH_NAME], nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_PORT_GROUP_NAME]);
                vm.Logout();
                return nOut.SetActionResultStatusOK(string.Format(Msg.REMOVE_PORT_GROUP_SUCCESS, nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_PORT_GROUP_NAME], nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_HOST_NAME], nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_SWITCH_NAME]));
            }
            catch (Exception e)
            {

                return nOut.SetActionResultStatusFailed(string.Format(Msg.REMOVE_PORT_GROUP_FAILED, nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_PORT_GROUP_NAME], nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_HOST_NAME], nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_SWITCH_NAME], e.Message));
            }

            finally
            {
                nOut.SetActionResultNodeEndTime();
            }
        }


        /// <summary>
        /// Function to change Network adapters portgroup
        /// </summary>
        /// <param name="nIn">Action node contains the mandatory attributes:
        /// <list type="bullet">
        /// <item><description><value>SchemaDrTestActionVM.ATTRIBUTE_NAME_SERVER_NAME</value></description></item> 
        /// <item><description><value>SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_NAME</value></description></item> 
        /// <item><description><value>SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_PWD</value></description></item> 
        /// <item><description><value>SchemaDrTestActionVM.ATTRIBUTE_NAME_VM_NAME</value></description></item>
        /// <item><description><value>SchemaDrTestActionVM.ATTRIBUTE_PORT_GROUP_NAME</value></description></item>
        /// </list> 
        /// </param>
        /// <returns></returns>
        public DDNode ChangeVMNicPortGrp(DDNode nIn)
        {
            var nOut = DrTestActionExt.GetStubActionResultNode();

            try
            {
                nIn.Attributes.ContainsAttributesOtherwiseThrow(SchemaDrTestActionVM.ATTRIBUTE_NAME_SERVER_NAME,
                                                                      SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_NAME,
                                                                      SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_PWD,
                                                                      SchemaDrTestActionVM.ATTRIBUTE_NAME_VM_NAME,
                                                                      SchemaDrTestActionVM.ATTRIBUTE_PORT_GROUP_NAME,
                                                                      SchemaDrTestActionVM.ATTRIBUTE_HOST_NAME);


          
                var vm = new WrapperDrVM(nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_NAME_SERVER_NAME]);
                vm.LogIn(nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_NAME], nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_PWD]);
                vm.ChangeVMNicPortGroup(nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_NAME_VM_NAME], nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_PORT_GROUP_NAME], nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_HOST_NAME]);
                vm.Logout();
                return nOut.SetActionResultStatusOK(string.Format(Msg.VM_NIC_CHANGE_SUCCESS, nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_NAME_VM_NAME], nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_PORT_GROUP_NAME]));
            }
            catch (Exception e)
            {

                return nOut.SetActionResultStatusFailed(string.Format(Msg.VM_NIC_CHANGE_FAILED, nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_NAME_VM_NAME], nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_PORT_GROUP_NAME], e.Message));
            }

            finally
            {
                nOut.SetActionResultNodeEndTime();
            }
        }



        /// <summary>
        /// VM delete from inventory
        /// </summary>
        /// <param name="nIn">Action node contains the mandatory attributes:
        /// <list type="bullet">
        /// <item><description><value>SchemaDrTestActionVM.ATTRIBUTE_NAME_SERVER_NAME</value></description></item> 
        /// <item><description><value>SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_NAME</value></description></item> 
        /// <item><description><value>SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_PWD</value></description></item> 
        /// <item><description><value>SchemaDrTestActionVM.ATTRIBUTE_NAME_VM_NAME</value></description></item>
        /// <item><description><value>SchemaDrTestActionVM.ATTRIBUTE_HOST_NAME</value></description></item>
        /// </list> 
        /// </param>
        /// <returns></returns>
        public DDNode VMDestroyVM(DDNode nIn)
        {
            var nOut = DrTestActionExt.GetStubActionResultNode();

            try
            {
                nIn.Attributes.ContainsAttributesOtherwiseThrow(SchemaDrTestActionVM.ATTRIBUTE_NAME_SERVER_NAME,
                                                                      SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_NAME,
                                                                      SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_PWD,
                                                                      SchemaDrTestActionVM.ATTRIBUTE_NAME_VM_NAME,
                                                                      SchemaDrTestActionVM.ATTRIBUTE_HOST_NAME);


                var vm = new WrapperDrVM(nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_NAME_SERVER_NAME]);
                vm.LogIn(nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_NAME], nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_PWD]);
                vm.RemoveVM(nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_NAME_VM_NAME], nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_HOST_NAME]);
                vm.Logout();
                return nOut.SetActionResultStatusOK(string.Format(Msg.VM_REMOVE_VM_SUCCESS, nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_NAME_VM_NAME]));
            }
            catch (Exception e)
            {

                return nOut.SetActionResultStatusFailed(string.Format(Msg.VM_REMOVE_VM_FAILED, nIn.Attributes[SchemaDrTestActionVM.ATTRIBUTE_NAME_VM_NAME], e.Message));
            }

            finally
            {
                nOut.SetActionResultNodeEndTime();
            }
        }









        #endregion VMPowerAction
    }
}
