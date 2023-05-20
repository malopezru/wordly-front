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

    private string currentTutorCost;

    public void SetTutorInfo(TutorInfo tutor)
    {
        TutorInfo newTutor = GetSpecificTutor(tutor);
        name.text = $"{tutor.name} {tutor.last_name}";
        tutorDescription.text = newTutor.description;
        currentTutorCost = tutor.cost;
    }

    public TutorInfo GetSpecificTutor(TutorInfo tutor)
    {
        tutor.description = "Tutor test";
        return tutor;
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
            bookClassController.SetTutorCost(currentTutorCost);
        }
    }
}
