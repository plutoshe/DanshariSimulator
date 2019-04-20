using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CharacterDemonstration : MonoBehaviour
{
    Image photo;
    //Text detail;
    TextMeshProUGUI background;

    // Start is called before the first frame update
    void Start()
    {
        photo = transform.FindDeepChild("Photo").GetComponent<Image>();
        //detail = transform.FindDeepChild("Detail").GetComponent<Text>();
        background = transform.FindDeepChild("BackgroundDetail").GetComponent<TextMeshProUGUI>();
        print("Demonstration start");
        photo.sprite = GameStatus.Instance.PhotoImage;
        //if (GameStatus.Instance.ExtraValue.ContainsKey("name"))
        //    detail.text = "name: " + GameStatus.Instance.ExtraValue["name"] + "\n";
        //if (GameStatus.Instance.ExtraValue.ContainsKey("gender"))
        //    detail.text += "gender: " + GameStatus.Instance.ExtraValue["gender"] + "\n";
        //if (GameStatus.Instance.ExtraValue.ContainsKey("age"))
        //    detail.text += "age: " + GameStatus.Instance.ExtraValue["age"] + "\n";
        if (GameStatus.Instance.ExtraValue.ContainsKey("background"))
            background.text = GameStatus.Instance.ExtraValue["background"] + "\n";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
