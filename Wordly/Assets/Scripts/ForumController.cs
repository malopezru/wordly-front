using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ForumController : MonoBehaviour
{
    [SerializeField] GameObject studentHomeView;
    [SerializeField] GameObject tutorHomeView;
    [SerializeField] ForumPostPrefab forumPost;
    [SerializeField] Transform content;
    [SerializeField] ForumPostController forumPostController;
    private Requester requester;

    public Requester Requester { get => requester; set => requester = value; }

    public string currentRole;

    void OnEnable()
    {
        if (!Requester)
        {
            Requester = GameObject.Find("App").GetComponent<Requester>();
        }

        for (var i = content.childCount - 1; i >= 0; i--)
        {
            Destroy(content.GetChild(i).gameObject);
        }

        StartCoroutine(GetAllPosts());
    }

    public IEnumerator GetAllPosts()
    {
        Dictionary<string, string> header = new Dictionary<string, string>();
        header.Add("Authorization", PlayerPrefs.GetString("Authorization"));

        OperationResult<List<ForumPost>> operation = Requester.GetOperation<List<ForumPost>>($"http://localhost:8000/api/forum/get", header);

        while (!operation.IsReady)
        {
            yield return null;
        }

        if (!operation.HasError)
        {
            foreach (ForumPost post in operation.Data)
            {
                ForumPostPrefab currentPost = Instantiate(forumPost, content);
                currentPost.posterName.text = post.user;
                currentPost.postTitle.text = post.body;
                currentPost.postMessage.text = post.description;
                currentPost.postDate.text = post.date.Substring(0, 10);
                currentPost.GetComponent<Button>().onClick.RemoveAllListeners();
                currentPost.GetComponent<Button>().onClick.AddListener(() => ShowPostDetails(post.id));
            }
        }
        else
        {
            Debug.Log(operation.ErrorMessage);
        }
    }

    public void ShowPostDetails(int postId)
    {
        StartCoroutine(GetPostDetails(postId));
    }

    public IEnumerator GetPostDetails(int postId)
    {
        Dictionary<string, string> header = new Dictionary<string, string>();
        header.Add("Authorization", PlayerPrefs.GetString("Authorization"));

        OperationResult<ForumPost> operation = Requester.GetOperation<ForumPost>($"http://localhost:8000/api/forum/get/{postId}", header);

        while (!operation.IsReady)
        {
            yield return null;
        }

        if (!operation.HasError)
        {
            this.gameObject.SetActive(false);
            forumPostController.gameObject.SetActive(true);
            forumPostController.SetPostView(operation.Data);
        }
        else
        {
            Debug.Log(operation.ErrorMessage);
        }

    }

    public void GoHomePage()
    {
        if (currentRole == "Estudiante" && !string.IsNullOrEmpty(currentRole))
        {
            this.gameObject.SetActive(false);
            forumPostController.gameObject.SetActive(false);
            studentHomeView.SetActive(true);
        }
        else if (currentRole == "Tutor" && !string.IsNullOrEmpty(currentRole))
        {
            this.gameObject.SetActive(false);
            forumPostController.gameObject.SetActive(false);
            tutorHomeView.SetActive(true);
        }
    }
}
