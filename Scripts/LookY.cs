using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookY : MonoBehaviour
{
    private float _mouseY;
    public float _minRot = 20f;
    public float _maxRot = 30f;

    [SerializeField]
    private float _sensitivity = 5f;

    private GameManager _gameManagerInstance;

    private void Start()
    {
        _gameManagerInstance = GameManager.instance;
    }

    void Update()
    {
        if (!_gameManagerInstance._gamePaused)
        {
            _mouseY = Input.GetAxis("Mouse Y");
            Vector3 newRotation = transform.localEulerAngles;

            newRotation.x += -_mouseY * _sensitivity;
            newRotation.x = Mathf.Clamp(newRotation.x <= 180 ? newRotation.x : -(360 - newRotation.x), -_minRot, _maxRot);

            transform.localEulerAngles = newRotation;
        }
    }
}
