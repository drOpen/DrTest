using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrTestActionSampleVM.DrAction.DrTest.Exceptions
{
    public abstract class VMException : Exception
    {
        /// <summary>
        /// virtual machine name
        /// </summary>
        public string Name { get; private set; }

        public VMException(string name, string message)
            : base(message)
        {
            this.Name = name;
        }

        public VMException(string name, string message, Exception innerException)
            : base(message, innerException)
        {
            this.Name = name;
        }

    }

    public class VMDoesntExistExeption : VMException
    {
        public VMDoesntExistExeption(string name) : base(name, string.Format(Res.Msg.VM_DOESNT_EXIST, name)) { }
        public VMDoesntExistExeption(string name, Exception innerException) : base(name, string.Format(Res.Msg.VM_DOESNT_EXIST, name), innerException) { }
    }

}
