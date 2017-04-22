using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrTest.DrAction.DrTestActionSampleVM.Res.Exceptions
{
    public abstract class VMException : Exception
    {

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


    public class HostDoesnotcontainVM : VMException
    {
        public HostDoesnotcontainVM(string name, string name2) : base(name, string.Format(Res.Msg.VM_CLONE_CURRENTSNAPSHOT, name, name2)) { }
        public HostDoesnotcontainVM(string name, string name2, Exception innerException) : base(name, string.Format(Res.Msg.VM_CLONE_CURRENTSNAPSHOT, name, name2), innerException) { }
    }


    public class HostDoesntExistExeption : VMException
    {
        public HostDoesntExistExeption(string name) : base(name, string.Format(Res.Msg.HOST_DOESNT_EXIST, name)) { }
        public HostDoesntExistExeption(string name, Exception innerException) : base(name, string.Format(Res.Msg.VM_DOESNT_EXIST, name), innerException) { }
    }



    public class VMSnapshotDoesntExistExeption : VMException
    {
        public VMSnapshotDoesntExistExeption(string name, string name2) : base(name, string.Format(Res.Msg.SNAPSHOT_DOES_NOT_EXIST, name, name2)) { }
        public VMSnapshotDoesntExistExeption(string name, string name2, Exception innerException) : base(name, string.Format(Res.Msg.SNAPSHOT_DOES_NOT_EXIST, name, name2), innerException) { }
    }


    public class VMSnapshotHaveMoreThenOneExeption : VMException
    {
        public VMSnapshotHaveMoreThenOneExeption(string name, string name2) : base(name, string.Format(Res.Msg.SNAPSHOT_HAVE_MORE_THEN_ONE, name, name2)) { }
        public VMSnapshotHaveMoreThenOneExeption(string name, string name2, Exception innerException) : base(name, string.Format(Res.Msg.SNAPSHOT_HAVE_MORE_THEN_ONE, name, name2), innerException) { }
    }

}
