using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CreatePostController : MonoBehaviour
{
    [SerializeField] TMP_InputField postTitleInput;
    [SerializeField] TMP_InputField postMessageInput;
    [SerializeField] ForumController forumController;
    Requester requester;

    public Requester Requester { get => requester; set => requester = value; }
    public TMP_InputField PostTitleInput { get => postTitleInput; set => postTitleInput = value; }
    public TMP_InputField PostMessageInput { get => postMessageInput; set => postMessageInput = value; }

    private void OnEnable()
    {
        if (!Requester)
        {
            Requester = GameObject.Find("App").GetComponent<Requester>();
        }
    }

    public void SavePost()
    {
        StartCoroutine(PostInForum());
    }

    public IEnumerator PostInForum()
    {

        string postTitle = this.PostTitleInput.text;
        string postMessage = this.PostMessageInput.text;

        Dictionary<string, string> header = new Dictionary<string, string>();
        Dictionary<string, string> body = new Dictionary<string, string>();
        header.Add("Authorization", PlayerPrefs.GetString("Authorization"));
        body.Add("body", postTitle);
        body.Add("description", postMessage);

        OperationResult<ClassSession> operation = Requester.PostOperation<ClassSession>($"http://127.0.0.1:8000/api/forum/post/", body, header);

        while (!operation.IsReady)
        {
            yield return null;
        }

        if (!operation.HasError)
        {
            this.PostTitleInput.text = "";
            this.PostMessageInput.text = "";
            this.gameObject.SetActive(false);
            forumController.gameObject.SetActive(true);
        }
        else
        {
            Debug.Log(operation.ErrorMessage);
        }
    }
}
