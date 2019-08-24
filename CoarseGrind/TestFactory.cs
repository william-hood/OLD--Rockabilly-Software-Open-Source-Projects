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
// OTHER DEALINGS IN THE SOFTWARE.
using System;
using System.Diagnostics;
using System.Text;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Rockabilly.MemoirV2;

namespace Rockabilly.CoarseGrind
{
    // A manufactured test has to be produced by a test factory in order to be included in the test run.
    public abstract class ManufacturedTest : Test
    {
        // There's no difference. Deriving from this allows the test runner to know not to try and instantiate one.
        protected ManufacturedTest(string Name, string DetailedDescription = null, string ID = null, params string[] Categories) : base(Name, DetailedDescription, ID, Categories)
        {
        }
    }

    public abstract class TestFactory
    {
        public abstract List<ManufacturedTest> Products { get; }
    }
}
