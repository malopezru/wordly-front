using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RateLessonController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI tutorName;
    [SerializeField] TextMeshProUGUI lessonDate;
    [SerializeField] TMP_InputField commentInput;
    [SerializeField] Button star1Button;
    [SerializeField] Button star2Button;
    [SerializeField] Button star3Button;
    [SerializeField] Button star4Button;
    [SerializeField] Button star5Button;
    [SerializeField] Button confirmButton;
    [SerializeField] Sprite emptyStar;
    [SerializeField] Sprite fullStar;

    private int rating = 0;
    private string lessonComment = "";

    public TMP_InputField CommentInput { get => commentInput; set => commentInput = value; }

    public void SendRating()
    {
        lessonComment = this.CommentInput.text;
        //request to server
    }

    public void SetRating(int numberOfStars)
    {
        if (numberOfStars == 1)
        {
            rating = 1;
            star1Button.image.sprite = fullStar;
            star2Button.image.sprite = emptyStar;
            star3Button.image.sprite = emptyStar;
            star4Button.image.sprite = emptyStar;
            star5Button.image.sprite = emptyStar;
        }
        if (numberOfStars == 2)
        {
            rating = 2;
            star1Button.image.sprite = fullStar;
            star2Button.image.sprite = fullStar;
            star3Button.image.sprite = emptyStar;
            star4Button.image.sprite = emptyStar;
            star5Button.image.sprite = emptyStar;
        }
        if (numberOfStars == 3)
        {
            rating = 3;
            star1Button.image.sprite = fullStar;
            star2Button.image.sprite = fullStar;
            star3Button.image.sprite = fullStar;
            star4Button.image.sprite = emptyStar;
            star5Button.image.sprite = emptyStar;
        }
        if (numberOfStars == 4)
        {
            rating = 4;
            star1Button.image.sprite = fullStar;
            star2Button.image.sprite = fullStar;
            star3Button.image.sprite = fullStar;
            star4Button.image.sprite = fullStar;
            star5Button.image.sprite = emptyStar;
        }
        if (numberOfStars == 5)
        {
            rating = 5;
            star1Button.image.sprite = fullStar;
            star2Button.image.sprite = fullStar;
            star3Button.image.sprite = fullStar;
            star4Button.image.sprite = fullStar;
            star5Button.image.sprite = fullStar;
        }
    }
}
