using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Button))]
public class MainPlayStartButton : MonoBehaviour
{
    void OnClick()
    {
        StackSceneManager.Instance.LoadSceneInStack(GameStatus.Instance.ExtraValue["MainPlayScene"]);
        //GameStatus.Instance.GotoMeta(gotoName);
    }

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    // Update is called once per frame
    void Update()
    {

    }
}