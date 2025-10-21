// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
﻿namespace SunamoHttp.Data;

public class HttpRequestData
{
    public string accept = null;


    public HttpContent content = null;
    public string contentType = null;
    public Encoding encodingPostData;


    public Encoding forcedEncoding = null;


    public bool? forceEndoding = false;
    public Dictionary<string, string> headers = new();

    public bool? keepAlive = null;
    public bool throwEx = true;
    public int timeoutInS = 60;
}

//namespace SunamoHttp.Code;
///// Here it cant be, is already in SunamoHttp.standard and even if I not directly reference SunamoHttp.standard, VS see it
//public class HttpRequestDataHttp
//{
//    public Dictionary<string, string> headers = new Dictionary<string, string>();
//    public string contentType = null;
//    public string accept = null;
//    public Encoding encodingPostData;
//    //public int? timeout = null; // Není v třídě HttpKnownHeaderNames
//    public bool? keepAlive = null;
//    /// <summary>
//    /// Assign: StreamContent,ByteArrayContent,FormUrlEncodedContent,StringContent,MultipartContent,MultipartFormDataContent
//    /// </summary>
//    public HttpContent content = null;
//}