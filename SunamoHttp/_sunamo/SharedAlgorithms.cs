// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy

namespace SunamoHttp._sunamo;

internal class SharedAlgorithms
{
    internal static int lastError = -1;


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
                var message = ex.Message;
                if (message.StartsWith("The remote server returned an error: "))
                {
                    var parameter = SHSplit.Split(
                        SHReplace.ReplaceOnce(message, "The remote server returned an error: ", string.Empty),
                        " ");
                    var text = parameter[0].TrimEnd(')').TrimStart('(');
                    lastError = int.Parse(text);
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