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
	public enum ValueFieldTargets
	{
		DEFAULT, NULL, HAPPY_PATH, EXPLICIT, MAXIMUM_POSSIBLE_VALUE, RANDOM_WITHIN_LIMITS, SLIGHTLY_BELOW_MAXIMUM, SLIGHTLY_ABOVE_MINIMUM, MINIMUM_POSSIBLE_VALUE, WELL_BEYOND_UPPER_LIMIT, SLIGHTLY_BEYOND_UPPER_LIMIT, AT_UPPER_LIMIT, SLIGHTLY_WITHIN_UPPER_LIMIT, WELL_WITHIN_UPPER_LIMIT, WELL_ABOVE_ZERO, SLIGHTLY_ABOVE_ZERO, AT_ZERO, SLIGHTLY_BELOW_ZERO, WELL_BELOW_ZERO, WELL_BEYOND_LOWER_LIMIT, SLIGHTLY_BEYOND_LOWER_LIMIT, AT_LOWER_LIMIT, SLIGHTLY_WITHIN_LOWER_LIMIT, WELL_WITHIN_LOWER_LIMIT
	}

	public static class ValueFieldTargets_Extensions
	{
		public static string ToString(this ValueFieldTargets it)
		{
			switch (it)
			{
				case ValueFieldTargets.AT_LOWER_LIMIT:
					return "At Lower Limit";
				case ValueFieldTargets.AT_UPPER_LIMIT:
					return "At Upper Limit";
				case ValueFieldTargets.AT_ZERO:
					return "Zero";
				case ValueFieldTargets.EXPLICIT:
					return "Explicit Value";
				case ValueFieldTargets.HAPPY_PATH:
					return "Happy Path";
				case ValueFieldTargets.MAXIMUM_POSSIBLE_VALUE:
					return "Maximum Possible";
				case ValueFieldTargets.MINIMUM_POSSIBLE_VALUE:
					return "Minimum Possible";
				case ValueFieldTargets.NULL:
					return "Explicit Null";
				case ValueFieldTargets.RANDOM_WITHIN_LIMITS:
					return "Random Within Limits";
				case ValueFieldTargets.SLIGHTLY_ABOVE_MINIMUM:
					return "Slightly Above Minimum";
				case ValueFieldTargets.SLIGHTLY_ABOVE_ZERO:
					return "Slightly Above Zero";
				case ValueFieldTargets.SLIGHTLY_BELOW_MAXIMUM:
					return "Slightly Below Maximum";
				case ValueFieldTargets.SLIGHTLY_BELOW_ZERO:
					return "Slightly Below Zero";
				case ValueFieldTargets.SLIGHTLY_BEYOND_LOWER_LIMIT:
					return "Slightly Beyond Lower Limit";
				case ValueFieldTargets.SLIGHTLY_BEYOND_UPPER_LIMIT:
					return "Slightly Beyond Upper Limit";
				case ValueFieldTargets.SLIGHTLY_WITHIN_LOWER_LIMIT:
					return "Slightly Within Lower Limit";
				case ValueFieldTargets.SLIGHTLY_WITHIN_UPPER_LIMIT:
					return "Slightly Within Upper Limit";
				case ValueFieldTargets.WELL_ABOVE_ZERO:
					return "Well Above Zero";
				case ValueFieldTargets.WELL_BELOW_ZERO:
					return "Well Below Zero";
				case ValueFieldTargets.WELL_BEYOND_LOWER_LIMIT:
					return "Well Beyond Lower Limit";
				case ValueFieldTargets.WELL_BEYOND_UPPER_LIMIT:
					return "Well Beyond Upper Limit";
				case ValueFieldTargets.WELL_WITHIN_LOWER_LIMIT:
					return "Well Within Lower Limit";
				case ValueFieldTargets.WELL_WITHIN_UPPER_LIMIT:
					return "Well Within Upper Limit";
			}

			return "Left Default";
		}

		public static bool IsLimitRelevant(this ValueFieldTargets it)
		{
			switch (it)
			{
				case ValueFieldTargets.WELL_BEYOND_LOWER_LIMIT:
				case ValueFieldTargets.WELL_BEYOND_UPPER_LIMIT:
				case ValueFieldTargets.WELL_WITHIN_LOWER_LIMIT:
				case ValueFieldTargets.WELL_WITHIN_UPPER_LIMIT:
				case ValueFieldTargets.SLIGHTLY_BEYOND_LOWER_LIMIT:
				case ValueFieldTargets.SLIGHTLY_BEYOND_UPPER_LIMIT:
				case ValueFieldTargets.SLIGHTLY_WITHIN_LOWER_LIMIT:
				case ValueFieldTargets.SLIGHTLY_WITHIN_UPPER_LIMIT:
				case ValueFieldTargets.RANDOM_WITHIN_LIMITS:
				case ValueFieldTargets.AT_LOWER_LIMIT:
				case ValueFieldTargets.AT_UPPER_LIMIT:
					return true;
			}
			return false;
		}

		public static bool IsHappyOrExplicit(this ValueFieldTargets it)
		{
			if (it == ValueFieldTargets.HAPPY_PATH)
				return true;
			if (it == ValueFieldTargets.EXPLICIT)
				return true;
			return false;
		}
	}
}