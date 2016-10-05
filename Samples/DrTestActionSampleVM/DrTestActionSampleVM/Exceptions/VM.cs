/*
  VM.cs -- exceptions for vm, September 11, 2016
  
  Copyright (c) 2013-2016 Kudryashov Andrey aka Dr
 
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
