/*
  TASrvComparision.cs -- DrTASrv - comparision of service properties from test automation, July 29, 2017
  
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
using DrOpen.DrCommon.DrSrv;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace DrOpen.DrTest.DrTASrv
{
    internal class TASrvComparision
    {
        #region TASrvComparision
        public TASrvComparision(DrSrvHelper.QUERY_SERVICE_CONFIG config, DrSrvHelper.SERVICE_STATUS status)
        {
            this.Status = status;
            this.Config = config;
        }

        #endregion TASrvComparision

        public DrSrvHelper.QUERY_SERVICE_CONFIG Config { get; private set; }
        public DrSrvHelper.SERVICE_STATUS Status { get; private set; }

        public bool Match(DDNode n)
        {
            
            if (n.Attributes.Contains(TASrvSchema.AttrPropBinaryPathName)) Regex.Match(this.Config.binaryPathName, n.Attributes[TASrvSchema.AttrPropBinaryPathName].GetValueAsString());

            return false;
        }

    }
}
