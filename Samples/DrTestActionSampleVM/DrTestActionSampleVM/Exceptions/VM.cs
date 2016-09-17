using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrTest.DrAction.DrTestActionSampleVM.Exceptions
{
    /// <summary>
    /// VM exceptions
    /// </summary>
    public abstract class VMException : Exception
    {
        /// <summary>
        /// virtual machine name
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// VM exceptions
        /// </summary>
        /// <param name="name">a virtual machine name</param>
        /// <param name="message">A message that describes the error</param>
        public VMException(string name, string message)
            : base(message)
        {
            this.Name = name;
        }
        /// <summary>
        /// VM exceptions and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="name">a virtual machine name</param>
        /// <param name="message">A message that describes the error</param>
        /// <param name="innerException">The exception that is the cause of the current exception. If the innerException parameter is not a null reference, the current exception is raised in a catch block that handles the inner exception.</param>
        public VMException(string name, string message, Exception innerException)
            : base(message, innerException)
        {
            this.Name = name;
        }

    }

    /// <summary>
    /// VM exception - describes exception when the specified virtual machine is not found.
    /// </summary>
    public class VMDoesntExistExeption : VMException
    {
        /// <summary>
        /// Initializes a new instance of the VMDoesntExistExeption class with the default error message.
        /// </summary>
        /// <param name="name">a virtual machine name</param>
        public VMDoesntExistExeption(string name) : base(name, string.Format(Res.Msg.VM_DOESNT_EXIST, name)) { }
        /// <summary>
        /// Initializes a new instance of the VMDoesntExistExeption class with the default error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="name">a virtual machine name</param>
        /// <param name="innerException">The exception that is the cause of the current exception. If the innerException parameter is not a null reference, the current exception is raised in a catch block that handles the inner exception.</param>
        public VMDoesntExistExeption(string name, Exception innerException) : base(name, string.Format(Res.Msg.VM_DOESNT_EXIST, name), innerException) { }
    }
    /// <summary>
    /// VM exception - describes exception when power status of specified virtual machine cannot be change to specified power status
    /// </summary>
    public class VMCannotChangePowerStatusException : VMException
    {
        /// <summary>
        /// Initializes a new instance of the VMDoesntExistExeption class with the default error message.
        /// </summary>
        /// <param name="name">a virtual machine name</param>
        /// <param name="status">power status of virtual machine</param>
        public VMCannotChangePowerStatusException(string name, string status) : base(name, string.Format(Res.Msg.VM_CANNOT_CHANGE_POWER_STATUS, status, name)) 
        {
            this.Status = status;
        }
        /// <summary>
        /// Initializes a new instance of the VMDoesntExistExeption class with the default error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="name">a virtual machine name</param>
        /// <param name="status">power status of virtual machine</param>
        /// <param name="innerException">The exception that is the cause of the current exception. If the innerException parameter is not a null reference, the current exception is raised in a catch block that handles the inner exception.</param>
        public VMCannotChangePowerStatusException(string name, string status, Exception innerException)
            : base(name, string.Format(Res.Msg.VM_CANNOT_CHANGE_POWER_STATUS, status, name), innerException) 
        {
            this.Status = status;
        }
        /// <summary>
        /// virtual machine power status
        /// </summary>
        public string Status { get; private set; }
    }

}
