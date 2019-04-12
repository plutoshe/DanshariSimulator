using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class themePanel : MonoBehaviour
{
    GameObject ContentPanel;

    // Start is called before the first frame update
    void Start()
    {
        ContentPanel = transform.Find("Content").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        ContentPanel.SetActive(transform.name.ToLower() == GameStatus.Instance.theme);
        if (transform.name.ToLower() == GameStatus.Instance.theme) {
            foreach (Transform child in ContentPanel.transform)
            {
                if (child.tag == "PlayerItem")
                {
                    if (!GameStatus.Instance.PlayerOwningItems.Contains(child.gameObject.name.ToLower()))
                        child.gameObject.SetActive(false);
                    else
                        child.gameObject.SetActive(true);
                }
                else
                    child.gameObject.SetActive(true);
            }
            
            //var items = GameObject.FindGameObjectsWithTag("PlayerItem");
            //foreach (var item in items) {
            //    item.SetActive(true);
            //}
        }
    }
}
