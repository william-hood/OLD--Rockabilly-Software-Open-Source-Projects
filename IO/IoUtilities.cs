// Copyright (c) 2019, 2016 William Arthur Hood
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
using System.Management;
using System.IO;
using System.Collections.Generic;
using Rockabilly.Strings;

namespace Rockabilly.IO
{
    public static class IoUtilities
    {

        public static string OperatingSystemName
        {
            get
            {
                return Environment.OSVersion.ToString();
            }
        }

        public static string ShortFileName(this string completeFilePath)
        {
            string baseExt = completeFilePath.Substring(completeFilePath.LastIndexOf(Path.DirectorySeparatorChar) + 1);
            if (completeFilePath.Contains("."))
            {
                return baseExt.Substring(0, baseExt.LastIndexOf('.'));
            }

            return baseExt;
        }

        public static string UserHomeFolder
        {
            get
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            }
        }

        public static string CurrentWorkingDirectory
        {
            get
            {
                return Environment.CurrentDirectory;
            }
        }

        public static void CopyCompletely(string sourcePath, string destinationPath)
        {
            FileAttributes check = File.GetAttributes(sourcePath);
            if (check.HasFlag(FileAttributes.Directory))
            {
                List<string> contents = new List<string>(Directory.EnumerateFileSystemEntries(sourcePath, "*"));
                foreach (string thisFile in contents)
                {
                    CopyCompletely(sourcePath + Path.DirectorySeparatorChar + Path.GetFileName(thisFile), destinationPath + Path.DirectorySeparatorChar + Path.GetFileName(thisFile));
                }
            }
            else
            {
                Directory.CreateDirectory(Path.GetDirectoryName(destinationPath));
                File.Copy(sourcePath, destinationPath);
            }
        }


        public static string ReadLineFromInputStream(BufferedStream rawInputStream)
        {
            StringBuilder result = new StringBuilder();
            int lastRead = default(int);
            while (lastRead != -1)
            {
                try
                {
                    if (rawInputStream.Length < 1) break; // Was BufferedInputStream.available()
                    lastRead = rawInputStream.ReadByte();
                    result.Append((char)lastRead);
                    if (((char)lastRead) == '\n') break;
                }
                catch (IOException)
                {
                    break;
                }
            }

            return result.ToString().Trim(); // Intentionally trimming off the carriage return at the end.
        }
    }
}
