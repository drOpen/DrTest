using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DrOpen.DrCommon.DrData;

namespace DrTest.DrAction.DrTestActionSampleVM
{
    [TestClass]
    public class UnitTest1
    {

        [TestMethod]
        public void TestMethod1()
        {


                var n = new DDNode();
                n.Attributes.Add(SchemaDrTestActionVM.ATTRIBUTE_NAME_SERVER_NAME, "https://172.28.2.80/sdk");
                n.Attributes.Add(SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_NAME, "root");
                n.Attributes.Add(SchemaDrTestActionVM.ATTRIBUTE_NAME_USER_PWD, "qwerty`123");
                n.Attributes.Add(SchemaDrTestActionVM.ATTRIBUTE_NAME_VM_NAME, "sqltr");
                var vm = new FacadeDrVM();
                var res = vm.VMPowerOn(n);
        }


    }
}
