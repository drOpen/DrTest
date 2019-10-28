﻿/*
  DrTest.cs -- schema for DrTest, August  28, 2016
  
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

namespace DrOpen.DrTest.DrTestExt
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
