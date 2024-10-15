using Microsoft.Extensions.Logging;

namespace SunamoHttp.Args;
public class GetResponseArgs
{
    public ILogger Logger { get; set; }
    public CookieContainer Cookies { get; set; }
}
