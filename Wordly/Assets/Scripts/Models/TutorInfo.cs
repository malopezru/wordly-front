using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorInfo : MonoBehaviour
{
    public string name { get; set; }
    public string last_name { get; set; }
    public string description { get; set; }
    public List<string> days { get; set; }
    public string cost { get; set; }
    public int rating { get; set; }
}
