namespace SunamoHttp._sunamo;

internal class SharedAlgorithms
{
    internal static int LastError { get; set; } = -1;

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
