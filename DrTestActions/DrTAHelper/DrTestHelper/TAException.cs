﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrOpen.DrTest.DrTAHelper
{
    /// <summary>
    /// DrTestExceptions -- test exception -- use should use this exception than test failed
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
        public DrTAFailedException(string reason)
            : base(reason) { }
        /// <summary>
        ///  DrTest failed exception
        /// </summary>
        /// <param name="innerException">The exception that is the cause of the current exception. If the innerException parameter is not a null reference, the current exception is raised in a catch block that handles the inner exception.</param>
        /// <param name="reason">A message that describes the error.</param>
        public DrTAFailedException(string reason, Exception innerException)
            : base(reason, innerException) { }

    }
}