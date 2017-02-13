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
using System.Threading;
using System.IO;

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
        /// Find all snapshots in Vm Snaoshots Tree. Return MO if its have only one of it. Throw exception if it have more then one 
        /// Recursion. RootTree -> ChildTree -> RootTree
        /// </summary>
        /// <param name="rootSnapshotList">List of Root Tree on VM</param>
        /// <param name="snapshotName">Snapshot name</param>
        /// <param name="vm">Object virtual Machine for Exception</param>
        /// <returns></returns>            
        private static ManagedObjectReference FindAllVMSnapshotsByName(VirtualMachineSnapshotTree[] rootSnapshotList, string snapshotName, VirtualMachine vm)
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
                        var tempm = FindAllVMSnapshotsByName(rootSnapshotList[i].ChildSnapshotList, snapshotName, vm);
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
        /// Checker does VM running on Host.
        /// Return True or False
        /// </summary>
        /// <param name="vm">Object Virtual Machine</param>
        /// <param name="host">Object Host System</param>
        /// <returns></returns>
        private static bool CheckerDoesHostIncludeVM(VirtualMachine vm, HostSystem host)
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
        /// Function of Clonning Virtual Machine
        /// </summary>
        /// <param name="VMName">Target VM Name</param>
        /// <param name="newVMName">New VM Name</param>
        /// <param name="hostName">Host on which old VM running</param>
        /// <param name="targetSnapshot">Snapshot from which will be create Clone VM</param>
        internal virtual void CloneVM(string VMName, string newVMName, string hostName, string targetSnapshot)
        {
            var vm = GetVirtualMachine(VMName);
            if (vm == null) throw new VMDoesntExistExeption(VMName);
            ManagedObjectReference location = new ManagedObjectReference();
            location.Type = vm.Parent.Type;
            location.Value = vm.Parent.Value;

            var host = GetHostSystem(hostName);
            if (host == null) throw new HostDoesntExistExeption(hostName);


            if (!CheckerDoesHostIncludeVM(vm, host)) throw new HostDoesNotContainVM(VMName, hostName);
            if (vm.Snapshot == null) throw new VMSnapshotDoesntExistExeption(targetSnapshot,VMName);
            VirtualMachineSnapshotTree[] rootSnapshotList = vm.Snapshot.RootSnapshotList;

            ManagedObjectReference snapshot = FindAllVMSnapshotsByName(rootSnapshotList, targetSnapshot, vm);
            if (snapshot.Value==null) throw new VMSnapshotDoesntExistExeption(vm.Name, targetSnapshot);

            VirtualMachineRelocateSpec relocSpec = new VirtualMachineRelocateSpec();
            relocSpec.DiskMoveType = VirtualMachineRelocateDiskMoveOptions.createNewChildDiskBacking.ToString();

            VirtualMachineCloneSpec cloneSpec = new VirtualMachineCloneSpec();
            cloneSpec.PowerOn = false;
            cloneSpec.Template = false;
            cloneSpec.Location = relocSpec;
            cloneSpec.Snapshot = snapshot;
        
            cloneSpec.Config = new VirtualMachineConfigSpec();
            cloneSpec.Config.Uuid = vm.Config.Uuid;

            vm.CloneVM(location, newVMName, cloneSpec);
        }

        /// <summary>
        /// Function to create virtual switch.
        /// </summary>
        /// <param name="hostName">Host on which will be create switch </param>
        /// <param name="switchName">Switch name</param>
        /// <param name="portNum">Number of ports on Switch</param>
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
        /// Function to create port group on virtual switch 
        /// </summary>
        /// <param name="hostName">Host where running switch</param>
        /// <param name="switchName">Switch name</param>
        /// <param name="portGrpName">Will create PortGroup with this name</param>
        internal virtual void CreatePortGrp(string hostName, string switchName, string portGrpName)
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
            portgrp.Name = portGrpName;
            portgrp.VswitchName = switchName;
            portgrp.Policy = new HostNetworkPolicy();

            _switch.AddPortGroup(portgrp);
        }

        /// <summary>
        /// Function to delete port group on virtual switch 
        /// </summary>
        /// <param name="hostName">Host where running switch</param>
        /// <param name="switchName">Switch name</param>
        /// <param name="portGrpName">Will delete PortGroup with this name</param>
        internal virtual void RemovePortGrp(string hostName, string switchName, string portGrpName)
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
            portgrp.Name = portGrpName;
            portgrp.VswitchName = switchName;
            portgrp.Policy = new HostNetworkPolicy();

            _switch.RemovePortGroup(portGrpName);
        }

        /// <summary>
        /// Function to delete virtual switch.
        /// </summary>
        /// <param name="hostName">Host where running switch</param>
        /// <param name="switchName">Switch name</param>
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
        /// Function which will reconfig VM network adapter to named Port Group
        /// </summary>
        /// <param name="vmName">Virtual machine Name</param>
        /// <param name="portGroup">Port Group Name</param>
        internal virtual void ChangeVMNicPortGroup(string vmName, string portGroup, string hostName)
        {
            var vm = GetVirtualMachine(vmName);
            if (vm == null) throw new VMDoesntExistExeption(vmName);
            var host = GetHostSystem(hostName);
            if (host == null) throw new HostDoesntExistExeption(hostName);
            if (!CheckerDoesHostIncludeVM(vm, host)) throw new HostDoesNotContainVM(vmName, hostName);
            VirtualMachineConfigSpec vmConfigSpec = new VirtualMachineConfigSpec();
            VirtualDeviceConfigSpec[] nicSpec = ChangeAllNetworkAdaptersInVMConfig(vm, portGroup);
            vmConfigSpec.DeviceChange = nicSpec;
            vm.ReconfigVM(vmConfigSpec);
        }

        /// <summary>
        /// Function Which will find all Network adapters and apply settings to Port Group Name
        /// </summary>
        /// <param name="vm">Virtual Machine Name</param>
        /// <param name="portGroup">Port Group Name</param>
        /// <returns></returns>
        private static VirtualDeviceConfigSpec[] ChangeAllNetworkAdaptersInVMConfig(VirtualMachine vm, string portGroup)
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
                    bi.DeviceName = portGroup;
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
        /// Function for Checking state of started virtual machine. Will wait for a while. until tool and required application is started.
        /// </summary>
        /// <param name="vmName">Virtual Machine Name</param>
        /// <param name="guestLogin">Guest Login / credentials</param>
        /// <param name="guestPwd">Guest Password / credentials</param>
        /// <param name="applicationName">Applection "EXPLORER>EXE" or something else</param> 
        /// <param name="attempts">Count of retry</param>
        /// <param name="attemptsTimeOut">Time out in Second. To wait between attempts </param>
        internal virtual void CheckVMState(string vmName, string guestLogin, string guestPwd, string applicationName, int attempts, int attemptsTimeOut)
        {
            VirtualMachine vm = null;
            for (int j = 0; j <= attempts; j++)
            {
                vm = GetVirtualMachine(vmName);
                if (vm == null) throw new VMDoesntExistExeption(vmName);
                if (Convert.ToString(vm.Guest.ToolsStatus.Value) == SchemaEPGuestStates.VM_TOOL_STATE_OK) break;
                    Thread.Sleep((attemptsTimeOut * 1000));
            }
            if (Convert.ToString(vm.Guest.ToolsStatus.Value) == SchemaEPGuestStates.VM_TOOL_STATE_NOT_STARTED) throw new CannotConnectToAgent(vmName, attempts, attemptsTimeOut);
            NamePasswordAuthentication auth = new NamePasswordAuthentication();
            auth.Username = guestLogin;
            auth.Password = guestPwd;
            auth.InteractiveSession = false;
         
            long[] pids = new long[] { };
            int count = 0;
            for (int j = 0; j <= attempts; j++)
            {
                    vm = GetVirtualMachine(vmName);
                if (vm == null) throw new VMDoesntExistExeption(vmName);
                var GuestProcessManager = new VMware.Vim.GuestProcessManager(vm.Client, new VMware.Vim.ManagedObjectReference("GuestProcessManager-guestOperationsProcessManager"));
                var processInfo = GuestProcessManager.ListProcessesInGuest(vm.MoRef, auth, pids);
                    foreach (var uno in processInfo)
                    {
                    if ((uno.Name.ToUpper()) == (applicationName.ToUpper())) count++; break;
                    }
                if (count == 3) return;
                Thread.Sleep((attemptsTimeOut * 1000));
            }
            throw new ApplicationDoNotFoundOnVM(applicationName, vmName, attempts, attemptsTimeOut);
        }



        /// <summary>
        /// Function to start Application on VM
        /// </summary>
        /// <param name="vmName">Virtual machine Name</param>
        /// <param name="guestLogin">Guest Login</param>
        /// <param name="guestPwd">Guest password</param>
        /// <param name="applicationPath">Path to applicqations you want to start</param>
        /// <param name="attempts">retry to find VM tool</param>
        /// <param name="attemptsTimeOut">Attempts timeout</param>
        /// <param name="arguments">Agruments to start with Application</param>
        internal virtual void StartVMApplication(string vmName, string guestLogin, string guestPwd, string applicationPath, int attempts, int attemptsTimeOut, string arguments)
        {
            VirtualMachine vm = null;
            for (int j = 0; j <= attempts; j++)
            {
                vm = GetVirtualMachine(vmName);
                if (vm == null) throw new VMDoesntExistExeption(vmName);
                if (Convert.ToString(vm.Guest.ToolsStatus.Value) == SchemaEPGuestStates.VM_TOOL_STATE_OK) break;
                Thread.Sleep((attemptsTimeOut * 1000));
            }
            if (Convert.ToString(vm.Guest.ToolsStatus.Value) == SchemaEPGuestStates.VM_TOOL_STATE_NOT_STARTED) throw new CannotConnectToAgent(vmName, attempts, attemptsTimeOut);
            NamePasswordAuthentication auth = new NamePasswordAuthentication();
            auth.Username = guestLogin;
            auth.Password = guestPwd;
            auth.InteractiveSession = false;

            GuestProgramSpec progSpec = new GuestProgramSpec();
            progSpec.ProgramPath = applicationPath;
            progSpec.Arguments = arguments;

            vm = GetVirtualMachine(vmName);
            if (vm == null) throw new VMDoesntExistExeption(vmName);
            var GuestProcessManager = new VMware.Vim.GuestProcessManager(vm.Client, new VMware.Vim.ManagedObjectReference("GuestProcessManager-guestOperationsProcessManager"));
            GuestProcessManager.StartProgramInGuest(vm.MoRef, auth, progSpec);
        }


        /// <summary>
        /// Function to copy file to destination on VM.
        /// </summary>
        /// <param name="vmName">Virtual machine Name</param>
        /// <param name="guestLogin">Guest Login</param>
        /// <param name="guestPwd">Guest password</param>
        /// <param name="pathToFileOnHost">Path to Sourcce file</param>
        /// <param name="pathToFileOnVM">Path to Destination file on VM, will be copy to</param>
        /// <param name="attempts">retry to find VM tool</param>
        /// <param name="attemptsTimeOut">Attempts timeout</param>
        /// <param name="hostName">Host where running switch</param>
        internal virtual void CopyFileToGuestVM(string vmName, string guestLogin, string guestPwd, string pathToFileOnHost, string pathToFileOnVM, string hostName, int attempts, int attemptsTimeOut)
        {
            VirtualMachine vm = null;
            for (int j = 0; j <= attempts; j++)
            {
                vm = GetVirtualMachine(vmName);
                if (vm == null) throw new VMDoesntExistExeption(vmName);
                if (Convert.ToString(vm.Guest.ToolsStatus.Value) == SchemaEPGuestStates.VM_TOOL_STATE_OK) break;
                Thread.Sleep((attemptsTimeOut * 1000));
            }
            if (Convert.ToString(vm.Guest.ToolsStatus.Value) == SchemaEPGuestStates.VM_TOOL_STATE_NOT_STARTED) throw new CannotConnectToAgent(vmName, attempts, attemptsTimeOut);

            ManagedObjectReference MoRefFileManager = new ManagedObjectReference("guestOperationsFileManager");
            GuestFileManager VMFileManager = new GuestFileManager(vClient, MoRefFileManager);
            ManagedObjectReference MoRefAuthManager = new ManagedObjectReference("guestOperationsAuthManager");
            GuestAuthManager VMAuthManager = new GuestAuthManager(vClient, MoRefAuthManager);

            NamePasswordAuthentication auth = new NamePasswordAuthentication();
            auth.Username = guestLogin;
            auth.Password = guestPwd;
            auth.InteractiveSession = false;

            System.IO.FileInfo FileToTransfer = new System.IO.FileInfo(pathToFileOnHost);
            GuestFileAttributes GFA = new GuestFileAttributes()
            {
                AccessTime = FileToTransfer.LastAccessTimeUtc,
                ModificationTime = FileToTransfer.LastWriteTimeUtc
            };

                string TransferOutput = VMFileManager.InitiateFileTransferToGuest(vm.MoRef, auth, pathToFileOnVM, GFA, FileToTransfer.Length, false);


            using (var web = new System.Net.WebClient())
            {
                web.UploadFile(TransferOutput.Replace("*", hostName), "PUT", pathToFileOnHost);
            }

        }

        /// <summary>
        /// Function to delete Virtual machine
        /// </summary>
        /// <param name="vmName">Virtual machine</param>
        /// <param name="hostName">Host Name</param>
        internal virtual void RemoveVM(string vmName, string hostName)
        {
            var vm = GetVirtualMachine(vmName);
            var host = GetHostSystem(hostName);
            if (host == null) throw new HostDoesntExistExeption(hostName);
            if (!CheckerDoesHostIncludeVM(vm, host)) throw new HostDoesNotContainVM(vmName, hostName);

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
