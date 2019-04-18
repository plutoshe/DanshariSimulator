using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterManager : MonoBehaviour
{

    public string name;

    public Sprite Marie;
    public Sprite Issac;
    public Sprite Susan;

    public Sprite MarieName;
    public Sprite IssacName;
    public Sprite SusanName;

    public Sprite female;
    public Sprite male;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void Awake()
    {
        DontDestroyOnLoad(this);
        /*if (Resources.FindObjectsOfTypeAll(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name.Equals("CharacterDemonstration"))
        {
            if(name == "marie")
            {
                MarieInfo();
            }
            else if(name == "issac")
            {
                IssacInfo();
            }
            else if (name == "susan")
            {
                SusanInfo();
            }
        }

        if (SceneManager.GetActiveScene().name.Equals("Conclusion"))
        {
            if (name == "marie")
            {
                GameObject.Find("Photo").GetComponent<Image>().sprite = Marie;
            }
            else if (name == "issac")
            {
                GameObject.Find("Photo").GetComponent<Image>().sprite = Issac;
            }
            else if (name == "susan")
            {
                GameObject.Find("Photo").GetComponent<Image>().sprite = Susan;
            }
        }
    }

    void MarieInfo()
    {
        GameObject.Find("Name").GetComponent<Image>().sprite = MarieName;
        GameObject.Find("Gender").GetComponent<Image>().sprite = female;
        GameObject.Find("Photo").GetComponent<Image>().sprite = Marie;
    }

    void IssacInfo()
    {
        GameObject.Find("Name").GetComponent<Image>().sprite = IssacName;
        GameObject.Find("Gender").GetComponent<Image>().sprite = male;
        GameObject.Find("Photo").GetComponent<Image>().sprite = Issac;
    }

    void SusanInfo()
    {
        GameObject.Find("Name").GetComponent<Image>().sprite = SusanName;
        GameObject.Find("Gender").GetComponent<Image>().sprite = female;
        GameObject.Find("Photo").GetComponent<Image>().sprite = Susan;
    }
}
