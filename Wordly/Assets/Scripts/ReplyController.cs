using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ReplyController : MonoBehaviour
{
    [SerializeField] TMP_InputField replyInput;
    [SerializeField] Button sendReplyButton;

    public TMP_InputField ReplyInput { get => replyInput; set => replyInput = value; }

    public string GetInputValue()
    {
        return this.ReplyInput.text;
    }
}
