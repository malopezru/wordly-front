using System;
using System.Collections.Generic;
using UnityEngine;

public class User : MonoBehaviour
{
    public string email { get; set; }
    public bool is_staff { get; set; }
    public bool is_active { get; set; }
    public string date_joined { get; set; }
    public string username { get; set; }
    public string name { get; set; }
    public string last_name { get; set; }
    public string gender { get; set; }
    public string user_type { get; set; }
    public string birthday { get; set; }
    public string language { get; set; }
    public string level { get; set; }
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