using System;
using System.Collections.Generic;
using UnityEngine;

public class User : MonoBehaviour
{
    public string username { get; set; }
    public string email { get; set; }
    public string name { get; set; }
    public string last_name { get; set; }
    public string gender { get; set; }
    public string birthday { get; set; }
    public string user_type { get; set; }
    public int payment { get; set; }
    public string expire_date { get; set; }
    public int ccv { get; set; }
    public Language language { get; set; }
    public string level { get; set; }
}

public class Language
{
    public int id;
    public string name;
    public string level;
    public int user;
}

[Serializable]
public class Error
{
    public string message;
}