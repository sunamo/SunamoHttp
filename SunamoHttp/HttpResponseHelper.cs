namespace SunamoHttp;

// Can be only in shared because is not available in standard
public class HttpResponseHelper
{
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
