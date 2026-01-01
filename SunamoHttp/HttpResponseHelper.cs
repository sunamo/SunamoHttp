namespace SunamoHttp;

/// <summary>
/// Helper class for HttpResponse operations
/// Can be only in shared because is not available in standard
/// </summary>
public class HttpResponseHelper
{
    /// <summary>
    /// Checks if the response contains any error
    /// </summary>
    /// <param name="response">The HTTP response message to check</param>
    /// <returns>True if there is an error (status code is not OK), false otherwise</returns>
    public static bool SomeError(HttpResponseMessage response)
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
    /// Checks if the response contains any error
    /// </summary>
    /// <param name="response">The HTTP web response to check</param>
    /// <returns>True if there is an error (status code is not OK), false otherwise</returns>
    public static bool SomeError(HttpWebResponse response)
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
    /// Checks if the response indicates a not found status
    /// </summary>
    /// <param name="response">The HTTP web response to check</param>
    /// <returns>True if the resource was not found, false otherwise</returns>
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