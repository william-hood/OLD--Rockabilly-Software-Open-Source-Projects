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

namespace MemoirV2
{
    public static class Constants
    {
        // Some of these might belong in Coarse Grind
        public const string EMOJI_SETUP = "🛠";
        public const string EMOJI_CLEANUP = "🧹";
        public const string EMOJI_PASSING_TEST = "👌";
        public const string EMOJI_SUBJECTIVE_TEST = "🤔";
        public const string EMOJI_INCONCLUSIVE_TEST = "🛑";
        public const string EMOJI_FAILING_TEST = "❌";
        public const string EMOJI_DEBUG = "👾";
        public const string EMOJI_ERROR = "😱";
        public const string EMOJI_MEMOIR = "📝";
        public const string EMOJI_TEXT_MEMOIR_CONCLUDE = "⤴️";
        public const string EMOJI_TEXT_BLANK_LINE = "";
        public const string EMOJI_OBJECT = "🔲";
        public const string EMOJI_CAUSED_BY = "↘️";

        public const string ALREADY_CONCLUDED_MESSAGE = "This memoir is already concluded.";

        public const int MAX_OBJECT_FIELDS_TO_DISPLAY = 10;

        public const string MEMOIR_LOG_STYLING = @"
    <style>
        html {
            font-family: sans-serif
        }

        [class*='lvl-'] {
            display: none;
        }

        input:checked~[class*='lvl-'] {
            display: block;
        }

        .gone {
            display: none;
        }

        .memoir {
            font-family: sans-serif;
            border-radius: 1em;
            border: 0.2em solid black;
            display: inline-block;
            background-image: linear-gradient(to bottom right, white, WhiteSmoke);
        }

        .failing_test_result {
            background-image: linear-gradient(to bottom right, MistyRose, salmon);
        }

        .inconclusive_test_result {
            background-image: linear-gradient(to bottom right, LemonChiffon, Moccasin);
        }

        .passing_test_result {
            background-image: linear-gradient(to bottom right, honeydew, palegreen);
        }

        .implied_good {
            background-image: linear-gradient(to bottom right, mintcream, honeydew);
        }

        .implied_caution {
            background-image: linear-gradient(to bottom right, LemonChiffon, oldlace);
        }

        .implied_bad {
            background-image: linear-gradient(to bottom right, Seashell, LavenderBlush);
        }

        .neutral {
            background-image: linear-gradient(to bottom right, WhiteSmoke, Gainsboro);
        }

        .old_parchment {
            background-image: radial-gradient(LightGoldenrodYellow, Cornsilk, Wheat);
        }

        .plate {
            background-image: radial-gradient(Gainsboro, LightSlateGray);
        }

        .exception {
            background-image: linear-gradient(to bottom right, yellow, salmon);
        }

        th,
        td {
            padding: 0.1em 0em;
            vertical-align: text-top;
        }

        h1 {
            font-size: 5em;
            margin: 0em
        }

        h2 {
            font-size: 2em;
            margin: 0.2em
        }

        hr {
            border: none;
            height: 0.3em;
            background-color: black;
        }

        .highlighted {
            background-image: linear-gradient(to bottom right, yellow, gold);
        }

        .outlined {
            border-radius: 0.5em;
            border: 0.05em solid black;
        }

        .object {
            border-radius: 1em;
            border: 0.5em solid black;
            padding: 0.1em 0.2em;
        }

        table.gridlines,
        table.gridlines th,
        table.gridlines td {
            padding: 0.1em 0.2em;
            border-collapse: collapse;
            border: 0.02em solid black;
        }
    </style>
";
    }
}
