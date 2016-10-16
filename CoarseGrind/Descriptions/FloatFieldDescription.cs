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
	public class FloatFieldDescription : ValueFieldDescription<float>
	{
		public FloatFieldDescription(LimitsDescription<float> limitsDescription) : base(limitsDescription)
		{
		}

		public FloatFieldDescription() : base()
		{
		}

		public FloatFieldDescription(float basisValue) : base(basisValue)
		{
		}


		public override float PositiveMinisculeValue
		{
			get
			{
				return (float)1;
			}
		}


		public override float PositiveModerateValue
		{
			get
			{
				return (float)100;
			}
		}


		public override float MaximumPossibleValue
		{
			get
			{
				return float.MaxValue;
			}
		}


		public override float MinimumPossibleValue
		{
			get
			{
				return float.MinValue;
			}
		}


		public override float ZeroOrOrigin
		{
			get
			{
				return (float)0;
			}
		}


		public override float Add(float x, float y)
		{
			return x + y;
		}


		public override float Subtract(float x, float y)
		{
			return x - y;
		}


		public override float Multiply(float x, float y)
		{
			return x * y;
		}


		public override float Divide(float x, float y)
		{
			return x / y;
		}


		public override float Half(float x)
		{
			return Divide(x, (float)2);
		}


		public override float Random(float min, float max)
		{
			// return add(min, new Random().nextInt(subtract(max, min)));
			// Will implement support later
			throw new InappropriateDescriptionException();
		}

	}
}
