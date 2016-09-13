using DrTestActionSampleVM.DrAction.DrTest.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using VMware.Vim;


namespace DrTestActionSampleVM.DrAction.DrTest
{
    internal class WrapperDrVM
    {

        protected internal WrapperDrVM(string serverUrl)
        {
            this.ServerUrl = serverUrl;
        }

        protected VimClient vClient;
        protected string ServerUrl { get; private set; }


        #region  connection
        protected internal void LogIn(string userName, string pwd)
        {
            if (vClient == null) vClient = new VimClient();
            vClient.Login(ServerUrl, userName, pwd);
        }

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

        protected internal void PowerOnVM(string vmName)
        {
            var vm = GetVirtualMachine(vmName);
            if (vm == null) throw new VMDoesntExistExeption(vmName);
            vm.PowerOnVM_Task(null);
        }

        protected VirtualMachine GetVirtualMachine(string vmName)
        {
            return FindEntityViewByName<VirtualMachine>(vmName);
        }

        private T FindEntityViewByName<T>(string name) where T : EntityViewBase
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
