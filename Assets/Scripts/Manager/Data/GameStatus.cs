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
        kvs = DialogMetaData.CleanStatusValue(kvs);
        if (kvs[0] == '{') kvs = kvs.Remove(0, 1);
        if (kvs[kvs.Length - 1] == '}') kvs = kvs.Remove(kvs.Length - 1,1);
        Debug.Log(kvs);
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
    public Dictionary<string, string> extraAttr;

    public void Clear()
    {
        extraAttr = new Dictionary<string, string>();
        name = "";
    }

    public string GetAttr(string attrName)
    {
        string lowerAttrName = attrName.ToLower();
        if (lowerAttrName == "name")
            return name;
        else if (extraAttr.ContainsKey(lowerAttrName))
            return extraAttr[lowerAttrName];
        return "";
    }

    public PlayerItem(string value)
    {
        Clear();
        string[] values = value.Split(new string[] { "," }, StringSplitOptions.None);
        foreach (var valueStatus in values)
        {
            var kv = DialogMetaData.GetKeyAndValue(valueStatus);
            switch (kv.Key.ToLower())
            {
                case "name": name = kv.Value.ToLower(); break;
                default:
                    extraAttr[kv.Key.ToLower()] = kv.Value;
                    break;
            }
        }
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
        items[item.name] = item;
    }

    public bool Contains(string key)
    {
        return items.ContainsKey(key);
    }

    public PlayerItem GetPlayerItem(string key)
    {
        if (Contains(key.ToLower()))
            return items[key.ToLower()];
        return null;
    }

    public void Remove(string key)
    {
        items.Remove(key);
    }
}


public class DialogMetaData
{
    public DialogMetaData()
    {
        Clear();
    }

    public void Clear()
    {
        statusSet = new Dictionary<string, List<string>>();
    }

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

    public void NewMetaData(string metaDataName)
    {
        Clear();
        LoadMetaData(metaDataName);
    }

    public void LoadMetaData(string metaDataName)
    {
        var path = Path.Combine("Assets/MetaData", metaDataName + ".txt");

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
    public string itemMetaDataName;
    [HideInInspector]
    public int DayID { get; private set; } = 0;

    public List<string> dayStartGotoName = new List<string> { "default", "Day2", "Day3" };

    [HideInInspector]
    public string DialogSentence { get; private set; } = "";

    [HideInInspector]
    public Sprite DialogImage { get; private set; }

    [HideInInspector]
    public Sprite PhotoImage { get; private set; }

    [HideInInspector]
    public Sprite BackgroundImage { get; private set; }

    [HideInInspector]
    public string DialogTime { get; private set; }

    [HideInInspector]
    public List<DialogOption> options { get; private set; }

    [HideInInspector]
    public string theme { get; private set; } = "";

    [HideInInspector]
    public PlayerItems PlayerOwningItems { get; private set; }

    [HideInInspector]
    public int Living, Satisfaction, Stress;

    [HideInInspector]
    public DialogMetaData MetaData { get; private set; }

    [HideInInspector]
    public GameStatusMode mode { get; private set; }

    [HideInInspector]
    public List<string> CurrentMeta { get; private set; }

    [HideInInspector]
    public Dictionary<string, string> ExtraValue { get; private set; }

    [HideInInspector]
    public int CurrentID { get; private set; }

    public void SetDialogSentence(string dialogSetence)
    {
        DialogSentence = dialogSetence;
    }

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

    public void NewDay()
    {
        DayID++;
        if (DayID < dayStartGotoName.Count)
            GotoMeta(dayStartGotoName[DayID]);
    }

    public bool HasNewDay()
    {
        return DayID + 1 < dayStartGotoName.Count;
    }

    GameStatus()
    {
        Clear();
    }

    void Clear()
    {
        MetaData = new DialogMetaData();
        mode = GameStatusMode.Idle;
        DayID = 0;
        CurrentMeta = new List<string>();
        PlayerOwningItems = new PlayerItems();
        CurrentID = 0;
        options = new List<DialogOption>();
        ExtraValue = new Dictionary<string, string>();
    }
    
    void LoadMetaData(string name)
    {
        MetaData.LoadMetaData(name);
        if(DayID < dayStartGotoName.Count)
            CurrentMeta = MetaData.GetMeta(dayStartGotoName[DayID]);
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
        LoadMetaData(defaultMetaDataName);
        LoadMetaData(itemMetaDataName);
    }

    private void Start()
    {
        
    }

    public int GetStatusValue(string valueName)
    {
        switch (valueName.ToLower())
        {
            case "stress": return Stress;
            case "satisfaction": return Satisfaction;
            case "living": return Living;
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
                case "stress": Stress += int.Parse(kv.Value); break;
                case "satisfaction": Satisfaction += int.Parse(kv.Value); break;
                case "living": Living += int.Parse(kv.Value); break;
            }
        }
    }
    void NewValueStatus(string status)
    {
        Living = 0;
        Stress = 0;
        Satisfaction = 0;
        UpdateValueStatus(status);
    }

