﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class PlayerItemBehavious : MonoBehaviour
{
    public int a, b, c, d;

    void OnClick()
    {
        //GameStatus.Instance.UntilGoto();
        //GameStatus.Instance.GotoMeta(gotoName);
        transform.Find("OperationPanel").gameObject.SetActive(true);
        transform.FindDeepChild("Status").GetComponentInChildren<Text>().text = 
            "a: " + a + "\nb: " + b + "\nc: " + c + "\nd: " + d;
    }

    public void CancelShow()
    {
        transform.Find("OperationPanel").gameObject.SetActive(false);
    }

    public void Drop()
    {
        GameStatus.Instance.playerItems.Remove(gameObject.name.ToLower());
        gameObject.SetActive(false);
    }


        // Start is called before the first frame update
    void Start()
    {
        transform.Find("OperationPanel").gameObject.SetActive(false);
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    // Update is called once per frame
    void Update()
    {
       // print(GameStatus.Instance.playerItems.Contains(gameObject.name.ToLower()));
       
    }
}