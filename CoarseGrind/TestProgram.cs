// Copyright (c) 2019 William Arthur Hood
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
// OTHER DEALINGS IN THE SOFTWARE.using System;
using System;
using System.IO;
using System.Reflection;


// Web UI goes away.
// TestCollection is merged into this

namespace Rockabilly.CoarseGrind
{
    public class TestProgram
    {
        public void Run(Assembly target = null, string outputFolder = null)
        {
            if (target == null)
            {
                target = Assembly.GetEntryAssembly();
            }

            string shortName = target.FullName.Split(',')[0];
            TestCollection AvailableTests = new TestCollection(shortName);

            if (outputFolder == null)
            {
                outputFolder = CoarseGrind.DEFAULT_PARENT_FOLDER + Path.DirectorySeparatorChar + DateTime.Now.ToString(CoarseGrind.DateFormatString) + " " + AvailableTests.Name;
            } else
            {
                outputFolder = outputFolder + Path.DirectorySeparatorChar + AvailableTests.Name;
            }

            foreach (Type candidate in target.GetTypes())
            {
                if (candidate.IsSubclassOf(typeof(Test)))
                {
                    if (! candidate.IsAbstract)
                    {
                        if (! candidate.IsSubclassOf(typeof(ManufacturedTest)))
                        {
                            // If it's not abstract, derived from Rockabilly.CoarseGrind.Test, but not a Manufactured Test, instantiate one and add it.
                            AvailableTests.Add((Test)Activator.CreateInstance(candidate));
                        }
                    }
                }

                if (candidate.IsSubclassOf(typeof(TestFactory)))
                {
                    if (! candidate.IsAbstract)
                    {
                        // For Test Factories, instantiate it and get their list of products (Manufactureds Tests)
                        AvailableTests.AddRange(((TestFactory)Activator.CreateInstance(candidate)).Products);
                    }
                }
            }

            AvailableTests.RunTestCollection(new Strings.MatchList(), outputFolder);
        }
    }
}
