using System;
using System.Collections.Generic;
using System.Text;
using VMware.Vim;

namespace DrTest.DrAction.DrTestActionSampleVM
{
    public static class DrVMExt
    {
        /// <summary>
        /// Powers on this virtual machine. If the virtual machine is suspended, this method resumes execution from the suspend point.
        /// </summary>
        /// <param name="vm">A reference to the VirtualMachine used to make the method call.</param>
        /// <returns>This method returns a Task object with which to monitor the operation.</returns>
        public static ManagedObjectReference PowerOnVM_Task(this VirtualMachine vm)
        {
            return vm.PowerOnVM_Task(null);
        }
    }
}
