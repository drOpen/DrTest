using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrOpen.DrTest.DrTAHelper
{
    /// <summary>
    /// DrTAFailedException -- test exception -- should use this exception than test failed
    /// </summary>
    public class DrTAFailedException : Exception
    {
        /// <summary>
        /// DrTest failed exception
        /// </summary>
        public DrTAFailedException()
            : base() { }
        /// <summary>
        ///  DrTest failed exception
        /// </summary>
        /// <param name="reason">A message that describes the error.</param>
        /// <param name="args">messages arguments</param>
        public DrTAFailedException(string reason, params object[] args)
            : base(String.Format(reason, args)) { }
        /// <summary>
        ///  DrTest failed exception
        /// </summary>
        /// <param name="innerException">The exception that is the cause of the current exception. If the innerException parameter is not a null reference, the current exception is raised in a catch block that handles the inner exception.</param>
        /// <param name="reason">A message that describes the error.</param>
        /// <param name="args">messages arguments</param>
        public DrTAFailedException(Exception innerException, string reason, params object[] args)
            : base(String.Format(reason, args), innerException) { }
    }

    /// <summary>
    /// DrTATimeOutException -- test exception -- should use this exception than test failed by time out
    /// </summary>
    public class DrTATimeOutException : DrTAFailedException
    {
        /// <summary>
        /// DrTest failed exception by time out
        /// </summary>
        /// <param name="timeOut">time out period, sec.</param>
        public DrTATimeOutException(int timeOut) : this(timeOut, "The time out period '{0}' sec. is exhausted and operation will be aborted.", timeOut.ToString())  {}
        /// <summary>
        ///  DrTest failed exception by time out
        /// </summary>
        /// <param name="reason">A message that describes the error.</param>
        /// <param name="args">messages arguments</param>
        public DrTATimeOutException(int timeOut, string reason, params object[] args)
            : base(reason, args)
        {
            this.TimeOut = timeOut;
        }
        /// <summary>
        /// Gets time out period sec. is exhausted
        /// </summary>
        public int TimeOut { get; private set; }
    }

    /// <summary>
    /// DrTAExpectedException -- test exception -- should use this exception than test result is not match expected result
    /// </summary>
    public class DrTAExpectedException: DrTAFailedException
    {
        /// <summary>
        /// DrTest failed exception because test result is not match expected result
        /// </summary>
        /// <param name="result">test result</param>
        /// <param name="expected">expected result as regular expression</param>
        public DrTAExpectedException(string result, string expected) : base("The test result '{0}' is not match expected result '{1}'.", result, expected) 
        {
            this.Expected = expected;
            this.Result = result;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Result{ get; private set; }
        public string Expected { get; private set; }
    }
}