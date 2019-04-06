using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum GameStatusMode
{
    Idle = 0,
    Dialog,
    Option,
    SelectItem,
    EndOfDay,
}

public class DialogOption
{
    public string name, gotoMeta;

    public DialogOption() { name = gotoMeta = ""; }
    public DialogOption(string kvs)
    {
        var optionKVs = kvs.Split(new string[] { "," }, StringSplitOptions.None);
        foreach (var option in optionKVs)
        {
            var optionKV = DialogMetaData.GetKeyAndValue(option);
            switch (optionKV.Key.ToLower())
            {
                case "name": name = optionKV.Value; break;
                case "goto": gotoMeta = optionKV.Value; break;
            }
        }
        

    }
}

public class PlayerItem
{
    public string name;
    public PlayerItem(string value)
    {
        name = value;
    }
}

public class PlayerItems
{
    public Dictionary<string, PlayerItem> items;

    public PlayerItems()
    {
        items = new Dictionary<string, PlayerItem>();
    }

    public void Add(PlayerItem item)
    {
        items.Add(item.name, item);
    }

    public bool Contains(string key)
    {
        return items.ContainsKey(key);
    }

    public void Remove(string key)
    {
        items.Remove(key);
    }
}


public class DialogMetaData
{
    public static string CleanStatusValue(string status)
    {
        int startID = 0, endID = status.Length - 1;
        while (startID <= endID && (status[startID] == '\r' || status[startID] == '\n' || status[startID] == ' '))
            startID++;
        while (endID >= 0 && (status[endID] == '\r' || status[endID] == '\n' || status[endID] == ' '))
            endID--;
        if (endID < startID) return "";
        
        return status.Substring(startID, endID - startID + 1);
    }

    public static KeyValuePair<string, string> GetKeyAndValue(string status)
    {
        int keyLen = status.IndexOf(":");
        if (keyLen < 0)
            return new KeyValuePair<string, string>("", "");
        
        return new KeyValuePair<string, string>(CleanStatusValue(status.Substring(0, keyLen)), CleanStatusValue(status.Substring(keyLen + 1)));
    }

    public static Texture2D LoadPNG(string filePath)
    {

        Texture2D tex = null;
        byte[] fileData;

        if (File.Exists(filePath))
        {
            fileData = File.ReadAllBytes(filePath);
            tex = new Texture2D(2, 2);
            tex.LoadImage(fileData); 
        }
        return tex;
    }

    public static Sprite LoadImage(string imageFileName)
    {
        Texture2D tex = LoadPNG(Path.Combine("Assets/MetaData", imageFileName));
        return Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2());
    }

    public Dictionary<string, List<string>> statusSet;

    public void LoadMetaData(string defaultMetaDataName)
    {
        statusSet = new Dictionary<string, List<string>>();
        var path = Path.Combine("Assets/MetaData", defaultMetaDataName + ".txt");

        using (StreamReader streamReader = File.OpenText(path))
        {
            string astring = streamReader.ReadToEnd();            
            string[] nstring = astring.Split(new string[] { "###" }, StringSplitOptions.None);
            foreach (var value in nstring)
            {
                string[] status = value.Split(new string[] { "  - " }, StringSplitOptions.None);
                string key = CleanStatusValue(status[0]);
                if (key == "") continue;
                List<string> meta = new List<string>(status);
                meta.RemoveAt(0);
                statusSet.Add(key, meta);
            }
        }
    }

    public List<string> GetMeta(string key)
    {
        if (statusSet.ContainsKey(key))
            return statusSet[key];
        return new List<string>();
    }
}


public class GameStatus : MonoBehaviour
{
    public string defaultMetaDataName;

    [HideInInspector]
    public string DialogSentence;
    [HideInInspector]
    public Sprite DialogImage;
    [HideInInspector]
    public Sprite BackgroundImage;

    [HideInInspector]
    public string DialogTime;

    [HideInInspector]
    public List<DialogOption> options;

    [HideInInspector]
    public string theme;

    [HideInInspector]
    public PlayerItems playerItems;

    [HideInInspector]
    public int Desire, Satisfiction, LivingQuality, Happiness;

    [HideInInspector]
    public DialogMetaData MetaData;

    [HideInInspector]
    public GameStatusMode mode;

    [HideInInspector]
    List<string> currentMeta;
    [HideInInspector]
    int currentID;

    private static GameStatus _instance;
    public static GameStatus Instance
    {
        get
        {
            if (_instance == null)
                _instance = new GameStatus();
            return _instance;
        }
    }

    GameStatus()
    {
        Clear();
    }

    void Clear()
    {
        MetaData = new DialogMetaData();
        mode = GameStatusMode.Idle;
    }

    

