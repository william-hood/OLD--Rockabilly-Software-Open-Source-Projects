using System;
using System.Text;

namespace MemoirV2
{
    public static class LogExceptionExtension
    {
        public static string LogException(this Memoir memoir, Exception target)
        {
            DateTime timeStamp = DateTime.Now;
            StringBuilder result = new StringBuilder("<div class=\"object exception\">\r\n");
            string name = target.GetType().ToString();
            string stackTrace = "(no stacktrace)";
            string plainTextStackTrace = stackTrace;

            if (target.StackTrace != null)
            {
                plainTextStackTrace = target.StackTrace.Trim().Replace("at ", "\r\nat ").Replace(" in ", "\r\nin ").Trim();
                stackTrace = target.StackTrace.Trim().Replace("at ", "<br><br>at ").Replace(" in ", "<br>in ").Trim().Substring(8);
            }

            if (memoir != null)
            {
                memoir.echoPlainText(String.Format("{0}\r\n\r\n{1}\r\n{2}",
                    name,
                    target.Message,
                    plainTextStackTrace), timeStamp, Constants.EMOJI_ERROR);
            }

            result.Append(String.Format("<label for=\"{0}\">\r\n<input id=\"{0}\" class=\"gone\" type=\"checkbox\">\r\n<h2>{1}</h2>\r\n{2}<div class=\"{3}\">\r\n<br><small><i>\r\n{4}\r\n</i></small>\r\n",
                Guid.NewGuid().ToString(),
                name,
                target.Message,
                Memoir.getEncapsulationTag(),
                stackTrace));

            if (target.InnerException != null)
            {
                result.Append(String.Format("<br>\r\n<table><tr><td>&nbsp;</td><td>{0}</td><td>&nbsp;</td><td>{1}</td></tr></table>",
                    Constants.EMOJI_CAUSED_BY,
                    LogException(null, target.InnerException)));
            }

            result.Append("</div>\r\n</label></div>");


            if (memoir != null)
            {
                memoir.writeToHTML(result.ToString(), timeStamp, Constants.EMOJI_ERROR);
            }

            return result.ToString();
        }
    }
}
