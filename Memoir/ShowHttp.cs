using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace Rockabilly.MemoirV2
{
    public partial class Memoir
    {

        public string ShowHttpRequest(HttpRequestMessage request)
        {
            DateTime timeStamp = DateTime.Now;
            StringBuilder result = new StringBuilder("<div class=\"outgoing implied_caution\">\r\n");

            result.Append(String.Format("<center><h2>{0} {1}</h2>", request.Method, request.RequestUri.AbsolutePath));
            result.Append(String.Format("<small><b><i>{0}</i></b></small>", request.RequestUri.Host));

            // Hide the Full URL
            result.Append(String.Format("<br><br><label for=\"{0}\">\r\n<input id=\"{0}\" type=\"checkbox\"><small><i>(show complete URL)</i></small>\r\n<div class=\"{1}\">\r\n",
                Guid.NewGuid().ToString(),
                Memoir.getEncapsulationTag()));

            result.Append(String.Format("<br>\r\n{0}\r\n", request.RequestUri.AbsoluteUri.Replace("&", "&amp;")));

            result.Append("</div>\r\n</label>");

            if (request.RequestUri.Query == null)
            {
                result.Append("<br><br><small><i>(no query)</i></small>");
            } else
            {
                string[] queries = request.RequestUri.Query.Split('&');
                result.Append("<br><br><b>Queries</b><br><table class=\"gridlines\">\r\n");

                foreach (string query in queries)
                {
                    string[] part = query.Split('=');
                    result.Append("<tr><td>");
                    result.Append(part[0].Replace("?", ""));
                    result.Append("</td><td>");
                    if (part.Length > 1)
                    {
                        // When possible, attempt JSON pretty-print here.
                        result.Append(attemptBase64Decode(part[1]));
                    } else
                    {
                        result.Append("(unset)");
                    }
                    result.Append("</td></tr>");
                }
                result.Append("\r\n</table><br>");
            }

            // Headers
            if (request.Headers.Count() > 0)
            {
                result.Append("<br><b>Headers</b><br><table class=\"gridlines\">\r\n");
                foreach (KeyValuePair<string, IEnumerable<string>> thisHeader in request.Headers)
                {
                    result.Append("<tr><td>");
                    result.Append(thisHeader.Key);
                    result.Append("</td><td>");

                    string[] allValues = thisHeader.Value.ToArray<string>();
                    if (allValues.Length == 0)
                    {
                        result.Append("<small><i>(empty)</i></small>");
                    }
                    else if (allValues.Length == 1)
                    {
                        // When possible, attempt JSON pretty-print here.
                        result.Append(attemptBase64Decode(allValues[0]));
                    }
                    else
                    {
                        result.Append("<center><table class=\"gridlines\">\r\n");
                        foreach (string thisValue in allValues)
                        {
                            result.Append("<tr><td>");
                            // When possible, attempt JSON pretty-print here.
                            result.Append(attemptBase64Decode(thisValue));
                            result.Append("</td></tr>");
                        }
                    }

                    result.Append("</td></tr>");
                }
                result.Append("\r\n</table><br>");
            }
            else
            {
                result.Append("<br><br><small><i>(no headers)</i></small>\r\n");
            }

            // Body
            if (request.Content == null)
            {
                result.Append("<br><br><small><i>(no payload)</i></small></center>");
            }
            else
            {
                result.Append("<br><b>Payload</b><br></center>\r\n");
                result.Append("<pre>\r\n");
                // When possible, attempt JSON pretty-print here.
                result.Append(request.Content.ToString());
                result.Append("\r\n</pre>\r\n");
            }

            result.Append("</div>");

            WriteToHTML(result.ToString(), timeStamp, Constants.EMOJI_OUTGOING);
            EchoPlainText(String.Format("{0} {1}", request.Method, request.RequestUri.AbsoluteUri), timeStamp, Constants.EMOJI_OUTGOING);

            return result.ToString();
        }

        public string ShowHttpResponse(HttpResponseMessage response)
        {
            DateTime timeStamp = DateTime.Now;
            string style = "implied_bad";
            if (((int)response.StatusCode).ToString()[0] == '2') style = "implied_good";
            StringBuilder result = new StringBuilder("<div class=\"incoming ");
            result.Append(style);
            result.Append("\">\r\n");

            // Status code & description
            result.Append(String.Format("<center><h2>{0} {1}</h2>", (int)response.StatusCode, response.ReasonPhrase));

            // Headers
            if (response.Headers.Count() > 0)
            {
                result.Append("<b>Headers</b><br><table class=\"gridlines\">\r\n");
                foreach (KeyValuePair<string, IEnumerable<string>> thisHeader in response.Headers)
                {
                    result.Append("<tr><td>");
                    result.Append(thisHeader.Key);
                    result.Append("</td><td>");

                    string[] allValues = thisHeader.Value.ToArray<string>();
                    if (allValues.Length == 0)
                    {
                        result.Append("<small><i>(empty)</i></small>");
                    }
                    else if (allValues.Length == 1)
                    {
                        result.Append(attemptBase64Decode(allValues[0]));
                    }
                    else
                    {
                        result.Append("<center><table class=\"gridlines\">\r\n");
                        foreach (string thisValue in allValues)
                        {
                            result.Append("<tr><td>");
                            result.Append(attemptBase64Decode(thisValue));
                            result.Append("</td></tr>");
                        }
                    }

                    result.Append("</td></tr>");
                }
                result.Append("\r\n</table><br>");
            } else
            {
                result.Append("<br><small><i>(no headers)</i></small>\r\n");
            }

            // Body
            if (response.Content == null)
            {
                result.Append("<br><small><i>(no payload)</i></small></center>");
            } else
            {
                result.Append("<b>Payload</b><br></center>\r\n");
                result.Append("<pre>\r\n");
                // When possible, attempt JSON pretty-print here.
                result.Append(response.Content.ReadAsStringAsync().Result);
                result.Append("\r\n</pre>\r\n");
            }

            result.Append("</div>");


            WriteToHTML(result.ToString(), timeStamp, Constants.EMOJI_INCOMING);
            EchoPlainText(String.Format("{0} {1}", (int)response.StatusCode, response.ReasonPhrase), timeStamp, Constants.EMOJI_INCOMING);

            return result.ToString();
        }

        public HttpResponseMessage ShowTransaction(HttpClient httpClient, HttpRequestMessage request)
        {
            return httpClient.ShowTransaction(this, request);
        }
    }

    public static class ShowTransactionExtension
    {

        public static HttpResponseMessage ShowTransaction(this HttpClient httpClient, Memoir memoir, HttpRequestMessage request)
        {
            HttpResponseMessage result = null;
            try
            {
                memoir.ShowHttpRequest(request);

                try
                {
                    Task<HttpResponseMessage> task = httpClient.SendAsync(request);
                    result = task.Result;
                    memoir.ShowHttpResponse(result);
                }
                catch (AggregateException thisAggregate)
                {
                    thisAggregate.Flatten().Handle((thisException) =>
                    {
                        if (!(thisException is AggregateException))
                        {
                            throw thisException;
                        }

                        return false;
                    });
                }

            }
            catch (Exception loggedException)
            {
                memoir.ShowException(loggedException);
                throw loggedException;
            }

            return result;
        }
    }
}
