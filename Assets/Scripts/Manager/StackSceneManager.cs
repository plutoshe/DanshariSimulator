using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StackSceneManager : MonoBehaviour
{
    List<string> SceneStack;
    string CurrentSceneName
    {
        get
        {
            return SceneStack.Last();
        }
    }

    public static StackSceneManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new StackSceneManager();
            return _instance;
        }
    }

    StackSceneManager()
    {
        Clear();
    }

    void Clear()
    {
        SceneStack = new List<string>();
    }

    private static StackSceneManager _instance;
    // Use this for initialization
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
        SceneStack.Add(SceneManager.GetActiveScene().name);
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadSceneInStack(string sceneName)
    {
        SceneStack.Add(sceneName);
        LoadScene(CurrentSceneName);
    }

    public void BackToPrevious()
    {
        if (SceneStack.Count > 1)
        {
            SceneStack.RemoveAt(SceneStack.Count - 1);
            LoadScene(CurrentSceneName);
        }
        else
        {
            Application.Quit();
        }
    }

    public void BackToTop()
    {
        if (SceneStack.Count > 1)
        {
            SceneStack.RemoveRange(1, SceneStack.Count - 1);
            LoadScene(CurrentSceneName);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            BackToPrevious();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            foreach (var i in SceneStack) print(i);
        }
    }
}
