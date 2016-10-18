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



//
// THIS CODE MAKES THE ASSUMPTION THAT YOU ARE RUNNING IT IN A YEAR WITH FOUR DIGITS.
// IF YOU ARE SOMEHOW STILL USING THIS CODE AFTER JANUARY 1, 10000, IT WILL NEED
// TO BE FIXED.
//




using System;
using Rockabilly.Common;

namespace Rockabilly.CoarseGrind.Descriptions
{
	public class DateTimeFieldDescription : FieldDescription<DateTime>
	{
		public DateTimeFieldTargets Target = DateTimeFieldTargets.HAPPY_PATH;
		private DateTimeLimitsDescription Bounds = null;

		public DateTimeLimitsDescription Limits
		{
			get
			{
				return Bounds;
			}
		}

		public DateTimeFieldDescription(DateTimeLimitsDescription limitsDescription = null) : this()
		{
			Bounds = limitsDescription;
		}

		public DateTimeFieldDescription() : base()
		{
		}

		public DateTimeFieldDescription(DateTime basisValue) : this()
		{
			BasisValue = basisValue;
		}

		public DateTimeFieldDescription(DateTime basisValue, DateTimeLimitsDescription limitsDescription) : this()
		{
			basisValue = BasisValue;
			Bounds = limitsDescription;
		}

		public bool isLimited()
		{
			return Bounds != null;
		}

		public override string ToString()
		{
			if (Target == DateTimeFieldTargets.EXPLICIT)
				return Target.ToString() + " (" + BasisValue + ")";
			return Target.ToString();
		}

		public const long ONE_DAY = (long)86400000;
		public static readonly DateTime ONE_DAY_DATE = new DateTime(ONE_DAY);
		public const long YEAR_AND_HALF = (long)4733538 * (long)1000;
		public static readonly DateTime YEAR_AND_HALF_DATE = new DateTime(YEAR_AND_HALF);
		public static readonly DateTime MAX_DATE = new DateTime(long.MaxValue);
		public static readonly DateTime MIN_DATE = new DateTime(long.MinValue);
		private static string dateFormatstring = "yyyyMMdd";
		private static readonly string CURRENT_MONTH = DateTime.Now.ToString(dateFormatstring).Substring(4, 6);
		private static readonly string CURRENT_DAY = DateTime.Now.ToString(dateFormatstring).Substring(6);
		private static readonly string HAPPY_YEAR = "" + (DateTime.Now.Year - 30); //string.format("%04d", Calendar.Instance().(Calendar.YEAR) - 30);

		public DateTime PositiveMinisculeValue
		{
			get
			{
				return ONE_DAY_DATE;
			}
		}

		public DateTime PositiveModerateValue
		{
			get
			{
				// 1.5 years
				return YEAR_AND_HALF_DATE;
			}
		}

		public DateTime MaximumPossibleValue
		{
			get
			{
				return MAX_DATE;
			}
		}

		public DateTime MinimumPossibleValue
		{
			get
			{
				return MIN_DATE;
			}
		}

		public DateTime Present
		{
			get
			{
				return new DateTime();
			}
		}

		public DateTime Add(DateTime x, DateTime y)
		{
			return new DateTime(x.Ticks + y.Ticks);
		}

		public DateTime Subtract(DateTime x, DateTime y)
		{
			return new DateTime(x.Ticks - y.Ticks);
		}

		// Needed here but should not be exposed to user.
		// User might want a "halfway between then and now" method.
		private DateTime Half(DateTime x)
		{
			return new DateTime(x.Ticks / 2);
		}

		public DateTime Random(DateTime min, DateTime max)
		{
			return new DateTime(Foundation.RandomLong(min.Ticks, max.Ticks));
			//return Add( min, new DateTime(Foundation.nextlong(new Random(), Subtract(max, min).Ticks)));
		}

		public DateTime Size
		{
			get
			{
				return Subtract(Bounds.UpperLimit, Bounds.LowerLimit);
			}
		}

		public DateTime NegativeMinisculeValue
		{
			get
			{
				return Subtract(Present, PositiveMinisculeValue);
			}
		}

		public DateTime NegativeModerateValue
		{
			get
			{
				return Subtract(Present, PositiveModerateValue);
			}
		}

