using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;

public class Requester : MonoBehaviour
{
    public Dictionary<string, string> headers = new Dictionary<string, string>();

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
                return error;
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

    //----------------------  POST RESQUEST ----------------------

    public OperationResult<T> PostOperation<T>(string URL, Dictionary<string, string> dataPost, Dictionary<string, string> headers = null) where T : class, new()
    {
        var operation = new OperationResult<T>();
        StartCoroutine(Post(URL, dataPost, operation, headers));
        return operation;
    }

    IEnumerator Post<T>(string URL, Dictionary<string, string> dataPost, OperationResult<T> operation, Dictionary<string, string> newHeaders) where T : class, new()
    {
        WWWForm form = new WWWForm();

        foreach (KeyValuePair<string, string> key in dataPost)
        {
            form.AddField(key.Key, key.Value);
        }
        form.headers["Content-Type"] = "application/json";
        UnityWebRequest request = UnityWebRequest.Post(URL, form);
        if (newHeaders != null)
        {
            SetHeadersToRequest(request, newHeaders);
        }
        yield return request.SendWebRequest();
        ProcessRequestResult(request, operation);
    }

    //----------------------  PUT RESQUEST ----------------------
    
    public OperationResult<T> PutOperation<T>(string URL, Dictionary<string, string> dataPost, Dictionary<string,string> headers) where T : class, new()
    {
        var operation = new OperationResult<T>();
        StartCoroutine(PutRequest(URL, dataPost, operation, headers));
        return operation;
    }
    private IEnumerator PutRequest<T>(string url, Dictionary<string, string> dataPost, OperationResult<T> operation, Dictionary<string, string> headers) where T : class, new()
    {
        Debug.Log("putRequest data " + dataPost);
        var binFormatter = new BinaryFormatter();
        var mStream = new MemoryStream();
        binFormatter.Serialize(mStream, dataPost);
        byte[] bytes = mStream.ToArray();

        UnityWebRequest www = UnityWebRequest.Put(url, bytes);

        foreach (var item in headers)
        {
            www.SetRequestHeader(item.Key,item.Value);
        }

        www.SetRequestHeader("Content-Type", "application/json");

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            operation.ErrorMessage = www.error;
            Debug.LogError(url);
            Debug.LogError(www.error);
        }
        else
        {
            operation.ResolveData(www.downloadHandler.text);
        }
        operation.IsReady = true;
        www.Dispose();
        www = null;
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

    void ResolveOperation(string data, OperationResult operation)
    {
        try
        {
            operation.ResolveData(data);
        }
        catch (Exception error)
        {
            operation.ErrorMessage = error.Message;
            Debug.LogError($"Error: Resolving an HTTP Response {operation.ErrorMessage}");
        }
    }

    void ProcessRequestResult(UnityWebRequest request, OperationResult operation)
    {
        if (request.result == UnityWebRequest.Result.Success)
        {
            ResolveOperation(request.downloadHandler.text, operation);
        }
        else
        {
            operation.ErrorMessage = ErrorHandle(request);
            Debug.LogError(operation.ErrorMessage);
        }
        operation.IsReady = true;
    }
}
