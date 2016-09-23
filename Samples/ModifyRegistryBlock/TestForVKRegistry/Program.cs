using DrOpen.DrCommon.DrData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestForVKRegistry.Helpers;
using DrAction.VKirillov.Registry;

namespace TestForVKRegistry
{
    class Program
    {
        static void Main(string[] args)
        {
            string createKeyPath = @"HKEY_CURRENT_USER\qqq\3\4";
            string deleteKeyPath = @"HKEY_CURRENT_USER\qqq\3\4";
            string setValueKeyPath = @"HKEY_CURRENT_USER\qqq\3";

            var propNode = new DDNode("InputNode");
            var inputParams = propNode; //stub //propNode.Add("InputParameters");
            inputParams.Attributes.Add(SchemaVKRegistry.ATTRIBYTE_NAME_PATH_TO_KEY, createKeyPath);

            var resultCreate1 = FacadeVKRegistry.CreateKey(propNode);
            var resultCreate2 = FacadeVKRegistry.CreateKey(propNode); // fail here - key already exists

            inputParams.Attributes.Add(SchemaVKRegistry.ATTRIBYTE_NAME_PATH_TO_KEY, deleteKeyPath, ResolveConflict.OVERWRITE);
            var resultDelete1 = FacadeVKRegistry.DeleteKey(propNode);
            var resultDelete2 = FacadeVKRegistry.DeleteKey(propNode); // fail here - no such key

            inputParams.Attributes.Add(SchemaVKRegistry.ATTRIBUTE_NAME_VALUE_NAME, "SetValueName");
            inputParams.Attributes.Add(SchemaVKRegistry.ATTRIBUTE_NAME_VALUE_VALUE, "SetValueValue");
            var resultSetValue1 = FacadeVKRegistry.SetValue(propNode); // fail here - no such key

            inputParams.Attributes.Add(SchemaVKRegistry.ATTRIBYTE_NAME_PATH_TO_KEY, setValueKeyPath, ResolveConflict.OVERWRITE);
            var resultSetValue2 = FacadeVKRegistry.SetValue(propNode);
        }
    }
}
