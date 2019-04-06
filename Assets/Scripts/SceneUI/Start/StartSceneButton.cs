using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class StartSceneButton : MonoBehaviour
{
    public string GotoSceneName;


    void OnClick()
    {
        StackSceneManager.Instance.LoadSceneInStack(GotoSceneName);
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
