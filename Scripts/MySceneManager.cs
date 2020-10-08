using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MySceneManager : MonoBehaviour
{
    public static MySceneManager instance;

    private GameManager _gameManagerInstance;
    private UIManager _uiManagerInstance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _gameManagerInstance = GameManager.instance;
        _uiManagerInstance = UIManager.instance;
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void LoadLevelOne()
    {
        SceneManager.LoadScene(1);
    }

    public int GetCurrentLevelID()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }
}
