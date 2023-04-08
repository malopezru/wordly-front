using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Networking;

public class Requester : MonoBehaviour
{
    public Dictionary<string, string> headers;

    public OperationResult<T> GetOperation<T>(string URL, Dictionary<string, string> headers = null) where T : class, new()
    {
        var operation = new OperationResult<T>();
        StartCoroutine(Get(URL, operation, headers));
        return operation;
    }

    string ErrorHandle(UnityWebRequest request)
    {
        try
        {
            string error = request.downloadHandler.text;

            if (error != null)
            {
                return JsonConvert.DeserializeObject<Error>(error).message;
            }

            switch (request.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    return "Error: " + request.error;
                case UnityWebRequest.Result.ProtocolError:
                    return "HTTP Error: " + request.error;
            }
            return "Unknown error";
        }
        catch (Exception e)
        {
            return e.Message;
        }
    }

    //----------------------  GET RESQUEST ----------------------

    IEnumerator Get<T>(string URL, OperationResult<T> operation, Dictionary<string, string> newHeaders) where T : class, new()
    {
        UnityWebRequest request = UnityWebRequest.Get(URL);

        if (newHeaders != null)
        {
            foreach (KeyValuePair<string, string> header in newHeaders)
            {
                request.SetRequestHeader(header.Key, header.Value);
            }
        }
        else
        {
            SetHeadersToRequest(request, newHeaders);
        }

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            operation.ErrorMessage = request.error;
            operation.ErrorMessage = ErrorHandle(request);
        }
        else
        {
            try
            {
                operation.ResolveData(request.downloadHandler.text);
            }

            catch (Exception e)
            {
                operation.Data = null;
                operation.ErrorMessage = e.Message;
                operation.IsReady = true;
                Debug.LogError(e);
                throw new Exception(e.Message);
            }
        }
        operation.IsReady = true;
    }

    void SetHeadersToRequest(UnityWebRequest request, Dictionary<string, string> newHeaders)
    {
        if (newHeaders != null)
        {
            AddHeaders(newHeaders);
        }
        foreach (KeyValuePair<string, string> header in headers)
        {
            request.SetRequestHeader(header.Key, header.Value);
        }
    }

    void AddHeaders(Dictionary<string, string> newHeaders)
    {
        foreach (KeyValuePair<string, string> header in newHeaders)
        {
            if (!headers.ContainsKey(header.Key))
            {
                headers.Add(header.Key, header.Value);
            }
        }
    }
}
