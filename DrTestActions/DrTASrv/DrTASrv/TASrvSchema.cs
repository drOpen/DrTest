/*
  TASrvSchema.cs -- stored schema for formating of the 'DrTASrv', July 29, 2017
 
  Copyright (c) 2013-2017 Kudryashov Andrey aka Dr
 
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

namespace DrOpen.DrTest.DrTASrv
{
    public class TASrvSchema
    {

        public const string TypeSrvValidate = "TASrv.ValidateServiceConfigurationAndState";
        public const string TypeListOfSrvValidate = "TASrv.ListOfValidateServiceConfigurationAndState";

        public const string AttrServerName = "ServerName";
        public const string DefaultServerName = "";

        public const string AttrServiceName = "ServiceName";
        public const string AttrTimeOut = "TimeOut";
        public const int DefaultTimeOut  = 90 ;

        public const string AttrSleepBetweenServiceAction = "SleepBetweenServiceAction";
        public const int DefaultSleepBetweenServiceAction = 0;

        public const string AttrServiceState = "ServiceState";

        public const string AttrPropBinaryPathName = "BinaryPathName";
        public const string AttrPropDisplayName = "DisplayName";
        public const string AttrPropErrorControl = "ErrorControl";
        public const string AttrPropLoadOrderGroup = "LoadOrderGroup";
        /// <summary>
        /// The type of service. This member can be one of the following values.
        /// </summary>
        public const string AttrPropServiceType = "ServiceType";
        public const string AttrPropStartName = "StartName";
        public const string AttrPropStartType = "StartType";
        public const string AttrPropTagID = "TagID";
        public const string AttrPropDependencies = "Dependencies";
        public const string AttrPropDescription = "Description";
        public const string AttrPropDelayedAutoStart = "DelayedAutoStart";
        
        /// <summary>
        /// The current state of the service. This member can be one of the following values.
        /// </summary>
        public const string AttrPropServiceState = "ServiceState";
        /// <summary>
        /// The control codes the service accepts and processes in its handler function (see Handler and HandlerEx). A user interface process can control a service by specifying a control command in the ControlService or ControlServiceEx function. By default, all services accept the SERVICE_CONTROL_INTERROGATE value. To accept the SERVICE_CONTROL_DEVICEEVENT value, the service must register to receive device events by using the RegisterDeviceNotification function.
        /// </summary>
        public const string AttrPropControlsAccepted = "ControlsAccepted";
        /// <summary>
        /// The error code the service uses to report an error that occurs when it is starting or stopping. To return an error code specific to the service, the service must set this value to ERROR_SERVICE_SPECIFIC_ERROR to indicate that the dwServiceSpecificExitCode member contains the error code. The service should set this value to NO_ERROR when it is running and on normal termination.
        /// </summary>
        public const string AttrPropWin32ExitCode = "Win32ExitCode";
        /// <summary>
        /// A service-specific error code that the service returns when an error occurs while the service is starting or stopping. This value is ignored unless the dwWin32ExitCode member is set to ERROR_SERVICE_SPECIFIC_ERROR.
        /// </summary>
        public const string AttrPropServiceSpecificExitCode = "ServiceSpecificExitCode";
        /// <summary>
        /// The check-point value the service increments periodically to report its progress during a lengthy start, stop, pause, or continue operation. For example, the service should increment this value as it completes each step of its initialization when it is starting up. The user interface program that invoked the operation on the service uses this value to track the progress of the service during a lengthy operation. This value is not valid and should be zero when the service does not have a start, stop, pause, or continue operation pending.
        /// </summary>
        public const string AttrPropCheckPoint = "CheckPoint";
        /// <summary>
        /// The estimated time required for a pending start, stop, pause, or continue operation, in milliseconds. Before the specified amount of time has elapsed, the service should make its next call to the SetServiceStatus function with either an incremented dwCheckPoint value or a change in dwCurrentState. If the amount of time specified by dwWaitHint passes, and dwCheckPoint has not been incremented or dwCurrentState has not changed, the service control manager or service control program can assume that an error has occurred and the service should be stopped. However, if the service shares a process with other services, the service control manager cannot terminate the service application because it would have to terminate the other services sharing the process as well.
        /// </summary>
        public const string AttrPropWaitHint = "WaitHint";
    }
}
