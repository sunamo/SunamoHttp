namespace SunamoHttp.Data;

public class HttpRequestData
{
    public string? Accept { get; set; } = null;

    // Assign: StreamContent, ByteArrayContent, FormUrlEncodedContent, StringContent, MultipartContent, MultipartFormDataContent
    public HttpContent? Content { get; set; } = null;

    public string? ContentType { get; set; } = null;

    public Encoding? EncodingPostData { get; set; }

    public Encoding? ForcedEncoding { get; set; } = null;

    public bool? ForceEncoding { get; set; } = false;

    public Dictionary<string, string> Headers { get; set; } = new();

    public bool? KeepAlive { get; set; } = null;

    public bool ThrowEx { get; set; } = true;

    public int TimeoutInS { get; set; } = 60;
}
