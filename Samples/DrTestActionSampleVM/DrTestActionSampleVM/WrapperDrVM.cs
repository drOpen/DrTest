/*
  WrapperDrVM.cs -- wrapper for vm, September 11, 2016
  
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

using DrTest.DrAction.DrTestActionSampleVM.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using VMware.Vim;
using DrTest.DrAction.DrTestActionSampleVM;
using DrTest.DrAction.DrTestActionSampleVM.Res;

namespace DrTest.DrAction.DrTestActionSampleVM
{
    /// <summary>
    /// wrapps the client VMware API
    /// </summary>
    internal class WrapperDrVM
    {
        /// <summary>
        /// binds vm power actions with power description and corresponding function 
        /// </summary>
        internal struct WrpVMPower
        {
            public WrpVMPower(string description, WrpVMPowerActionDelegate action)
                : this()
            {
                Description = description;
                Action = action;
            }
            /// <summary>
            /// delegate to corresponding power action function
            /// </summary>
            public WrpVMPowerActionDelegate Action { get; private set; }
            /// <summary>
            /// description of power action
            /// </summary>
            public string Description { get; private set; }
        }
        /// <summary>
        /// delegate to corresponding power action function
        /// </summary>
        /// <param name="name">virtual machine name</param>
        internal protected delegate void WrpVMPowerActionDelegate(string name);
        /// <summary>
        /// dictionary contains binds vm power actions with power description and corresponding function 
        /// </summary>
        internal protected Dictionary<vmPowerAction, WrpVMPower> dVMPowerAction;    
        /// <summary>
        /// creats instans for specified VMware server
        /// </summary>
        /// <param name="serverUrl">vmware server. Example: <example>https://127.0.0.1/sdk</example></param>
        protected internal WrapperDrVM(string serverUrl)
        {
            this.ServerUrl = serverUrl;

            var vmPADescription = GetVMPowerActionDescription();
            dVMPowerAction = new Dictionary<vmPowerAction, WrpVMPower>()
            {
                {vmPowerAction.VM_POWER_ON, new WrpVMPower(vmPADescription[vmPowerAction.VM_POWER_ON], PowerOnVM)},
                {vmPowerAction.VM_POWER_OFF, new WrpVMPower( vmPADescription[vmPowerAction.VM_POWER_OFF], PowerOffVM)},
                {vmPowerAction.VM_POWER_RESET, new WrpVMPower(vmPADescription[vmPowerAction.VM_POWER_RESET],ResetVM)},
                {vmPowerAction.VM_POWER_SUSPEND, new WrpVMPower(vmPADescription[vmPowerAction.VM_POWER_SUSPEND], SuspendVM)}
            };
        }

        /// <summary>
        /// Power actions supported by virtual machine
        /// </summary>
        internal enum vmPowerAction
        {
            VM_POWER_ON,
            VM_POWER_OFF,
            VM_POWER_RESET,
            VM_POWER_SUSPEND
        }

        /// <summary>
        /// storages of VimClient
        /// </summary>
        protected VimClient vClient;
        /// <summary>
        /// gets url for the current VMware server 
        /// </summary>
        protected string ServerUrl { get; private set; }


        #region  connection
        /// <summary>
        /// login to VMware server by specified credential
        /// </summary>
        /// <param name="userName">user name</param>
        /// <param name="pwd">password for specified user</param>
        protected internal void LogIn(string userName, string pwd)
        {
            if (vClient == null) vClient = new VimClient();
            vClient.Login(ServerUrl, userName, pwd);
        }

        /// <summary>
        /// disconnect for current server and set null value of VimClient instance 
        /// </summary>
        protected internal void Disconnect()
        {
            if (vClient != null)
            {
                try
                {
                    vClient.Disconnect();
                    vClient = null;
                }
                catch { }
            }
        }
        /// <summary>
        /// logout from current server and set null value of VimClient instance
        /// </summary>
        protected internal void Logout()
        {
            if (vClient != null)
            {
                try
                {
                    vClient.Logout();
                    if (vClient != null) vClient.Disconnect();
                    vClient = null;
                }
                catch { }
            }
        }

        #endregion connection

        #region VM Action
        /// <summary>
        /// Powers on this virtual machine. 
        /// If the virtual machine is suspended, this method resumes execution from the suspend point. 
        /// When powering on a virtual machine in a cluster, the system might implicitly or due to the host argument, do an implicit relocation of the virtual machine to another host. 
        /// Hence, errors related to this relocation can be thrown.
        /// </summary>
        /// <param name="vmName">virtual machine name</param>
        /// <exception cref="VMCannotChangePowerStatusException"/>

        protected virtual internal void PowerOnVM(string vmName)
        {
            VMChangePowerState(vmName, vmPowerAction.VM_POWER_ON);
        }
        /// <summary>
        /// Powers off this virtual machine.
        /// </summary>
        /// <param name="vmName">virtual machine name</param>
        /// <exception cref="VMCannotChangePowerStatusException"/>
        protected virtual internal void PowerOffVM(string vmName)
        {
            VMChangePowerState(vmName, vmPowerAction.VM_POWER_OFF);
        }
        /// <summary>
        /// Suspends execution in this virtual machine.
        /// </summary>
        /// <param name="vmName">virtual machine name</param>
        /// <exception cref="VMCannotChangePowerStatusException"/>
        protected virtual internal void SuspendVM(string vmName)
        {
            VMChangePowerState(vmName, vmPowerAction.VM_POWER_SUSPEND);
        }
        /// <summary>
        /// Resets power on this virtual machine. If the current state is poweredOn, then this method first performs powerOff(hard). Once the power state is poweredOff, then this method performs powerOn(option).
        /// Although this method functions as a powerOff followed by a powerOn, the two operations are atomic with respect to other clients, meaning that other power operations cannot be performed until the reset method completes.
        /// </summary>
        /// <param name="vmName">virtual machine name</param>
        /// <exception cref="VMCannotChangePowerStatusException"/>
        protected virtual internal void ResetVM(string vmName)
        {
            VMChangePowerState(vmName, vmPowerAction.VM_POWER_RESET);
        }
        /// <summary>
        /// Changes power status of virtual michine
        /// </summary>
        /// <param name="vmName">virtual machine name</param>
        /// <param name="vmPA">power actions</param>
        /// <exception cref="VMCannotChangePowerStatusException"/>
        /// <exception cref="VMForbiddenPowerAction"/>
        protected virtual internal void VMChangePowerState(string vmName, vmPowerAction vmPA)
        {
            try
            {
                var vm = GetVirtualMachine(vmName);
                if (vm == null) throw new VMDoesntExistExeption(vmName);
                switch (vmPA)
                {
                    case vmPowerAction.VM_POWER_RESET:
                        vm.ResetVM_Task();
                        break;
                    case vmPowerAction.VM_POWER_ON:
                        vm.PowerOnVM_Task();
                        break;
                    case vmPowerAction.VM_POWER_SUSPEND:
                        vm.SuspendVM_Task();
                        break;
                    case vmPowerAction.VM_POWER_OFF:
                        vm.PowerOffVM_Task();
                        break;
                    default:
                        throw new VMForbiddenPowerAction(vmPA.ToString());
                }
            }
            catch (Exception e)
            {
                throw new VMCannotChangePowerStatusException(vmName, dVMPowerAction[vmPA].Description, e);
            }
        }


        /// <summary>
        /// returns virtual machine by name
        /// </summary>
        /// <param name="vmName"></param>
        /// <returns></returns>
        protected virtual VirtualMachine GetVirtualMachine(string vmName)
        {
            return FindEntityViewByName<VirtualMachine>(vmName);
        }
        /// <summary>
        /// returns entity by name
        /// </summary>
        /// <typeparam name="T">type of entity</typeparam>
        /// <param name="name">object name for search</param>
        /// <returns></returns>
        protected virtual T FindEntityViewByName<T>(string name) where T : EntityViewBase
        {
            NameValueCollection filter = new NameValueCollection();
            filter.Add("name", '^' + name + '$');
            return (T)vClient.FindEntityView(typeof(T), null, filter, null);
        }

        #endregion VM Action

        #region static
        /// <summary>
        /// returns dictionary with binds power action with corresponding description
        /// </summary>
        /// <returns></returns>
        internal static Dictionary<vmPowerAction, string> GetVMPowerActionDescription()
        {
            return
                new Dictionary<vmPowerAction, string> 
                {
                    {vmPowerAction.VM_POWER_ON, Msg.VM_POWER_STATUS_ON},
                    {vmPowerAction.VM_POWER_OFF, Msg.VM_POWER_STATUS_OFF},
                    {vmPowerAction.VM_POWER_RESET, Msg.VM_POWER_STATUS_RESET},
                    {vmPowerAction.VM_POWER_SUSPEND, Msg.VM_POWER_STATUS_SUSPEND}
                };
        }
        #endregion static


        ~WrapperDrVM()
        {
            this.Logout();
        }
    }
}
