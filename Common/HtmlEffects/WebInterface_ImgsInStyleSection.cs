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

using Rockabilly.Common;
using System.Text;
using System.Collections.Generic;

namespace Rockabilly.Common.HtmlEffects
{
	public class WebInterface_ImgsInStyleSection : WebInterface
	{
		public const string HTML_BODY_START = "\t<body>";
		public const string HTML_BODY_END = "\t</body>";

		private readonly Dictionary<string, InStyleImage> images = new Dictionary<string, InStyleImage>();

		public string externalStyleSheetName = default(string);

		public InStyleImage useInStyleImage(InStyleImage thisImage)
		{
			if (!images.ContainsKey(thisImage.GetType().Name))
			{
				images.Add(thisImage.GetType().Name, thisImage);
			}
			return thisImage;
		}

		public void clearInStyleImages()
		{
			images.Clear();
		}

		protected override bool HasCssStyle()
		{
			return (styles != default(string)) || (images.Count > 0);
		}


		protected override string getCssStyle()
		{
			StringBuilder result = new StringBuilder(base.getCssStyle());

			if (images.Count > 0)
			{
				foreach (InStyleImage thisImage in images.Values)
				{
					result.Append(thisImage.ToCssClassString());
					result.Append(Symbols.CarriageReturnLineFeed);
				}
			}

			return result.ToString();
		}

	}
}