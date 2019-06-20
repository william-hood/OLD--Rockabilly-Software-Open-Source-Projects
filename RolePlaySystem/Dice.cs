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
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RolePlaySystem
{
    public class Dice
    {
        private static Random RANDOM = new Random();
        public int Faces = default(int);

        public Dice(int faces)
        {
            Faces = faces;
        }

        public int Roll
        {
            get
            {
                return RANDOM.Next(Faces) + 1;
            }
        }

        public override string ToString()
        {
            return Identify();
        }

        public string Identify()
        {
            return "d" + Faces;
        }

            public static List<Dice> Group(int count, int faces)
        {
            List<Dice> result = new List<Dice>();
            for (int index = 0; index < count; index++)
            {
                result.Add(new Dice(faces));
            }

            return result;
        }
    }

    public static class DiceExtensions
    {
        public static int Roll(this List<Dice> theseDice)
        {
            int result = 0;
            foreach (Dice thisDice in theseDice)
            {
                result += thisDice.Roll;
            }

            return result;
        }

        public static string Identify(this List<Dice> theseDice)
        {
            Dictionary<int, int> diceCounts = new Dictionary<int, int>();

            foreach (Dice thisDice in theseDice)
            {
                if (diceCounts.ContainsKey(thisDice.Faces))
                {
                    diceCounts[thisDice.Faces] += 1;
                }
                else
                {
                    diceCounts[thisDice.Faces] = 1;
                }
            }

            var allKeys = diceCounts.Keys.ToArray();
            Array.Sort(allKeys);

            StringBuilder result = new StringBuilder();

            for (int index = 0; index < allKeys.Length; index++)
            {
                if (result.Length > 0)
                {
                    result.Append(" + ");
                }

                result.Append(diceCounts[allKeys[index]]);
                result.Append('d');
                result.Append(allKeys[index]);
            }

            return result.ToString();
        }
    }
}