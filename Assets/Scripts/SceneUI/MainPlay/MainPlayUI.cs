using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class MainPlayUI : MonoBehaviour
{
    [HideInInspector]
    TextMeshProUGUI DialogText;
    [HideInInspector]
    TextMeshProUGUI TimeText;
    [HideInInspector]
    GameObject OptionPanel;
    [HideInInspector]
    GameObject DisplayImage;
    [HideInInspector]
    Image Photo;

    public GameObject OptionPrefab;

    private UnityAction GanmeStatusUIListener;
    private void OnEnable()
    {
        PersonalEventManager.StartListening("RefreshGameStatusUI", GanmeStatusUIListener);
    }

    private void OnDisable()
    {
        PersonalEventManager.StopListening("RefreshGameStatusUI", GanmeStatusUIListener);
    }


    private void Awake()
    {
        GanmeStatusUIListener = new UnityAction(RefreshUI);
    }

    // Start is called before the first frame update
    void Start()
    {
        print(transform.FindDeepChild("Dialog").name);

        Photo = transform.FindDeepChild("Photo").GetComponent<Image>();
        DisplayImage = transform.FindDeepChild("Dialog").FindDeepChild("Image").gameObject;
        DialogText = transform.FindDeepChild("Dialog").FindDeepChild("Sentence").GetComponent<TextMeshProUGUI>();
        TimeText = transform.FindDeepChild("Time").FindDeepChild("Text").GetComponent<TextMeshProUGUI>();
        OptionPanel = transform.FindDeepChild("Option").gameObject;
        OptionPrefab = transform.FindDeepChild("Option").FindDeepChild("ButtonSample").gameObject;
        GameStatus.Instance.Activate();
        GameStatus.Instance.AnalysisCurrentStatus();
        RefreshUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void RefreshUI()
    {
        DialogText.text = GameStatus.Instance.DialogSentence;
        TimeText.text = GameStatus.Instance.DialogTime;
        foreach (Transform child in OptionPanel.transform)
        {
            if (child.name != "ButtonSample")
                Destroy(child.gameObject);
        }
        if (GameStatus.Instance.PhotoImage)
        {
            Photo.sprite = GameStatus.Instance.PhotoImage;
        }
        if (GameStatus.Instance.DialogImage)
        {
            DisplayImage.SetActive(true);
            DisplayImage.GetComponent<Image>().sprite = GameStatus.Instance.DialogImage;
        } else
            DisplayImage.SetActive(false);
        if (GameStatus.Instance.options.Count > 0)
        {
            float optionWidth = OptionPanel.GetComponent<RectTransform>().rect.width / GameStatus.Instance.options.Count;
            float optionHeight = OptionPanel.GetComponent<RectTransform>().rect.height- 100;
            float optionGap = 100;
            for (var i = 0; i < GameStatus.Instance.options.Count; i++)
            {
                var option = Instantiate(OptionPrefab);
                option.GetComponent<RectTransform>().sizeDelta = new Vector2(optionWidth - optionGap, optionHeight);
                option.transform.Find("Text").GetComponent<Text>().text = GameStatus.Instance.options[i].name;
                option.GetComponent<OptionButton>().gotoName = GameStatus.Instance.options[i].gotoMeta;
                option.transform.SetParent(OptionPanel.transform, true);
                option.transform.localPosition = new Vector3(optionWidth * i, 0, 0);

                option.SetActive(true);
            }
        }
    }
}
