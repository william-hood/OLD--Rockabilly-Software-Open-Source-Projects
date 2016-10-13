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
using System.IO;

namespace Rockabilly.Common
{
	public class TextOutputManager
	{
		// Creates a file with header only if the first call to write is made.
		// Delimited data to use???
		// Need method to properly flush and close.
		private TextWriter internalTextWriter = null;
		private string expectedFileName = null;
		private bool shouldAppend = false;

		internal TextOutputManager() { }

		public TextOutputManager(TextWriter outputStream)
		{
			internalTextWriter = outputStream;
		}

		public TextOutputManager(string filename, bool append = false)
		{
			expectedFileName = filename;
			shouldAppend = append;
		}

		private void CreateTextWriterIfNeeded()
		{
			if (internalTextWriter == null)
			{
				if (expectedFileName.IsBlank())
				{
					internalTextWriter = Console.Out;
				}
				else
				{
					try
					{
						new FileInfo(expectedFileName).Directory.Create();
						if (shouldAppend)
						{
							internalTextWriter = File.AppendText(expectedFileName);
						}
						else
						{
							internalTextWriter = File.CreateText(expectedFileName);
						}
					}
					catch
					{
						internalTextWriter = Console.Out;
						WriteLine("WARNING: Could not create output file (" + expectedFileName + ").  Reverting to standard console output.");
					}
				}
			}
		}

		public void WriteLine(string output)
		{
			CreateTextWriterIfNeeded();
			internalTextWriter.Write(output);
			internalTextWriter.Write(Symbols.CarriageReturnLineFeed);
		}

		public void Flush()
		{
			try
			{
				internalTextWriter.Flush();
			}
			catch
			{

			}
		}

		public void Close()
		{
			try
			{
				Flush();
				internalTextWriter.Dispose();
			}
			catch
			{

			}
			finally
			{
				internalTextWriter = null;
				GC.Collect();
			}
		}
	}
}
