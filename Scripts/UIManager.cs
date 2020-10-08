using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityScript.Steps;
using System.Globalization;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    private GameManager _gameManagerInstance;
    private MySceneManager _sceneManagerInstance;
    private AudioManager _audioManagerInstance;

    //main menu scene UI components
    #region
    private GameObject _loadingScreen;
    private GameObject _settingsPanel;
    private GameObject _playButton;
    private GameObject _enableSettingsButton;
    private GameObject _backSettingsButton;
    private GameObject _confirmSettingsButton;
    private GameObject _soundFXToggle;
    private GameObject _volumeSlider;
    private GameObject _timerInputField;
    #endregion

    //level 1 scene UI components
    #region
    private GameObject _timerText;
    private GameObject _exitButton;
    private GameObject _continueButton;
    private GameObject _pauseMenu;
    private GameObject _pauseButton;
    private GameObject _gameOverMenu;
    private GameObject _restartButton;
    private GameObject _gameOverExitButton;
    private GameObject _ammoAmountText;
    private GameObject _lifeAmountText;

    private Text _timerTextComponent;
    private Text _ammoTextComponent;
    private Text _lifeTextComponent;

    #endregion


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
        SceneManager.sceneLoaded += LoadListeners;

    }
    private void LoadListeners(Scene arg0, LoadSceneMode arg1)
    {
        //if main_menu do this
        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 0:
                AddListenersMainMenu();
                break;
            case 1:
                AddListenersLevelOne();
                break;
        }
    }

    private void Start()
    {
        _sceneManagerInstance = MySceneManager.instance;
        _gameManagerInstance = GameManager.instance;
        _audioManagerInstance = AudioManager.instance;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OpenPauseMenu();
        }
    }


    public void AddListenersMainMenu()
    {
        _loadingScreen = GameObject.Find("Loading_Screen");
        _settingsPanel = GameObject.Find("Settings_Panel");
        _playButton = GameObject.Find("Play_Button");
        _backSettingsButton = GameObject.Find("Back_Button");
        _enableSettingsButton = GameObject.Find("Settings_Button");
        _confirmSettingsButton = GameObject.Find("Confirm_Button");
        _soundFXToggle = GameObject.Find("SFX_Toggle");
        _volumeSlider = GameObject.Find("Volume_Slider");
        _timerInputField = GameObject.Find("Time_Input_Field");

        if (_playButton)
        {
            _playButton.GetComponent<Button>().onClick.AddListener(() => StartFadeToLevelOne());
        }
        if (_enableSettingsButton)
            _enableSettingsButton.GetComponent<Button>().onClick.AddListener(() => OpenSettings());

        if (_backSettingsButton)
            _backSettingsButton.GetComponent<Button>().onClick.AddListener(() => CloseSettings());

        if (_soundFXToggle)
            _soundFXToggle.GetComponent<Toggle>().onValueChanged.AddListener((value) => { ChangeSoundFX(); });

        if (_volumeSlider)
            _volumeSlider.GetComponent<Slider>().onValueChanged.AddListener((value) => { ChangeVolume(value); });

        if (_timerInputField)
        {
            _timerInputField.GetComponent<InputField>().text = "180";
            _timerInputField.GetComponent<InputField>().onValueChanged.AddListener((value) => { ChangeGameTime(value); });
        }
    }

    public void AddListenersLevelOne()
    {
        _timerText = GameObject.Find("Timer_Text");
        _ammoAmountText = GameObject.Find("Ammo_Text");
        _lifeAmountText = GameObject.Find("Life_Text");

        _continueButton = GameObject.Find("Continue_Button");
        _exitButton = GameObject.Find("Exit_Button");
        _pauseMenu = GameObject.Find("Pause_Menu");
        _pauseButton = GameObject.Find("Pause_Button");
        _gameOverMenu = GameObject.Find("Game_Over_Menu");
        _restartButton = GameObject.Find("Restart_Button");
        _gameOverExitButton = GameObject.Find("GameOver_Exit_Button");

        if (_timerText)
            _timerTextComponent = _timerText.GetComponent<Text>();

        if (_lifeAmountText)
            _lifeTextComponent = _lifeAmountText.GetComponent<Text>();
       
        if (_ammoAmountText)
            _ammoTextComponent = _ammoAmountText.GetComponent<Text>();

        if (_continueButton)
            _continueButton.GetComponent<Button>().onClick.AddListener(() => ContinueGame());

        if (_exitButton)
            _exitButton.GetComponent<Button>().onClick.AddListener(() => BackToMainMenu());

        if (_gameOverExitButton)
            _gameOverExitButton.GetComponent<Button>().onClick.AddListener(() => BackToMainMenu());

        if (_pauseButton)
            _pauseButton.GetComponent<Button>().onClick.AddListener(() => OpenPauseMenu());

        if (_restartButton)
            _restartButton.GetComponent<Button>().onClick.AddListener(() => RestartGame());

    }

    public void UpdateAmmo(float currentAmmo)
    {
        _ammoTextComponent.text = currentAmmo.ToString();
    }

    public void UpdateLife(float currentLife)
    {
        _lifeTextComponent.text = currentLife.ToString();
    }

    public void UpdateTime(float currentTime)
    {
        _timerTextComponent.text = currentTime.ToString();
    }

    private void OpenSettings()
    {
        _settingsPanel.GetComponent<Animator>().Play("OpenMenuAnim");
    }

    private void CloseSettings()
    {
        _settingsPanel.GetComponent<Animator>().Play("CloseMenuAnim");
    }

    private void OpenPauseMenu()
    {
        _pauseMenu.GetComponent<Animator>().Play("OpenPauseMenu");
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        _gameManagerInstance.PauseGame();
    }

    private void ClosePauseMenu()
    {
        _pauseMenu.GetComponent<Animator>().Play("ClosePauseMenu");
    }

    private void ChangeGameTime(string value)
    {
        int fValue = Int16.Parse(value);
        _gameManagerInstance.ChangeMatchTime(fValue);
    }

    private void ChangeVolume(float value)
    {
        _audioManagerInstance.ChangeVolume(value);
    }

    private void ChangeSoundFX()
    {
        _audioManagerInstance.EnableOrDisableSoundFX();
    }

    private void StartFadeToLevelOne()
    {
        //start and wait for fade, then load level
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        string timeValue = _timerInputField.GetComponent<InputField>().text;
        ChangeGameTime(timeValue);

        _loadingScreen.GetComponent<Image>().enabled = true;
        _loadingScreen.GetComponent<Animator>().Play("FadeInTransition");
        StartCoroutine(LoadLevelOne());
    }

    private void BackToMainMenu()
    {
        _gameManagerInstance._gameStarted = false;
        _sceneManagerInstance.BackToMainMenu();
    }

    private void RestartGame()
    {
        _gameOverMenu.GetComponent<Animator>().Play("CloseGameOverMenu");
        _gameManagerInstance.RestartGame();
    }

    public void CallGameOver()
    {
        _gameOverMenu.GetComponent<Animator>().Play("OpenGameOverMenu");
    }

    private void ContinueGame()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        ClosePauseMenu();
        _gameManagerInstance.ResumeGame();
    }

    private IEnumerator LoadLevelOne()
    {
        yield return new WaitForSeconds(1.7f);
        _sceneManagerInstance.LoadLevelOne();
        _gameManagerInstance.RestartGame();
    }
}
