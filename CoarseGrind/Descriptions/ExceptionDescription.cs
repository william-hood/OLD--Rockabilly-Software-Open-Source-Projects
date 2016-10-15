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
using System.Text;

namespace Rockabilly.CoarseGrind.Descriptions
{
	public class ExceptionDescription
	{
		private Type<T> where T : Exception ExceptionType = null;
		public string MessageSubstring = "";
		public ExceptionDescription Cause = null;
		private string ExceptionTypePartialName = Values.Defaultstring;

		public ExceptionDescription(Class<? extends Exception> failureType, string messageSubstring)
		{
			ExceptionType = failureType;
			MessageSubstring = messageSubstring;
		}

		public ExceptionDescription(Class<? extends Exception> failureType,
				string messageSubstring, ExceptionDescription cause)
		{
			this(failureType, messageSubstring);
			Cause = cause;
		}

		public ExceptionDescription(string failureTypePartial, string messageSubstring)
		{
			ExceptionTypePartialName = failureTypePartial;
			MessageSubstring = messageSubstring;
		}

		private bool MessageMatches(string candidate)
		{
			if (MessageSubstring.Length > 0)
			{
				if (!candidate.Contains(MessageSubstring))
					return false;
			}

			return true;
		}

		public bool IsMatchTo(Exception candidateException)
		{
			if (ExceptionTypePartialName != default(string))
				return IsMatchTo(candidateException.GetType().FullName,
						candidateException.getMessage());
			if (candidateException.getClass() != ExceptionType)
				return false;
			if (!MessageMatches(candidateException.Message))
				return false;
			if (Cause != null)
				return Cause.IsMatchTo(candidateException);
			return true;
		}

		public bool IsMatchTo(string candidateExceptionName, string candidateExceptionMessage)
		{
			if (!candidateExceptionName.Contains(ExceptionTypePartialName))
				return false;
			if (!MessageMatches(candidateExceptionMessage))
				return false;
			return true;
		}

	public override string ToString()
		{
			StringBuilder result = new StringBuilder();

			if (ExceptionTypePartialName == default(string))
			{
				result.Append("failure of type " + ExceptionType.ToString());
			}
			else {
				result.Append("failure with type name containing \""
						+ ExceptionTypePartialName + "\"");
			}

			if (MessageSubstring.Length > 0)
				result.Append(" with message containing \"" + MessageSubstring
						+ "\"");

			if (Cause != null)
				result.Append("; Caused by " + Cause.ToString());
			return result.ToString();
		}
	}
}
