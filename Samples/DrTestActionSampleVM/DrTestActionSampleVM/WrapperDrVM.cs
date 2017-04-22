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
using DrTest.DrAction.DrTestActionSampleVM.Res.Exceptions;
using System.Threading;

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
        /// returns HostSystem by name
        /// </summary>
        /// <param name="vmName"></param>
        /// <returns></returns>
        protected virtual HostSystem GetHostSystem(string vmName)
        {
            return FindEntityViewByName<HostSystem>(vmName);
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



        /// <summary>
        /// 
        /// </summary>
        /// <param name="vm"></param>
        /// <param name="snapshotName"></param>
        /// <returns></returns>
        private static ManagedObjectReference HelperSnapshotChecker(VirtualMachineSnapshotTree[] rootSnapshotList, string snapshotName, VirtualMachine vm)
        {



            ManagedObjectReference resultInfo = new ManagedObjectReference();
            int count = 0;
            if (rootSnapshotList != null)
            {
                VirtualMachineSnapshotTree snapshot = rootSnapshotList[0];
                for (int i = 0; i < rootSnapshotList.Length; i++)
                {
                    snapshot = rootSnapshotList[i];
                    String name = snapshot.Name;
                    if (name == snapshotName)
                    {
                        resultInfo = snapshot.Snapshot;
                        count++;
                        if (count > 1) throw new VMSnapshotHaveMoreThenOneExeption(vm.Name, snapshotName);
                    }
                    if (rootSnapshotList[i].ChildSnapshotList != null)
                    {
                        var tempm = HelperSnapshotChecker(rootSnapshotList[i].ChildSnapshotList, snapshotName, vm);
                        if (tempm.Value != null)
                        {
                            resultInfo = tempm;
                            count++;
                            if (count > 1) throw new VMSnapshotHaveMoreThenOneExeption(vm.Name, snapshotName);
                        }
                    }
                }
            }
            return resultInfo;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="vm"></param>
        /// <param name="host"></param>
        /// <returns></returns>
        private static bool HelperCheckerIncludeHost(VirtualMachine vm, HostSystem host)
        {
            foreach (var VMHost in host.Vm)
            {
                if (VMHost.ToString() == vm.MoRef.ToString())
                {
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="vmName"></param>
        /// <param name="newName"></param>
        internal virtual void CloneVM(string vmName, string newName, string hostName, string snapshotName)
        {
            var vm = GetVirtualMachine(vmName);
            if (vm == null) throw new VMDoesntExistExeption(vmName);
            ManagedObjectReference location = new ManagedObjectReference();
            location.Type = vm.Parent.Type;
            location.Value = vm.Parent.Value;

            var host = GetHostSystem(hostName);
            if (host == null) throw new HostDoesntExistExeption(hostName);


            if (!HelperCheckerIncludeHost(vm, host)) throw new HostDoesnotcontainVM(vmName, hostName);
            if (vm.Snapshot == null) throw new VMSnapshotDoesntExistExeption(snapshotName,vmName);
            VirtualMachineSnapshotTree[] rootSnapshotList = vm.Snapshot.RootSnapshotList;

            ManagedObjectReference snapshot = HelperSnapshotChecker(rootSnapshotList, snapshotName, vm);
            if (snapshot.Value==null) throw new VMSnapshotDoesntExistExeption(vm.Name, snapshotName);

            VirtualMachineRelocateSpec relocSpec = new VirtualMachineRelocateSpec();
            relocSpec.DiskMoveType = VirtualMachineRelocateDiskMoveOptions.createNewChildDiskBacking.ToString();

            VirtualMachineCloneSpec cloneSpec = new VirtualMachineCloneSpec();
            cloneSpec.PowerOn = false;
            cloneSpec.Template = false;
            cloneSpec.Location = relocSpec;
            cloneSpec.Snapshot = snapshot;
        
            cloneSpec.Config = new VirtualMachineConfigSpec();
            cloneSpec.Config.Uuid = vm.Config.Uuid;

            vm.CloneVM(location, newName, cloneSpec);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hostName"></param>
        /// <param name="switchName"></param>
        /// <param name="portNum"></param>
        internal virtual void CreateVirtualSwitch(string hostName, string switchName, string portNum)
        {
            var host = GetHostSystem(hostName);
            if (host == null) throw new HostDoesntExistExeption(hostName);
            HostNetworkSystem networkSystem = (HostNetworkSystem)vClient.GetView(host.ConfigManager.NetworkSystem, null);
            HostVirtualNicSpec virtNicSpec = networkSystem.NetworkConfig.Vnic[0].Spec;

            ManagedObjectReference dcmor = new ManagedObjectReference(); ;
            dcmor.Type = networkSystem.MoRef.Type;
            dcmor.Value = networkSystem.MoRef.Value;

            HostNetworkSystem _switch = new HostNetworkSystem(vClient, dcmor);
            HostVirtualSwitchSpec spec = new HostVirtualSwitchSpec();
            spec.NumPorts = Convert.ToInt32(portNum);
            _switch.AddVirtualSwitch(switchName, spec);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hostName"></param>
        /// <param name="switchName"></param>
        /// <param name="portgrpName"></param>
        internal virtual void CreatePortGrp(string hostName, string switchName, string portgrpName)
        {
            var host = GetHostSystem(hostName);
            if (host == null) throw new HostDoesntExistExeption(hostName);
            HostNetworkSystem networkSystem = (HostNetworkSystem)vClient.GetView(host.ConfigManager.NetworkSystem, null);
            HostVirtualNicSpec virtNicSpec = networkSystem.NetworkConfig.Vnic[0].Spec;

            ManagedObjectReference dcmor = new ManagedObjectReference(); ;
            dcmor.Type = networkSystem.MoRef.Type;
            dcmor.Value = networkSystem.MoRef.Value;

            HostNetworkSystem _switch = new HostNetworkSystem(vClient, dcmor);

            HostPortGroupSpec portgrp = new HostPortGroupSpec();
            portgrp.Name = portgrpName;
            portgrp.VswitchName = switchName;
            portgrp.Policy = new HostNetworkPolicy();

            _switch.AddPortGroup(portgrp);

        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="hostName"></param>
        /// <param name="switchName"></param>
        /// <param name="portgrpName"></param>
        internal virtual void RemovePortGrp(string hostName, string switchName, string portgrpName)
        {
            var host = GetHostSystem(hostName);
            if (host == null) throw new HostDoesntExistExeption(hostName);
            HostNetworkSystem networkSystem = (HostNetworkSystem)vClient.GetView(host.ConfigManager.NetworkSystem, null);
            HostVirtualNicSpec virtNicSpec = networkSystem.NetworkConfig.Vnic[0].Spec;

            ManagedObjectReference dcmor = new ManagedObjectReference(); ;
            dcmor.Type = networkSystem.MoRef.Type;
            dcmor.Value = networkSystem.MoRef.Value;

            HostNetworkSystem _switch = new HostNetworkSystem(vClient, dcmor);

            HostPortGroupSpec portgrp = new HostPortGroupSpec();
            portgrp.Name = portgrpName;
            portgrp.VswitchName = switchName;
            portgrp.Policy = new HostNetworkPolicy();

            _switch.RemovePortGroup(portgrpName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hostName"></param>
        /// <param name="switchName"></param>
        /// <param name="portNum"></param>
        internal virtual void RemoveVirtualSwitch(string hostName, string switchName)
        {
            var host = GetHostSystem(hostName);
            if (host == null) throw new HostDoesntExistExeption(hostName);
            HostNetworkSystem networkSystem = (HostNetworkSystem)vClient.GetView(host.ConfigManager.NetworkSystem, null);
            HostVirtualNicSpec virtNicSpec = networkSystem.NetworkConfig.Vnic[0].Spec;

            ManagedObjectReference dcmor = new ManagedObjectReference(); ;
            dcmor.Type = networkSystem.MoRef.Type;
            dcmor.Value = networkSystem.MoRef.Value;

            HostNetworkSystem _switch = new HostNetworkSystem(vClient, dcmor);
            _switch.RemoveVirtualSwitch(switchName);

        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="vmName"></param>
        /// <param name="portGroup"></param>
        internal virtual void ChangeNicPortGroup(string vmName, string portGroup)
        {

            var vm = GetVirtualMachine(vmName);
            if (vm == null) throw new VMDoesntExistExeption(vmName);
            VirtualMachineConfigSpec vmConfigSpec = new VirtualMachineConfigSpec();
            VirtualDeviceConfigSpec[] nicSpec = HelperGetNICDeviceConfigSpec(vm, portGroup);
            vmConfigSpec.DeviceChange = nicSpec;
            vm.ReconfigVM(vmConfigSpec);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="vm"></param>
        /// <param name="pgname"></param>
        /// <returns></returns>
        private static VirtualDeviceConfigSpec[] HelperGetNICDeviceConfigSpec(VirtualMachine vm, String pgname)
        {





            List<VirtualDeviceConfigSpec> updates = new List<VirtualDeviceConfigSpec>();
            VirtualMachineConfigInfo vmConfigInfo = vm.Config;
            VirtualDevice[] vds = vm.Config.Hardware.Device;

            for (int i = 0; i < vds.Length; i++)
            {
                if (vds[i] is VirtualEthernetCard)
                {
                    VirtualDeviceConfigSpec nicSpec = new VirtualDeviceConfigSpec();
                    nicSpec.Operation = VirtualDeviceConfigSpecOperation.edit;
                    VirtualEthernetCardNetworkBackingInfo oldbi = (VirtualEthernetCardNetworkBackingInfo)vds[i].Backing;
                    VirtualEthernetCardNetworkBackingInfo bi = new VirtualEthernetCardNetworkBackingInfo();
                    bi.DeviceName = pgname;
                    vds[i].Backing = bi;
                    nicSpec.Device = vds[i];
                    updates.Add(nicSpec);
                }
            }

            VirtualDeviceConfigSpec[] ret = new VirtualDeviceConfigSpec[updates.Count];
            int x = 0;
            foreach (var test in updates)
            {
                ret[x] = test;
                x++;
            }
            return ret;

        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="vmName"></param>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <param name="application"></param>
        /// <param name="retry"></param>
        /// <param name="timeout"></param>
        internal virtual void CheckVMState(string vmName, string login, string password, string application, string retry, string timeout)
        {
            var vm = GetVirtualMachine(vmName);
            if (vm == null) throw new VMDoesntExistExeption(vmName);
            int retrycount = Convert.ToInt32(retry);
            for (int j = 0; j <= retrycount; j++)
            {
                vm = GetVirtualMachine(vmName);
                if (vm == null) throw new VMDoesntExistExeption(vmName);
                if (Convert.ToString(vm.Guest.ToolsStatus.Value) == "toolsOk") break;
                    Thread.Sleep((Convert.ToInt32(timeout) * 1000));
            }
            if (Convert.ToString(vm.Guest.ToolsStatus.Value) == "toolsNotRunning") throw new CannotConnectToAgent(vmName, retry, timeout);
            NamePasswordAuthentication auth = new NamePasswordAuthentication();
            auth.Username = login;
            auth.Password = password;
            auth.InteractiveSession = false;
         
            long[] pids = new long[] { };
            int count = 0;
            for (int j = 0; j <= retrycount; j++)
            {
                    vm = GetVirtualMachine(vmName);
                if (vm == null) throw new VMDoesntExistExeption(vmName);
                var _gpm = new VMware.Vim.GuestProcessManager(vm.Client, new VMware.Vim.ManagedObjectReference("GuestProcessManager-guestOperationsProcessManager"));
                var processInfo = _gpm.ListProcessesInGuest(vm.MoRef, auth, pids);
                    foreach (var uno in processInfo)
                    {
                        if (uno.Name == application) count++;
                    }
                if (count == 3) break;
                Thread.Sleep((Convert.ToInt32(timeout) * 1000));
            }
            if (count <= 1) throw new ApplicationDoNotFoundOnVM(application, vmName, retry, timeout);
        }







        /// <summary>
        /// 
        /// </summary>
        /// <param name="vmName"></param>
        internal virtual void RemoveVM(string vmName)
        {
            var vm = GetVirtualMachine(vmName);
            vm.Destroy();


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
