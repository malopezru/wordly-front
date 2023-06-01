using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForumPost : MonoBehaviour
{
    public int id { get; set; }
    public string user { get; set; }
    public List<Comment> comments { get; set; }
    public string body { get; set; }
    public string description{ get; set; }
    public string date { get; set; }
}

public class Comment : MonoBehaviour
{
    public int id { get; set; }
    public string user{ get; set; }
    public string text{ get; set; }
    public string date{ get; set; }
    public int blog{ get; set; }

}