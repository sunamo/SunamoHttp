namespace SunamoHttp._sunamo;

internal class SharedAlgorithms
{
    internal static int lastError = -1;

    internal static Out RepeatAfterTimeXTimes<Out>(int times, int timeoutMs, Func<Out> a)
    {
        lastError = -1;
        var result = default(Out);
        var ok = false;
        for (var i = 0; i < times; i++)
        {
            try
            {
                result = a();
                ok = true;
            }
            catch (Exception ex)
            {
                var m = ex.Message;
                if (m.StartsWith("The remote server returned an error: "))
                {
                    var p = SHSplit.SplitMore(
                        SHReplace.ReplaceOnce(m, "The remote server returned an error: ", string.Empty),
                        "");
                    var s = p[0].TrimEnd(')').TrimStart('(');
                    lastError = int.Parse(s);
                }

                if (lastError == 404) return result;
                //The remote server returned an error: (404) Not Found.
                Thread.Sleep(timeoutMs);
            }

            if (ok) break;
        }

        return result;
    }

    internal static async Task<Out> RepeatAfterTimeXTimesAsync<Out>(int times, int timeoutMs, Func<Task<Out>> a)
    {
        lastError = -1;
        var result = default(Out);
        var ok = false;
        for (var i = 0; i < times; i++)
        {
            try
            {
                result = await a();
                ok = true;
            }
            catch (Exception ex)
            {
                var m = ex.Message;
                if (m.StartsWith("The remote server returned an error: "))
                {
                    var p = SHSplit.SplitMore(
                        SHReplace.ReplaceOnce(m, "The remote server returned an error: ", string.Empty),
                        "");
                    var s = p[0].TrimEnd(')').TrimStart('(');
                    lastError = int.Parse(s);
                }

                if (lastError == 404) return result;
                //The remote server returned an error: (404) Not Found.
                Thread.Sleep(timeoutMs);
            }

            if (ok) break;
        }

        return result;
    }
}