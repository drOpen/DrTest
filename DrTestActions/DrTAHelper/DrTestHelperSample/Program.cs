using DrOpen.DrCommon.DrData;
using DrOpen.DrCommon.DrDataSx;
using DrOpen.DrTest.DrTAHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrTAHelperSample
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Threading.Thread.CurrentThread.Name = "Main";

            var tSample = new TASample();
            tSample.CheckFile(getTestFileNode());

            tSample.CheckFiles(getTestFilesNode());

        }


        private static DDNode getTestFileNode()
        {
            return DDNodeSxe.Deserialize
                    (
                        @"<n n='TestFile'>
                            <a n='File' t='System.String'>c:\text.txt</a>
                            <a n='Exists' t='System.Boolean'>false</a>
                         </n>"
                    );
        }

        private static DDNode getTestFilesNode()
        {
            return DDNodeSxe.Deserialize
                    (
                        @"<n n='TestFiles'>
                            <n n='TestFile1'>
                                <a n='File' t='System.String'>c:\text.txt</a>
                                <a n='Exists' t='System.Boolean'>false</a>
                            </n>
                            <n n='TestFile2'>
                                <a n='File' t='System.String'>c:\pagefile.sys</a>
                                <a n='Exists' t='System.Boolean'>true</a>
                            </n>
                            <n n='TestFile3'>
                                <a n='File' t='System.String'>c:\doesntexistfile.txt</a>
                                <a n='Exists' t='System.Boolean'>true</a>
                            </n>
                         </n>"
                    );

        }
    }
}
