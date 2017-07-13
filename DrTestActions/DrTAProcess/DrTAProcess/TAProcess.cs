using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DrOpen.DrTest.DrTAHelper;
using DrOpen.DrCommon.DrData;
using System.Diagnostics;
using DrOpen.DrCommon.DrLog.DrLogClient;

namespace DrOpen.DrTest.DrTAProcess
{
    public class TAProcess : TAHelper
    {

        private TALog stdOut;
        private TALog stdErr;

        private DDNode stdOutMessages;
        private DDNode stdErrMessages;


        public void CreateProcess(DDNode n)
        {
            createProcess(n);
        }

        private void initializeStdOut()
        {
            this.stdOutMessages = base.OutPut.Add(TAProcessSchema.DrTestStdOut, new DDType(TASchema.DrTestTypeMessages));
            this.stdOut = LoggerST<TALog>.GetInstance(TAProcessSchema.DrTestStdOut);
            this.stdOut.SetNodeOfMessages(this.stdOutMessages);
        }

        private void initializeStdErr()
        {
            this.stdErrMessages = base.OutPut.Add(TAProcessSchema.DrTestStdErr, new DDType(TASchema.DrTestTypeMessages));
            this.stdErr = LoggerST<TALog>.GetInstance(TAProcessSchema.DrTestStdErr);
            this.stdErr.SetNodeOfMessages(this.stdErrMessages);
        }

        private void createProcess(DDNode n)
        {
            n.Attributes.ContainsAttributesOtherwiseThrow(TAProcessSchema.AttrFileName);

            var p = new Process();
            p.StartInfo.FileName= n.Attributes[TAProcessSchema.AttrFileName];
            p.StartInfo.Arguments = n.Attributes.GetValue(TAProcessSchema.AttrArguments, TAProcessSchema.DefaultArguments);
            p.StartInfo.LoadUserProfile = n.Attributes.GetValue(TAProcessSchema.AttrLoadUserProfile, TAProcessSchema.DefaultLoadUserProfile);
            var expectedExitCode = n.Attributes.GetValue(TAProcessSchema.AttrExpectedExitCode, TAProcessSchema.DefaultExpectedExitCode);
            var timeOut = n.Attributes.GetValue(TAProcessSchema.AttrTimeOut, TAProcessSchema.DefaultTimeOut);

            p.StartInfo.WorkingDirectory = n.Attributes.GetValue(TAProcessSchema.AttrWorkingDirectory, TAProcessSchema.DefaultWorkingDirectory);
            p.StartInfo.UseShellExecute = n.Attributes.GetValue(TAProcessSchema.AttrUseShellExecute, TAProcessSchema.DefaultUseShellExecute);

            p.StartInfo.RedirectStandardOutput = n.Attributes.GetValue(TAProcessSchema.AttrRedirectStandardOutput, TAProcessSchema.DefaultRedirectStandardOutput);
            p.StartInfo.RedirectStandardError = n.Attributes.GetValue(TAProcessSchema.AttrRedirectStandardError, TAProcessSchema.DefaultRedirectStandardError);

            if (p.StartInfo.RedirectStandardOutput)
            {
                p.OutputDataReceived += new DataReceivedEventHandler(StdOutHandler); // subscribe to events from std_out
                p.StartInfo.StandardOutputEncoding = GetEncodingByName(n.Attributes.GetValue(TAProcessSchema.AttrStandardOutputEncoding, TAProcessSchema.DefaultStandardOutputEncoding));
                initializeStdOut();
            }
            if (p.StartInfo.RedirectStandardError)
            {
                p.ErrorDataReceived += new DataReceivedEventHandler(StdErrHandler); // subscribe to events from std_err
                p.StartInfo.StandardErrorEncoding = GetEncodingByName(n.Attributes.GetValue(TAProcessSchema.AttrStandardErrorEncoding, TAProcessSchema.DefaultStandardErrorEncoding));
                initializeStdErr();
            }
            logProcessStartInfo(p.StartInfo, expectedExitCode);

            p.Start();
            int iWait = 0;
            while (p.HasExited)
            {
                p.WaitForExit(1000);
                iWait++;
                if ((timeOut != 0) && (iWait > timeOut))
                {
                    log.WriteError("The time out period '{0}' sec. is exhausted and process '{1}' will be killed.", iWait.ToString(), p.ProcessName);
                    p.Kill();
                }
            }
            var exitCode = p.ExitCode;

        }

        private void logProcessStartInfo(ProcessStartInfo p, string expectedExitCode)
        {
            log.WriteInfo("Starting '{0}' with arguments '{1}'. Expected exit code '{2}'.", p.FileName, p.Arguments, expectedExitCode);
            log.WriteTrace("Working directory '{0}'", p.WorkingDirectory);
            log.WriteTrace("Load user profile '{0}'", p.LoadUserProfile);
            log.WriteTrace("Use shell execute '{0}'", p.UseShellExecute);
            log.WriteTrace("Redirect std_out '{0}' with encoding '{1}'", p.RedirectStandardOutput, p.StandardOutputEncoding);
            log.WriteTrace("Redirect std_err '{0}' with encoding '{1}'", p.RedirectStandardError, p.StandardErrorEncoding);


        }

        private Encoding GetEncodingByName(string name)
        {
            try
            {
                return Encoding.GetEncoding(name);
            }
            catch (Exception e)
            {
                if (name.Length != 0) log.WriteError(e, "Encoding name '{0}' is incorrect. The encoding value will be set by default.", name, Encoding.Default.ToString()); // logs encoding convertion error
                return Encoding.Default;
            }
        }

        private void StdOutHandler(object sendingProcess, DataReceivedEventArgs outLine)
        {
            if ((outLine==null) && (String.IsNullOrEmpty(outLine.Data.Trim()))) return;
            this.stdOut.WriteInfo(outLine.Data.Trim());
        }

        private void StdErrHandler(object sendingProcess, DataReceivedEventArgs outLine)
        {
            if ((outLine == null) && (String.IsNullOrEmpty(outLine.Data.Trim()))) return;
            this.stdErr.WriteInfo(outLine.Data.Trim());
        }


                
    }
}
