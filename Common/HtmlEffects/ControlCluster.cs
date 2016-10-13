// Copyright (c) 2016 William Arthur Hood
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


using System.Text;
using System.Collections.Generic;
using Rockabilly.Common;

namespace Rockabilly.Common.HtmlEffects
{

	public class ControlCluster : List<WebInterfaceControl>, WebInterfaceControl
	{

		public int columns = 4;

		public override string ToString()
		{
			int colIndex = 0;
			StringBuilder result = new StringBuilder("\t\t<table border=\"0\" ><tr>");
			for (int index = 0; index < Count; index++)
			{
				colIndex++;
				if (colIndex > columns)
				{
					colIndex = 1;
					result.Append("\t\t</tr><tr>");
				}
				if (colIndex > 1)
				{
					result.Append("<td>&nbsp;</td>");
				}
				result.Append("\t\t\t<td>");
				result.Append(this[index].ToString());
				result.Append("</td>");
				result.Append(Symbols.CarriageReturnLineFeed);
			}
			result.Append("\t\t</tr></table>");
			return result.ToString();
		}

	}
}
