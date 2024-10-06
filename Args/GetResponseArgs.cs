using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunamoHttp.Args;
public class GetResponseArgs
{
    public ILogger Logger { get; set; }
    public CookieContainer Cookies { get; set; }
}
