namespace SunamoHttp._sunamo.SunamoExceptions;

/// <summary>
/// Exception throwing helper methods
/// </summary>
internal partial class ThrowEx
{
    /// <summary>
    /// Throws a custom exception with the specified message
    /// </summary>
    /// <param name="message">The primary message</param>
    /// <param name="isReallyThrow">Whether to really throw the exception (default true)</param>
    /// <param name="secondMessage">The secondary message (optional)</param>
    /// <returns>True if exception would be thrown, false otherwise</returns>
    internal static bool Custom(string message, bool isReallyThrow = true, string secondMessage = "")
    {
        string joined = string.Join(" ", message, secondMessage);
        string? str = Exceptions.Custom(FullNameOfExecutedCode(), joined);
        return ThrowIsNotNull(str, isReallyThrow);
    }

    /// <summary>
    /// Checks if the argument is null or empty and throws if it is
    /// </summary>
    /// <param name="argName">The argument name</param>
    /// <param name="argValue">The argument value</param>
    /// <returns>True if exception was thrown, false otherwise</returns>
    internal static bool IsNullOrEmpty(string argName, string argValue)
    {
        return ThrowIsNotNull(Exceptions.IsNullOrWhitespace(FullNameOfExecutedCode(), argName, argValue, true));
    }

    #region Other
    /// <summary>
    /// Gets the full name of executed code
    /// </summary>
    /// <returns>The full name including type and method</returns>
    internal static string FullNameOfExecutedCode()
    {
        Tuple<string, string, string> placeOfExc = Exceptions.PlaceOfException();
        string fullName = FullNameOfExecutedCode(placeOfExc.Item1, placeOfExc.Item2, true);
        return fullName;
    }

    /// <summary>
    /// Gets the full name of executed code from type and method name
    /// </summary>
    /// <param name="type">The type object</param>
    /// <param name="methodName">The method name</param>
    /// <param name="isFromThrowEx">Whether called from ThrowEx (default false)</param>
    /// <returns>The full name including type and method</returns>
    static string FullNameOfExecutedCode(object type, string methodName, bool isFromThrowEx = false)
    {
        if (methodName == null)
        {
            int depth = 2;
            if (isFromThrowEx)
            {
                depth++;
            }

            methodName = Exceptions.CallingMethod(depth);
        }
        string typeFullName;
        if (type is Type typeInstance)
        {
            typeFullName = typeInstance.FullName ?? "Type cannot be get via type is Type type2";
        }
        else if (type is MethodBase method)
        {
            typeFullName = method.ReflectedType?.FullName ?? "Type cannot be get via type is MethodBase method";
            methodName = method.Name;
        }
        else if (type is string)
        {
            typeFullName = type.ToString() ?? "Type cannot be get via type is string";
        }
        else
        {
            Type typeObj = type.GetType();
            typeFullName = typeObj.FullName ?? "Type cannot be get via type.GetType()";
        }
        return string.Concat(typeFullName, ".", methodName);
    }

    /// <summary>
    /// Throws exception if the exception string is not null
    /// </summary>
    /// <param name="exception">The exception message</param>
    /// <param name="isReallyThrow">Whether to really throw (default true)</param>
    /// <returns>True if exception was thrown or would be thrown, false otherwise</returns>
    internal static bool ThrowIsNotNull(string? exception, bool isReallyThrow = true)
    {
        if (exception != null)
        {
            Debugger.Break();
            if (isReallyThrow)
            {
                throw new Exception(exception);
            }
            return true;
        }
        return false;
    }
    #endregion
}
