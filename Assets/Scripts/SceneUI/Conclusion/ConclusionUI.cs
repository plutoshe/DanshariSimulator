using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class ConclusionUI : MonoBehaviour
{
    public List<Sprite> gradeImages;
    //public List<Image> gradeImage1;
    //public int test1;
    [HideInInspector]
    Image Photo;
    [HideInInspector]
    TextMeshProUGUI DialogText;
    //[HideInInspector]
    //TextMeshProUGUI StatusText;
    [HideInInspector]
    Text LivingText, StressText, SatisfactionText;

    [HideInInspector]
    TextMeshProUGUI GradeCommentText;
    [HideInInspector]
    Image GradeImage;

    int grade = 0;

    int evaluate(int value)
    {
        print(value);
        if (value > 160) return 5;
        if (value > 120) return 4;
        if (value > 80) return 3;
        if (value > 40) return 2;
        return 1;
    }

    string gradeToText(int value)
    {
        switch (evaluate(value))
        {
            case 5: return "Excellent"; 
            case 4: return "Good"; 
            case 3: return "OK"; 
            case 2: return "Meh"; 
            case 1: return "Need to be improved"; 
        }
        return "";
    }

    string commentOfGrade(int grade)
    {
        if (grade > 12) return "Master of Danshari";
        if (grade > 8) return "Super Danshari-er";
        if (grade > 5) return "Danshari learner";
        return "Danshari Beginner";

    }

    string dialogOfGrade(int grade)
    {
        if (grade > 12) return "Incredible! You’re a danshari master! You’re ready to take what you’ve learned to the real world!";
        if (grade > 8) return "Awesome! You’re well on your way to becoming a Danshari Master! Make sure you read each client’s profile so you know exactly what they need.";
        if (grade > 5) return "Good job, you’re learning a lot! Make sure you pay attention to what your clients think about each object. Is it something they regularly use?";
        return "You’ve only just started on your danshari journey and have a long way to go! Make sure you pay attention to what your client’s need and don’t… does the item match their priorities?";
    }

    Sprite generatingImageOfgrade(int grade)
    {
        if (grade > 12) return gradeImages[3];
        if (grade > 8) return gradeImages[2];
        if (grade > 5) return gradeImages[1];
        return gradeImages[0];
    }

    void calculateGrade()
    {
        grade =
            evaluate(GameStatus.Instance.Living) +
            evaluate(GameStatus.Instance.Stress) +
            evaluate(GameStatus.Instance.Satisfaction);
        print(grade);
    }

    void refreshUI()
    {
        if (GameStatus.Instance.PhotoImage)
        {
            Photo.sprite = GameStatus.Instance.PhotoImage;
        }
        LivingText.text =
            gradeToText(GameStatus.Instance.Living);
        StressText.text =
            gradeToText(GameStatus.Instance.Stress);
        SatisfactionText.text = 
            gradeToText(GameStatus.Instance.Satisfaction);
        calculateGrade();
        GradeCommentText.text = commentOfGrade(grade);
        DialogText.text = dialogOfGrade(grade);
        GradeImage.sprite = generatingImageOfgrade(grade);
        //DialogText.text = GameStatus.Instance.DialogSentence;
    }

    // Start is called before the first frame update
    void Start()
    {
        Photo = transform.FindDeepChild("Photo").GetComponent<Image>();
        //StatusText = transform.FindDeepChild("Status").GetComponent<TextMeshProUGUI>();
        LivingText = transform.FindDeepChild("Living").FindDeepChild("Text").GetComponent<Text>();
        StressText = transform.FindDeepChild("Stress").FindDeepChild("Text").GetComponent<Text>();
        SatisfactionText = transform.FindDeepChild("Satisfaction").FindDeepChild("Text").GetComponent<Text>();

        DialogText = transform.FindDeepChild("Dialog").FindDeepChild("Sentence").GetComponent<TextMeshProUGUI>();
        GradeImage = transform.FindDeepChild("Grade").FindDeepChild("Image").GetComponent<Image>();
        GradeCommentText = transform.FindDeepChild("Grade").FindDeepChild("Comment").GetComponent<TextMeshProUGUI>();

        refreshUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            GameStatus.Instance.Living += 10;
            GameStatus.Instance.Satisfaction += 10;
            GameStatus.Instance.Stress += 10;
            refreshUI();
        }
    }
}
