/*
  TAProcess.cs -- DrTestAction - create process, July 2, 2017
  
  Copyright (c) 2013-2017 Kudryashov Andrey aka Dr
 
  This software is provided 'as-is', without any express or implied
  warranty. In no event will the authors be held liable for any damages
  arising from the use of this software.

  Permission is granted to anyone to use this software for any purpose,
  including commercial applications, and to alter it and redistribute it
  freely, subject to the following restrictions:

      1. The origin of this software must not be misrepresented; you must not
      claim that you wrote the original software. If you use this software
      in a product, an acknowledgment in the product documentation would be
      appreciated but is not required.

      2. Altered source versions must be plainly marked as such, and must not be
      misrepresented as being the original software.

      3. This notice may not be removed or altered from any source distribution.

      Kudryashov Andrey <kudryashov.andrey at gmail.com>

 */
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
            p.StartInfo.FileName = n.Attributes[TAProcessSchema.AttrFileName];
            try
            {
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

                p.StartInfo.Verb = n.Attributes.GetValue(TAProcessSchema.AttrVerb, TAProcessSchema.DefaultVerb);
                p.StartInfo.UserName = n.Attributes.GetValue(TAProcessSchema.AttrUserName, TAProcessSchema.DefaultUserName);
                p.StartInfo.Password = n.Attributes.GetValue(TAProcessSchema.AttrPassword, TAProcessSchema.DefaultPassword).ToSecureString();
                p.StartInfo.Domain = n.Attributes.GetValue(TAProcessSchema.AttrDomain, TAProcessSchema.DefaultDomain);

                logProcessStartInfo(p.StartInfo, expectedExitCode);

                p.Start();
                if (p.StartInfo.RedirectStandardOutput) p.BeginOutputReadLine();
                if (p.StartInfo.RedirectStandardError) p.BeginErrorReadLine();
                int iWait = 0;
                while (p.HasExited == false)
                {
                    if ((timeOut != 0) && (iWait > timeOut))
                    {
                            p.Kill();
                            throw new DrTATimeOutException(iWait);
                    }
                    p.WaitForExit(1000);
                    iWait++;
                }
                var exitCode = p.ExitCode;
                IsExpectedOtherwiseThrowException(exitCode.ToString(), expectedExitCode);
            }
            catch (Exception e)
            {
                throw new DrTAFailedException(e, "The process '{0}' didn't start successfully.", p.StartInfo.FileName);
            }
            finally
            {
                if (p != null) p.Dispose();
            }

        }

        private void logProcessStartInfo(ProcessStartInfo pi, string expectedExitCode)
        {
            log.WriteInfo("Starting '{0}' with arguments '{1}' and working directory '{2}'. Expected exit code '{3}'.", pi.FileName, pi.Arguments, pi.WorkingDirectory, expectedExitCode);
            log.WriteTrace("Load user profile '{0}'", pi.LoadUserProfile);
            log.WriteTrace("Use shell execute '{0}'", pi.UseShellExecute);
            log.WriteTrace("Redirect std_out '{0}' with encoding '{1}'", pi.RedirectStandardOutput, pi.StandardOutputEncoding);
            log.WriteTrace("Redirect std_err '{0}' with encoding '{1}'", pi.RedirectStandardError, pi.StandardErrorEncoding);
            log.WriteTrace("Verb, the action to take with the file that the process opens is '{0}'.", pi.Verb);
            log.WriteTrace("The user name '{0}\\{1}' to use when starting the process.", pi.Domain, pi.UserName);
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
            if ((outLine == null) || (String.IsNullOrEmpty(outLine.Data))) return;
            this.stdOut.WriteInfo(outLine.Data.Trim());
        }

        private void StdErrHandler(object sendingProcess, DataReceivedEventArgs outLine)
        {
            if ((outLine == null) || (String.IsNullOrEmpty(outLine.Data))) return;
            this.stdErr.WriteError(outLine.Data.Trim());
        }



    }
}
