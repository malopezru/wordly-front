using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuLanguage : MonoBehaviour
{
    private bool isSelectingLanguage = false;
    [SerializeField] GameObject opaqueBackground;
    [SerializeField] GameObject selectionWindow;
    [SerializeField] GameObject triangle;
    [SerializeField] ForumController forumController;

    public void ToggleSelectionWindow()
    {
        isSelectingLanguage = !isSelectingLanguage;
        opaqueBackground.SetActive(isSelectingLanguage);
        selectionWindow.SetActive(isSelectingLanguage);
        triangle.SetActive(isSelectingLanguage);
    }

    public void SetForumRole(string role)
    {
        forumController.currentRole = role;
    }
}
