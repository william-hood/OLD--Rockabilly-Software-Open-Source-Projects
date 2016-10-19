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
using Rockabilly.Common;

namespace Rockabilly.CoarseGrind.Descriptions
{
	public class IntFieldDescription : ValueFieldDescription<int>
	{
		public IntFieldDescription(LimitsDescription<int> limitsDescription) : base(limitsDescription)
		{
		}

		public IntFieldDescription() : base()
		{
		}

		public IntFieldDescription(int BasisValue) : base(BasisValue)
		{
		}

		public override int PositiveMinisculeValue
		{
			get
			{
				return 1;
			}
		}

		public override int PositiveModerateValue
		{
			get
			{
				// PROBLEM: If limits are from 0 to 30 this is not moderate.
				return 100;
			}
		}

		public override int MaximumPossibleValue
		{
			get
			{
				return int.MaxValue;
			}
		}

		public override int MinimumPossibleValue
		{
			get
			{
				return int.MinValue;
			}
		}

		public override int ZeroOrOrigin
		{
			get
			{
				return 0;
			}
		}

		public override int Add(int x, int y)
		{
			return x + y;
		}

		public override int Subtract(int x, int y)
		{
			return x - y;
		}

		public override int Multiply(int x, int y)
		{
			return x * y;
		}

		public override int Divide(int x, int y)
		{
			return (int)Math.Round((decimal)x / (decimal)y);
		}

		public override int Half(int x)
		{
			return Divide(x, 2);
		}

		public override int Random(int min, int max)
		{
			// return add(min, new Random().nextInt(subtract(max, min)));
			return Add(min, Foundation.Random.Next(min, max));
		}

	}
}
