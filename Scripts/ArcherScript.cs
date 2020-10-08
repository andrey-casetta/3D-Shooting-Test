using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherScript : MonoBehaviour
{
    private ObjectPooler _poolerInstance;

    [SerializeField]
    private int _damage = 1;

    [SerializeField]
    private GameObject _ammoRef;

    [SerializeField]
    private float _radius = 15f;

    [SerializeField]
    private bool _isAttacking = false;

    private Animator an;

    void Start()
    {
        an = GetComponent<Animator>();
        _poolerInstance = ObjectPooler.instance;
    }

    void Update()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _radius);
        {
            foreach (Collider collider in colliders)
            {
                if (collider.gameObject.CompareTag("Player"))
                {
                    _isAttacking = true;
                    Quaternion newRot = Quaternion.LookRotation(collider.transform.position - transform.position);
                    transform.rotation = Quaternion.Lerp(transform.rotation, newRot, 10 * Time.deltaTime);
                }
                else
                {
                    _isAttacking = false;
                }
            }
        }

        an.SetBool("Attack", _isAttacking);
    }

    public void Shoot()
    {
        GameObject newAmmo = _poolerInstance.GetObject(ObjectPoolerIDS.AMMO);
        newAmmo.transform.localRotation = _ammoRef.transform.rotation;
        newAmmo.transform.localPosition = _ammoRef.transform.position;
        newAmmo.transform.localScale = _ammoRef.transform.localScale;
        newAmmo.SetActive(true);
        newAmmo.GetComponent<Rigidbody>().AddForce(transform.forward * 40, ForceMode.Impulse);
    }
}
