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
	public abstract class ValueFieldDescription<T> : FieldDescription<T>
	{
		public ValueFieldTargets target = ValueFieldTargets.HAPPY_PATH;
		private LimitsDescription<T> bounds = null;

		public LimitsDescription<T> getLimits()
		{
			return bounds;
		}

		public ValueFieldDescription(LimitsDescription<T> limitsDescription) : this()
		{
			bounds = limitsDescription;
		}

		public ValueFieldDescription() : base()
		{
		}

		public ValueFieldDescription(T BasisValue) : this()
		{
			BasisValue = BasisValue;
		}

		public ValueFieldDescription(T BasisValue, LimitsDescription<T> limitsDescription) : this()
		{
			BasisValue = BasisValue;
			bounds = limitsDescription;
		}

		public bool IsLimited
		{
			get
			{
				return bounds != null;
			}
		}

		public override string ToString()
		{
			if (target == ValueFieldTargets.EXPLICIT)
				return target.ToString() + " (" + BasisValue + ")";
			return target.ToString();
		}

		public abstract T PositiveMinisculeValue { get; } //throws InappropriateDescriptionException;
		public abstract T PositiveModerateValue { get; } //throws InappropriateDescriptionException;
		public abstract T MaximumPossibleValue { get; } //throws InappropriateDescriptionException;
		public abstract T MinimumPossibleValue { get; } //throws InappropriateDescriptionException;
		public abstract T ZeroOrOrigin { get; } //throws InappropriateDescriptionException;

		public abstract T Add(T x, T y); //throws InappropriateDescriptionException;

		public abstract T Subtract(T x, T y);//throws InappropriateDescriptionException;

		public abstract T Multiply(T x, T y); //throws InappropriateDescriptionException;

		public abstract T Divide(T x, T y); // throws InappropriateDescriptionException;

		public abstract T Half(T x); // throws InappropriateDescriptionException;

		public abstract T Random(T min, T max); //throws InappropriateDescriptionException;

		public T Size// throws InappropriateDescriptionException
		{
			get
			{
				return Subtract(bounds.UpperLimit, bounds.LowerLimit);
			}
		}

		public T NegativeMinisculeValue //throws InappropriateDescriptionException
		{
			get
			{
				return Subtract(ZeroOrOrigin, PositiveMinisculeValue);
			}
		}

		public T NegativeModerateValue //throws InappropriateDescriptionException
		{
			get
			{
				return Subtract(ZeroOrOrigin, PositiveModerateValue);
			}
		}

		public override T DescribedValue
		{
			get
			{
				switch (target)
				{
					case ValueFieldTargets.EXPLICIT:
						if (BasisValue == null)
							throw new InappropriateDescriptionException();
						return BasisValue;
					case ValueFieldTargets.HAPPY_PATH:
						if (BasisValue == null)
						{
							if (!IsLimited)
								throw new InappropriateDescriptionException();
							return Add(bounds.LowerLimit, Half(Size));
						}

						return BasisValue;
					case ValueFieldTargets.MAXIMUM_POSSIBLE_VALUE:
						return MaximumPossibleValue;
					case ValueFieldTargets.MINIMUM_POSSIBLE_VALUE:
						return MinimumPossibleValue;
					case ValueFieldTargets.AT_LOWER_LIMIT:
						return bounds.LowerLimit;
					case ValueFieldTargets.AT_UPPER_LIMIT:
						return bounds.UpperLimit;
					case ValueFieldTargets.AT_ZERO:
						return ZeroOrOrigin;
					case ValueFieldTargets.NULL:
						return default(T);
					case ValueFieldTargets.RANDOM_WITHIN_LIMITS:
						return Random(Add(bounds.LowerLimit, Half(Half(Size))), Subtract(bounds.UpperLimit, Half(Half(Size))));
					case ValueFieldTargets.SLIGHTLY_ABOVE_ZERO:
						return PositiveMinisculeValue;
					case ValueFieldTargets.SLIGHTLY_BELOW_ZERO:
						return NegativeMinisculeValue;
					case ValueFieldTargets.SLIGHTLY_BEYOND_LOWER_LIMIT:
						return Subtract(bounds.LowerLimit, PositiveMinisculeValue);
					case ValueFieldTargets.SLIGHTLY_BEYOND_UPPER_LIMIT:
						return Add(bounds.UpperLimit, PositiveMinisculeValue);
					case ValueFieldTargets.SLIGHTLY_WITHIN_LOWER_LIMIT:
						return Add(bounds.LowerLimit, PositiveMinisculeValue);
					case ValueFieldTargets.SLIGHTLY_WITHIN_UPPER_LIMIT:
						return Subtract(bounds.UpperLimit, PositiveMinisculeValue);
					case ValueFieldTargets.WELL_ABOVE_ZERO:
						return PositiveModerateValue;
					case ValueFieldTargets.WELL_BELOW_ZERO:
						return NegativeModerateValue;
					case ValueFieldTargets.WELL_BEYOND_LOWER_LIMIT:
						return Subtract(bounds.LowerLimit, PositiveModerateValue);
					case ValueFieldTargets.WELL_BEYOND_UPPER_LIMIT:
						return Add(bounds.UpperLimit, PositiveModerateValue);
					case ValueFieldTargets.WELL_WITHIN_LOWER_LIMIT:
						return Add(bounds.LowerLimit, PositiveModerateValue);
					case ValueFieldTargets.WELL_WITHIN_UPPER_LIMIT:
						return Subtract(bounds.UpperLimit, PositiveModerateValue);
					case ValueFieldTargets.SLIGHTLY_ABOVE_MINIMUM:
						return Add(MinimumPossibleValue, PositiveMinisculeValue);
					case ValueFieldTargets.SLIGHTLY_BELOW_MAXIMUM:
						return Subtract(MaximumPossibleValue, PositiveMinisculeValue);
				}

				throw new NoValueException();
			}
		}

		public override bool HasSpecificHappyValue
		{
			get
			{
				return ((target == ValueFieldTargets.HAPPY_PATH) && BasisValue != null);
			}
		}

		public override bool IsExplicit
		{
			get
			{
				return target == ValueFieldTargets.EXPLICIT;
			}
		}

		public override bool IsDefault
		{
			get
			{
				return target == ValueFieldTargets.DEFAULT;
			}
		}

		public override void SetToExplicitValue(T value)
		{
			BasisValue = value;
			target = ValueFieldTargets.EXPLICIT;
		}
	}
}