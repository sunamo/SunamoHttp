namespace SunamoHttp._sunamo.SunamoExceptions;

/// <summary>
/// Exception helper methods
/// © www.sunamo.cz. All Rights Reserved.
/// </summary>
internal sealed partial class Exceptions
{
    #region Other
    /// <summary>
    /// Checks the before parameter and formats it
    /// </summary>
    /// <param name="before">The before text</param>
    /// <returns>The formatted before text with colon, or empty string</returns>
    internal static string CheckBefore(string before)
    {
        return string.IsNullOrWhiteSpace(before) ? string.Empty : before + ": ";
    }

    /// <summary>
    /// Gets the text of all exception messages
    /// </summary>
    /// <param name="ex">The exception</param>
    /// <param name="alsoInner">Whether to include inner exceptions</param>
    /// <returns>The formatted exception text</returns>
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

    /// <summary>
    /// Gets the place of exception from stack trace
    /// </summary>
    /// <param name="isFillAlsoFirstTwo">Whether to fill also first two items</param>
    /// <returns>Tuple of type, method name, and stack trace</returns>
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

    /// <summary>
    /// Extracts type and method name from stack trace line
    /// </summary>
    /// <param name="stackTraceLine">The stack trace line</param>
    /// <param name="type">Output parameter for the type name</param>
    /// <param name="methodName">Output parameter for the method name</param>
    internal static void TypeAndMethodName(string stackTraceLine, out string type, out string methodName)
    {
        var trimmedLine = stackTraceLine.Split("at ")[1].Trim();
        var fullMethodPath = trimmedLine.Split("(")[0];
        var pathParts = fullMethodPath.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        methodName = pathParts[^1];
        pathParts.RemoveAt(pathParts.Count - 1);
        type = string.Join(".", pathParts);
    }

    /// <summary>
    /// Gets the calling method name
    /// </summary>
    /// <param name="frameIndex">The frame index in stack trace (default 1)</param>
    /// <returns>The method name, or error message if cannot be retrieved</returns>
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
    /// <summary>
    /// Checks if the argument is null or whitespace
    /// </summary>
    /// <param name="before">The before text</param>
    /// <param name="argName">The argument name</param>
    /// <param name="argValue">The argument value</param>
    /// <param name="isNotAllowOnlyWhitespace">Whether to not allow only whitespace</param>
    /// <returns>The error message, or null if valid</returns>
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

    /// <summary>
    /// Gets or sets the inner additional info string builder
    /// </summary>
    internal readonly static StringBuilder AdditionalInfoInnerStringBuilder = new();

    /// <summary>
    /// Gets or sets the additional info string builder
    /// </summary>
    internal readonly static StringBuilder AdditionalInfoStringBuilder = new();

    /// <summary>
    /// Adds additional parameters to error message
    /// </summary>
    /// <returns>The formatted additional parameters</returns>
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
    /// <summary>
    /// Creates a custom exception message
    /// </summary>
    /// <param name="before">The before text</param>
    /// <param name="message">The message</param>
    /// <returns>The formatted exception message</returns>
    internal static string? Custom(string before, string message)
    {
        return CheckBefore(before) + message;
    }
    #endregion
}