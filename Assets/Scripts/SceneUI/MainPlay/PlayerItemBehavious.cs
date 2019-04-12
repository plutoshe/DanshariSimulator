using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerItemBehavious : MonoBehaviour
{
    public bool finished = false;
    public void OnClick()
    {
        if (finished) return;
        //GameStatus.Instance.UntilGoto();
        //GameStatus.Instance.GotoMeta(gotoName);
        PlayerItem currentItem = GameStatus.Instance.PlayerOwningItems.GetPlayerItem(gameObject.name);
        GameStatus.Instance.SetDialogSentence(
            "This is my " + 
            currentItem.GetAttr("Dialog 01 Name") + ","  +
            currentItem.GetAttr("Dialog 02 Space") + "." +
            currentItem.GetAttr("Dialog 03 Newness") + ", and I think " +
            currentItem.GetAttr("Dialog 04 Necessity") + "." +
            currentItem.GetAttr("Dialog 05 Likability") + ".\n");
        
        PersonalEventManager.TriggerEvent("RefreshGameStatusUI");
        transform.Find("OperationPanel").gameObject.SetActive(true);
        transform.FindDeepChild("Status").GetComponentInChildren<Text>().text =
            "";
            //"a: " + a + "\nb: " + b + "\nc: " + c + "\nd: " + d;
    }

    public void CancelShow()
    {
        transform.Find("OperationPanel").gameObject.SetActive(false);
    }

    public void Organize()
    {
        PlayerItem currentItem = GameStatus.Instance.PlayerOwningItems.GetPlayerItem(gameObject.name);
        GameStatus.Instance.Stress += int.Parse(currentItem.GetAttr("Organize Stress"));
        GameStatus.Instance.Living += int.Parse(currentItem.GetAttr("Organize Living"));
        GameStatus.Instance.Satisfaction += int.Parse(currentItem.GetAttr("Organize Satisfaction"));

        GameStatus.Instance.SetDialogSentence(
            "I feel " +
            currentItem.GetAttr("Dialog 06 Organize Stress") + ". I think my room becomes " +
            currentItem.GetAttr("Dialog 07 Organize Living") + "." +
            currentItem.GetAttr("Dialog 08 Organize Satisfaction") + ".\n");
        PersonalEventManager.TriggerEvent("RefreshGameStatusUI");
        finished = true;
        transform.position = transform.FindDeepChild("OrganizedPoint").transform.position;
        CancelShow();
    }

    public void Drop()
    {
        PlayerItem currentItem = GameStatus.Instance.PlayerOwningItems.GetPlayerItem(gameObject.name);
        GameStatus.Instance.Stress += int.Parse(currentItem.GetAttr("Throw Stress"));
        GameStatus.Instance.Living += int.Parse(currentItem.GetAttr("Throw Living"));
        GameStatus.Instance.Satisfaction += int.Parse(currentItem.GetAttr("Throw Satisfaction"));

        GameStatus.Instance.SetDialogSentence(
            "I feel " +
            currentItem.GetAttr("Dialog 09 Throw Stress") + ". I think my room becomes " +
            currentItem.GetAttr("Dialog 10 Throw Living") + "." +
            currentItem.GetAttr("Dialog 11 Throw Satisfaction") + ".\n");
        PersonalEventManager.TriggerEvent("RefreshGameStatusUI");

        PersonalEventManager.TriggerEvent("RefreshGameStatusUI");
        GameStatus.Instance.PlayerOwningItems.Remove(gameObject.name.ToLower());
        gameObject.SetActive(false);
    }


        // Start is called before the first frame update
    void Start()
    {
        transform.Find("OperationPanel").gameObject.SetActive(false);
        //GetComponent<Button>().onClick.AddListener(OnClick);
    }

    // Update is called once per frame
    void Update()
    {
       // print(GameStatus.Instance.playerItems.Contains(gameObject.name.ToLower()));
       
    }
}