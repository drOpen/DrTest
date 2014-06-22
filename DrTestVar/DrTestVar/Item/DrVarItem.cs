/*
  DrVarItem.cs -- stored variable item for 'DrTestVar' general purpose Builder variables 1.0.0, June 22, 2014
 
  Copyright (c) 2013-2014 Kudryashov Andrey aka Dr
 
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

namespace DrOpen.DrTestVar.Item
{
    /// <summary>
    /// stored variable item
    /// </summary>
    internal struct DrVarItem 
    {
        /// <summary>
        /// Initializes a new instance of the variable item
        /// </summary>
        /// <param name="startIndex">start index of substitution symbol</param>
        /// <param name="endIndex">end index of substitution symbol</param>
        /// <param name="name">name of substitution without substitution symbols</param>
        /// <param name="fullName">name of substitution with substitution symbols</param>
        internal DrVarItem(int startIndex,
                           int endIndex,
                           string name,
                           string fullName): this()
        {
            StartIndex = startIndex;
            EndIndex = endIndex;
            Name = name;
            FullName = fullName;
        }
        /// <summary>
        /// Gets/sets the start index of substitution symbol
        /// </summary>
        internal int StartIndex { private set; get; }
        /// <summary>
        /// Gets/sets the end index of substitution symbol
        /// </summary>
        public int EndIndex { private set; get; }
        /// <summary>
        /// Gets/sets the name of substitution without substitution symbols
        /// </summary>
        public string Name { private set; get; }
        /// <summary>
        /// Gets/sets the name of substitution with substitution symbols
        /// </summary>
        public string FullName { private set; get; }
    }
}
