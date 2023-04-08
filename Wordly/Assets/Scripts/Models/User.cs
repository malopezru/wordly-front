using System;
using UnityEngine;

public class User : MonoBehaviour
{
    public int id {  get; set; }
    public string name { get; set; }
    public string email { get; set; }
    public string gender { get; set; }
    public string status { get; set; }
}

[Serializable]
public class Error
{
    public string message;
}