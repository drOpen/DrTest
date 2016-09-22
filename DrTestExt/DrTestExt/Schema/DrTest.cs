using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrTestExt.Schema
{
    public static class SchemaDrTest
    {
        /// <summary>
        /// enums for actions and tests status
        /// </summary>
        [Flags]
        public enum DrTestExecStatus
        {
            /// <summary>
            /// test/action is implemented
            /// </summary>
            EXEC_STATUS_IMPLEMENTED=0,
            /// <summary>
            /// test/action is passed
            /// </summary>
            EXEC_STATUS_OK=1,
            /// <summary>
            /// test/action is failed
            /// </summary>
            EXEC_STATUS_FAILED=2,
            /// <summary>
            /// test/action is disabled
            /// </summary>
            EXEC_STATUS_DISABLED=4,
            /// <summary>
            /// test/action is skipped
            /// </summary>
            EXEC_STATUS_SKIPPED = 8,
            /// <summary>
            /// test/action has been started
            /// </summary>
            EXEC_STATUS_STARTED = 16,

        }

    }
}
