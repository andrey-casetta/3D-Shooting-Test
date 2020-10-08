using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class AmmoScript : MonoBehaviour
{
    [SerializeField]
    private int _damage = 1;

    private Rigidbody rg;

    void Start()
    {
        rg = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            Player player = collider.GetComponent<Player>();
            if (player != null)
            {
                player.LoseLife(_damage);
                gameObject.SetActive(false);
            }
        }

        if (collider.CompareTag("Enemy"))
        {
            Enemy Enemy = collider.GetComponent<Enemy>();
            if (Enemy != null)
            {
                Enemy.LoseLifeOrDie(_damage);
                gameObject.SetActive(false);
            }
        }

        if (collider.CompareTag("Terrain"))
        {
            gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        StartCoroutine(Deactivate());
    }

    private IEnumerator Deactivate()
    {
        yield return new WaitForSeconds(2);
        transform.position = Vector3.zero;
        rg.velocity = Vector3.zero;
        gameObject.SetActive(false);
    }
}
