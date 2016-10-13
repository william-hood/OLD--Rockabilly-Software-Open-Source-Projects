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



using System;
using System.Text;
namespace Rockabilly.Common
{
	// Based on http://en.wikipedia.org/wiki/Hexavigesimal
	public class SubnameFactory
	{
		private const char DEFAULT_PLACE_HOLDER = '.';
		private const long DEFAULT_INDEX = 0;
		private const int DEFAULT_PLACES = 0;
		private long index = DEFAULT_INDEX;
		private int places = DEFAULT_PLACES;
		public char placeholder = DEFAULT_PLACE_HOLDER;

		public SubnameFactory(long startingIndex = DEFAULT_INDEX, int totalPlaces = DEFAULT_PLACES)
		{
			index = startingIndex;
			places = totalPlaces;
		}

		public string NextIndexAsString
		{
			get
			{
				Advance();
				return CurrentIndexAsString;
			}
		}

		public string CurrentIndexAsString
		{
			get
			{
				return Prepend(index.ToString());
			}
		}

		public long NextIndex
		{
			get
			{
				Advance();
				return index;
			}
		}

		public long CurrentIndex
		{
			get
			{
				return index;
			}
		}

		public void Advance()
		{
			index++;
		}

		public string NextSubname
		{
			get
			{
				Advance();
				return CurrentSubname;
			}
		}

		private string Prepend(string result)
		{
			string prefix = "";
			if (places > 0)
			{
				if (result.Length < places)
					prefix = new String(placeholder,
						places - result.Length);
			}
			return prefix + result;
		}

		public string CurrentSubname
		{
			get
			{
				StringBuilder result = new StringBuilder();
				long cursor = index;
				while (cursor > 0)
				{
					--cursor;
					result.Append((char)('A' + cursor % 26));
					cursor /= 26;
				}

				return Prepend(result.ToString().Reverse());
			}
		}
	}
}
