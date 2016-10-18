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
using Rockabilly.Common.HtmlEffects;

namespace Rockabilly.CoarseGrind
{
	internal class CoarseGrindBanner : ConjoinedControls
	{
		internal CoarseGrindBanner(TestProgram ui) : base(TestProgram.ICON_COARSEGRINDLOGO, new CaptionedControl(new Label("Coarse Grind Test Server", 350), new Label("For performance reasons, the Coarse Grind web interface is optimized for browsers that support images defined in the HTML style section.<br>This includes Chrome, Safari, and other WebKit-based browsers. Images may not show in other browsers.", 35), CaptionedControl.Orientation.AboveCaption), Orientation.AlphaLeft)

		{
			ui.useInStyleImage(TestProgram.ICON_COARSEGRINDLOGO);
		}
	}
}
