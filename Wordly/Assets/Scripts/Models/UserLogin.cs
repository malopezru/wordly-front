using System;
using UnityEngine;

public class UserLogin : MonoBehaviour
{
    public Token token { get; set; }
    public string msg { get; set; }
}

public class Token
{
    public string refresh;
    public string access;
}