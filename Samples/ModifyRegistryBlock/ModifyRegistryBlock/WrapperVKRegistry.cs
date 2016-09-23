using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DrOpen.DrCommon.DrData;
using DrAction.VKirillov.Registry.Exceptions;

namespace DrAction.VKirillov.Registry
{
    class WrapperVKRegistry
    {
        private static Dictionary<string, Microsoft.Win32.RegistryKey> baseKeyDictionary = new Dictionary<string, Microsoft.Win32.RegistryKey>
        {
            { "HKEY_CLASSES_ROOT", Microsoft.Win32.Registry.ClassesRoot},
            { "HKEY_CURRENT_CONFIG", Microsoft.Win32.Registry.CurrentConfig },
            { "HKEY_CURRENT_USER", Microsoft.Win32.Registry.CurrentUser },
            { "HKEY_LOCAL_MACHINE", Microsoft.Win32.Registry.LocalMachine },
            { "HKEY_PERFORMANCE_DATA", Microsoft.Win32.Registry.PerformanceData },
            { "HKEY_USERS", Microsoft.Win32.Registry.Users }
        };

        protected internal static void createKey(string path)
        {
            string[] parts = path.Split(new char[] { '\\'}, 2);
            var baseKey = baseKeyDictionary[parts[0]];
            string relativePath = parts[1];

            try
            {
                var createdKey = baseKey.OpenSubKey(relativePath);
                if(createdKey != null)
                    throw new VKRegistryException(path, String.Format("Specified key already exists: {0}", path));

                baseKey.CreateSubKey(relativePath); // if key exists - no exception!
            }
            catch (UnauthorizedAccessException e)
            {
                throw new VKRegistryAccessDeniedException(path, e);
            }
        }

        protected internal static void deleteKey(string path)
        {
            string[] parts = path.Split(new char[] { '\\' }, 2);
            var baseKey = baseKeyDictionary[parts[0]];
            string relativePath = parts[1];
            try
            {
                baseKey.DeleteSubKeyTree(relativePath);
            }
            catch (UnauthorizedAccessException e)
            {
                throw new VKRegistryAccessDeniedException(path, e);
            }
            catch (ArgumentException e)
            {
                throw new VKRegistrySpecifiedKeyDoesNotExistException(path, e);
            }
            //baseKey.DeleteSubKey(relativePath);
            // ArgumentException if not exists
            // UnauthorizedAccessException if no access
        }

        protected internal static void setValue(string path, string name, object value)
        {
            string[] parts = path.Split(new char[] { '\\' }, 2);
            var baseKey = baseKeyDictionary[parts[0]];
            string relativePath = parts[1];
            try
            {
                var parentKey = baseKey.OpenSubKey(relativePath, true);
                parentKey.SetValue(name, value);
            }
            catch (UnauthorizedAccessException e)
            {
                throw new VKRegistryAccessDeniedException(path, e);
            }
            catch (NullReferenceException e)
            {
                throw new VKRegistrySpecifiedKeyDoesNotExistException(path, e);
            }

        }
    }
}
