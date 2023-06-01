using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuLanguage : MonoBehaviour
{
    private bool isSelectingLanguage = false;
    [SerializeField] GameObject opaqueBackground;
    [SerializeField] GameObject selectionWindow;
    [SerializeField] GameObject triangle;
    [SerializeField] ForumController forumController;
    [SerializeField] Sprite englishSprite;
    [SerializeField] Sprite germanSprite;
    [SerializeField] Sprite japaneseSprite;
    [SerializeField] Sprite spanishSprite;
    [SerializeField] Sprite frenchSprite;
    [SerializeField] Button currentLanguageButton;
    [SerializeField] Button button1;
    [SerializeField] Button button2;
    [SerializeField] Button button3;
    [SerializeField] Button button4;
    [SerializeField] Button button5;
    private List<Button> buttonList = new List<Button>();
    private Requester requester;

    public Requester Requester { get => requester; set => requester = value; }

    private void OnEnable()
    {
        buttonList.Add(button1);
        buttonList.Add(button2);
        buttonList.Add(button3);
        buttonList.Add(button4);
        buttonList.Add(button5);
        StartCoroutine(GetUserLanguages());
    }

    public void ToggleSelectionWindow()
    {
        isSelectingLanguage = !isSelectingLanguage;
        opaqueBackground.SetActive(isSelectingLanguage);
        selectionWindow.SetActive(isSelectingLanguage);
        triangle.SetActive(isSelectingLanguage);
    }

    public IEnumerator GetUserLanguages()
    {
        if (!requester)
        {
            Requester = GameObject.Find("App").GetComponent<Requester>();
        }

        Dictionary<string, string> header = new Dictionary<string, string>();
        header.Add("Authorization", PlayerPrefs.GetString("Authorization"));

        OperationResult<List<Language>> operation = Requester.GetOperation<List<Language>>($"http://127.0.0.1:8000/api/my-languages/", header);

        while (!operation.IsReady)
        {
            yield return null;
        }

        if (!operation.HasError)
        {
            if (string.IsNullOrEmpty(PlayerPrefs.GetString("CurrentLanguage")))
            {
                SetCurrentButton(operation.Data[0].name);
            }
            else
            {
                SetCurrentButton(PlayerPrefs.GetString("CurrentLanguage"));
            }
            for(var i = 0; i < operation.Data.Count; i++)
            {
                SetButtonFlag(buttonList[i], operation.Data[i].name);
            }
        }
    }

    public void SetCurrentButton(string language)
    {
        if (language == "english")
        {
            currentLanguageButton.GetComponent<Image>().sprite = englishSprite;
            PlayerPrefs.SetString("CurrentLanguage", "english");
        }
        else if (language == "german")
        {
            currentLanguageButton.GetComponent<Image>().sprite = germanSprite;
            PlayerPrefs.SetString("CurrentLanguage", "german");
        }
        else if (language == "japanese")
        {
            currentLanguageButton.GetComponent<Image>().sprite = japaneseSprite;
            PlayerPrefs.SetString("CurrentLanguage", "japanese");
        }
        else if (language == "spanish")
        {
            currentLanguageButton.GetComponent<Image>().sprite = spanishSprite;
            PlayerPrefs.SetString("CurrentLanguage", "spanish");
        }
        else if (language == "french")
        {
            currentLanguageButton.GetComponent<Image>().sprite = frenchSprite;
            PlayerPrefs.SetString("CurrentLanguage", "french");
        }
    }

    public void SetButtonFlag(Button button, string language)
    {
        button.gameObject.SetActive(true);
        button.onClick.RemoveAllListeners();
        if (language == "english")
        {
            button.GetComponent<Image>().sprite = englishSprite;
            button.GetComponent<Button>().onClick.AddListener(() => currentLanguageButton.GetComponentInChildren<Image>().sprite = englishSprite);
            button.GetComponent<Button>().onClick.AddListener(() => PlayerPrefs.SetString("CurrentLanguage", "english"));
        }
        else if (language == "german")
        {
            button.GetComponent<Image>().sprite = germanSprite;
            button.GetComponent<Button>().onClick.AddListener(() => currentLanguageButton.GetComponentInChildren<Image>().sprite = germanSprite);
            button.GetComponent<Button>().onClick.AddListener(() => PlayerPrefs.SetString("CurrentLanguage", "german"));
        }
        else if (language == "japanese")
        {
            button.GetComponent<Image>().sprite = japaneseSprite;
            button.GetComponent<Button>().onClick.AddListener(() => currentLanguageButton.GetComponentInChildren<Image>().sprite = japaneseSprite);
            button.GetComponent<Button>().onClick.AddListener(() => PlayerPrefs.SetString("CurrentLanguage", "japanese"));
        }
        else if (language == "spanish")
        {
            button.GetComponent<Image>().sprite = spanishSprite;
            button.GetComponent<Button>().onClick.AddListener(() => currentLanguageButton.GetComponentInChildren<Image>().sprite = spanishSprite);
            button.GetComponent<Button>().onClick.AddListener(() => PlayerPrefs.SetString("CurrentLanguage", "spanish"));
        }
        else if (language == "french")
        {
            button.GetComponent<Image>().sprite = frenchSprite;
            button.GetComponent<Button>().onClick.AddListener(() => currentLanguageButton.GetComponentInChildren<Image>().sprite = frenchSprite);
            button.GetComponent<Button>().onClick.AddListener(() => PlayerPrefs.SetString("CurrentLanguage", "french"));
        }
        button.GetComponent<Button>().onClick.AddListener(() => ToggleSelectionWindow());
    }

    public void SetForumRole(string role)
    {
        forumController.currentRole = role;
    }
}
