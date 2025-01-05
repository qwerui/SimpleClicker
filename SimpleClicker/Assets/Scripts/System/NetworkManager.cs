using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;

public struct ErrorResult
{
    public int code;
    public string message;
}

public class NetworkManager : Singleton<NetworkManager>
{
    [HideInInspector]
    public string accessToken;
    [HideInInspector]
    public string refreshToken;

    public IEnumerator Get(string uri, Action<string> callback, Dictionary<string, string> requestHeaders = null, Dictionary<string, string> responseHeaders = null, Action<ErrorResult> catchCallback = null)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(uri))
        {
            if (requestHeaders != null)
            {
                foreach (var header in requestHeaders)
                {
                    request.SetRequestHeader(header.Key, header.Value);
                }
            }

            yield return request.SendWebRequest();

            if (responseHeaders != null)
            {
                responseHeaders = request.GetResponseHeaders();
            }

            if (request.result != UnityWebRequest.Result.Success)
            {
                catchCallback?.Invoke(ParseError(request.error));
            }
            else
            {
                using (var handler = request.downloadHandler)
                {
                    callback?.Invoke(handler.text);
                }
            }
        }
    }

    public IEnumerator Post(string uri, string form, Dictionary<string, string> requestHeaders = null, Dictionary<string, string> responseHeaders = null, Action<string> callback = null, Action<ErrorResult> catchCallback = null)
    {
        using (UnityWebRequest request = UnityWebRequest.PostWwwForm(uri, form))
        {
            if (requestHeaders != null)
            {
                foreach (var header in requestHeaders)
                {
                    request.SetRequestHeader(header.Key, header.Value);
                }
            }

            yield return request.SendWebRequest();

            if (responseHeaders != null)
            {
                responseHeaders = request.GetResponseHeaders();
            }

            if (request.result != UnityWebRequest.Result.Success)
            {
                catchCallback?.Invoke(ParseError(request.error));
            }
            else
            {
                using (var handler = request.downloadHandler)
                {
                    callback?.Invoke(handler.text);
                }
            }
        }
    }

    public IEnumerator Post(string uri, WWWForm form, Dictionary<string, string> requestHeaders = null, Dictionary<string, string> responseHeaders = null, Action<string> callback = null, Action<ErrorResult> catchCallback = null)
    {
        using (UnityWebRequest request = UnityWebRequest.Post(uri, form))
        {
            if (requestHeaders != null)
            {
                foreach (var header in requestHeaders)
                {
                    request.SetRequestHeader(header.Key, header.Value);
                }
            }

            yield return request.SendWebRequest();

            if (responseHeaders != null)
            {
                responseHeaders = request.GetResponseHeaders();
            }

            if (request.result != UnityWebRequest.Result.Success)
            {
                catchCallback?.Invoke(ParseError(request.error));
            }
            else
            {
                using (var handler = request.downloadHandler)
                {
                    callback?.Invoke(handler.text);
                }
            }
        }
    }

    public IEnumerator Put(string uri, string form, Dictionary<string, string> requestHeaders = null, Dictionary<string, string> responseHeaders = null, Action<string> callback = null, Action<ErrorResult> catchCallback = null)
    {
        using (UnityWebRequest request = UnityWebRequest.Put(uri, form))
        {
            if (requestHeaders != null)
            {
                foreach (var header in requestHeaders)
                {
                    request.SetRequestHeader(header.Key, header.Value);
                }
            }

            yield return request.SendWebRequest();

            if (responseHeaders != null)
            {
                responseHeaders = request.GetResponseHeaders();
            }

            if (request.result != UnityWebRequest.Result.Success)
            {
                catchCallback?.Invoke(ParseError(request.error));
            }
            else
            {
                using (var handler = request.downloadHandler)
                {
                    callback?.Invoke(handler.text);
                }
            }
        }
    }

    public IEnumerator Delete(string uri, Dictionary<string, string> requestHeaders = null, Dictionary<string, string> responseHeaders = null, Action<string> callback = null, Action<ErrorResult> catchCallback = null)
    {
        using (UnityWebRequest request = UnityWebRequest.Delete(uri))
        {
            if (requestHeaders != null)
            {
                foreach (var header in requestHeaders)
                {
                    request.SetRequestHeader(header.Key, header.Value);
                }
            }

            yield return request.SendWebRequest();

            if (responseHeaders != null)
            {
                responseHeaders = request.GetResponseHeaders();
            }

            if (request.result != UnityWebRequest.Result.Success)
            {
                catchCallback?.Invoke(ParseError(request.error));
            }
            else
            {
                using (var handler = request.downloadHandler)
                {
                    callback?.Invoke(handler.text);
                }
            }
        }
    }

    private ErrorResult ParseError(string error)
    {
        string[] split = error.Split(' ');
        return new ErrorResult
        {
            code = int.Parse(split[1]),
            message = error
        };

    }
}
