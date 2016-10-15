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
	public class SimpleFieldDescription<T> : FieldDescription<T>
	{
		public SimpleFieldTargets Target = SimpleFieldTargets.HAPPY_PATH;

		public SimpleFieldDescription() : base()
		{
		}

		public SimpleFieldDescription(T basisValue) : base()
		{
			BasisValue = basisValue;
		}

		public override bool HasSpecificHappyValue
		{
			get
			{
				return ((Target == SimpleFieldTargets.HAPPY_PATH) && BasisValue != null);
			}
		}

		public override bool IsExplicit
		{
			get
			{
				return Target == SimpleFieldTargets.EXPLICIT;
			}
		}

		public override bool IsDefault
		{
			get
			{
				return Target == SimpleFieldTargets.DEFAULT;
			}
		}

		public override T DescribedValue
		{
			get
			{
				switch (Target)
				{
					case SimpleFieldTargets.DEFAULT:
						throw new NoValueException();
					case SimpleFieldTargets.HAPPY_PATH:
					case SimpleFieldTargets.EXPLICIT:
						if (BasisValue == null)
							throw new InappropriateDescriptionException();
						return BasisValue;
				}

				return default(T);
			}
		}

		public override string ToString()
		{
			if (Target == SimpleFieldTargets.EXPLICIT)
		return Target.ToString() + " (" + BasisValue + ")";
			return Target.ToString();
		}

		public override void SetToExplicitValue(T value)
		{
			BasisValue = value;
			Target = SimpleFieldTargets.EXPLICIT;
		}
	}
}
