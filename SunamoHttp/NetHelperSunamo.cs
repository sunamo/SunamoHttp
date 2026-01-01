namespace SunamoHttp;

/// <summary>
/// Network helper utilities for .NET applications
/// </summary>
public class NetHelperSunamo
{
    /// <summary>
    /// WARNING: Disables SSL certificate validation - DO NOT USE IN PRODUCTION!
    /// Disabling certificate validation can expose you to a man-in-the-middle attack
    /// which may allow your encrypted message to be read by an attacker
    /// https://stackoverflow.com/a/14907718/740639
    /// </summary>
    public static void NEVER_EAT_POISON_Disable_CertificateValidation()
    {
        ServicePointManager.ServerCertificateValidationCallback =
            delegate (
                object sender,
                X509Certificate certificate,
                X509Chain chain,
                SslPolicyErrors sslPolicyErrors
            )
            {
                return true;
            };
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
    }
}