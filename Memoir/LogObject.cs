using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MemoirV2
{
    public static class LogObjectExtension
    {
        private static bool shouldRecurse(object candidate)
        {
            if (candidate == null) return false;

            Type candidateType = candidate.GetType();
            if (candidateType == typeof(int)) { return false; }
            if (candidateType == typeof(bool)) { return false; }
            if (candidateType == typeof(char)) { return false; }
            if (candidateType == typeof(string)) { return false; }
            if (candidateType == typeof(String)) { return false; }
            if (candidateType == typeof(uint)) { return false; }
            if (candidateType == typeof(float)) { return false; }
            if (candidateType == typeof(Decimal)) { return false; }
            if (candidateType == typeof(decimal)) { return false; }
            if (candidateType == typeof(long)) { return false; }
            if (candidateType == typeof(Int16)) { return false; }
            if (candidateType == typeof(Int32)) { return false; }
            if (candidateType == typeof(Int64)) { return false; }
            if (candidateType == typeof(UInt16)) { return false; }
            if (candidateType == typeof(UInt32)) { return false; }
            if (candidateType == typeof(UInt64)) { return false; }
            if (candidateType == typeof(ulong)) { return false; }
            if (candidateType == typeof(double)) { return false; }
            if (candidateType == typeof(Double)) { return false; }
            if (candidateType == typeof(short)) { return false; }
            if (candidateType == typeof(ushort)) { return false; }
            if (candidateType == typeof(byte)) { return false; }
            if (candidateType == typeof(sbyte)) { return false; }

            return true;
        }

        public static string LogObject(this Memoir memoir, object target, string nameof_target = "")
        {
            DateTime timeStamp = DateTime.Now;
            StringBuilder result = new StringBuilder("<div class=\"object neutral\">\r\n");
            Type thisType = target.GetType();
            string title = String.Format("{0} {1}", thisType.Name, nameof_target).Trim();

            if (memoir != null)
            {
                memoir.echoPlainText(title, timeStamp, Constants.EMOJI_OBJECT);
            }
            result.Append(String.Format("<label for=\"{0}\">\r\n<input id=\"{0}\" class=\"gone\" type=\"checkbox\">\r\n<center><h2>{1}</h2></center>\r\n<div class=\"{2}\">\r\n",
                Guid.NewGuid().ToString(),
                title,
                Memoir.getEncapsulationTag()));

            IEnumerable<FieldInfo> theseFields = thisType.GetRuntimeFields();
            int fieldCount = theseFields.Count();

            StringBuilder content = new StringBuilder();

            content.Append("<center><table class=\"gridlines\">\r\n");
            foreach (FieldInfo thisField in theseFields)
            {
                string fieldName = thisField.Name;
                object fieldValue = thisField.GetValue(target);

                content.Append("<tr><td>");
                content.Append(thisField.FieldType.Name);
                content.Append("</td><td>");
                content.Append(fieldName);
                content.Append("</td><td>");
                if (fieldValue == null)
                {
                    fieldValue = "(null)";
                }

                if (shouldRecurse(fieldValue))
                {
                    content.Append(LogObject(null, fieldValue, fieldName));
                } else
                {
                    content.Append(fieldValue);
                }
                content.Append("</td></tr>\r\n");
            }
            content.Append("\r\n</table></center><br></div>");



            if (fieldCount > Constants.MAX_OBJECT_FIELDS_TO_DISPLAY)
            {
                result.Append(String.Format("<label for=\"{0}\">\r\n<input id=\"{0}\" type=\"checkbox\">\r\n(reveal {1} fields)\r\n<div class=\"{2}\">\r\n",
                    Guid.NewGuid().ToString(),
                    fieldCount,
                    Memoir.getEncapsulationTag()));
                result.Append(content.ToString());
                result.Append("</div></label>");
            }
            else
            {
                result.Append(content.ToString());
            }

            result.Append("\r\n</label></div>");
            if (memoir != null)
            {
                memoir.writeToHTML(result.ToString(), timeStamp, Constants.EMOJI_OBJECT);
            }

            return result.ToString();
        }
    }
}
