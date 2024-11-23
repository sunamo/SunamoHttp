namespace SunamoHttp._sunamo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal static class StringExtensions
{
public static string FromSpace160To32(this string input)
{
return Regex.Replace(input, @"\p{Z}", " ");
}
}