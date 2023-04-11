using System;
using System.Collections.Generic;
using UnityEngine;

public class User : MonoBehaviour
{
    public int id {  get; set; }
    public string username { get; set; }
    public string email { get; set; }
    public string name { get; set; }
    public string role { get; set; }
    public string date_of_birth { get; set; }
    public List<Languages> languages { get; set; }
}

public class PaymentMethods
{
    public string cardNumber;
    public string expirationDate;
    public string cvv;
}

public class Languages 
{
    public string language;
    public string level;
}

[Serializable]
public class Error
{
    public string message;
}