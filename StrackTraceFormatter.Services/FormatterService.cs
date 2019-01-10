using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace StrackTraceFormatter.Services
{
    public class FormatterService
    {
        private readonly Regex _strackTraceRegex = new Regex(@"(\s*Stack Trace:)");
        private readonly Regex _beiRegex = new Regex(@"(\s{2,}bei )");

        public string RemoveSuperflousNewLines(string stackTrace)
        {
            var lines = stackTrace.Split(new[] {Environment.NewLine}, StringSplitOptions.None);
            StringBuilder result = new StringBuilder();
            foreach (var line in lines)
            {
                if (_strackTraceRegex.IsMatch(line))
                {
                    result.AppendLine();
                    result.Append(line);
                    continue;
                }


                if (_beiRegex.IsMatch(line))
                    result.AppendLine();

                result.Append(line);
            }

            return result.ToString();
        }

        public string AddNewLines(string input)
        {
            var working = _strackTraceRegex.Replace(input, Environment.NewLine + "$1");
            working = _beiRegex.Replace(working, Environment.NewLine + "$1");
            return working;
        }

        public string Format(string input)
        {
            input = DecodeHtml(input);
            if (input.Split(new[] {Environment.NewLine}, StringSplitOptions.None).Take(2).Count() > 1)
            {
                return RemoveSuperflousNewLines(input);
            }

            return AddNewLines(input);
        }

        public string DecodeHtml(string input)
        {
            return HttpUtility.HtmlDecode(input);
        }
    }
}
