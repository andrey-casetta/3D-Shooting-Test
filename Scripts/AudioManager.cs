using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    private GameManager _gameManagerInstance;
    private UIManager _uiManagerInstance;

    private AudioSource _audioSource;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _gameManagerInstance = GameManager.instance;
        _uiManagerInstance = UIManager.instance;

        _audioSource = GetComponent<AudioSource>();
    }

    public void EnableOrDisableSoundFX()
    {
        _audioSource.mute = !_audioSource.mute;
    }

    public void ChangeVolume(float value)
    {
        _audioSource.volume = value;
    }
}
