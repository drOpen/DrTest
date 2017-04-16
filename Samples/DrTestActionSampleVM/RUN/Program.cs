using DrOpen.DrCommon.DrData;
using DrTestExt;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using static RUN.Schema.SchemaEPVM;

namespace DrTest.DrAction.DrTestActionSampleVM
{
    class Program
    {

        /// <summary>
        /// example
        /// </summary>
        /// <param name="nIn"></param>
        static void Main(string[] args)
        {

            var Inviroment = new DDNode();


            ////var SQLNode = Inviroment.Add("vSwitchConfiguration");
            ////SQLNode.Attributes.Add(SchemaDrTestActionVM.ATTRIBUTE_NAME_SERVER_NAME, "https://172.28.2.101/sdk");
            ////SQLNode.Attributes.Add(SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_NAME, "root");
            ////SQLNode.Attributes.Add(SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_PWD, "Qwerty`123");
            ////SQLNode.Attributes.Add(SchemaDrTestActionVM.ATTRIBUTE_RESOURCE_POOL_SOURCE, "Panarin");


            //var VMs = Inviroment.Add("VmCollection");
            //var VM1 = VMs.Add("VM1");
            //VM1.Attributes.Add(SchemaDrTestActionVM.ATTRIBUTE_NAME_SERVER_NAME, "https://172.28.2.101/sdk");
            //VM1.Attributes.Add(SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_NAME, "root");
            //VM1.Attributes.Add(SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_PWD, "Qwerty`123");
            //VM1.Attributes.Add(SchemaDrTestActionVM.ATTRIBUTE_OLD_NAME_VM_NAME, "Java Time");
            //VM1.Attributes.Add(SchemaDrTestActionVM.ATTRIBUTE_NAME_VM_NAME, "New");
            //VM1.Attributes.Add(SchemaDrTestActionVM.ATTRIBUTE_PORT_GROUP_NAME, "NewPortGroup");
            //VM1.Attributes.Add(SchemaDrTestActionVM.ATTRIBUTE_HOST_NAME, "172.28.2.105");
            //VM1.Attributes.Add(SchemaDrTestActionVM.ATTRIBUTE_VM_APPLICATION_NAME, "LSASS.EXE");
            //VM1.Attributes.Add(SchemaDrTestActionVM.ATTRIBUTE_VM_SNAPSHOT_NAME, "2");
            //VM1.Attributes.Add(SchemaDrTestActionVM.ATTRIBUTE_VM_GUEST_LOGIN_NAME, "Administrator");
            //VM1.Attributes.Add(SchemaDrTestActionVM.ATTRIBUTE_VM_GUEST_LOGIN_PWD, "Qwerty123");
            //VM1.Attributes.Add(SchemaDrTestActionVM.ATTRIBUTE_RESOURCE_POOL_NAME, "NewPool");
            //VM1.Attributes.Add(SchemaDrTestActionVM.ATTRIBUTE_VM_DOWNLOAD_FILE_SOURCE, "c:\\111\\1.txt");
            //VM1.Attributes.Add(SchemaDrTestActionVM.ATTRIBUTE_HOST_DOWNLOAD_FILE_PATH, "c:\\111\\2323.txt");


            //     ConfigurationInviroment(Inviroment);

        }


            public void ConfigurationInviroment(DDNode nIn)
        {
            var vm = new FacadeDrVM();

            var vSwitchConfiguration = nIn.Values.FirstOrDefault(t => t.Name.Equals("vSwitchConfiguration"));

            if (vSwitchConfiguration != null)
            {
                vm.CreateVirtualSwitchOnHost(vSwitchConfiguration);
                vm.CreatePortGrpOnSwitch(vSwitchConfiguration);
            }

            var VmCollection = nIn.Values.FirstOrDefault(t => t.Name.Equals("VmCollection"));

            if (VmCollection != null)
            {
                foreach (var VM in VmCollection.Values)
                {
                    vm.VMCloneVM(VM);
                    vm.ChangeSomeCustomAction(VM);
                    vm.ChangeVMNicPortGrp(VM);
                    vm.VMPowerOn(VM);
                    vm.VMCheckProcess(VM);
                }
            }

        }
    }
}
