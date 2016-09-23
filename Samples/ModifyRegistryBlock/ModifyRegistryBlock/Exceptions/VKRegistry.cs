using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrAction.VKirillov.Registry.Exceptions
{
    /// <summary>
    /// Registry exceptions
    /// </summary>
    public class VKRegistryException : Exception
    {
        /// <summary>
        /// Path to the registry key
        /// </summary>
        public string KeyPath { get; private set; }
        /// <summary>
        /// Registry exceptions
        /// </summary>
        /// <param name="keyPath">Path to the registry key</param>
        /// <param name="message">A message that desctibes the error</param>
        public VKRegistryException(string keyPath, string message)
            :base(message)
        {
            this.KeyPath = keyPath;
        }
        /// <summary>
        /// Registry exceptions and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="keyPath">Path to the registry key</param>
        /// <param name="message">A message that desctibes the error</param>
        /// <param name="innerException">The exception that is the cause of the current exception. If the innerException parameter is not a null reference, the current exception is raised in a catch block that handles the inner exception.</param>
        public VKRegistryException(string keyPath, string message, Exception innerException)
            : base(message, innerException)
        {
            this.KeyPath = KeyPath;
        }
    }

    public class VKRegistrySpecifiedKeyDoesNotExistException : VKRegistryException
    {
        /// <summary>
        /// Initializes a new insance of the VKRegistrySpecifiedKeyDoesNotExist exception with the default error message.
        /// </summary>
        /// <param name="keyPath">Path to the registry key</param>
        public VKRegistrySpecifiedKeyDoesNotExistException(string keyPath)
            : base(keyPath, String.Format("Key does not exist: {0}", keyPath))
        { }
        /// <summary>
        /// Initializes a new insance of the VKRegistrySpecifiedKeyDoesNotExist exception with the default error message and a reference to the inner exception that is the cause of this exception. 
        /// </summary>
        /// <param name="keyPath">Path to the registry key</param>
        /// <param name="innerException">The exception that is the cause of the current exception. If the innerException parameter is not a null reference, the current exception is raised in a catch block that handles the inner exception.</param>
        public VKRegistrySpecifiedKeyDoesNotExistException(string keyPath, Exception innerException)
            : base(keyPath, String.Format("Key does not exist: {0}", keyPath), innerException)
        { }
    }

    public class VKRegistryAccessDeniedException : VKRegistryException
    {
        /// <summary>
        /// Current user's name for whom access is denied
        /// </summary>
        public string UserName { get; private set; }
        /// <summary>
        /// Initializes a new instance of the VKRegistryAccessDeniedException with the default error message.
        /// </summary>
        /// <param name="keyPath">Path to the registry key</param>
        public VKRegistryAccessDeniedException(string keyPath)
            : base(keyPath, String.Format("Access is denied for {0} on registry key: {1}", System.Security.Principal.WindowsIdentity.GetCurrent().Name, keyPath))
        {
            UserName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
        }
        /// <summary>
        /// Initializes a new insance of the VKRegistryAccessDeniedException exception with the default error message and a reference to the inner exception that is the cause of this exception. 
        /// </summary>
        /// <param name="keyPath">Path to the registry key</param>
        /// <param name="innerException">The exception that is the cause of the current exception. If the innerException parameter is not a null reference, the current exception is raised in a catch block that handles the inner exception.</param>
        public VKRegistryAccessDeniedException(string keyPath, Exception innerException)
            : base(keyPath, String.Format("Access is denied for {0} on registry key: {1}", System.Security.Principal.WindowsIdentity.GetCurrent().Name, keyPath), innerException)
        {
            UserName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
        }
    }
}
