using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class BackToTitleButton : MonoBehaviour
{
    
    void OnClick()
    {
        StackSceneManager.Instance.BackToTop();
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
