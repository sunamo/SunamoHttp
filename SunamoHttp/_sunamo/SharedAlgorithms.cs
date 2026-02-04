namespace SunamoHttp._sunamo;

/// <summary>
/// Shared algorithm methods for HTTP operations
/// </summary>
internal class SharedAlgorithms
{
    /// <summary>
    /// Gets or sets the last HTTP error code encountered
    /// </summary>
    internal static int LastError { get; set; } = -1;

    /// <summary>
    /// Repeats an async operation multiple times with timeout between attempts
    /// </summary>
    /// <typeparam name="Out">The output type</typeparam>
    /// <param name="times">Number of times to retry</param>
    /// <param name="timeoutMs">Timeout in milliseconds between attempts</param>
    /// <param name="action">The async action to execute</param>
    /// <returns>The result of the operation, or default value if all attempts failed</returns>
    internal static async Task<Out?> RepeatAfterTimeXTimesAsync<Out>(int times, int timeoutMs, Func<Task<Out>> action)
    {
        LastError = -1;
        Out? result = default;
        var ok = false;
        for (var i = 0; i < times; i++)
        {
            try
            {
                result = await action();
                ok = true;
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                if (message.StartsWith("The remote server returned an error: "))
                {
                    var errorParts = SHSplit.Split(
                        SHReplace.ReplaceOnce(message, "The remote server returned an error: ", string.Empty),
                        " ");
                    var errorCode = errorParts[0].TrimEnd(')').TrimStart('(');
                    LastError = int.Parse(errorCode);
                }

                if (LastError == 404) return result;
                // The remote server returned an error: (404) Not Found.
                Thread.Sleep(timeoutMs);
            }

            if (ok) break;
        }

        return result;
    }
}