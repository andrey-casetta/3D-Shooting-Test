using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private UIManager _uiManagerInstance;
    private GameManager _gameManagerInstance;
    private ObjectPooler _objectPoolerInstance;

    [SerializeField]
    private GameObject _ammoRef;

    [SerializeField]
    private GameObject _ammoTarget;

    [SerializeField]
    private int _lifeAmount = 10;

    [SerializeField]
    private int _ammoAmount = 10;

    [SerializeField]
    private float _speed;

    private CharacterController _characterController;
    private Rigidbody rg;
    private int _currentPoints = 0;
    private float _gravity = 9.8f;
    private LookX _lookXScript;

    public int CurrentPoints
    {
        get
        {
            return _currentPoints;
        }
    }

    void Start()
    {
        _uiManagerInstance = UIManager.instance;
        _gameManagerInstance = GameManager.instance;
        _objectPoolerInstance = ObjectPooler.instance;
        _characterController = GetComponent<CharacterController>();
        _lookXScript = GetComponent<LookX>();
        rg = GetComponent<Rigidbody>();
    }


    void Update()
    {
        if (!_gameManagerInstance._gamePaused)
        {
            _lookXScript.enabled = true;
            Movement();

            if (Input.GetButtonDown("Fire1") && _ammoAmount > 0)
            {
                Shoot();
            }

            if (Input.GetButtonDown("Jump"))
            {
                Jump();
            }
        }
        else
        {
            _lookXScript.enabled = false;
        }
    }

    private void Jump()
    {
        rg.AddForce(200, 200, 0);
    }

    private void Shoot()
    {
        GameObject newAmmo = _objectPoolerInstance.GetObject(ObjectPoolerIDS.AMMO);
        newAmmo.transform.rotation = _ammoRef.transform.rotation;
        newAmmo.transform.position = _ammoRef.transform.position;
        newAmmo.SetActive(true);
        newAmmo.GetComponent<Rigidbody>().AddForce(-newAmmo.transform.up * 30, ForceMode.Impulse);

        _ammoAmount--;
        _uiManagerInstance.UpdateAmmo(_ammoAmount);
        //Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        //RaycastHit hit;

        //if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        //{
        //    if (hit.collider.gameObject.layer.Equals(9))
        //    {
        //        //enemy loses life
        //    }
        //}
    }

    public void LoseLife(int amount)
    {
        _lifeAmount -= amount;
        _uiManagerInstance.UpdateLife(_lifeAmount);

        if (_lifeAmount <= 0)
        {
            _gameManagerInstance.GameOver(CurrentPoints);
        }
    }

    private void Movement()
    {
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        Vector3 pMove = new Vector3(horizontal, 0, vertical);
        Vector3 velocity = pMove * _speed;

        velocity.y -= _gravity;
        velocity = transform.TransformDirection(velocity);

        _characterController.Move(velocity * Time.deltaTime);
    }

    public void AddPoint(int points)
    {
        _currentPoints += points;
    }
}
