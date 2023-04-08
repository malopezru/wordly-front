using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MessagePopUp : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI popUpMessage;
    [SerializeField] Color errorColor;
    [SerializeField] Color successColor;
    [SerializeField] Animator animator;

    public void SetPopUpMessage(string message, bool isError)
    {
        this.gameObject.SetActive(true);
        popUpMessage.text = message;
        if (isError)
        {
            this.GetComponent<Image>().color = errorColor;
        }
        else
        {
            this.GetComponent<Image>().color = successColor;
        }
    }

    public void ClosePopUp()
    {
        this.GetComponent<RectTransform>().anchorMin = new Vector2((float)0.35, (float)1.1);
        this.GetComponent<RectTransform>().anchorMax = new Vector2((float)0.65, (float)1.18);
        this.gameObject.SetActive(false);
    }
}
