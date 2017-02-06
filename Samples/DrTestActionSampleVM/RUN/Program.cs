using DrOpen.DrCommon.DrData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DrTest.DrAction.DrTestActionSampleVM
{
    class Program
    {
        static void Main(string[] args)
        {
            var Clone1 = new DDNode();
            Clone1.Attributes.Add(SchemaDrTestActionVM.ATTRIBUTE_NAME_SERVER_NAME, "https://172.28.2.101/sdk");
            Clone1.Attributes.Add(SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_NAME, "root");
            Clone1.Attributes.Add(SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_PWD, "Qwerty`123");
            Clone1.Attributes.Add(SchemaDrTestActionVM.ATTRIBUTE_NAME_VM_NAME, "Oracle12");
            Clone1.Attributes.Add(SchemaDrTestActionVM.ATTRIBUTE_VM_APPLICATION, "dwm.exe");
            Clone1.Attributes.Add(SchemaDrTestActionVM.ATTRIBUTE_VM_LOGIN_NAME, "oracle");
            Clone1.Attributes.Add(SchemaDrTestActionVM.ATTRIBUTE_VM_LOGIN_PWD, "Qq123456");


             //var Switch = new DDNode();
             //Switch.Attributes.Add(SchemaDrTestActionVM.ATTRIBUTE_NAME_SERVER_NAME, "https://172.28.2.101/sdk");
             //Switch.Attributes.Add(SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_NAME, "root");
             //Switch.Attributes.Add(SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_PWD, "Qwerty`123");
             //Switch.Attributes.Add(SchemaDrTestActionVM.ATTRIBUTE_PORT_GROUP_NAME, "PortGroupName");
             //Switch.Attributes.Add(SchemaDrTestActionVM.ATTRIBUTE_SWITCH_NAME, "SwitchName");
             //Switch.Attributes.Add(SchemaDrTestActionVM.ATTRIBUTE_SWITCH_PORT_NUM, "24");
             //Switch.Attributes.Add(SchemaDrTestActionVM.ATTRIBUTE_HOST_NAME, "172.28.2.105");
             //Switch.Attributes.Add(SchemaDrTestActionVM.ATTRIBUTE_NAME_VM_NAME, "CloneNewNewEx10");

             //var Clone2 = new DDNode();
             //Clone2.Attributes.Add(SchemaDrTestActionVM.ATTRIBUTE_NAME_SERVER_NAME, "https://172.28.2.101/sdk");
             //Clone2.Attributes.Add(SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_NAME, "root");
             //Clone2.Attributes.Add(SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_PWD, "Qwerty`123");
             //Clone2.Attributes.Add(SchemaDrTestActionVM.ATTRIBUTE_NAME_VM_NAME, "New2");
             //Clone2.Attributes.Add(SchemaDrTestActionVM.ATTRIBUTE_NEW_NAME_VM_NAME, "CloneNewNewEx20");
             //Clone2.Attributes.Add(SchemaDrTestActionVM.ATTRIBUTE_HOST_NAME, "172.28.2.105");
             //Clone2.Attributes.Add(SchemaDrTestActionVM.ATTRIBUTE_VM_SNAPSHOT_NAME, "45");

             //var NIC = new DDNode();
             //NIC.Attributes.Add(SchemaDrTestActionVM.ATTRIBUTE_NAME_SERVER_NAME, "https://172.28.2.101/sdk");
             //NIC.Attributes.Add(SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_NAME, "root");
             //NIC.Attributes.Add(SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_PWD, "Qwerty`123");
             //NIC.Attributes.Add(SchemaDrTestActionVM.ATTRIBUTE_PORT_GROUP_NAME, "PortGroupName");
             //NIC.Attributes.Add(SchemaDrTestActionVM.ATTRIBUTE_HOST_NAME, "172.28.2.105");
             //NIC.Attributes.Add(SchemaDrTestActionVM.ATTRIBUTE_NAME_VM_NAME, "CloneNewNewEx20");


             var vm = new FacadeDrVM();

            var res1 = vm.VMCheckProcess(Clone1);
            //var res2 = vm.CreatePortGrp(Switch);
            //var res3 = vm.VMCloneVM(Clone1);


            //var res4 = vm.VMChangeNicPortGrp(Switch);
            //var res5 = vm.VMCloneVM(Clone2);
            //var res6 = vm.VMChangeNicPortGrp(NIC);


            /*
            var res5 = vm.RemovePortGrp(m);
            var res6 = vm.RemoveVirtualSwitch(m);
            */



        }
    }
}
