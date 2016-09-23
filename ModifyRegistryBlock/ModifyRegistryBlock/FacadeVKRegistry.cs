using DrOpen.DrCommon.DrData;
using DrTestExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DrAction.VKirillov.Registry.Res;

namespace DrAction.VKirillov.Registry
{
    public class FacadeVKRegistry
    {
        /// <summary>
        /// Creates registry key under specified path
        /// </summary>
        /// <param name="nIn">Contains information on new key path and excepted result of entire operation.
        /// If key under specified path already exists it considers as a failure
        /// If this node does not contain required parameters for execution - ArgumentException is thrown</param>
        /// <returns>DDnode containing result of matching actual and expected results
        /// as well as exception, if there was any</returns>
        public static DDNode CreateKey(DDNode nIn)
        {
            var nOut = DrTestActionExt.GetStubActionResultNode();

            try
            {
                nIn.Attributes.ContainsAttributesOtherwiseThrow(SchemaVKRegistry.ATTRIBYTE_NAME_PATH_TO_KEY);

                WrapperVKRegistry.createKey(nIn.Attributes[SchemaVKRegistry.ATTRIBYTE_NAME_PATH_TO_KEY]);

                return nOut.SetActionResultStatusOK(String.Format(Msg.REGISTRY_SUCCEESS_CREATE_KEY, nIn.Attributes[SchemaVKRegistry.ATTRIBYTE_NAME_PATH_TO_KEY])); 
            }
            catch(Exception e)
            {
                return nOut.SetActionResultStatusFailed(String.Format(Msg.REGISTRY_FAIL_CREATE_KEY, nIn.Attributes[SchemaVKRegistry.ATTRIBYTE_NAME_PATH_TO_KEY]), e);
            }
            finally
            {
                nOut.SetActionResultNodeEndTime();
            }
        }

        /// <summary>
        /// Deletes registry key under specified path
        /// </summary>
        /// <param name="nIn">Contains information on marked for delete key path and excepted result of entire operation.
        /// If there is no key under specified path it considers as a failure
        /// If this node does not contain required parameters for execution - ArgumentException is thrown</param>
        /// <returns>DDnode containing result of matching actual and expected results
        /// as well as exception, if there was any</returns>
        public static DDNode DeleteKey(DDNode nIn)
        {
            var nOut = DrTestActionExt.GetStubActionResultNode();

            try
            {
                nIn.Attributes.ContainsAttributesOtherwiseThrow(SchemaVKRegistry.ATTRIBYTE_NAME_PATH_TO_KEY);

                WrapperVKRegistry.deleteKey(nIn.Attributes[SchemaVKRegistry.ATTRIBYTE_NAME_PATH_TO_KEY]);

                return nOut.SetActionResultStatusOK(String.Format(Msg.REGISTRY_SUCCESS_DELETE_KEY, nIn.Attributes[SchemaVKRegistry.ATTRIBYTE_NAME_PATH_TO_KEY]));
            }
            catch (Exception e)
            {
                return nOut.SetActionResultStatusFailed(String.Format(Msg.REGISTRY_FAIL_DELETE_KEY, nIn.Attributes[SchemaVKRegistry.ATTRIBYTE_NAME_PATH_TO_KEY]), e); 
            }
            finally
            {
                nOut.SetActionResultNodeEndTime();
            }
        }

        /// <summary>
        /// Set value for specified registry key with provied Name and Data
        /// </summary>
        /// <param name="nIn">Contains information on path which values need to be changed
        /// as well as Name and Data of value for change (or set if not exists)
        /// If this node does not contain required parameters for execution - ArgumentException is thrown</param>
        /// <returns>DDnode containing result of matching actual and expected results
        /// as well as exception, if there was any</returns>
        public static DDNode SetValue(DDNode nIn)
        {
            var nOut = DrTestActionExt.GetStubActionResultNode();

            try
            {
                nIn.Attributes.ContainsAttributesOtherwiseThrow(SchemaVKRegistry.ATTRIBYTE_NAME_PATH_TO_KEY,
                                                                SchemaVKRegistry.ATTRIBUTE_NAME_VALUE_NAME,
                                                                SchemaVKRegistry.ATTRIBUTE_NAME_VALUE_VALUE);
                WrapperVKRegistry.setValue(nIn.Attributes[SchemaVKRegistry.ATTRIBYTE_NAME_PATH_TO_KEY],
                                            nIn.Attributes[SchemaVKRegistry.ATTRIBUTE_NAME_VALUE_NAME],
                                            nIn.Attributes[SchemaVKRegistry.ATTRIBUTE_NAME_VALUE_VALUE]);


                return nOut.SetActionResultStatusOK(String.Format(Msg.REGISTRY_SUCCESS_SET_VALUE, nIn.Attributes[SchemaVKRegistry.ATTRIBUTE_NAME_VALUE_NAME],
                                                                                                    nIn.Attributes[SchemaVKRegistry.ATTRIBUTE_NAME_VALUE_VALUE],
                                                                                                    nIn.Attributes[SchemaVKRegistry.ATTRIBYTE_NAME_PATH_TO_KEY]));
            }
            catch (Exception e)
            {
                return nOut.SetActionResultStatusFailed(String.Format(Msg.REGISTRY_FAIL_SET_VALUE, nIn.Attributes[SchemaVKRegistry.ATTRIBUTE_NAME_VALUE_NAME],
                                                                                                    nIn.Attributes[SchemaVKRegistry.ATTRIBUTE_NAME_VALUE_VALUE],
                                                                                                    nIn.Attributes[SchemaVKRegistry.ATTRIBYTE_NAME_PATH_TO_KEY]), e);
            }
            finally
            {
                nOut.SetActionResultNodeEndTime();
            }
        }
    }
}
