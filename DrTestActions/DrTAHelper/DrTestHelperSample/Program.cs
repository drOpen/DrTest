using DrOpen.DrCommon.DrData;
using DrOpen.DrCommon.DrDataSx;
using DrOpen.DrTest.DrTAHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DrOpen.DrTest.DrTAProcess;

namespace DrTAHelperSample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var p = new TAProcess();
                p.CreateProcess(getTestStartProcess());
            }
            catch (Exception e)
            {

            }


            System.Threading.Thread.CurrentThread.Name = "Main";

            var tSample = new TASample();
            tSample.CheckFile(getTestFileNode());

            tSample.CheckFiles(getTestFilesNode());

        }

        private static DDNode getTestStartProcess()
        {
            return DDNodeSxe.Deserialize
                   (
                       @"<n n='StartCalc'>
                            <a n='FileName' t='System.String'>calc.exe</a>
                            <a n='TimeOut' t='System.Int32'>12</a>
                            <a n='UseShellExecute' t='System.Boolean'>false</a>
                            <a n='RedirectStandardOutput' t='System.Boolean'>true</a>
                            <a n='RedirectStandardError' t='System.Boolean'>true</a>
                         </n>"
                   );
        }


        private static DDNode getTestFileNode()
        {
            return DDNodeSxe.Deserialize
                    (
                        @"<n n='TestFile'>
                            <a n='File' t='System.String'>c:\text.txt</a>
                            <a n='Expected' t='System.Boolean'>false</a>
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
                                <a n='Expected' t='System.Boolean'>false</a>
                            </n>
                            <n n='TestFile2'>
                                <a n='File' t='System.String'>c:\pagefile.sys</a>
                                <a n='Expected' t='System.Boolean'>true</a>
                            </n>
                            <n n='TestFile3'>
                                <a n='File' t='System.String'>c:\doesntexistfile.txt</a>
                                <a n='Expected' t='System.Boolean'>false</a>
                            </n>
                         </n>"
                    );

        }
    }
}
