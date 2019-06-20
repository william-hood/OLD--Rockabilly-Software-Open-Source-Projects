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

using Rockabilly.Common;
using System.Text;

namespace Rockabilly.Common.HtmlEffects
{

	public class ProgressBar : WebInterfaceControl
	{
		private int value = default(int);
		private int max = default(int);
		public int widthPixels = 600;
		public int heightPixels = 50;

		public ProgressBar()
		{
			// Indeterminate Throbberr
			max = 100;
		}

		public ProgressBar(int currentProgress, int maximum)
		{
			value = currentProgress;
			max = maximum;

			if (currentProgress < 0) currentProgress = 0;
			if (max < currentProgress) max = currentProgress;
		}

		public override string ToString()
		{
			StringBuilder result = new StringBuilder("<progress ");

			result.Append("style=\"border:0; width:");
			result.Append(widthPixels);
			result.Append("px; height:");
			result.Append(heightPixels);
			result.Append("px;\" ");

			if (value != default(int))
			{
				result.Append("value=\"");
				result.Append(value);
				result.Append("\" ");
			}

			result.Append("max=\"");
			result.Append(max);
			result.Append('"');

			result.Append("></progress>");
			return result.ToString();
		}
	}
}