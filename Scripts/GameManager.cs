using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private MySceneManager _sceneManagerInstance;
    private UIManager _uiManagerInstance;
    private ObjectPooler _poolerInstance;

    private int _maxPoints;
    private int _maxEnemiesKilled;

    //gameplay variables
    [SerializeField]
    private float _defaultMatchTime = 180f;

    private float _personalizedMatchTime = 0f;
    public bool _gameOver = false;
    public bool _gamePaused = false;
    public bool _gameStarted = false;

    private GameObject _player;
    private float _currentTime = 180;

    private Terrain terrain;

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

    void Start()
    {
        _poolerInstance = ObjectPooler.instance;
        _sceneManagerInstance = MySceneManager.instance;
        _uiManagerInstance = UIManager.instance;
        _currentTime = _defaultMatchTime;
        _player = GameObject.FindGameObjectWithTag("Player");

        if (_sceneManagerInstance.GetCurrentLevelID() == 1)
        {
            terrain = GameObject.Find("Terrain").GetComponent<Terrain>();
        }
    }

    private void Update()
    {
        if (!_gamePaused)
        {
            if (_gameStarted)
            {
                _currentTime -= Time.deltaTime;
                _uiManagerInstance.UpdateTime(Convert.ToInt32(_currentTime));

                if (_currentTime <= 0)
                {
                    GameOver(_player.GetComponent<Player>().CurrentPoints);
                }
            }
        }
    }

    public void GameOver(int points)
    {
        if (points > _maxPoints)
        {
            _maxPoints = points;
        }

        _player.SetActive(false);
        _gameOver = true;
        _gamePaused = true;
        _gameStarted = false;
        _uiManagerInstance.CallGameOver();
    }

    public void ResumeGame()
    {
        _gamePaused = false;
    }

    public void PauseGame()
    {
        _gamePaused = true;
    }

    public void RestartGame()
    {
        _currentTime = _defaultMatchTime;
        _gameStarted = true;
        _gamePaused = false;
        _gameOver = false;
        //current points to 0
        ChangeMatchTime((int)(_currentTime));
        //hp to default
    }

    public void ChangeMatchTime(int value)
    {
        _defaultMatchTime = value;
    }

    private void StartEnemiesSpawn()
    {
        //positions to spawn
        float posX = terrain.transform.position.x;
        float posZ = terrain.transform.position.z;

        float sizeX = terrain.terrainData.size.x;
        float sizeZ = terrain.terrainData.size.z;

        //every 4s
        int i = UnityEngine.Random.Range(ObjectPoolerIDS.ARCHER, ObjectPoolerIDS.WARRIOR + 1);
        GameObject enemy = _poolerInstance.GetObject(i);
        enemy.transform.position = new Vector3(UnityEngine.Random.Range(posX, posX + sizeX), 0.5f, UnityEngine.Random.Range(posZ, posZ + sizeZ));
        enemy.transform.rotation = Quaternion.identity;
        enemy.SetActive(true);
    }

}
