using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class StartNewDayButton : MonoBehaviour
{
    public string gotoName;

    void OnClick()
    {
        //GameStatus.Instance.UntilGoto();
        GameStatus.Instance.NewDay();
        StackSceneManager.Instance.BackToPrevious();
    }

    void Quit()
    {
        Application.Quit();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (GameStatus.Instance.HasNewDay())
            GetComponent<Button>().onClick.AddListener(OnClick);
        else
        {
            GetComponentInChildren<Text>().text = "Done";
            GetComponent<Button>().onClick.AddListener(Quit);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}