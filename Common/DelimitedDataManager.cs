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
using System.Text;
using System.Collections.Generic;
namespace Rockabilly.Common
{
	public class DelimitedDataManager<T>
	{
		public const char DEFAULT_DELIMITER = ',';
		public const int DEFAULT_SPACING = 1;
		// public const string DEFAULT_EXTENSION = "csv";

		private List<string> headers = new List<string>();
		private List<List<T>> data = new List<List<T>>();
		char delimiter = DEFAULT_DELIMITER;
		int spacing = DEFAULT_SPACING;

		public DelimitedDataManager(char delimitingChar = DEFAULT_DELIMITER, int spacesAfterDelimitingChar = DEFAULT_SPACING, params string[] columnNames)
		{
			// for (int cursor = 0; cursor < columnNames.length; cursor++)
			foreach (string thisColumn in columnNames)
			{
				headers.Add(thisColumn);
			}
			delimiter = delimitingChar;
			spacing = spacesAfterDelimitingChar;
		}

		// Explicitly prohibit default constructor.
		private DelimitedDataManager()
		{
		}

		public string stripDelimiters(string input)
		{
			char replacement = ' ';
			if (delimiter == replacement) replacement = '-';
			return (input.Replace(delimiter, replacement));
		}

		public List<List<T>> AllData
		{
			get
			{
				return data;
			}
		}

		public void addDataRow(List<T> newDataRow)
		{
			data.Add(newDataRow);
		}

		public void removeDataRow(string columnName, T value)
		{
			List<T> dataRow = GetDataRow(columnName, value);
			if (dataRow != null)
			{
				data.Remove(dataRow);
			}
		}

		public int length
		{
			get
			{
				if (headers.Count < 1) throw new ImproperObjectConstructionException("A " + this.GetType().Name + " must have at least one header column.");
				return headers.Count;
			}
		}

		public List<T> GetDataRow(string columnName, T value)
		{
			int selectedColumn = default(int);

			for (int columnCursor = 0; columnCursor < headers.Count; columnCursor++)
			{
				if (headers[columnCursor].Equals(columnName))
				{
					selectedColumn = columnCursor;
					break;
				}
			}

			if (selectedColumn == default(int))
				return null;

			foreach (List<T> thisDataRow in data)
			{
				if (thisDataRow[selectedColumn].Equals(value))
					return thisDataRow;
			}

			return null;
		}

		public bool HasDataRow(string columnName, T value)
		{
			return (GetDataRow(columnName, value) != null);
		}

		private string LineOut<SPECIFIED_TYPE>(List<SPECIFIED_TYPE> dataRow)
		{
			StringBuilder lineOutBuilder = new StringBuilder();

			for (int cursor = 0; cursor < dataRow.Count; cursor++)
			{
				if (cursor != 0)
				{
					lineOutBuilder.Append(delimiter);
					lineOutBuilder.Append(new String(' ', spacing));
				}
				lineOutBuilder.Append(dataRow[cursor]);
			}

			return lineOutBuilder.ToString();
		}

		public void ToFile(string completeFilePath, bool append = true)
		{
			TextOutputManager thisOutputManager = new TextOutputManager(completeFilePath, append);
			thisOutputManager.WriteLine(LineOut(headers));

			foreach (List<T> thisData in data)
			{
				thisOutputManager.WriteLine(LineOut(thisData));
			}

			thisOutputManager.Close();
		}

		public List<string> StringRowFromTextLine(string textLine)
		{
			List<string> result = new List<string>();
			string[] theseRows = textLine.Split(delimiter);

			foreach (string thisRow in theseRows)
			{
				result.Add(thisRow.Replace(delimiter.ToString(), ""));
			}

			return result;
		}

		public static DelimitedDataManager<T> FromFile<T>(string completeFilePath, Parser<T> parser, char delimitingChar = DEFAULT_DELIMITER)
		{
			DelimitedDataManager<T> dataFromFile = new DelimitedDataManager<T>();
			dataFromFile.delimiter = delimitingChar;
			TextReader fileStream = File.OpenText(completeFilePath);
			string currentLine = fileStream.ReadLine();
			if (currentLine != null)
				dataFromFile.headers = dataFromFile.StringRowFromTextLine(currentLine);
			while (currentLine != null)
			{
				currentLine = fileStream.ReadLine();
				if (currentLine != null)
					dataFromFile.data.Add(dataFromFile.dataRowFromTextLine(currentLine, parser));
			}

			fileStream.Close();
			return dataFromFile;
		}

		public List<T> dataRowFromTextLine(string textLine, Parser<T> parser)
		{
			List<string> textRow = StringRowFromTextLine(textLine);

			List<T> thisRow = new List<T>(textRow.Count);

			foreach (string thisTextRow in textRow)
			{
				thisRow.Add(parser.parseMethod(thisTextRow));
			}

			return thisRow;
		}
	}
}
