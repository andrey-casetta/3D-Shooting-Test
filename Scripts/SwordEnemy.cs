using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordEnemy : MonoBehaviour
{
    [SerializeField]
    private GameObject _fatherPrefab;

    [SerializeField]
    private float _coolDownAttack;

    private BoxCollider _colliderComponent;
    private int _damage;

    void Start()
    {
        _damage = _fatherPrefab.GetComponent<WarriorScript>()._damage;
        _colliderComponent = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            Player player = collider.GetComponent<Player>();
            if (player != null)
            {
                player.LoseLife(_damage);
                _colliderComponent.enabled = false;
            }
        }
    }

    public void ColliderRestart()
    {
        _colliderComponent.enabled = true;
    }
}
