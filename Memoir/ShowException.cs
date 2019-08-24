using System;
using System.Text;

namespace Rockabilly.MemoirV2
{
    public partial class Memoir
    {
        const string DEFAULT_STACKTRACE = "no stacktrace";
        public string ShowException(Exception target)
        {
            DateTime timeStamp = DateTime.Now;
            StringBuilder result = new StringBuilder("<div class=\"object exception\">\r\n");
            string name = target.GetType().ToString();
            string stackTrace = DEFAULT_STACKTRACE;
            string plainTextStackTrace = stackTrace;

            if (target.StackTrace != null)
            {
                plainTextStackTrace = target.StackTrace.Trim().Replace("at ", "\r\nat ").Replace(" in ", "\r\nin ").Trim();
                stackTrace = target.StackTrace.Trim().Replace("at ", "<br><br>at ").Replace(" in ", "<br>in ").Trim().Substring(8);
            }

            EchoPlainText(String.Format("{0}\r\n\r\n{1}\r\n{2}",
                    name,
                    target.Message,
                    plainTextStackTrace), timeStamp, Constants.EMOJI_ERROR);

            StringBuilder indicator = new StringBuilder();
            if ((stackTrace != DEFAULT_STACKTRACE) || (target.InnerException != null))
            {
                indicator.Append("show ");
                if (stackTrace != DEFAULT_STACKTRACE)
                {
                    indicator.Append("stacktrace");
                }

                if (target.InnerException != null)
                {
                    if (indicator.Length > 5)
                    {
                        indicator.Append(" & ");
                    }

                    indicator.Append("cause");
                }

                result.Append(String.Format("<label for=\"{0}\">\r\n<h2>{1}</h2>\r\n{2}<br><br><input id=\"{0}\" type=\"checkbox\"><small><i>({3})</i></small>\r\n<div class=\"{4}\">\r\n<br><small><i>\r\n{5}\r\n</i></small>\r\n",
                    Guid.NewGuid().ToString(),
                    name,
                    target.Message,
                    indicator,
                    Memoir.getEncapsulationTag(),
                    stackTrace));

                if (target.InnerException != null)
                {
                    result.Append(String.Format("<br>\r\n<table><tr><td>&nbsp;</td><td>Caused by {0}</td><td>&nbsp;</td><td>{1}</td></tr></table>",
                        Constants.EMOJI_CAUSED_BY,
                        ShowException(target.InnerException)));
                }

                result.Append("</div>\r\n</label>");
            } else
            {
                result.Append(String.Format("<h2>{0}</h2>\r\n{1}\r\n<br><br><small><i>({2})</i></small>", name, target.Message, DEFAULT_STACKTRACE));
            }


            result.Append("</div>");

            WriteToHTML(result.ToString(), timeStamp, Constants.EMOJI_ERROR);

            return result.ToString();
        }
    }
}
