using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrTest.DrAction.DrTestActionSampleVM.Res.Exceptions
{
    public abstract class CheckVMStateException : Exception
    {

        public string Name { get; private set; }
        public CheckVMStateException(string name, string message)
            : base(message)
        {
            this.Name = name;
        }
        public CheckVMStateException(string name, string message, Exception innerException)
             : base(message, innerException)
        {
            this.Name = name;
        }

    }


    public class CannotConnectToAgent : CheckVMStateException
    {
        public CannotConnectToAgent(string VMname, string retry, string timeout) : base(VMname, string.Format(Res.Msg.CANNOT_CONNECT_TO_AGENT, VMname, retry, timeout)) { }
        public CannotConnectToAgent(string VMname, string retry, string timeout, Exception innerException) : base(VMname, string.Format(Res.Msg.CANNOT_CONNECT_TO_AGENT, VMname, retry, timeout), innerException) { }
    }



    public class ApplicationDoNotFoundOnVM : CheckVMStateException
    {
        public ApplicationDoNotFoundOnVM(string application, string VMname, string retry, string timeout) : base(VMname, string.Format(Res.Msg.APPLICATION_DO_NOT_STATED, application, VMname, retry, timeout)) { }
        public ApplicationDoNotFoundOnVM(string application, string VMname, string retry, string timeout, Exception innerException) : base(VMname, string.Format(Res.Msg.APPLICATION_DO_NOT_STATED, application, VMname, retry, timeout), innerException) { }
    }

}
