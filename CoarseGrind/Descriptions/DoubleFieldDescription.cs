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
namespace Rockabilly.CoarseGrind.Descriptions
{
	public class DoubleFieldDescription : ValueFieldDescription<double>
	{
		public DoubleFieldDescription(LimitsDescription<double> limitsDescription) : base(limitsDescription)
		{
		}

		public DoubleFieldDescription() : base()
		{
		}

		public DoubleFieldDescription(double basisValue) : base(basisValue)
		{
		}


		public override double PositiveMinisculeValue
		{
			get
			{
				return (double)1;
			}
		}


		public override double PositiveModerateValue
		{
			get
			{
				return (double)100;
			}
		}


		public override double MaximumPossibleValue
		{
			get
			{
				return double.MaxValue;
			}
		}


		public override double MinimumPossibleValue
		{
			get
			{
				return double.MinValue;
			}
		}


		public override double ZeroOrOrigin
		{
			get
			{
				return (double)0;
			}
		}


		public override double Add(double x, double y)
		{
			return x + y;
		}


		public override double Subtract(double x, double y)
		{
			return x - y;
		}


		public override double Multiply(double x, double y)
		{
			return x * y;
		}


		public override double Divide(double x, double y)
		{
			return x / y;
		}


		public override double Half(double x)
		{
			return Divide(x, (double)2);
		}


		public override double Random(double min, double max)
		{
			// return add(min, new Random().nextInt(subtract(max, min)));
			// Will implement support later
			throw new InappropriateDescriptionException();
		}

	}
}
