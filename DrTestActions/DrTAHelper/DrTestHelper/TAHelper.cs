using DrOpen.DrCommon.DrData;
using System;
using System.Collections.Generic;
using System.Text;
using DrOpen.DrCommon.DrLog.DrLogClient;

namespace DrOpen.DrTest.DrTAHelper
{
    public abstract class TAHelper : ITAHelper
    {
        public TAHelper()
        {
            this.OutPut = GetStubResultNode();
            this.outVariables = this.OutPut[TASchema.DrTestVariables];
            this.outMessages = this.OutPut[TASchema.DrTestMessages];
            this.log = LoggerST<TALog>.GetInstance();
            this.log.SetNodeOfMessages(this.outMessages);
        }
        /// <summary>
        /// Test logger
        /// </summary>
        protected TALog log;
        /// <summary>
        /// Outgoing node with results
        /// </summary>
        public DDNode OutPut { get; private set; }
        /// <summary>
        /// test variables
        /// </summary>
        private DDNode outVariables;
        /// <summary>
        /// test log messages
        /// </summary>
        private DDNode outMessages;
        /// <summary>
        /// retruns new structure outgoing node with results
        /// </summary>
        /// <returns></returns>
        protected virtual DDNode GetStubResultNode()
        {
            var n = new DDNode(TASchema.DrTestResult, new DDType(TASchema.DrTestTypeResult));
            this.outMessages = n.Add(TASchema.DrTestMessages, new DDType(TASchema.DrTestTypeMessages));
            this.outVariables = n.Add(TASchema.DrTestVariables, new DDType(TASchema.DrTestTypeVariables));
            return n;
        }

        #region SetTestFailed
        /// <summary>
        /// Sets test status failed. Throws exception DrTAFailedException
        /// </summary>
        /// <param name="reason">The reason for the unsuccessful test</param>
        /// <param name="args">arguments</param>
        public void SetTestFailed(string reason, params string[] args)
        {
            throw new DrTAFailedException(String.Format(reason, args));
        }
        /// <summary>
        /// Sets test status failed. Throws exception DrTAFailedException
        /// </summary>
        /// <param name="e">Inner exception for reason</param>
        /// <param name="reason">The reason for the unsuccessful test</param>
        /// <param name="args">arguments</param>
        public void SetTestFailed(Exception e, string reason, params string[] args)
        {
            throw new DrTAFailedException(String.Format(reason, args), e);
        }
        #endregion SetTestFailed
    }
}
