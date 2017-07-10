using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DrOpen.DrTest.DrTAHelper;
using DrOpen.DrCommon.DrData;
using System.Diagnostics;

namespace DrOpen.DrTest.DrTAProcess
{
    public class TAProcess : TAHelper
    {

        public DDNode CreateProcess(DDNode n)
        {


            return base.OutResult;
        }


        private DDNode createProcess(DDNode n)
        {
            n.Attributes.ContainsAttributesOtherwiseThrow(TAProcessSchema.AttrFileName);

            var p = new Process();
            p.StartInfo.FileName= n.Attributes[TAProcessSchema.AttrFileName];
            p.StartInfo.Arguments = n.Attributes.GetValue(TAProcessSchema.AttrArguments, TAProcessSchema.DefaultArguments);
            p.StartInfo.LoadUserProfile = n.Attributes.GetValue(TAProcessSchema.AttrLoadUserProfile, TAProcessSchema.DefaultLoadUserProfile);

            p.StartInfo.WorkingDirectory = n.Attributes.GetValue(TAProcessSchema.AttrWorkingDirectory, TAProcessSchema.DefaultWorkingDirectory);
            p.StartInfo.UseShellExecute = n.Attributes.GetValue(TAProcessSchema.AttrUseShellExecute, TAProcessSchema.DefaultUseShellExecute);

            p.StartInfo.RedirectStandardOutput = n.Attributes.GetValue(TAProcessSchema.AttrRedirectStandardOutput, TAProcessSchema.DefaultRedirectStandardOutput);
            p.StartInfo.RedirectStandardError = n.Attributes.GetValue(TAProcessSchema.AttrRedirectStandardError, TAProcessSchema.DefaultRedirectStandardError);

            if (p.StartInfo.RedirectStandardOutput)
            {
                p.OutputDataReceived += new DataReceivedEventHandler(StdOutHandler); // subscribe to events from std_out
                p.StartInfo.StandardOutputEncoding = GetEncodingByName(n.Attributes.GetValue(TAProcessSchema.AttrStandardOutputEncoding, TAProcessSchema.DefaultStandardOutputEncoding));
            }
            if (p.StartInfo.RedirectStandardError)
            {
                p.ErrorDataReceived += new DataReceivedEventHandler(StdErrHandler); // subscribe to events from std_err
                p.StartInfo.StandardErrorEncoding = GetEncodingByName(n.Attributes.GetValue(TAProcessSchema.AttrStandardErrorEncoding, TAProcessSchema.DefaultStandardErrorEncoding));
            }


            return null;
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
            if (outLine.Data == null) return;
        }

        private void StdErrHandler(object sendingProcess, DataReceivedEventArgs outLine)
        {
            if (outLine.Data == null) return;
        }
       


                
    }
}
