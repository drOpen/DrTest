﻿/*
  TASrv.cs -- DrTASrv - service control manager from test automation, July 29, 2017
  
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
using DrOpen.DrTest.DrTAHelper;
using DrOpen.DrData.DrDataObject;
using DrOpen.DrCommon.DrLog.DrLogClient;
using DrOpen.DrCommon.DrSrv;
using System.Threading;

namespace DrOpen.DrTest.DrTASrv
{
    public class TASrv : TAHelper
    {

        #region subscriptios
        void DoBeforeServiceDelete(object sender, DrSrvEventArgsBeforeServiceDelete e)
        {
            log.WriteTrace("Service will be deleted by handle '{0}'.", e.HService);
        }
        void DoAfterServiceDelete(object sender, DrSrvEventArgService e)
        {
            log.WriteTrace("Service by handle '{0}' was successfully marked as deletion.", e.HService);
        }

        void DoBeforeCreateService(object sender, DrSrvEventArgsBeforeCreateService e)
        {
            log.WriteTrace("Starting create service '{0}'.", e.ServiceName);
        }
        void DoAfterCreateService(object sender, DrSrvEventArgsAfterCreateService e)
        {
            log.WriteTrace("Service '{0}' successfully created and new service handle is '{1}'.", e.ServiceName, e.HService);
        }

        void DoBeforeServiceStart(object sender, DrSrvEventArgsBeforeServiceStart e)
        {
            log.WriteTrace("Starting service by handle '{0}' remainig time '{1}'.", e.HService, e.TimeOut);
        }
        void DoAfterServiceStart(object sender, DrSrvEventArgsAfterServiceStart e)
        {
            log.WriteTrace("Service successfully started by handle '{0}'.", e.HService);
        }
        void DoBeforeServiceControl(object sender, DrSrvEventArgsBeforeServiceControl e)
        {
            log.WriteTrace("Sending service notification '{0}' and waiting expected state '{1}' remainig time '{2}'. The service handle is '{3}'.", e.ServiceControl, e.ExpectedSrvState, e.TimeOut, e.HService);
        }
        void DoAfterServiceControl(object sender, DrSrvEventArgsAfterServiceControl e)
        {
            log.WriteTrace("Service received notification '{0}' successfully. The service handle is '{1}'.", e.ServiceControl, e.HService);
        }
        void DoBeforeOpenSCM(object sender, DrSrvEventArgsBeforeOpenSCM e)
        {
            log.WriteTrace("Openinig SCM on the server '{0}' with access '{1}'.", e.ServerName, e.Access);
        }
        void DoAfterOpenSCM(object sender, DrSrvEventArgsAfterOpenSCM e)
        {
            log.WriteTrace("SCM on the server '{0}' with access '{1}' is successfully openned. The SCM handle value is '{2}'.", e.ServerName, e.Access, e.HSCM);
        }

        void DoBeforeOpenService(object sender, DrSrvEventArgsBeforeOpenService e)
        {
            log.WriteTrace("Openning service '{0}' with access '{1}' using SCM handle '{2}'.", e.ServiceName, e.Access, e.HSCM);
        }

        void DoAfterOpenService(object sender, DrSrvEventArgsAfterOpenService e)
        {
            log.WriteTrace("The service '{0}' is successfully openned with access '{1}'. The service handle is '{2}'.", e.ServiceName, e.Access, e.HService);
        }
        void DoBeforeWaitExpectedStatus(object sender, DrSrvEventArgsBeforeWaitExpectedServiceState e)
        {
            log.WriteTrace("Waiting service status '{0}' by handle '{1}'.", e.ExpectedSrvState, e.HService);
        }
        void DoWaitExpectedStatus(object sender, DrSrvEventArgsWaitExpectedServiceState e)
        {
            if (e.SpentOnTimeWait % 10 == 0) log.WriteDebug("Spent on wait service status '{0}' sec.", e.SpentOnTimeWait);
        }
        void DoAferWaitExpectedStatus(object sender, DrSrvEventArgsAfterWaitExpectedServiceState e)
        {
            log.WriteTrace("The service state '{0}' is expected. Spent on wait time is '{1}'. The service handle is '{2}'.", e.CurrentSrvState, e.SpentOnTimeWait, e.HService);
        }
        void DoBeforeThrowWin32Error(object sender, DrSrvEventArgBeforeThrowWin32Error e)
        {
            log.WriteError(e.Win32Exception, "");
        }
        #endregion subscription


        DrSrvMgr GetSrvManagerWrapped(string server, DrSrvHelper.SC_MANAGER access)
        {
            var srvMgr = new DrSrvMgr(false);
            srvMgr.EventBeforeOpenSCM += DoBeforeOpenSCM;
            srvMgr.EventAfterOpenSCM += DoAfterOpenSCM;
            srvMgr.EventBeforeOpenService += DoBeforeOpenService;
            srvMgr.EventAfterOpenService += DoAfterOpenService;
            srvMgr.EventWaitExpectedServiceState += DoWaitExpectedStatus;
            srvMgr.EventBeforeWaitExpectedServiceState += DoBeforeWaitExpectedStatus;
            srvMgr.EventAfterWaitExpectedServiceState += DoAferWaitExpectedStatus;
            srvMgr.EventBeforeServiceControl += DoBeforeServiceControl;
            srvMgr.EventAfterServiceControl += DoAfterServiceControl;
            srvMgr.EventBeforeServiceStart += DoBeforeServiceStart;
            srvMgr.EventAfterServiceStart += DoAfterServiceStart;
            srvMgr.EventBeforeCreateService += DoBeforeCreateService;
            srvMgr.EventAfterCreateService += DoAfterCreateService;
            srvMgr.EventBeforeServiceDelete += DoBeforeServiceDelete;
            srvMgr.EventAfterServiceDelete += DoAfterServiceDelete;
            srvMgr.EventBeforeThrowWin32Error += DoBeforeThrowWin32Error;

            log.WriteInfo("Open service control manager database on the server '{0}'.", (String.IsNullOrEmpty(server) ? "local" : server));
            if (!srvMgr.OpenSCM(server, access)) throw new DrTAHelper.DrTAFailedException(srvMgr.LastError, "Cannot open service control manager database on the server '{0}'.", (String.IsNullOrEmpty(server) ? "local" : server));
            return srvMgr;
        }

        #region stop
        public void Stop(DDNode n)
        {
            n.Attributes.ContainsAttributesOtherwiseThrow(TASrvSchema.AttrServiceName);
            var service = n.Attributes[TASrvSchema.AttrServiceName].GetValueAsStringArray();

            var server = n.GetAttributeValue(TASrvSchema.AttrServerName, TASrvSchema.DefaultServerName).GetValueAsString();
            var srvMgr = GetSrvManagerWrapped(server, DrSrvHelper.SC_MANAGER.SC_MANAGER_ALL_ACCESS);
            var timeOut = n.GetAttributeValue(TASrvSchema.AttrTimeOut, TASrvSchema.DefaultTimeOut);

            if (service.Length > 1)
            {

                int iFail = 0;
                int iTotal = 0;
                var sleepBetweenAction = n.GetAttributeValue(TASrvSchema.AttrSleepBetweenServiceAction, TASrvSchema.DefaultSleepBetweenServiceAction);

                foreach (var item in service)
                {
                    if (!stop(srvMgr, item, timeOut)) iFail++;
                    iTotal++;
                    if (sleepBetweenAction != 0) Thread.Sleep(sleepBetweenAction * 1000);
                }
                if (iFail != 0) throw new DrTAFailedException("Could not stop '{0}' services from '{1}'.", iFail.ToString(), iTotal.ToString());
            }
            else
            {
                if (!this.stop(srvMgr, service[0], timeOut)) throw new DrTAHelper.DrTAFailedException(srvMgr.LastError, "Cannot stop service '{0}'", service);
            }
        }

        private bool stop(DrSrvMgr srvMgr, string service, int timeOut)
        {
            log.WriteInfo("Stop service '{0}' with dependents. Time wait period is '{1}'", service, timeOut);
            return srvMgr.ServiceStop(service, timeOut, true);
        }
        #endregion stop
        #region start
        public void Start(DDNode n)
        {
            n.Attributes.ContainsAttributesOtherwiseThrow(TASrvSchema.AttrServiceName);
            var service = n.Attributes[TASrvSchema.AttrServiceName].GetValueAsStringArray();

            var server = n.GetAttributeValue(TASrvSchema.AttrServerName, TASrvSchema.DefaultServerName).GetValueAsString();
            var srvMgr = GetSrvManagerWrapped(server, DrSrvHelper.SC_MANAGER.SC_MANAGER_ALL_ACCESS);
            var timeOut = n.GetAttributeValue(TASrvSchema.AttrTimeOut, TASrvSchema.DefaultTimeOut);

            int iFail = 0;
            int iTotal = 0;
            var sleepBetweenAction = n.GetAttributeValue(TASrvSchema.AttrSleepBetweenServiceAction, TASrvSchema.DefaultSleepBetweenServiceAction);
            if (service.Length > 1)
            {
                foreach (var item in service)
                {
                    if (!start(srvMgr, item, timeOut)) iFail++;
                    iTotal++;
                    if (sleepBetweenAction != 0) Thread.Sleep(sleepBetweenAction * 1000);
                }
                if (iFail != 0) throw new DrTAFailedException("Could not start '{0}' services from '{1}'.", iFail.ToString(), iTotal.ToString());
            }
            else
            {
                if (!this.start(srvMgr, service[0], timeOut)) throw new DrTAHelper.DrTAFailedException(srvMgr.LastError, "Cannot start service '{0}'", service);
            }
        }

        private bool start(DrSrvMgr srvMgr, string service, int timeOut)
        {
            log.WriteInfo("Start service '{0}'. Time wait period is '{1}'", service, timeOut);
            return srvMgr.ServiceStart(service, timeOut);
        }
        #endregion start
        #region delete
        public void DeleteServices(DDNode n)
        {
            n.Attributes.ContainsAttributesOtherwiseThrow(TASrvSchema.AttrServiceName);
            var service = n.Attributes[TASrvSchema.AttrServiceName].GetValueAsStringArray();

            var server = n.GetAttributeValue(TASrvSchema.AttrServerName, TASrvSchema.DefaultServerName).GetValueAsString();
            var srvMgr = GetSrvManagerWrapped(server, DrSrvHelper.SC_MANAGER.SC_MANAGER_ALL_ACCESS);
            var timeOut = n.GetAttributeValue(TASrvSchema.AttrTimeOut, TASrvSchema.DefaultTimeOut);

            int iFail = 0;
            int iTotal = 0;
            var sleepBetweenAction = n.GetAttributeValue(TASrvSchema.AttrSleepBetweenServiceAction, TASrvSchema.DefaultSleepBetweenServiceAction);
            if (service.Length > 1)
            {
                foreach (var item in service)
                {
                    if (!delete(srvMgr, item, timeOut)) iFail++;
                    iTotal++;
                    if (sleepBetweenAction != 0) Thread.Sleep(sleepBetweenAction * 1000);
                }
                if (iFail != 0) throw new DrTAFailedException("Could not delete '{0}' services from '{1}'.", iFail.ToString(), iTotal.ToString());
            }
            else
            {
                if (!this.delete(srvMgr, service[0], timeOut)) throw new DrTAHelper.DrTAFailedException(srvMgr.LastError, "Cannot delete service '{0}'", service);
            }
        }


        private bool delete(DrSrvMgr srvMgr, string service, int timeOut)
        {
            log.WriteInfo("Delete service '{0}'. If service is runnig it will be stopped with all depenedents services. Time wait period is '{1}'", service, timeOut);
            return srvMgr.ServiceDelete(service, true, timeOut, true);
        }
        #endregion delete
        #region WaitState
        public void WaitStateServices(DDNode n)
        {
            var server = n.GetAttributeValue(TASrvSchema.AttrServerName, TASrvSchema.DefaultServerName).GetValueAsString();
            var srvMgr = GetSrvManagerWrapped(server, DrSrvHelper.SC_MANAGER.SC_GENERIC_READ);
            var sleepBetweenAction = n.GetAttributeValue(TASrvSchema.AttrSleepBetweenServiceAction, TASrvSchema.DefaultSleepBetweenServiceAction);
            int iFail = 0;
            int iTotal = 0;

            foreach (var item in n)
            {
                item.Value.Attributes.ContainsAttributesOtherwiseThrow(TASrvSchema.AttrServiceName);
                item.Value.Attributes.ContainsAttributesOtherwiseThrow(TASrvSchema.AttrServiceState);

                var service = n.Attributes[TASrvSchema.AttrServiceName];
                var state = (DrSrvHelper.SERVICE_CURRENT_STATE)Enum.Parse(typeof(DrSrvHelper.SERVICE_CURRENT_STATE), n.Attributes[TASrvSchema.AttrServiceState]);
                var timeOut = item.Value.GetAttributeValue(TASrvSchema.AttrTimeOut, TASrvSchema.DefaultTimeOut);
                if (!this.waitState(srvMgr, service, state, timeOut)) iFail++;
                iTotal++;
                if (sleepBetweenAction != 0) Thread.Sleep(sleepBetweenAction * 1000);
            }
            if (iFail != 0) throw new DrTAFailedException("I tired wait service state '{0}' services from '{1}'.", iFail.ToString(), iTotal.ToString());
        }
        public void WaitState(DDNode n)
        {
            n.Attributes.ContainsAttributesOtherwiseThrow(TASrvSchema.AttrServiceName);
            n.Attributes.ContainsAttributesOtherwiseThrow(TASrvSchema.AttrServiceState);

            var service = n.Attributes[TASrvSchema.AttrServiceName];
            var state = (DrSrvHelper.SERVICE_CURRENT_STATE)Enum.Parse(typeof(DrSrvHelper.SERVICE_CURRENT_STATE), n.Attributes[TASrvSchema.AttrServiceState]);
            var timeOut = n.GetAttributeValue(TASrvSchema.AttrTimeOut, TASrvSchema.DefaultTimeOut);

            var server = n.GetAttributeValue(TASrvSchema.AttrServerName, TASrvSchema.DefaultServerName).GetValueAsString();
            var srvMgr = GetSrvManagerWrapped(server, DrSrvHelper.SC_MANAGER.SC_GENERIC_READ);

            if (!this.waitState(srvMgr, service, state, timeOut)) throw new DrTAHelper.DrTAFailedException(srvMgr.LastError, "I tired wait service '{0}' state {1}.", service, state.ToString());
        }

        private bool waitState(DrSrvMgr srvMgr, string service, DrSrvHelper.SERVICE_CURRENT_STATE state, int timeOut)
        {
            log.WriteInfo("Waits service '{0}' expected '{1}' state. The timeout period is '{2}'.", service, state, timeOut);
            return srvMgr.ServiceWaitStatus(service, state, timeOut);
        }
        #endregion WaitState

        #region ValidateServiceConfigurationAndState

        public void ValidateServiceConfigurationAndStateCollection(DDNode n)
        {
            n.Type.ValidateExpectedNodeType(TASrvSchema.TypeSrvValidateCollection);
            int iSrvValidationFail = 0;
            int iSrvValidationOK = 0;
            foreach (var cNode in n)
            {
                try
                {
                    cNode.Value.Merge(n, DDNode.DDNODE_MERGE_OPTION.ATTRIBUTES, ResolveConflict.SKIP); // Inherits attributes from parent
                    ValidateServiceConfigurationAndState(cNode.Value);
                    iSrvValidationOK++;
                }
                catch (Exception e)
                {
                    log.WriteError(e, "Cannot analyze service. Test key is '{0}', see inner exception for details.", cNode.Key);
                    iSrvValidationFail++;
                }
            }
            if (iSrvValidationFail > 0) throw new DrTAFailedException("There are '{0}' fails service from '{1}'.", iSrvValidationFail.ToString(), (iSrvValidationFail + iSrvValidationOK).ToString());
            log.WriteInfo("'{0}' services were successfully validated.", iSrvValidationOK.ToString());
        }

        public void ValidateServiceConfigurationAndState(DDNode n)
        {

            n.Type.ValidateExpectedNodeType(TASrvSchema.TypeSrvValidate);

            n.Attributes.ContainsAttributesOtherwiseThrow(TASrvSchema.AttrServiceName);
            var server = n.GetAttributeValue(TASrvSchema.AttrServerName, TASrvSchema.DefaultServerName).GetValueAsString();
            var service = n.Attributes[TASrvSchema.AttrServiceName];

            var srvMgr = GetSrvManagerWrapped(server, DrSrvHelper.SC_MANAGER.SC_GENERIC_READ);
            srvMgr.OpenService(service, DrSrvHelper.SERVICE_ACCESS.SERVICE_QUERY_CONFIG | DrSrvHelper.SERVICE_ACCESS.SERVICE_QUERY_STATUS);
            DrSrvHelper.SERVICE_STATUS state;
            log.WriteTrace("Getting service '{0}' state.", service);
            if (!srvMgr.GetServiceCurrentStatus(out state)) throw new DrTAFailedException(srvMgr.LastError, "Cannot get service '{0}' state.", service);
            DrSrvHelper.QUERY_SERVICE_CONFIG config;
            if (!srvMgr.GetServiceConfig(out config)) throw new DrTAFailedException(srvMgr.LastError, "Cannot query service '{0}' config.", service);
            string description;
            if (!srvMgr.GetServiceDescription(out description)) throw new DrTAFailedException(srvMgr.LastError, "Cannot get service '{0}' description.", service);
            bool delayedAutostart;
            if (!srvMgr.GetServiceDelayAutostartInfo(out delayedAutostart)) throw new DrTAFailedException(srvMgr.LastError, "Cannot get service '{0}' delayed aut start info.", service);

            #region Validate config and state
            var iFail = 0;
            var iSuccess = 0;

            var c = new System.Collections.Generic.Dictionary<string, object> 
            {
                    {TASrvSchema.AttrPropDelayedAutoStart, delayedAutostart},
                    {TASrvSchema.AttrPropDescription, description},
                    {TASrvSchema.AttrPropDependencies, (new DDValue(config.dependencies)).ToString() }, // convert string Array to null-separated names
                    {TASrvSchema.AttrPropBinaryPathName, config.binaryPathName},
                    {TASrvSchema.AttrPropDisplayName, config.displayName},
                    {TASrvSchema.AttrPropErrorControl, config.errorControl},
                    {TASrvSchema.AttrPropLoadOrderGroup, config.loadOrderGroup},
                    {TASrvSchema.AttrPropServiceType, config.serviceType},
                    {TASrvSchema.AttrPropStartName, config.startName},
                    {TASrvSchema.AttrPropStartType, config.startType},
                    {TASrvSchema.AttrPropTagID, config.tagID},
                    {TASrvSchema.AttrPropCheckPoint, state.checkPoint},
                    {TASrvSchema.AttrPropControlsAccepted, state.controlsAccepted},
                    {TASrvSchema.AttrPropServiceSpecificExitCode, state.serviceSpecificExitCode},
                    {TASrvSchema.AttrPropServiceState, state.serviceState},
                    {TASrvSchema.AttrPropWaitHint, state.waitHint},
                    {TASrvSchema.AttrPropWin32ExitCode, state.win32ExitCode}
            };

            foreach (var i in c)
            {
                var res = checkExpectedResult(n.Attributes, i.Key, i.Value.ToString());
                if (res == CHECK_EXPECTED_RESULT.SUCCESS) iSuccess++;
                else if (res == CHECK_EXPECTED_RESULT.FAILED) iFail++;
            }

            if (iFail > 0) throw new DrTAFailedException("The service '{0}' has '{1}' unexpected properies from '{2}'.", service, iFail.ToString(), c.Count.ToString());
            else if (iSuccess > 0) log.WriteInfo("The service '{0}' has '{1}' expected properies from '{2}'.", service, iSuccess.ToString(), c.Count.ToString());
            else log.WriteInfo("Skipped '{0}' properties validation of the service '{1}'.", c.Count.ToString(), service);

            #endregion Validate config and state

        }
        private enum CHECK_EXPECTED_RESULT
        {
            SKIPPED,
            FAILED,
            SUCCESS
        }
        private CHECK_EXPECTED_RESULT checkExpectedResult(DDAttributesCollection a, string attrName, string value)
        {
            if (a.Contains(attrName))
            {
                var p = a[attrName].GetValueAsString();
                log.WriteTrace("Starting match service property '{0}' value '{1}' by pattern '{2}'.", attrName, value, p);
                return (base.IsExpected(value, p) ? CHECK_EXPECTED_RESULT.SUCCESS : CHECK_EXPECTED_RESULT.FAILED);
            }
            return CHECK_EXPECTED_RESULT.SKIPPED;
        }
        #endregion ValidateServiceConfigurationAndState
    }



}
