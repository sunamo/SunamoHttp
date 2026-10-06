namespace SunamoHttp;

public class HttpWebResponseHelperHttp
{
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
