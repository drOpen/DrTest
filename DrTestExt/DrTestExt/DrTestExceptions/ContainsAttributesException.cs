/*
  ContainsAttributesException.cs -- stored test attribute exceptions for DrData, August  28, 2016
  
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

namespace DrTestExt.DrTestExceptions
{
    /// <summary>
    /// Represents errors that occur during test execution.
    /// </summary>
    public class ContainsAttributesException : Exception
    {

        private string message = string.Empty;
        /// <summary>
        /// Gets array a names which was not found in the list of attributes
        /// </summary>
        public  IEnumerable<string> Names { private set; get; }
        /// <summary>
        /// Gets string a names separated by comma which was not found in the list of attributes
        /// </summary>
        public string NamesList { private set; get; }
        /// <summary>
        /// Gets a message that describes the current exception.
        /// </summary>
        public override string Message { get { return this.message; } }
        /// <summary>
        /// Initializes a new instance of the ContainsAttributesException class.
        /// </summary>
        /// <param name="names">names which was not found in the list of attributes</param>
        public ContainsAttributesException(IEnumerable<string> names)
        {
            string nameList = string.Empty;
            foreach (var name in names)
            {
                if (nameList.Length > 0) nameList += ", ";
                nameList += name ;
            }
            this.Names = names;
            this.NamesList = nameList;
            this.message = String.Format(Res.Msg.ERR_MANDATORY_ATTR_FAILED, nameList);
        }

    }
}