		public override DateTime DescribedValue
		{
			get
			{
				DateTime result = DateTime.Now;
				switch (Target)
				{
					case DateTimeFieldTargets.EXPLICIT:
						if (BasisValue == null)
						{
							throw new InappropriateDescriptionException();
						}

						return BasisValue;
					case DateTimeFieldTargets.HAPPY_PATH:
						if (BasisValue == null)
						{
							if (!isLimited())
								throw new InappropriateDescriptionException();
							return Add(Bounds.LowerLimit, Half(Size));
						}

						return BasisValue;
					case DateTimeFieldTargets.MAXIMUM_POSSIBLE_VALUE:
						return MaximumPossibleValue;
					case DateTimeFieldTargets.MINIMUM_POSSIBLE_VALUE:
						return MinimumPossibleValue;
					case DateTimeFieldTargets.AT_LOWER_LIMIT:
						return Bounds.LowerLimit;
					case DateTimeFieldTargets.AT_UPPER_LIMIT:
						return Bounds.UpperLimit;
					case DateTimeFieldTargets.AT_PRESENT:
						return Present;
					case DateTimeFieldTargets.NULL:
						throw new InappropriateDescriptionException();
					case DateTimeFieldTargets.RANDOM_WITHIN_LIMITS:
						return Random(Add(Bounds.LowerLimit, Half(Half(Size))), Subtract(Bounds.UpperLimit, Half(Half(Size))));
					case DateTimeFieldTargets.SLIGHTLY_IN_FUTURE:
						return Add(Present, PositiveMinisculeValue);
					case DateTimeFieldTargets.SLIGHTLY_IN_PAST:
						return Subtract(Present, PositiveMinisculeValue);
					case DateTimeFieldTargets.SLIGHTLY_BEYOND_LOWER_LIMIT:
						return Subtract(Bounds.LowerLimit, PositiveMinisculeValue);
					case DateTimeFieldTargets.SLIGHTLY_BEYOND_UPPER_LIMIT:
						return Add(Bounds.UpperLimit, PositiveMinisculeValue);
					case DateTimeFieldTargets.SLIGHTLY_WITHIN_LOWER_LIMIT:
						return Add(Bounds.LowerLimit, PositiveMinisculeValue);
					case DateTimeFieldTargets.SLIGHTLY_WITHIN_UPPER_LIMIT:
						return Subtract(Bounds.UpperLimit, PositiveMinisculeValue);
					case DateTimeFieldTargets.WELL_IN_FUTURE:
						return Add(Present, PositiveModerateValue);
					case DateTimeFieldTargets.WELL_IN_PAST:
						return Subtract(Present, PositiveModerateValue);
					case DateTimeFieldTargets.WELL_BEYOND_LOWER_LIMIT:
						return Subtract(Bounds.LowerLimit, PositiveModerateValue);
					case DateTimeFieldTargets.WELL_BEYOND_UPPER_LIMIT:
						return Add(Bounds.UpperLimit, PositiveModerateValue);
					case DateTimeFieldTargets.WELL_WITHIN_LOWER_LIMIT:
						return Add(Bounds.LowerLimit, PositiveModerateValue);
					case DateTimeFieldTargets.WELL_WITHIN_UPPER_LIMIT:
						return Subtract(Bounds.UpperLimit, PositiveModerateValue);
					case DateTimeFieldTargets.SLIGHTLY_ABOVE_MINIMUM:
						return Add(MinimumPossibleValue, PositiveMinisculeValue);
					case DateTimeFieldTargets.SLIGHTLY_BELOW_MAXIMUM:
						return Subtract(MaximumPossibleValue, PositiveMinisculeValue);
					case DateTimeFieldTargets.FIVE_DIGIT_YEAR:
						result.AddYears(10000);
						return result;
					case DateTimeFieldTargets.FOUR_DIGIT_YEAR:
						return Present; // Make sure you remember to change this
										// before January 1'st, 10000.
					case DateTimeFieldTargets.THREE_DIGIT_YEAR:
						result.AddYears(-1000);
						break;
					case DateTimeFieldTargets.TWO_DIGIT_YEAR:
						result.AddYears(-1 * (DateTime.Now.Year - 50));
						break;
					case DateTimeFieldTargets.SINGLE_DIGIT_YEAR:
						result.AddYears(-1 * (DateTime.Now.Year - 3));
						break;
					case DateTimeFieldTargets.TWO_DIGIT_MONTH:
						return new DateTime(result.Year - 1, Foundation.Random.Next(11, 12), result.Day);
					case DateTimeFieldTargets.SINGLE_DIGIT_MONTH:
						return new DateTime(result.Year - 1, Foundation.Random.Next(1, 9), result.Day);
					case DateTimeFieldTargets.TWO_DIGIT_DAY:
						return new DateTime(result.Year - 1, result.Month - 1, Foundation.Random.Next(10, 28));
					case DateTimeFieldTargets.SINGLE_DIGIT_DAY:
						return new DateTime(result.Year - 1, result.Month - 1, Foundation.Random.Next(1, 9));
					case DateTimeFieldTargets.SINGLE_DIGIT_MONTH_AND_DAY:
						return new DateTime(result.Year - 1, Foundation.Random.Next(11, 12), Foundation.Random.Next(10, 28));
					default:
						throw new NoValueException();
				}

				return result;
			}
		}

		public override bool HasSpecificHappyValue
		{
			get
			{
				return ((Target == DateTimeFieldTargets.HAPPY_PATH) && BasisValue != null);
			}
		}

		public override bool IsExplicit
		{
			get
			{
				return Target == DateTimeFieldTargets.EXPLICIT;
			}
		}

		public override bool IsDefault
		{
			get
			{
				return Target == DateTimeFieldTargets.DEFAULT;
			}
		}

		public override void SetToExplicitValue(DateTime value)
		{
			BasisValue = value;
			Target = DateTimeFieldTargets.EXPLICIT;
		}
	}
}