    // Start is called before the first frame update
    void Awake()
    {
        if (_instance == null)
            _instance = this;
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        Clear();
        MetaData = new DialogMetaData();
        MetaData.LoadMetaData(defaultMetaDataName);
        currentMeta = MetaData.GetMeta("default");
        playerItems = new PlayerItems();
        currentID = 0;
        options = new List<DialogOption>();
        foreach (var e in currentMeta)
        {
            Debug.Log(e);
        }
        Debug.Log(currentMeta.Count);
        AnalysisCurrentStatus();
    }

    private void Start()
    {
        
    }

    public int GetStatusValue(string valueName)
    {
        switch (valueName.ToLower())
        {
            case "desire": return Desire;
            case "satisfiction": return Satisfiction;
            case "livingquality": return LivingQuality;
            case "happiness": return Happiness;
        }
        return 0;
    }

    void UpdateValueStatus(string status)
    {
        string[] values = status.Split(new string[] { "," }, StringSplitOptions.None);
        foreach (var valueStatus in values)
        {
            var kv = DialogMetaData.GetKeyAndValue(valueStatus);
            switch (kv.Key.ToLower())
            {
                case "desire": Desire += int.Parse(kv.Value); break;
                case "satisfiction": Satisfiction += int.Parse(kv.Value); break;
                case "livingquality": LivingQuality += int.Parse(kv.Value); break;
                case "happiness": Happiness += int.Parse(kv.Value); break;
            }
        }
    }

    void Analysis(string status)
    {
        print("Analysis!");
        var kv = DialogMetaData.GetKeyAndValue(status);
        string key = kv.Key;
        string value = kv.Value;
        options.Clear();
        DialogImage = null;
        switch (key)
        {
            case "theme":
                if (theme.ToLower() != "dialog")
                    mode = GameStatusMode.SelectItem;
                else
                    mode = GameStatusMode.Dialog;
                theme = value;
                break;
            case "item":
                playerItems.Add(new PlayerItem(value));
                NextStatus();
                break;

            case "dialog":
                mode = GameStatusMode.Dialog;
                DialogSentence = value;
                break;
            case "backgroundImage":
                BackgroundImage = DialogMetaData.LoadImage(value);
                NextStatus();
                break;
            case "dialogImage":
            case "image":
                mode = GameStatusMode.Dialog;
                DialogImage = DialogMetaData.LoadImage(value);
                break;
            case "time":
                DialogTime = value;
                NextStatus();
                break;
            case "value":
                mode = GameStatusMode.Dialog;
                var valueChange = value;
                int start = valueChange.IndexOf("{");
                int end = valueChange.IndexOf("}");
                valueChange = valueChange.Substring(start + 1, end - start - 1);
                UpdateValueStatus(valueChange);
                break;
            case "option":
                mode = GameStatusMode.Option;
                valueChange = value;
                //start = valueChange.IndexOf("{");
                //end = valueChange.IndexOf("}");
                //valueChange.Substring(start + 1, end - start - 1);
                var values = valueChange.Split(new string[] { "$" }, StringSplitOptions.None);
                
                foreach (var valueStatus in values)
                {
                    if (DialogMetaData.CleanStatusValue(valueStatus) == "")
                        continue;
                    options.Add(new DialogOption(valueStatus));
                }
                break;
            case "mode":

                break;
            case "goto":
                GotoMeta(value);
                break;
        }
    }

    public bool UntilGoto()
    {
        //while (currentID < currentMeta.Count && mode != GameStatusMode.Option)
        //    NextStatus();
        //return 
        return true;
    }

    public void AnalysisCurrentStatus()
    {
        if (currentID < currentMeta.Count)
            Analysis(currentMeta[currentID]);
    }

    public void GotoMeta(string metaName)
    {
        print("goto:" + metaName);
        currentMeta = MetaData.GetMeta(metaName);
        currentID = 0;
        AnalysisCurrentStatus();
        PersonalEventManager.TriggerEvent("RefreshGameStatusUI");
    }

    public void NextStatus()
    {
        currentID++;
        AnalysisCurrentStatus();
        
        PersonalEventManager.TriggerEvent("RefreshGameStatusUI");
    }

    void CheckUserOperations()
    {
        switch (mode)
        {
            case GameStatusMode.Dialog:
                if (Input.GetMouseButtonDown(0))
                {
                    NextStatus();
                }
                break;
        }
    }


    // Update is called once per frame
    void Update()
    {
        CheckUserOperations();

        //test
        if (Input.GetKeyDown(KeyCode.S))
        {
            mode = GameStatusMode.Dialog;
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            print(currentMeta[currentID]);
        }
    }

}