    List<string> SplitByDelim(string status, char delim)
    {
        List<string> splitArr = new List<string>();
        int degree = 0; int reference = 0; int arrDegree = 0;
        int lastIndex = 0;
        for (int i = 0; i < status.Length; i++)
        {
            if (status[i] == '{') degree++;
            if (status[i] == '[') arrDegree++;
            if (status[i] == '}') degree--;
            if (status[i] == ']') arrDegree--;
            if (status[i] == '"') reference = 1 - reference;
            if (status[i] == delim && arrDegree == 0 && degree == 0 && reference == 0)
            {
                splitArr.Add(status.Substring(lastIndex, i - lastIndex));
                lastIndex = i + 1;
            }
        }
        if (lastIndex < status.Length)
            splitArr.Add(status.Substring(lastIndex, status.Length - lastIndex));
        return splitArr;
    }

    void AnalysisKV(string status)
    {
        print("Analysis!");
        var kv = DialogMetaData.GetKeyAndValue(status);
        string key = kv.Key;
        string value = kv.Value;
        print(key);
        print(value);
        print("==========");
        switch (key)
        {
            case "photo":
                PhotoImage = DialogMetaData.LoadImage(value);
                break;
            case "theme":
                theme = value;
                if (theme != "dialog")
                    mode = GameStatusMode.SelectItem;
                break;
            case "item":
                //playerItems.Add(new PlayerItem(value));
                int start = value.IndexOf("{");
                int end = value.IndexOf("}");
                value = value.Substring(start + 1, end - start - 1);
                PlayerOwningItems.Add(new PlayerItem(value));

                break;

            case "dialog":
                if (value[0] == '"') value = value.Remove(0, 1);
                if (value[value.Length - 1] == '"') value = value.Remove(value.Length - 1, 1);
                DialogSentence = value;
                break;
            case "backgroundImage":
                BackgroundImage = DialogMetaData.LoadImage(value);
                break;
            case "dialogImage":
            case "image":
                DialogImage = DialogMetaData.LoadImage(value);
                break;
            case "time":
                DialogTime = value;
                break;
            case "valueset":
                var valueChange = value;
                start = valueChange.IndexOf("{");
                end = valueChange.IndexOf("}");
                valueChange = valueChange.Substring(start + 1, end - start - 1);
                NewValueStatus(valueChange);
                break;
            case "valueupdate":
                valueChange = value;
                start = valueChange.IndexOf("{");
                end = valueChange.IndexOf("}");
                valueChange = valueChange.Substring(start + 1, end - start - 1);
                UpdateValueStatus(valueChange);
                break;
            case "option":
                mode = GameStatusMode.Option;
                valueChange = value;
                print(valueChange);
                start = valueChange.IndexOf("[");
                end = valueChange.IndexOf("]");
                valueChange = valueChange.Substring(start + 1, end - start - 1);
                var values = SplitByDelim(valueChange, ',');
                foreach (var valueStatus in values)
                {
                    if (DialogMetaData.CleanStatusValue(valueStatus) == "")
                        continue;
                    options.Add(new DialogOption(valueStatus));
                }
                print(options.Count);
                break;
            case "mode":
                switch (value.ToLower())
                {
                    case "dialog": mode = GameStatusMode.Dialog;break;
                    case "option": mode = GameStatusMode.Option;break;
                    case "selectitem": mode = GameStatusMode.SelectItem;break;
                    case "idle": mode = GameStatusMode.Idle;break;
                }
                break;
            case "goto":
                mode = GameStatusMode.Dialog;
                GotoMeta(value);
                break;
            default:
                ExtraValue[key] = value;
                print("extra");
                break;
        }
    }
    
    void Analysis(string status)
    {
        GameStatusMode oldMode = mode;
        mode = GameStatusMode.Idle;
        options.Clear();
        theme = "";
        DialogImage = null;
        var kvs = SplitByDelim(status, ',');
        foreach (var kv in kvs)
        {
            AnalysisKV(kv);
        }
        
        if (mode == GameStatusMode.Idle)
        {
            if (oldMode == GameStatusMode.Option || oldMode == GameStatusMode.SelectItem)
                mode = GameStatusMode.Dialog;
            else mode = oldMode;
        }
        
    }

    public void Activate()
    {
        mode = GameStatusMode.Dialog;
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
        if (CurrentID < CurrentMeta.Count)
            Analysis(CurrentMeta[CurrentID]);
    }

    public void GotoMeta(string metaName)
    {
        print("goto:" + metaName);
        CurrentMeta = MetaData.GetMeta(metaName);
        CurrentID = 0;
        AnalysisCurrentStatus();
        PersonalEventManager.TriggerEvent("RefreshGameStatusUI");
    }

    public void NextStatus()
    {
        CurrentID++;
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
            GotoMeta("JohnItemList");
            for (int i = 0; i < CurrentMeta.Count; i++)
            {
                print(CurrentMeta[i]);
            }
            //print(currentMeta[currentID]);
        }
    }

}
