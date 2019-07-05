// Copyright (c) 2019, 2016 William Arthur Hood
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights to
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies
// of the Software, and to permit persons to whom the Software is furnished
// to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included
// in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Text;

namespace Rockabilly.Strings
{
    public static class StringUtilities
    {
        private static string quickDateFormatString = "yyyy-MM-dd HH-mm-ss.ffff";
        public static string TimeStamp
        {
            get
            {
                return DateTime.Now.ToString(quickDateFormatString);
            }
        }

        public static string GetDimensional(int x, int y)
        {
            return (x + " x " + y).MakeParenthetic();
        }

        private const string nullString = "(null)";

        public static string RobustGetString(object candidate)
        {
            if (candidate == null)
                return nullString;
            try
            {
                return candidate.ToString().FilterOutNonPrintables();
            }
            catch (NullReferenceException)
            {
                return nullString;
            }
            catch (Exception)
            {
                return "(ERROR)";
            }
        }


        public static string DepictException(Exception thisException)
        {
            StringBuilder result = new StringBuilder();
            result.Append(thisException.GetType().ToString());
            result.Append(":  ");
            result.Append(thisException.Message);
            result.Append(Symbols.CarriageReturnLineFeed);
            result.Append(Symbols.CarriageReturnLineFeed);
            result.Append(thisException.StackTrace);

            if (thisException.InnerException != null)
            {
                result.Append(Symbols.CarriageReturnLineFeed);
                result.Append(("Inner " + DepictException(thisException.InnerException)).IndentEveryLineBy(8));
            }

            return result.ToString();
        }
    }
}
