using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ForumPostController : MonoBehaviour
{
    [SerializeField] ForumPostPrefab forumPost;
    [SerializeField] ReplyController replyPrefab;
    [SerializeField] Transform postContent;
    private Requester requester;

    public Requester Requester { get => requester; set => requester = value; }


    void OnEnable()
    {
        if (!Requester)
        {
            Requester = GameObject.Find("App").GetComponent<Requester>();
        }

        for (var i = postContent.childCount - 1; i >= 0; i--)
        {
            Destroy(postContent.GetChild(i).gameObject);
        }
    }

    public void SetPostView(ForumPost post)
    {
        ForumPostPrefab currentPost = Instantiate(forumPost, postContent);
        currentPost.posterName.text = post.user;
        currentPost.postTitle.text = post.body;
        currentPost.postMessage.text = post.description;
        currentPost.postDate.text = post.date.Substring(0, 10);
        currentPost.GetComponent<Button>().interactable = false;

        ReplyController reply = Instantiate(replyPrefab, postContent);

        foreach (Comment comment in post.comments)
        {
            ForumPostPrefab currentComment = Instantiate(forumPost, postContent);
            currentComment.posterName.text = comment.user;
            currentComment.postTitle.gameObject.SetActive(false);
            currentComment.postMessage.text = comment.text;
            currentComment.postDate.text = comment.date.Substring(0, 10);
            currentComment.transform.GetChild(6).gameObject.SetActive(false);
            currentComment.postMessage.GetComponent<RectTransform>().anchorMax = new Vector2(0.9702725f, 0.6481178f);
        }

        reply.transform.GetChild(2).GetComponent<Button>().onClick.RemoveAllListeners();
        reply.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() => SendReply(reply, post.id));
    }

    public void SendReply(ReplyController reply, int postId)
    {
        string replyText = reply.GetInputValue();
        StartCoroutine(PostReply(replyText, postId));
    }

    public IEnumerator PostReply(string replyText, int postId)
    {

        Dictionary<string, string> header = new Dictionary<string, string>();
        Dictionary<string, string> body = new Dictionary<string, string>();
        header.Add("Authorization", PlayerPrefs.GetString("Authorization"));
        body.Add("text", replyText);

        OperationResult<Comment> operation = Requester.PostOperation<Comment>($"http://localhost:8000/api/forum/comment/{postId}/", body, header);

        while (!operation.IsReady)
        {
            yield return null;
        }

        if (!operation.HasError)
        {
            ForumPostPrefab currentComment = Instantiate(forumPost, postContent);
            currentComment.posterName.text = operation.Data.user;
            currentComment.postTitle.gameObject.SetActive(false);
            currentComment.postMessage.text = operation.Data.text;
            currentComment.postDate.text = operation.Data.date.Substring(0, 10);
            currentComment.transform.GetChild(6).gameObject.SetActive(false);
            currentComment.postMessage.GetComponent<RectTransform>().anchorMax = new Vector2(0.9702725f, 0.6481178f);
        }
        else
        {
            Debug.Log(operation.ErrorMessage);
        }
    }
}
