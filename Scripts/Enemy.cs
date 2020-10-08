using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Animator an;

    [SerializeField]
    private int _lifeAmount;

    [SerializeField]
    private int _pointValue;

    private GameObject _player;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        an = GetComponent<Animator>();
    }

    public void LoseLifeOrDie(int amount)
    {
        _lifeAmount -= amount;
        if (_lifeAmount > 0)
        {
            an.Play("GetHit");
        }
        else
        {
            an.Play("Die");
        }
    }

    public void Die()
    {
        an.enabled = false;
        _player.GetComponent<Player>().AddPoint(_pointValue);
    }
}
