using System.Linq;

namespace AsteriskMod
{
    internal class FakeIniUtil
    {
        internal static readonly char[] ValidParameterNameChars = new char[] {
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
            'A', 'B', 'C', 'D', 'E', 'F', 'G',
            'H', 'I', 'J', 'K', 'L', 'M', 'N',
            'O', 'P', 'Q', 'R', 'S', 'T', 'U',
            'V', 'W', 'X', 'Y', 'Z',
            'a', 'b', 'c', 'd', 'e', 'f', 'g',
            'h', 'i', 'j', 'k', 'l', 'm', 'n',
            'o', 'p', 'q', 'r', 's', 't', 'u',
            'v', 'w', 'x', 'y', 'z',
            '-', '_', '$'
        };

        internal static readonly char[] ValidSectionNameChars = new char[] {
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
            'A', 'B', 'C', 'D', 'E', 'F', 'G',
            'H', 'I', 'J', 'K', 'L', 'M', 'N',
            'O', 'P', 'Q', 'R', 'S', 'T', 'U',
            'V', 'W', 'X', 'Y', 'Z',
            'a', 'b', 'c', 'd', 'e', 'f', 'g',
            'h', 'i', 'j', 'k', 'l', 'm', 'n',
            'o', 'p', 'q', 'r', 's', 't', 'u',
            'v', 'w', 'x', 'y', 'z',
            '$'
        };

        internal static bool IsValidParameterName(string parameterName)
        {
            if (parameterName.IsNullOrWhiteSpace()) return false;
            bool first = true;
            foreach (char keyChar in parameterName)
            {
                if (!ValidParameterNameChars.Contains(keyChar)) return false;
                if (first && keyChar == '-') return false;
                if (!first && keyChar == '$') return false;
                first = false;
            }
            return true;
        }

        internal static bool IsMainSection(string sectionName)
        {
            return sectionName == "$" || sectionName.ToLower() == "$main";
        }

        internal static bool IsValidSectionName(string sectionName)
        {
            if (sectionName.IsNullOrWhiteSpace()) return false;
            if (IsMainSection(sectionName)) return false;
            bool first = true;
            foreach (char keyChar in sectionName)
            {
                if (!ValidSectionNameChars.Contains(keyChar)) return false;
                if (!first && keyChar == '$') return false;
                first = false;
            }
            return true;
        }

        private static string ConvertEscapeSequence(string text)
        {
            return text.Replace("\\\\", "\0").Replace("\\n", "\n").Replace("\\r", "\r").Replace("\\t", "\t").Replace("\\\"", "\"").Replace("\\\'", "\'").Replace("\0", "\\");
        }

        internal static bool TryGetParameter(string rawParameter, out FakeIniParameter parameter)
        {
            parameter = new FakeIniParameter();
            if (rawParameter.StartsAndEndsWith("\""))
            {
                parameter = new FakeIniParameter(ConvertEscapeSequence(rawParameter.TrimOnce('\"')));
                return true;
            }
            if (!rawParameter.StartsAndEndsWith("{", "}")) return false;
            string rawArrayText = rawParameter.TrimOnce('{', '}');
            // Empty Array
            if (rawArrayText.IsNullOrWhiteSpace())
            {
                parameter = new FakeIniParameter(new string[0]);
                return true;
            }
            if (!rawArrayText.Contains(",")) return false;
            string[] rawArray = rawArrayText.Split(',');
            for (var i = 0; i < rawArray.Length; i++)
            {
                rawArray[i] = rawArray[i].Trim();
                if (!rawArray[i].StartsAndEndsWith("\"")) return false;
                rawArray[i] = ConvertEscapeSequence(rawArray[i].TrimOnce('\"'));
            }
            parameter = new FakeIniParameter(rawArray);
            return true;
        }
    }
}
