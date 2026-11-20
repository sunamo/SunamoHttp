namespace SunamoHttp;

// Can be only in shared because is not available in standard
/// <summary>
/// Represents the http response helper type.
/// </summary>
public class HttpResponseHelper
{
    /// <summary>
    /// Some error.
    /// </summary>
    /// <param name="response">The response.</param>
    /// <returns>The some error result.</returns>
    public static bool SomeError(HttpResponseMessage? response)
    {
        if (response == null)
        {
            return true;
        }

        switch ((HttpStatusCode)response.StatusCode)
        {
            case HttpStatusCode.OK:
                return false;
        }
        return true;
    }

    /// <summary>
    /// Some error.
    /// </summary>
    /// <param name="response">The response.</param>
    /// <returns>The some error result.</returns>
    public static bool SomeError(HttpWebResponse? response)
    {
        if (response == null)
        {
            return true;
        }

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
    public static bool IsNotFound(HttpWebResponse? response)
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
