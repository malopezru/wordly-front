using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForumController : MonoBehaviour
{
    [SerializeField] GameObject studentHomeView;
    [SerializeField] GameObject tutorHomeView;
    [SerializeField] ForumPostPrefab forumPost;
    [SerializeField] Transform content;

    public string currentRole;

    public void GoHomePage()
    {
        if (currentRole == "Estudiante" && !string.IsNullOrEmpty(currentRole))
        {
            this.gameObject.SetActive(false);
            studentHomeView.SetActive(true);
        }
        else if (currentRole == "Tutor" && !string.IsNullOrEmpty(currentRole))
        {
            this.gameObject.SetActive(false);
            tutorHomeView.SetActive(true);
        }
    }
}
