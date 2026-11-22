namespace SunamoHttp;

/// <summary>
/// Synchronous members that need no logger and always send a User-Agent header.
/// </summary>
partial class HttpRequestHelper
{
    private static readonly HttpClient userAgentClient = new HttpClient();

    /// <summary>
    /// Gets or sets the User-Agent header sent by the members without a logger parameter; some servers return an empty response without it.
    /// </summary>
    public static string DefaultUserAgent { get; set; } = "Mozilla/5.0 (Windows NT 10.0; Win64; x64)";

    /// <summary>
    /// Sends the request with the default User-Agent and returns the response, synchronously and without a logger.
    /// </summary>
    /// <param name="address">The address.</param>
    /// <param name="method">The HTTP method.</param>
    /// <returns>The response; the caller disposes it.</returns>
    private static HttpResponseMessage SendWithUserAgent(string address, HttpMethod method)
    {
        if (address == null) throw new ArgumentNullException(nameof(address));
        if (method == null) throw new ArgumentNullException(nameof(method));
        using var request = new HttpRequestMessage(method, address);
        if (!string.IsNullOrEmpty(DefaultUserAgent))
        {
            request.Headers.TryAddWithoutValidation(HttpKnownHeaderNames.UserAgent, DefaultUserAgent);
        }

#if NET5_0_OR_GREATER
        return userAgentClient.Send(request);
#else
        return userAgentClient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
#endif
    }

    /// <summary>
    /// Gets the response body as text, synchronously, without a logger and with the <see cref="DefaultUserAgent"/> header; the status code is not checked and network errors are thrown.
    /// </summary>
    /// <param name="address">The address.</param>
    /// <param name="method">The HTTP method.</param>
    /// <returns>The response text, decoded by the charset of the response (UTF-8 when it has none).</returns>
    public static string GetResponseText(string address, HttpMethod method)
    {
        using var response = SendWithUserAgent(address, method);
        var bytes = response.Content.ReadAsByteArrayAsync().ConfigureAwait(false).GetAwaiter().GetResult();
        using var reader = new StreamReader(new MemoryStream(bytes), GetResponseEncoding(response), true);
        return reader.ReadToEnd();
    }

    /// <summary>
    /// Gets the response body as bytes, synchronously, without a logger and with the <see cref="DefaultUserAgent"/> header; an unsuccessful status code is thrown as HttpRequestException.
    /// </summary>
    /// <param name="address">The address.</param>
    /// <param name="method">The HTTP method.</param>
    /// <returns>The response bytes.</returns>
    public static byte[] GetResponseBytes(string address, HttpMethod method)
    {
        using var response = SendWithUserAgent(address, method);
        response.EnsureSuccessStatusCode();
        return response.Content.ReadAsByteArrayAsync().ConfigureAwait(false).GetAwaiter().GetResult();
    }

    /// <summary>
    /// Downloads the address to folder\fileNameWithoutExtension + extension, synchronously, without a logger and with the <see cref="DefaultUserAgent"/> header; the folder is created and an existing file is overwritten.
    /// </summary>
    /// <param name="uri">The address of the file.</param>
    /// <param name="folder">The target folder; created when it does not exist.</param>
    /// <param name="fileNameWithoutExtension">The target file name without the extension.</param>
    /// <param name="extension">The extension with the leading dot.</param>
    /// <param name="dontHaveAllowedExtension">Optional filter; when it returns true for the extension, nothing is downloaded.</param>
    /// <returns>True when the file was written, false when the extension was refused by the filter.</returns>
    public static bool DownloadFile(string uri, string folder, string fileNameWithoutExtension, string extension, Func<string, bool>? dontHaveAllowedExtension = null)
    {
        if (folder == null) throw new ArgumentNullException(nameof(folder));
        if (fileNameWithoutExtension == null) throw new ArgumentNullException(nameof(fileNameWithoutExtension));
        if (dontHaveAllowedExtension != null && dontHaveAllowedExtension(extension))
        {
            return false;
        }

        var bytes = GetResponseBytes(uri, HttpMethod.Get);
        Directory.CreateDirectory(folder);
        File.WriteAllBytes(Path.Combine(folder, fileNameWithoutExtension + extension), bytes);
        return true;
    }

    /// <summary>
    /// Returns the encoding named by the charset of the response; UTF-8 when the charset is missing or unknown.
    /// </summary>
    /// <param name="response">The response.</param>
    /// <returns>The encoding used to decode the body.</returns>
    private static Encoding GetResponseEncoding(HttpResponseMessage response)
    {
        var charset = response.Content.Headers.ContentType?.CharSet?.Trim('"');
        if (string.IsNullOrEmpty(charset))
        {
            return Encoding.UTF8;
        }

        try
        {
            return Encoding.GetEncoding(charset);
        }
        catch (ArgumentException)
        {
            // An unknown charset name must not lose the response; UTF-8 is the web default.
            return Encoding.UTF8;
        }
    }
}
