using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WarriorScript : MonoBehaviour
{
    [SerializeField]
    private float _radius;

    [SerializeField]
    public int _damage;

    private GameManager _gameManagerInstance;

    public GameObject _sword;

    private Rigidbody rg;
    private bool _isRunning;
    private bool _isAttacking;
    private NavMeshAgent _nav;
    private Animator _an;

    void Start()
    {
        _nav = GetComponent<NavMeshAgent>();
        _an = GetComponent<Animator>();
        _gameManagerInstance = GameManager.instance;
        rg = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (!_gameManagerInstance._gamePaused)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, _radius);
            {
                foreach (Collider collider in colliders)
                {
                    if (collider.gameObject.CompareTag("Player"))
                    {
                        Quaternion newRot = Quaternion.LookRotation(collider.transform.position - transform.position);
                        transform.rotation = Quaternion.Lerp(transform.rotation, newRot, 10 * Time.deltaTime);
                        if (Vector3.Distance(transform.position, collider.transform.position) < _radius - 2f)
                        {
                            _nav.SetDestination(collider.transform.position);
                            if (Vector3.Distance(transform.position, collider.transform.position) > _nav.stoppingDistance)
                            {
                                _isRunning = true;
                                _nav.isStopped = false;
                                _isAttacking = false;
                            }
                            else
                            {
                                _nav.isStopped = true;
                                _isAttacking = true;
                            }
                        }
                        else
                        {
                            _isRunning = false;
                            _nav.SetDestination(transform.position);
                        }
                    }
                }
            }

            _an.SetBool("Attack", _isAttacking);
            _an.SetBool("Run", _isRunning);
        }
    }
}
