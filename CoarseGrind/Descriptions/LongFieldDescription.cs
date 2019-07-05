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
using Rockabilly.Common;

namespace Rockabilly.CoarseGrind.Descriptions
{
	public class LongFieldDescription : ValueFieldDescription<long>
	{
		public LongFieldDescription(LimitsDescription<long> limitsDescription) : base(limitsDescription)
		{
		}

		public LongFieldDescription() : base()
		{
		}

		public LongFieldDescription(long BasisValue) : base(BasisValue)
		{
		}

		public override long PositiveMinisculeValue
		{
			get
			{
				return (long)1;
			}
		}

		public override long PositiveModerateValue
		{
			get
			{
				return (long)100;
			}
		}

		public override long MaximumPossibleValue
		{
			get
			{
				return long.MaxValue;
			}
		}

		public override long MinimumPossibleValue
		{
			get
			{
				return long.MinValue;
			}
		}

		public override long ZeroOrOrigin
		{
			get
			{
				return (long)0;
			}
		}

		public override long Add(long x, long y)
		{
			return x + y;
		}

		public override long Subtract(long x, long y)
		{
			return x - y;
		}

		public override long Multiply(long x, long y)
		{
			return x * y;
		}

		public override long Divide(long x, long y)
		{
			return x / y; // Does this round properly???
		}

		public override long Half(long x)
		{
			return Divide(x, (long)2);
		}

		// Based on http://stackoverflow.com/questions/6651554/random-number-in-long-range-is-this-the-way
		public override long Random(long min, long max)
		{
			return RandomUtilities.RandomLong(min, max);
			//return add(min, Foundation.Random..nextLong(new Random(), subtract(max, min)));
		}

	}
}
