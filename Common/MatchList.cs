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
using System.Collections.Generic;

namespace Rockabilly.Common
{
	public class MatchList : List<string>
	{
		public bool Matches(string candidateString)
		{
			foreach (string thisListedString in this)
			{
				if (candidateString.Contains(thisListedString)) return true;
			}

			return false;
		}

		public bool Contains(string candidateString)
		{
			foreach (string thisListedString in this)
			{
				if (thisListedString.Equals(candidateString)) return true;
			}

			return false;
		}

		public bool MatchesCaseInspecific(string candidateString)
		{
			foreach (string thisListedString in this)
			{
				if (candidateString.ToUpper().Contains(thisListedString.ToUpper())) return true;
			}

			return false;
		}

		public bool ContainsCaseInspecific(string candidateString)
		{
			foreach (string thisListedString in this)
			{
				if (thisListedString.ToUpper().Equals(candidateString.ToUpper())) return true;
			}

			return false;
		}
	}
}
