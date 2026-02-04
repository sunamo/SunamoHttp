// variables names: ok
namespace SunamoHttp.Args;

/// <summary>
/// Arguments for HTTP response retrieval
/// </summary>
public class GetResponseArgs
{
    /// <summary>
    /// Gets or sets the cookie container for the request
    /// </summary>
    public CookieContainer? Cookies { get; set; }
}