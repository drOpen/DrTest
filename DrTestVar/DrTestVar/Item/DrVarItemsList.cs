/*
  DrVarItemsList.cs -- list of variable items for 'DrTestVar' general purpose Builder variables 1.0.0, June 22, 2014
  
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

using System;
using System.Collections;
using System.Collections.Generic;

namespace DrOpen.DrTestVar.Item
{
    /// <summary>
    /// list of variable items
    /// </summary>
    internal class DrVarItemsList: IEnumerable<DrVarItem>
    {

        internal DrVarItemsList(string value)
        {
            varItems= new List<DrVarItem>();
            Analyze(value);
        }

        private List<DrVarItem> varItems; 

        #region GetVarSymbol
        public string VarSymbol
        {
            get { return varSymbol.ToString(); }
        }

        public string EscapedVarSymbol
        {
            get { return escapedVarSymbol; }
        }

        public bool AreThereVars
        {
            get { return varItems.Count!=0;}
        }

        #endregion GetVarSymbol
        #region static
        /// <summary>
        /// Обозначение переменной, %
        /// </summary>
        static public readonly char varSymbol = '%';
        /// <summary>
        /// Экранипрвание символа переменной, %%
        /// </summary>
        static public readonly string escapedVarSymbol = "%%";



        #endregion static


        public int OpenedTagCounter { get; private set; }
        public int ClosedTagCounter { get; private set; }
        public int EscapedTagCounter { get; private set; }
        /// <summary>
        /// Анализирует строку на вхождения в нее переменных
        /// Возращает список подстановок, в противном случае возращает пустой список
        /// </summary>
        /// <param name="value">Строка для анализа</param>
        /// <returns>Возращает список подстановок, в противном случае возращает пустой список</returns>
        internal void Analyze(string value)
        {

            if (value.Contains(varSymbol.ToString())) // если нет вхождения символа переменной, сразу выходим возращая null
            {

                int iPosition = 0;

                int openTagFlag = 0;
                bool isPreviosTagEscaped = false;
                bool isPreviosCharTag = false;
                bool isTagNameStarted = false;

                string TagValue = "";

                foreach (char ch in value) // бежим по символам стринги
                {
                    iPosition++; // считаем позицию символа
                    if (ch == varSymbol)
                    {
                        if (isTagNameStarted)
                        {
                            int StartIndex = iPosition - TagValue.Length - 2; // начала тега с символом начала тега
                            int EndIndex = iPosition; // конец тега с окончаниятега
                            varItems.Add(new DrVarItem(StartIndex, EndIndex, TagValue, varSymbol + TagValue + varSymbol)); // так как закончилось имя тега, добавляем его в очередь
                            TagValue = ""; // сбрасываем имя тега
                            ClosedTagCounter++; // увеличиваем счетчик закрытия тега, по нему проверяем, что мы нашли и добавили имя тега в очередь
                        }
                        else
                        {
                            if ((isPreviosCharTag) && (!isPreviosTagEscaped))
                            {
                                EscapedTagCounter++; // считаем количество escaped
                                isPreviosTagEscaped = true;
                            }
                            else
                                isPreviosTagEscaped = false;
                            OpenedTagCounter++; // увеличиваем счетчик открытия тега
                            openTagFlag++; // увеличиваем флаг открытия тега
                            isPreviosCharTag = true; //запоминаем, что символ был тегом
                        }
                        isTagNameStarted = false; // сбрасываем флаг записи имя тега
                    }
                    else
                    {
                        // выставляем флаг, что пишется имя тега
                        if ((isPreviosCharTag) && (openTagFlag % 2 != 0)) isTagNameStarted = true;  //если предыдущий символ tag и если их число было не четным
                        if (isTagNameStarted) TagValue += ch.ToString(); //пишем имя тега
                        openTagFlag = 0;
                        isPreviosCharTag = false; //запоминаем, что символ не был тегом
                    }
                }
                if (OpenedTagCounter != (ClosedTagCounter + EscapedTagCounter * 2)) throw new FormatException(string.Format(Res.Msg.CANNOT_BUILD_VAR_NOT_CLOSED_SYMBOL,value, varSymbol));
            }
        }

        #region Enumerator
        /// <summary>
        /// Returns an enumerator that iterates through the DrVarItemsList (IEnumerator&lt;DrVarItem&gt;).
        /// </summary>
        /// <returns>IEnumerator&lt;DrVarItem&gt;</returns>
        public IEnumerator<DrVarItem> GetEnumerator()
        {
                        foreach (var item in varItems)
            {
                yield return item;
            }
        }
                        /// <summary>
        /// Returns an enumerator that iterates through the DrVarItemsList (IEnumerator&lt;DrVarItem&gt;).
        /// </summary>
        /// <returns>IEnumerator&lt;DrVarItem&gt;</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion Enumerator
    }

}
