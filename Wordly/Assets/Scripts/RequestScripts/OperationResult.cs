using Newtonsoft.Json;
using UnityEngine;

public class OperationResult
{
    public bool IsReady;
    public bool HasError { get { return !string.IsNullOrEmpty(ErrorMessage); } }
    public string ErrorMessage;
    public string JsonSource;

    public virtual void ResolveData(string data)
    {
        JsonSource = data;
    }
}

public class OperationResult<T> : OperationResult where T : class, new()
{
    public T Data;
    public override void ResolveData(string data)
    {
        base.ResolveData(data);
        var settings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            MissingMemberHandling = MissingMemberHandling.Ignore
        };
        Data = JsonConvert.DeserializeObject<T>(data, settings);
    }
}