using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class ConclusionUI : MonoBehaviour
{
    [HideInInspector]
    Image Photo;
    [HideInInspector]
    TextMeshProUGUI DialogText;
    [HideInInspector]
    TextMeshProUGUI StatusText;
    [HideInInspector]
    TextMeshProUGUI GradeCommentText;

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
        StatusText.text =
            "Living: " + gradeToText(GameStatus.Instance.Living) + "\n\n" +
            "Stress: " + gradeToText(GameStatus.Instance.Stress) + "\n\n" +
            "Satisfaction: " + gradeToText(GameStatus.Instance.Satisfaction);
        calculateGrade();
        GradeCommentText.text = commentOfGrade(grade);
        DialogText.text = GameStatus.Instance.DialogSentence;
    }

    // Start is called before the first frame update
    void Start()
    {
        Photo = transform.FindDeepChild("Photo").GetComponent<Image>();
        StatusText = transform.FindDeepChild("Status").GetComponent<TextMeshProUGUI>();
        DialogText = transform.FindDeepChild("Dialog").FindDeepChild("Sentence").GetComponent<TextMeshProUGUI>();
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
