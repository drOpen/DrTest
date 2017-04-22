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
        public const string ATTRIBUTE_SWITCH_PORT_NUM = "8";
        public const string ATTRIBUTE_PORT_GROUP_NAME = "PortGrpName";
        public const string ATTRIBUTE_VM_SNAPSHOT_NAME = "ShapshotName";



        public const string ATTRIBUTE_VM_LOGIN_NAME = "Login";
        public const string ATTRIBUTE_VM_LOGIN_PWD = "Password";
        public const string ATTRIBUTE_VM_APPLICATION = "Application";
        public const string ATTRIBUTE_VM_APPLICATION_RETRY = "3";
        public const string ATTRIBUTE_VM_APPLICATION_TIMEOUT = "2000";


    }
}
