using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LanguageLevel : MonoBehaviour
{
    [SerializeField] public Image languageFlag;
    public int currentLanguageId;
    public string currentLanguageName;
    public string currrentLanguageLevel;

    public void HandleLanguageLevel(int value)
    {
        if (value == 0)
        {
            currrentLanguageLevel = "a1";
        }
        else if (value == 1)
        {
            currrentLanguageLevel = "a2";
        }
        else if (value == 2)
        {
            currrentLanguageLevel = "b1";
        }
        else if (value == 3)
        {
            currrentLanguageLevel = "b2";
        }
        else if (value == 4)
        {
            currrentLanguageLevel = "c1";
        }
        else if (value == 5)
        {
            currrentLanguageLevel = "c2";
        }
    }

    public string GetCurrentLanguageLevel()
    {
        return currrentLanguageLevel;
    }
}
