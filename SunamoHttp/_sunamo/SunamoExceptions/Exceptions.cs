namespace SunamoHttp._sunamo.SunamoExceptions;

internal sealed partial class Exceptions
{
    #region Other
    internal static string CheckBefore(string before)
    {
        return string.IsNullOrWhiteSpace(before) ? string.Empty : before + ": ";
    }

    internal static string TextOfExceptions(Exception ex, bool alsoInner = true)
    {
        if (ex == null) return string.Empty;
        StringBuilder stringBuilder = new();
        stringBuilder.Append("Exception:");
        stringBuilder.AppendLine(ex.Message);
        if (alsoInner)
            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
                stringBuilder.AppendLine(ex.Message);
            }
        var result = stringBuilder.ToString();
        return result;
    }

    internal static Tuple<string, string, string> PlaceOfException(bool isFillAlsoFirstTwo = true)
    {
        StackTrace stackTrace = new();
        var value = stackTrace.ToString();
        var lines = value.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
        lines.RemoveAt(0);
        var currentIndex = 0;
        string type = string.Empty;
        string methodName = string.Empty;
        for (; currentIndex < lines.Count; currentIndex++)
        {
            var item = lines[currentIndex];
            if (isFillAlsoFirstTwo)
                if (!item.StartsWith("   at ThrowEx"))
                {
                    TypeAndMethodName(item, out type, out methodName);
                    isFillAlsoFirstTwo = false;
                }
            if (item.StartsWith("at System."))
            {
                lines.Add(string.Empty);
                lines.Add(string.Empty);
                break;
            }
        }
        return new Tuple<string, string, string>(type, methodName, string.Join(Environment.NewLine, lines));
    }

    internal static void TypeAndMethodName(string stackTraceLine, out string type, out string methodName)
    {
        var trimmedLine = stackTraceLine.Split(new[] { "at " }, StringSplitOptions.None)[1].Trim();
        var fullMethodPath = trimmedLine.Split(new[] { "(" }, StringSplitOptions.None)[0];
        var pathParts = fullMethodPath.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        methodName = pathParts[^1];
        pathParts.RemoveAt(pathParts.Count - 1);
        type = string.Join(".", pathParts);
    }

    internal static string CallingMethod(int frameIndex = 1)
    {
        StackTrace stackTrace = new();
        var methodBase = stackTrace.GetFrame(frameIndex)?.GetMethod();
        if (methodBase == null)
        {
            return "Method name cannot be get";
        }
        var methodName = methodBase.Name;
        return methodName;
    }
    #endregion

    #region IsNullOrWhitespace
    internal static string? IsNullOrWhitespace(string before, string argName, string argValue, bool isNotAllowOnlyWhitespace)
    {
        string addParams;
        if (argValue == null)
        {
            addParams = AddParams();
            return CheckBefore(before) + argName + " is null" + addParams;
        }
        if (argValue == string.Empty)
        {
            addParams = AddParams();
            return CheckBefore(before) + argName + " is empty (without trim)" + addParams;
        }
        if (isNotAllowOnlyWhitespace && argValue.Trim() == string.Empty)
        {
            addParams = AddParams();
            return CheckBefore(before) + argName + " is empty (with trim)" + addParams;
        }
        return null;
    }

    internal readonly static StringBuilder AdditionalInfoInnerStringBuilder = new();

    internal readonly static StringBuilder AdditionalInfoStringBuilder = new();

    internal static string AddParams()
    {
        AdditionalInfoStringBuilder.Insert(0, Environment.NewLine);
        AdditionalInfoStringBuilder.Insert(0, "Outer:");
        AdditionalInfoStringBuilder.Insert(0, Environment.NewLine);
        AdditionalInfoInnerStringBuilder.Insert(0, Environment.NewLine);
        AdditionalInfoInnerStringBuilder.Insert(0, "Inner:");
        AdditionalInfoInnerStringBuilder.Insert(0, Environment.NewLine);
        var addParams = AdditionalInfoStringBuilder.ToString();
        var addParamsInner = AdditionalInfoInnerStringBuilder.ToString();
        return addParams + addParamsInner;
    }
    #endregion

    #region OnlyReturnString
    internal static string? Custom(string before, string message)
    {
        return CheckBefore(before) + message;
    }
    #endregion
}
