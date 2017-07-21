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
        /// <summary>
        /// Sets the set of command-line arguments to use when starting the application.
        /// </summary>
        public const string AttrArguments = "Arguments";
        /// <summary>
        /// Default value set of command-line arguments to use when starting the application.
        /// </summary>
        public const string DefaultArguments = "";
        /// <summary>
        /// Expected exit process code as regex.
        /// </summary>
        public const string AttrExpectedExitCode = "ExpectedExitCode";
        /// <summary>
        /// Default expected exit code
        /// </summary>
        public const string DefaultExpectedExitCode = "0";
        /// <summary>
        /// Waits the specified number of seconds for the associated process to exit. If this time is exhausted process will be terminated and appropriated exception will throw. Specify 0 for infinitely
        /// </summary>
        public const string AttrTimeOut = "TimeOut";
        /// <summary>
        /// Default time wait period, 0 is infinitely
        /// </summary>
        public const int DefaultTimeOut = 0;
        /// <summary>
        /// When the UseShellExecute property is false,  sets the working directory for the process to be started. When UseShellExecute is true, sets the directory that contains the process to be started.
        /// </summary>
        public const string AttrWorkingDirectory = "WorkingDirectory";
        /// <summary>
        /// Default empty working directory
        /// </summary>
        public const string DefaultWorkingDirectory = "";
        /// <summary>
        /// Sets a value that indicates whether the Windows user profile is to be loaded from the registry.
        /// </summary>
        public const string AttrLoadUserProfile = "LoadUserProfile";
        /// <summary>
        /// Default load user profile is true
        /// </summary>
        public const bool DefaultLoadUserProfile = true;
        /// <summary>
        /// Sets a value indicating whether to use the operating system shell to start the process.
        /// </summary>
        public const string AttrUseShellExecute = "UseShellExecute";
        /// <summary>
        /// Default shell execute is true
        /// </summary>
        public const bool DefaultUseShellExecute = true;
        /// <summary>
        /// Sets a value that indicates whether the standart output of an application is written to the Process::StandardOutput stream.
        /// </summary>
        public const string AttrRedirectStandardOutput = "RedirectStandardOutput";
        /// <summary>
        /// Default is false
        /// </summary>
        public const bool DefaultRedirectStandardOutput = false;
        /// <summary>
        /// sets a value that indicates whether the error output of an application is written to the Process::StandardError stream.
        /// </summary>
        public const string AttrRedirectStandardError = "RedirectStandardError";
        /// <summary>
        /// Default is false
        /// </summary>
        public const bool DefaultRedirectStandardError = false;
        /// <summary>
        /// sets the preferred encoding for standart output.
        /// </summary>
        public const string AttrStandardOutputEncoding = "StandardOutputEncoding";
        /// <summary>
        /// Default encoding is ""
        /// </summary>
        public const string DefaultStandardOutputEncoding = "";
        /// <summary>
        /// Sets the preferred encoding for error output.
        /// </summary>
        public const string AttrStandardErrorEncoding = "StandardErrorEncoding";
        /// <summary>
        /// Default encoding is ""
        /// </summary>
        public const string DefaultStandardErrorEncoding = "";
        /// <summary>
        /// The action to take with the file that the process opens. The default is an empty string (""), which signifies no action.
        /// </summary>
        public const string AttrVerb = "Verb";
        /// <summary>
        /// The user name to use when starting the process.
        /// </summary>
        public const string AttrUserName = "";
        /// <summary>
        /// user password
        /// </summary>
        public const string AttrPassword = "";
        /// <summary>
        /// The Active Directory domain to use when starting the process. The domain property is primarily of interest to users within enterprise environments that use Active Directory.
        /// </summary>
        public const string AttrDomain = "";
        /// <summary>
        /// Message queue of standart output
        /// </summary>
        public const string DrTestStdOut = "StdOut";
        /// <summary>
        /// Message queue of error output
        /// </summary>
        public const string DrTestStdErr = "StdErr";

    }
}
