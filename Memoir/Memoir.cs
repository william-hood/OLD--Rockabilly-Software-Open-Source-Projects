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
using System.IO;
using System.Text;

namespace MemoirV2
{
    // Basic log segment will be called a memoir.
    // An encapsulation might hold a stack trace, HTTP message, another memoir, anything better depicted in HTML than plain text.
    public class Memoir
    {
        private TextWriter htmlTextWriter = null;
        private TextWriter plainTextWriter = null;
        private StringBuilder content = new StringBuilder("<table>\r\n");
        private bool isConcluded = false;
        private string titleName = default(string);
        
        public Memoir(string title, TextWriter forPlainText = null, TextWriter forHTML = null, Func<string, string> Header = null)
        {
            titleName = title;
            htmlTextWriter = forHTML;
            plainTextWriter = forPlainText;

            DateTime timeStamp = DateTime.Now;

            if (plainTextWriter != null)
            {
                EchoPlainText("");
                EchoPlainText(titleName, timeStamp, Constants.EMOJI_MEMOIR);
            }

            if (htmlTextWriter != null)
            {
                if (Header == null)
                {
                    Header = defaultHeader;
                }

                // This should be the start of a newly opened file
                htmlTextWriter.Write(String.Format("<html>\r\n<meta charset=\"UTF-8\">\r\n<head>\r\n<title>{0}</title>\r\n", title));
                htmlTextWriter.Write(Constants.MEMOIR_LOG_STYLING);
                htmlTextWriter.Write("</head>\r\n<body>\r\n");
                htmlTextWriter.Write(Header(title));
            }
        }

        internal static string getEncapsulationTag()
        {
            return String.Format("lvl-{0}", Guid.NewGuid().ToString());
        }

        private string defaultHeader(string title)
        {
            return String.Format("<h1>{0}</h1>\r\n<hr>\r\n<small><i>Powered by the Memoir Logging System...</i></small>\r\n\r\n", title);
        }

        public string Conclude()
        {
            if (! isConcluded)
            {
                EchoPlainText("", emoji: Constants.EMOJI_TEXT_MEMOIR_CONCLUDE);
                EchoPlainText("");

                isConcluded = true;

                content.Append("\r\n</table>");

                if (htmlTextWriter != null)
                {
                    htmlTextWriter.Write(content.ToString());
                    htmlTextWriter.Write("\r\n</body>\r\n</html>");
                    htmlTextWriter.Flush();
                }
            }

            return content.ToString();
        }

        public void EchoPlainText(string message, DateTime timeStamp = default(DateTime), string emoji = default(string))
        {
            if (plainTextWriter == null)
            {
                // Silently decline
                return;
            }

            if (isConcluded)
            {
                throw new Exception(Constants.ALREADY_CONCLUDED_MESSAGE);
            }

            // Deliberate. Emoji will be clobbered to default(string) at many levels above otherwise.
            if (emoji == default(string))
            {
                emoji = Constants.EMOJI_TEXT_BLANK_LINE;
            }

            string dateTime = "                        ";
            if (timeStamp != default(DateTime))
            {
                dateTime = timeStamp.ToString("yyyy-MM-dd HH:mm:ss.ffff");
            }

            plainTextWriter.WriteLine(string.Format("{0} {1}\t{2} ", dateTime, emoji, message));
        }

        public void WriteToHTML(string message, DateTime timeStamp = default(DateTime), string emoji = default(string))
        {
            if (isConcluded)
            {
                throw new Exception(Constants.ALREADY_CONCLUDED_MESSAGE);
            }

            string date = "&nbsp;";
            string time = "&nbsp;";
            if (timeStamp != default(DateTime))
            {
                date = timeStamp.ToString("yyyy-MM-dd");
                time = timeStamp.ToString("HH:mm:ss.ffff");
            }

            content.Append(String.Format("<tr><td>{0}</td><td>&nbsp;</td><td>{1}</td><td>&nbsp;</td><td><h2>{2}</h2></td><td>{3}</td></tr>\r\n", date, time, emoji, message));
        }

        private string highlight(string message, string style = "highlighted")
        {
            return String.Format("<p class=\"{0} outlined\">&nbsp;{1}&nbsp;</p>", style, message);
        }

        public void LogInfo(string message, string emoji = default(string))
        {
            DateTime timeStamp = DateTime.Now;
            WriteToHTML(message, timeStamp, emoji);
            EchoPlainText(message, timeStamp, emoji);
        }

        public void LogDebug(string message)
        {
            DateTime timeStamp = DateTime.Now;
            WriteToHTML(highlight(message), timeStamp, Constants.EMOJI_DEBUG);
            EchoPlainText(message, timeStamp, Constants.EMOJI_DEBUG);
        }

        public void LogError(string message)
        {
            DateTime timeStamp = DateTime.Now;
            WriteToHTML(highlight(message, "exception"), timeStamp, Constants.EMOJI_ERROR);
            EchoPlainText(message, timeStamp, Constants.EMOJI_ERROR);
        }

        public void SkipLine()
        {
            WriteToHTML("");
            EchoPlainText("");
        }

        private string wrapAsSublog(string memoirTitle, string memoirContent, string style = "neutral")
        {
            return String.Format("\r\n\r\n<div class=\"memoir {0}\">\r\n<label for=\"{1}\">\r\n<input id=\"{1}\" class=\"gone\" type=\"checkbox\">\r\n<h2>{2}</h2>\r\n<div class=\"{3}\">\r\n{4}\r\n</div></label>",
                style,
                Guid.NewGuid().ToString(),
                memoirTitle,
                getEncapsulationTag(),
                memoirContent);
        }

        // The Memoir submitted will be concluded.
        public void LogMemoir(Memoir subordinate, string emoji = Constants.EMOJI_MEMOIR, string style = "neutral")
        {
            DateTime timeStamp = DateTime.Now;
            string subordinateContent = subordinate.Conclude();
            WriteToHTML(wrapAsSublog(subordinate.titleName, subordinateContent, style), timeStamp, emoji);
            // This does not echo to plain text as it is assumed the subordinate memoir already did that.
        }
    }
}
