/*
  DrTestAction.cs -- schema for DrTestAction, August  28, 2016
  
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
    public static class SchemaDrTestAction
    {

        public const string TYPE_ACTION = "DrTestActionStatus";

        public const string NODE_ACTION_NAME = "ActionStatus";

        public const string ATTR_STATUS_EXECUTE_STATUS = "ExecStatus";
        public const string ATTR_STATUS_START_TIME = "StartTime";
        public const string ATTR_STATUS_END_TIME = "EndTime";
        public const string ATTR_STATUS_DESCRIPTION = "Description";
        //public const string ATTR_STATUS_

    }
}
