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
	public enum DateTimeFieldTargets
	{
		DEFAULT, NULL, HAPPY_PATH, EXPLICIT, MAXIMUM_POSSIBLE_VALUE, RANDOM_WITHIN_LIMITS, SLIGHTLY_BELOW_MAXIMUM, SLIGHTLY_ABOVE_MINIMUM, MINIMUM_POSSIBLE_VALUE, WELL_BEYOND_UPPER_LIMIT, SLIGHTLY_BEYOND_UPPER_LIMIT, AT_UPPER_LIMIT, SLIGHTLY_WITHIN_UPPER_LIMIT, WELL_WITHIN_UPPER_LIMIT, WELL_IN_FUTURE, SLIGHTLY_IN_FUTURE, AT_PRESENT, SLIGHTLY_IN_PAST, WELL_IN_PAST, WELL_BEYOND_LOWER_LIMIT, SLIGHTLY_BEYOND_LOWER_LIMIT, AT_LOWER_LIMIT, SLIGHTLY_WITHIN_LOWER_LIMIT, WELL_WITHIN_LOWER_LIMIT, FIVE_DIGIT_YEAR, FOUR_DIGIT_YEAR, THREE_DIGIT_YEAR, TWO_DIGIT_YEAR, SINGLE_DIGIT_YEAR, TWO_DIGIT_MONTH, SINGLE_DIGIT_MONTH, TWO_DIGIT_DAY, SINGLE_DIGIT_DAY, SINGLE_DIGIT_MONTH_AND_DAY
	}


	public static class DateTimeFieldTargets_Extensions
	{
		public static string ToString(this DateTimeFieldTargets it)
		{
			switch (it)
			{
				case DateTimeFieldTargets.AT_LOWER_LIMIT:
					return "At Lower Limit";
				case DateTimeFieldTargets.AT_UPPER_LIMIT:
					return "At Upper Limit";
				case DateTimeFieldTargets.AT_PRESENT:
					return "Now";
				case DateTimeFieldTargets.EXPLICIT:
					return "Explicit Value";
				case DateTimeFieldTargets.HAPPY_PATH:
					return "Happy Path";
				case DateTimeFieldTargets.MAXIMUM_POSSIBLE_VALUE:
					return "Maximum Possible";
				case DateTimeFieldTargets.MINIMUM_POSSIBLE_VALUE:
					return "Minimum Possible";
				case DateTimeFieldTargets.NULL:
					return "Explicit Null";
				case DateTimeFieldTargets.RANDOM_WITHIN_LIMITS:
					return "Random Within Limits";
				case DateTimeFieldTargets.SLIGHTLY_ABOVE_MINIMUM:
					return "Slightly Above Minimum";
				case DateTimeFieldTargets.SLIGHTLY_IN_FUTURE:
					return "Slightly into the Future";
				case DateTimeFieldTargets.SLIGHTLY_BELOW_MAXIMUM:
					return "Slightly Below Maximum";
				case DateTimeFieldTargets.SLIGHTLY_IN_PAST:
					return "Slightly into the Past";
				case DateTimeFieldTargets.SLIGHTLY_BEYOND_LOWER_LIMIT:
					return "Slightly Beyond Lower Limit";
				case DateTimeFieldTargets.SLIGHTLY_BEYOND_UPPER_LIMIT:
					return "Slightly Beyond Upper Limit";
				case DateTimeFieldTargets.SLIGHTLY_WITHIN_LOWER_LIMIT:
					return "Slightly Within Lower Limit";
				case DateTimeFieldTargets.SLIGHTLY_WITHIN_UPPER_LIMIT:
					return "Slightly Within Upper Limit";
				case DateTimeFieldTargets.WELL_IN_FUTURE:
					return "Well into the Future";
				case DateTimeFieldTargets.WELL_IN_PAST:
					return "Well into the Past";
				case DateTimeFieldTargets.WELL_BEYOND_LOWER_LIMIT:
					return "Well Beyond Lower Limit";
				case DateTimeFieldTargets.WELL_BEYOND_UPPER_LIMIT:
					return "Well Beyond Upper Limit";
				case DateTimeFieldTargets.WELL_WITHIN_LOWER_LIMIT:
					return "Well Within Lower Limit";
				case DateTimeFieldTargets.WELL_WITHIN_UPPER_LIMIT:
					return "Well Within Upper Limit";
				case DateTimeFieldTargets.FIVE_DIGIT_YEAR:
					return "Five Digit Year";
				case DateTimeFieldTargets.FOUR_DIGIT_YEAR:
					return "Four Digit Year";
				case DateTimeFieldTargets.THREE_DIGIT_YEAR:
					return "Three Digit Year";
				case DateTimeFieldTargets.TWO_DIGIT_YEAR:
					return "Two Digit Year";
				case DateTimeFieldTargets.SINGLE_DIGIT_YEAR:
					return "Single Digit Year";
				case DateTimeFieldTargets.TWO_DIGIT_MONTH:
					return "Two Digit Month";
				case DateTimeFieldTargets.SINGLE_DIGIT_MONTH:
					return "Single Digit Month";
				case DateTimeFieldTargets.TWO_DIGIT_DAY:
					return "Two Digit Day";
				case DateTimeFieldTargets.SINGLE_DIGIT_DAY:
					return "Single Digit Day";
				case DateTimeFieldTargets.SINGLE_DIGIT_MONTH_AND_DAY:
					return "Single Digit Month and Day";
			}

			return "Left Default";
		}

		public static bool IsLimitRelevant(this DateTimeFieldTargets it)
		{
			switch (it)
			{
				case DateTimeFieldTargets.WELL_BEYOND_LOWER_LIMIT:
				case DateTimeFieldTargets.WELL_BEYOND_UPPER_LIMIT:
				case DateTimeFieldTargets.WELL_WITHIN_LOWER_LIMIT:
				case DateTimeFieldTargets.WELL_WITHIN_UPPER_LIMIT:
				case DateTimeFieldTargets.SLIGHTLY_BEYOND_LOWER_LIMIT:
				case DateTimeFieldTargets.SLIGHTLY_BEYOND_UPPER_LIMIT:
				case DateTimeFieldTargets.SLIGHTLY_WITHIN_LOWER_LIMIT:
				case DateTimeFieldTargets.SLIGHTLY_WITHIN_UPPER_LIMIT:
				case DateTimeFieldTargets.RANDOM_WITHIN_LIMITS:
				case DateTimeFieldTargets.AT_LOWER_LIMIT:
				case DateTimeFieldTargets.AT_UPPER_LIMIT:
					return true;
			}
			return false;
		}

		public static bool IsDigitRelevant(this DateTimeFieldTargets it)
		{
			switch (it)
			{
				case DateTimeFieldTargets.FIVE_DIGIT_YEAR:
				case DateTimeFieldTargets.FOUR_DIGIT_YEAR:
				case DateTimeFieldTargets.THREE_DIGIT_YEAR:
				case DateTimeFieldTargets.TWO_DIGIT_YEAR:
				case DateTimeFieldTargets.SINGLE_DIGIT_YEAR:
				case DateTimeFieldTargets.TWO_DIGIT_MONTH:
				case DateTimeFieldTargets.SINGLE_DIGIT_MONTH:
				case DateTimeFieldTargets.TWO_DIGIT_DAY:
				case DateTimeFieldTargets.SINGLE_DIGIT_DAY:
				case DateTimeFieldTargets.SINGLE_DIGIT_MONTH_AND_DAY:
					return true;
			}
			return false;
		}

		public static bool IsHappyOrExplicit(this DateTimeFieldTargets it)
		{
			if (it == DateTimeFieldTargets.HAPPY_PATH)
				return true;
			if (it == DateTimeFieldTargets.EXPLICIT)
				return true;
			return false;
		}
	}
}
