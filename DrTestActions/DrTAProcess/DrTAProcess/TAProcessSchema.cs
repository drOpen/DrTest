/*
  TAProcessSchema.cs -- stored schema for formating of the 'DrTAProcess', July 2, 2017
 
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
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrOpen.DrTest.DrTAProcess
{
    public static class TAProcessSchema
    {
        public const string AttrFileName = "FileName";
        public const string AttrArguments = "Arguments";
        public const string DefaultArguments = "";
        public const string AttrExpectedExitCode = "ExpectedExitCode";
        public const string DefaultExpectedExitCode = "0";

        public const string AttrTimeOut = "TimeOut";
        public const int DefaultTimeOut = 0;

        public const string AttrWorkingDirectory = "WorkingDirectory";
        public const string DefaultWorkingDirectory = "";

        public const string AttrLoadUserProfile = "LoadUserProfile";
        public const bool DefaultLoadUserProfile = true;
        public const string AttrUseShellExecute = "UseShellExecute";
        public const bool DefaultUseShellExecute = true;
        
        public const string AttrRedirectStandardOutput = "RedirectStandardOutput";
        public const bool DefaultRedirectStandardOutput = false;
        public const string AttrRedirectStandardError = "RedirectStandardError";
        public const bool DefaultRedirectStandardError = false;

        public const string AttrStandardOutputEncoding= "StandardOutputEncoding";
        public const string DefaultStandardOutputEncoding = "";

        public const string AttrStandardErrorEncoding = "StandardErrorEncoding";
        public const string DefaultStandardErrorEncoding = "";


        public const string DrTestStdOut = "StdOut";
        public const string DrTestStdErr = "StdErr";

    }
}
