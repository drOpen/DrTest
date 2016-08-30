using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DrOpen.DrCommon.DrData;
using DrTestExt;
using System.Collections.Generic;

namespace UTestDrTestExt
{
    [TestClass]
    public class DrTestActionExt
    {

        const string sFirst = "First";
        const string sSecond = "Second";
        const string sThird = "Third";

        #region ContainsAttributesOtherwiseThrow

        [TestMethod]
        public void ContainsAttributesOtherwiseThrow()
        {
            var node = new DDNode();
            node.Attributes.Add (sFirst, sFirst);
            node.Attributes.Add(sSecond, true);

            try
            {
                node.Attributes.ContainsAttributesOtherwiseThrow(sFirst, sSecond, sThird);
                Assert.Fail("Cannot catch expected exception.");  // TBD *** 
            }
            catch (DrTestExt.DrTestExceptions.ContainsAttributesException e)
            {
                Assert.IsTrue(e.NamesList == sThird, "The list of names '{0}' is incorrect. Expected list is '{1}'", e.NamesList, sThird);
                Assert.IsTrue(CompareNameList(e.Names,sThird));
            }
        }

        private bool CompareNameList(IEnumerable<string> namesList, params string[] names)
        {
            int i=0;
            foreach (var n in namesList)
            {
                if (n  != names[i]) Assert.Fail("The name '{0}' is not equls to '{1}'.", n, names[i]);
                i ++;
            }
            return true;
        }

        #endregion ContainsAttributesOtherwiseThrow

    }
}
