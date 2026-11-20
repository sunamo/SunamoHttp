namespace SunamoHttp;

/// <summary>
/// Represents the http web response helper http type.
/// </summary>
public class HttpWebResponseHelperHttp
{
    /// <summary>
    /// Some error.
    /// </summary>
    /// <param name="response">The response.</param>
    /// <returns>The some error result.</returns>
    public static bool SomeError(HttpWebResponse response)
    {
        if (response == null)
        {
            return true;
        }

        // 400 errors for artists and other which doesn't exist
        // 429 Too many errors (mainly for rate limiting)
        switch (response.StatusCode)
        {
            case HttpStatusCode.OK:
                return false;
        }
        return true;
    }

    /// <summary>
    /// Is not found.
    /// </summary>
    /// <param name="response">The response.</param>
    /// <returns>The is not found result.</returns>
    public static bool IsNotFound(HttpWebResponse response)
    {
        if (response == null)
        {
            return true;
        }

        switch (response.StatusCode)
        {
            case HttpStatusCode.NotFound:
                return true;
        }
        return false;
    }
}
