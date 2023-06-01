using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorInfoController : MonoBehaviour
{
    public TextMeshProUGUI name;
    public TextMeshProUGUI tutorDescription;
    public GameObject selectedButtonBackground;
    public BookClassController bookClassController;
    public MainMenuLanguage mainMenuController;
    public Button currentLanguageButton;
    Requester requester;

    private string currentTutorCost;
    public Requester Requester { get => requester; set => requester = value; }

    public void SetTutorInfo(TutorInfo tutor)
    {
        if (!Requester)
        {
            Requester = GameObject.Find("App").GetComponent<Requester>();
        }
        mainMenuController.SetButtonFlag(currentLanguageButton, PlayerPrefs.GetString("CurrentLanguage"));
        StartCoroutine(GetSpecificTutor(tutor));
        currentTutorCost = tutor.cost;
    }

    public IEnumerator GetSpecificTutor(TutorInfo tutor)
    {
        Dictionary<string, string> header = new Dictionary<string, string>();
        Dictionary<string, string> body = new Dictionary<string, string>();
        header.Add("Authorization", PlayerPrefs.GetString("Authorization"));
        body.Add("id", tutor.id.ToString());

        OperationResult<List<TutorInfo>> operation = Requester.PostOperation<List<TutorInfo>>($"http://127.0.0.1:8000/api/search/tutor", body, header);

        while (!operation.IsReady)
        {
            yield return null;
        }

        if (!operation.HasError)
        {
            name.text = $"{operation.Data[0].name} {operation.Data[0].last_name}";
            tutorDescription.text = operation.Data[0].description;
            bookClassController.SetTutorId(tutor.id);
            bookClassController.SetTutorCost(tutor.cost);
        }
    }

    public void MoveButtonBackground(GameObject button)
    {
        selectedButtonBackground.SetActive(true);
        selectedButtonBackground.GetComponent<RectTransform>().anchorMin = button.GetComponent<RectTransform>().anchorMin;
        selectedButtonBackground.GetComponent<RectTransform>().anchorMax = button.GetComponent<RectTransform>().anchorMax;
    }

    public void BookClassSession()
    {
        if (!string.IsNullOrEmpty(currentTutorCost))
        {
            this.gameObject.SetActive(false);
            bookClassController.gameObject.SetActive(true);
            bookClassController.SetTutorCost(currentTutorCost);
        }
    }
}
