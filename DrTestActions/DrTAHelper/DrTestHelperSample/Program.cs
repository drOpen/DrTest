using DrOpen.DrCommon.DrData;
using DrOpen.DrCommon.DrDataSx;
using DrOpen.DrTest.DrTAHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DrOpen.DrTest.DrTAProcess;
using DrOpen.DrTest.DrTASrv;

namespace DrTAHelperSample
{
    class Program
    {
        static void Main(string[] args)
        {


            //var script = new TAESP();
            //script.CreateProcess(getTestScript());


            var srv = new TASrv();
            try
            {
                srv.ValidateServiceConfigurationAndStateCollection(getTestService());
            }
            catch (Exception e)
            {

            }


            var p = new TAProcess();
            try
            {
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


        private static DDNode getTestScript()
        {
            return DDNodeSxe.Deserialize
                   (
                       @"<n n='StartSampleVbs'>
                            <a n='FileName' t='System.String'>wscript.exe</a>
                            <a n='FileName' t='System.String'>wscript.exe</a>
                            <a n='TimeOut' t='System.Int32'>10</a>
                            <a n='UseShellExecute' t='System.Boolean'>true</a>
                            <a n='RedirectStandardOutput' t='System.Boolean'>false</a>
                            <a n='RedirectStandardError' t='System.Boolean'>false</a>
                         </n>"
                   );
        }

        private static DDNode getTestService()
        {
            return DDNodeSxe.Deserialize
                   (
                       @"
                        <n n='CheckServices' t='TASrv.ValidateServiceConfigurationAndStateCollection'>
                            <a n='ServerName' t='System.String'></a>
                            <a n='ErrorControl' t='System.String'>^SERVICE_ERROR_NORMAL$</a>
                            <a n='ServiceType' t='System.String'>^272$</a>
                            <a n='StartName' t='System.String'>^LocalSystem$</a>
                            <a n='StartType' t='System.String'>^SERVICE_AUTO_START$</a>
                            <a n='TagID' t='System.String'>^0$</a>
                            <a n='CheckPoint' t='System.String'>^0$</a>
                            <a n='ControlsAccepted' t='System.String'>1217</a>
                            <a n='ServiceSpecificExitCode' t='System.String'>^0$</a>
                            <a n='ServiceState' t='System.String'>^SERVICE_RUNNING$</a>
                            <a n='WaitHint' t='System.String'>^0$</a>
                            <a n='Win32ExitCode' t='System.String'>^0$</a>
                            <a n='DelayedAutoStart' t='System.String'>^false$</a>
                            <n n='CheckSpooler' t='TASrv.ValidateServiceConfigurationAndState'>
                                <a n='ServiceName' t='System.String'>Spooler</a>
                                <a n='BinaryPathName' t='System.String'>^C:\\Windows\\System32\\spoolsv.exe$</a>
                                <a n='Description' t='System.String'>^This service spools print jobs and handles interaction with the printer.  If you turn off this service, you won’t be able to print or see your printers.$</a>
                                <a n='DisplayName' t='System.String'>Print Spooler</a>
                                <a n='LoadOrderGroup' t='System.String'>^SpoolerGroup$</a>
                                <a n='Dependencies' t='System.String'>^RPCSS\0http$</a>
                            </n>
                            <n n='CheckSpoolerAgain' t='TASrv.ValidateServiceConfigurationAndState'>
                                <a n='ServiceName' t='System.String'>Spooler</a>
                                <a n='BinaryPathName' t='System.String'>^C:\\Windows\\System32\\spoolsv.exe$</a>
                                <a n='Description' t='System.String'>^This service spools print jobs and handles interaction with the printer.  If you turn off this service, you won’t be able to print or see your printers.$</a>
                                <a n='DisplayName' t='System.String'>Print Spooler</a>
                                <a n='LoadOrderGroup' t='System.String'>^SpoolerGroup$</a>
                                <a n='Dependencies' t='System.String'>^RPCSS\0http$</a>
                            
                            </n>
                        </n>
                      "
                   );
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
