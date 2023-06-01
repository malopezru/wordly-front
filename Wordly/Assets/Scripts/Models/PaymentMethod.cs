using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaymentMethod : MonoBehaviour
{
    public int id { get; set; }
    public string number { get; set; }
    public string expire_date { get; set; }
    public int ccv { get; set; }
    public int user { get; set; }
}