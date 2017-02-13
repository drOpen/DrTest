/*
  SchemaDrVM.cs -- schema for DrVM, September 11, 2016
  
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

namespace DrTest.DrAction.DrTestActionSampleVM
{
    public class SchemaDrTestActionVM
    {
        public const string ATTRIBUTE_NAME_SERVER_NAME = "ServerName";
        public const string ATTRIBUTE_NAME_USER_NAME = "UserName";
        public const string ATTRIBUTE_NAME_USER_PWD = "Pwd";
        public const string ATTRIBUTE_NAME_VM_NAME = "VMName";
        public const string ATTRIBUTE_NEW_NAME_VM_NAME = "NewVMName";
        public const string ATTRIBUTE_SWITCH_NAME = "SwitchName";
        public const string ATTRIBUTE_HOST_NAME = "HostName";
        public const string ATTRIBUTE_SWITCH_PORT_NUM = "NumberOfPorts";
        public const int ATTRIBUTE_SWITCH_PORT_NUM_VALUE = 8;
        public const string ATTRIBUTE_PORT_GROUP_NAME = "PortGrpName";
        public const string ATTRIBUTE_VM_SNAPSHOT_NAME = "ShapshotName";
        public const string ATTRIBUTE_VM_GUEST_LOGIN_NAME = "Login";
        public const string ATTRIBUTE_VM_GUEST_LOGIN_PWD = "Password";
        public const string ATTRIBUTE_VM_APPLICATION_NAME = "Application";
        public const string ATTRIBUTE_VM_ATTEMPTS_RETRY = "Attemps";
        public const int ATTRIBUTE_VM_ATTEMPTS_RETRY_VALUE = 3;

        public const string ATTRIBUTE_VM_ATTEMPTS_TIMEOUT = "TimeOutOfAttemps";
        public const int ATTRIBUTE_VM_ATTEMPTS_TIMEOUT_VALUE = 20;

        public const string ATTRIBUTE_VM_APPLICATION_PATH = "AppliactionPATH";
        public const string ATTRIBUTE_VM_APPLICATION_ARGUMENTS = "Arguments";
        public const string ATTRIBUTE_VM_APPLICATION_ARGUMENTS_VALUE = "";
        public const string ATTRIBUTE_VM_COPY_FILE_SOURCE = "SourceFilePath";
        public const string ATTRIBUTE_VM_COPY_FILE_DESTINATION = "DestinationFilePath";
    }

    public class SchemaEPGuestStates
    {
        public const string VM_TOOL_STATE_OK = "toolsOk";
        public const string VM_TOOL_STATE_NOT_STARTED = "toolsNotRunning";
    }





}
