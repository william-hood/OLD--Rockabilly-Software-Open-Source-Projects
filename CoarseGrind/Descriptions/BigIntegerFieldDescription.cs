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
using System.Numerics;

namespace Rockabilly.CoarseGrind.Descriptions
{
	public class BigIntegerFieldDescription : ValueFieldDescription<BigInteger>
	{
		public BigIntegerFieldDescription(LimitsDescription<BigInteger> limitsDescription) : base(limitsDescription)
		{
		}

		public BigIntegerFieldDescription() : base()
		{
		}

		public BigIntegerFieldDescription(BigInteger BasisValue) : base(BasisValue)
		{
		}

		public override BigInteger PositiveMinisculeValue
		{
			get
			{
				return BigInteger.One;
			}
		}

		public override BigInteger PositiveModerateValue
		{
			get
			{
				return new BigInteger((long)100);
			}
		}

		public override BigInteger MaximumPossibleValue
		{
			get
			{
				// Going by spec, there is no max or min
				throw new InappropriateDescriptionException();
			}
		}

		public override BigInteger MinimumPossibleValue
		{
			get
			{
				// Going by spec, there is no max or min
				throw new InappropriateDescriptionException();
			}
		}

		public override BigInteger ZeroOrOrigin
		{
			get
			{
				return BigInteger.Zero;
			}
		}

		public override BigInteger Add(BigInteger x, BigInteger y)
		{
			return x + y;
		}

		public override BigInteger Subtract(BigInteger x, BigInteger y)
		{
			return x - y;
		}

		public override BigInteger Multiply(BigInteger x, BigInteger y)
		{
			return x * y;
		}

		public override BigInteger Divide(BigInteger x, BigInteger y)
		{
			return x / y;
		}

		public override BigInteger Half(BigInteger x)
		{
			return Divide(x, new BigInteger((long)2));
		}

		public override BigInteger Random(BigInteger min, BigInteger max)
		{
			// return Add(min, new Random().nextInt(Subtract(max, min)));
			// Will implement support later
			throw new InappropriateDescriptionException();
		}

	}
}
