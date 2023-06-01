using UnityEngine;
using UnityEngine.UI;

public class DayHoursButtonPrefab : MonoBehaviour
{
    [SerializeField] Color unselectedColor;
    [SerializeField] Color selectedColor;
    [SerializeField] Button currentButton;
    public bool isSelected = false;

    public void ChangeButtonColor()
    {
        ColorBlock colors = currentButton.colors;
        if (!isSelected)
        {
            colors.normalColor = selectedColor;
            isSelected = true;
        }
        else
        {
            colors.normalColor = unselectedColor;
            isSelected = false;
        }
        currentButton.colors = colors;
    }
}
