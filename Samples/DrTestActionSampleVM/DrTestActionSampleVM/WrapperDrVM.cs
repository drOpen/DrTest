using DrTest.DrAction.DrTestActionSampleVM.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using VMware.Vim;
using DrTest.DrAction.DrTestActionSampleVM.Res;

namespace DrTest.DrAction.DrTestActionSampleVM
{
    /// <summary>
    /// wrapps the client VMware API
    /// </summary>
    internal class WrapperDrVM
    {
        /// <summary>
        /// creats instans for specified VMware server
        /// </summary>
        /// <param name="serverUrl">vmware server. Example: <example>https://127.0.0.1/sdk</example></param>
        protected internal WrapperDrVM(string serverUrl)
        {
            this.ServerUrl = serverUrl;
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
        
        protected internal void PowerOnVM(string vmName)
        {
            try
            {
                var vm = GetVirtualMachine(vmName);
                if (vm == null) throw new VMDoesntExistExeption(vmName);
                vm.PowerOnVM_Task(null);
            }
            catch (Exception e)
            {
                throw new VMCannotChangePowerStatusException(vmName, Msg.VM_POWER_STATUS_ON, e);
            }
        }
        /// <summary>
        /// Powers off this virtual machine.
        /// </summary>
        /// <param name="vmName">virtual machine name</param>
        /// <exception cref="VMCannotChangePowerStatusException"/>
        protected internal void PowerOffVM(string vmName)
        {
            try
            {
                var vm = GetVirtualMachine(vmName);
                if (vm == null) throw new VMDoesntExistExeption(vmName);
                vm.PowerOffVM_Task();
            }
            catch (Exception e)
            {
                throw new VMCannotChangePowerStatusException(vmName, Msg.VM_POWER_STATUS_OFF, e);
            }
        }
        /// <summary>
        /// Suspends execution in this virtual machine.
        /// </summary>
        /// <param name="vmName">virtual machine name</param>
        /// <exception cref="VMCannotChangePowerStatusException"/>
        protected internal void SuspendVM(string vmName)
        {
            try
            {
                var vm = GetVirtualMachine(vmName);
                if (vm == null) throw new VMDoesntExistExeption(vmName);
                vm.SuspendVM_Task();
            }
            catch (Exception e)
            {
                throw new VMCannotChangePowerStatusException(vmName, Msg.VM_POWER_STATUS_SUSPEND, e);
            }
        }
        /// <summary>
        /// Resets power on this virtual machine. If the current state is poweredOn, then this method first performs powerOff(hard). Once the power state is poweredOff, then this method performs powerOn(option).
        /// Although this method functions as a powerOff followed by a powerOn, the two operations are atomic with respect to other clients, meaning that other power operations cannot be performed until the reset method completes.
        /// </summary>
        /// <param name="vmName">virtual machine name</param>
        /// <exception cref="VMCannotChangePowerStatusException"/>
        protected internal void ResetVM(string vmName)
        {
            try
            {
                var vm = GetVirtualMachine(vmName);
                if (vm == null) throw new VMDoesntExistExeption(vmName);
                vm.ResetVM_Task();
            }
            catch (Exception e)
            {
                throw new VMCannotChangePowerStatusException(vmName, Msg.VM_POWER_STATUS_RESET, e);
            }
        }
        /// <summary>
        /// returns virtual machine by name
        /// </summary>
        /// <param name="vmName"></param>
        /// <returns></returns>
        protected VirtualMachine GetVirtualMachine(string vmName)
        {
            return FindEntityViewByName<VirtualMachine>(vmName);
        }
        /// <summary>
        /// returns entity by name
        /// </summary>
        /// <typeparam name="T">type of entity</typeparam>
        /// <param name="name">object name for search</param>
        /// <returns></returns>
        protected T FindEntityViewByName<T>(string name) where T : EntityViewBase
        {
            NameValueCollection filter = new NameValueCollection();
            filter.Add("name", '^' + name + '$');
            return (T)vClient.FindEntityView(typeof(T), null, filter, null);
        }

        #endregion VM Action

        ~WrapperDrVM()
        {
            this.Logout();
        }
    }
}
