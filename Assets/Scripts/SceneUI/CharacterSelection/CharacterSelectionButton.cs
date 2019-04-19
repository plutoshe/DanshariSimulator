using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class CharacterSelectionButton : MonoBehaviour
{
    public string GotoName;
    public string SceneName;

    void OnClick()
    {
        GameStatus.Instance.GotoMeta(GotoName);
        StackSceneManager.Instance.LoadSceneInStack(SceneName);
        if (GotoName.Contains("1"))
        {
            GameObject.Find("CharacterManager").GetComponent<CharacterManager>().name = "marie";
        }
        else if( GotoName.Contains("2"))
        {
            GameObject.Find("CharacterManager").GetComponent<CharacterManager>().name = "issac";
        }
        else if(GotoName.Contains("3"))
        {
            GameObject.Find("CharacterManager").GetComponent<CharacterManager>().name = "susan";
        }
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
