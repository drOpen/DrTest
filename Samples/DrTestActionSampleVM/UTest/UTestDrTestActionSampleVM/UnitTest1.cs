using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DrOpen.DrCommon.DrData;

namespace DrTest.DrAction.DrTestActionSampleVM
{
    [TestClass]
    public class UnitTest1
    {

        [TestMethod]
        public void Clone()
        {


            var n = new DDNode();
            n.Attributes.Add(SchemaDrTestActionVM.ATTRIBUTE_NAME_SERVER_NAME, "https://172.28.2.101/sdk");
            n.Attributes.Add(SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_NAME, "root");
            n.Attributes.Add(SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_PWD, "Qwerty`123");
            n.Attributes.Add(SchemaDrTestActionVM.ATTRIBUTE_NAME_VM_NAME, "New");
            n.Attributes.Add(SchemaDrTestActionVM.ATTRIBUTE_NEW_NAME_VM_NAME, "Clone1");
            n.Attributes.Add(SchemaDrTestActionVM.ATTRIBUTE_PORT_GROUP_NAME, "THIS2");
            n.Attributes.Add(SchemaDrTestActionVM.ATTRIBUTE_SWITCH_NAME, "Swithc10");
            n.Attributes.Add(SchemaDrTestActionVM.ATTRIBUTE_SWITCH_PORT_NUM, "8");

            n.Attributes.Add(SchemaDrTestActionVM.ATTRIBUTE_HOST_NAME, "172.28.2.105");
            n.Attributes.Add(SchemaDrTestActionVM.ATTRIBUTE_VM_SNAPSHOT_NAME, "THIS2");



            var vm = new FacadeDrVM();



     //       var res = vm.CreateVirtualSwitch(n);
        //    var res2 = vm.CreatePortGrp(n);
            var res3 = vm.VMCloneVM(n);
        //    var res4 = vm.VMChangeNicPortGrp(n);
       //     var res5 = vm.RemovePortGrp(n);
       //     var res6 = vm.RemoveVirtualSwitch(n);




        }



    }
}
