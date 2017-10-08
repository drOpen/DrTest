/*
  TAHelper.cs -- helper for DrTest, July 2, 2017
  
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

using DrOpen.DrCommon.DrData;
using System;
using System.Collections.Generic;
using System.Text;
using DrOpen.DrCommon.DrLog.DrLogClient;
using System.Text.RegularExpressions;

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
        protected DDNode outVariables;
        /// <summary>
        /// test log messages
        /// </summary>
        protected DDNode outMessages;
        /// <summary>
        /// status of test for legacy support
        /// </summary>
        protected DDNode legacyStatus;
        /// <summary>
        /// retruns new structure outgoing node with results
        /// </summary>
        /// <returns></returns>
        protected virtual DDNode GetStubResultNode()
        {
            var n = new DDNode(TASchema.DrTestOutPut, new DDType(TASchema.DrTestTypeOutPut));
            this.outMessages = n.Add(TASchema.DrTestMessages, new DDType(TASchema.DrTestTypeMessages));
            this.outVariables = n.Add(TASchema.DrTestVariables, new DDType(TASchema.DrTestTypeVariables));
            this.legacyStatus = n.Add(TASchema.DrTestLegacyStatus, new DDType(TASchema.DrTestTypeLegacyStatus));
            return n;
        }
        /// <summary>
        /// Validates expected result as regular expression. If it not match throw <typeparamref name="DrTAExpectedException"/> exception
        /// </summary>
        /// <param name="result">test result</param>
        /// <param name="expected">expected result as regular expression</param>
        protected virtual void IsExpectedOtherwiseThrowException(string result, string expected)
        {
            Regex reg = new Regex(expected, RegexOptions.IgnoreCase);
            var res =  reg.IsMatch(result);
            log.WriteTrace("The result '{0}' is matched expected value '{1}'.", result, expected);
            if (res == false) throw new DrTAExpectedException(result, expected);
        }

        /// <summary>
        /// Validates expected result as regular expression and log result
        /// </summary>
        /// <param name="result">test result</param>
        /// <param name="expected">expected result as regular expression</param>
        protected virtual bool IsExpected(string result, string expected)
        {
            Regex reg = new Regex(expected, RegexOptions.IgnoreCase);
            var res = reg.IsMatch(result);

            if (res)
                log.WriteInfo("The result '{0}' is matched by pattern '{1}'.", result, expected);
            else
                log.WriteError("The result '{0}' is not matched by pattern '{1}'.", result, expected);

            return res;
        }
    }
}
