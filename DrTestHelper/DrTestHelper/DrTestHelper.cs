using DrOpen.DrCommon.DrData;
using System;
using System.Collections.Generic;
using System.Text;

namespace DrOpen.DrTestHelper
{
    public abstract class DrTestHelper : IDrTestHelper
    {
        public DrTestHelper()
        {

            this.OutResult = GetStubResultNode();
            this.outVariables = this.OutResult[DrTestSchema.DrTestMessages];
            this.outMessages = this.OutResult[DrTestSchema.DrTestMessages];
            this.log = new DrTestLog(this.outMessages);
        }
        /// <summary>
        /// Test logger
        /// </summary>
        protected DrTestLog log;
        /// <summary>
        /// Outgoing node with results
        /// </summary>
        public DDNode OutResult { get; private set; }
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
        private DDNode GetStubResultNode()
        {
            var n = new DDNode(DrTestSchema.DrTestResult, new DDType(DrTestSchema.DrTestTypeResult));
            this.outMessages = n.Add(DrTestSchema.DrTestMessages, new DDType(DrTestSchema.DrTestTypeMessages));
            this.outVariables = n.Add(DrTestSchema.DrTestVariables, new DDType(DrTestSchema.DrTestTypeVariables));
            return n;
        }

        #region SetTestFailed
        /// <summary>
        /// Sets test status failed. Throws exception DrTestFailedException
        /// </summary>
        /// <param name="reason">The reason for the unsuccessful test</param>
        /// <param name="args">arguments</param>
        public void SetTestFailed(string reason, params string[] args)
        {
            throw new DrTestFailedException(String.Format(reason, args));
        }
        /// <summary>
        /// Sets test status failed. Throws exception DrTestFailedException
        /// </summary>
        /// <param name="e">Inner exception for reason</param>
        /// <param name="reason">The reason for the unsuccessful test</param>
        /// <param name="args">arguments</param>
        public void SetTestFailed(Exception e, string reason, params string[] args)
        {
            throw new DrTestFailedException(String.Format(reason, args), e);
        }
        #endregion SetTestFailed
    }
}
