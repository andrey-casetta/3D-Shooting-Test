using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler instance;

    [SerializeField]
    private MyScriptableObject[] _objectsToSpawn;

    [SerializeField]
    private int _spawnAmount = 10;

    private List<List<GameObject>> _fatherList;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        _fatherList = new List<List<GameObject>>();

        int i = 0;
        while (i < _objectsToSpawn.Length)
        {
            List<GameObject> newList = new List<GameObject>();
            _fatherList.Add(newList);
            i++;
        }

        int j = 0;
        while (j < _fatherList.Count)
        {
            for (int k = 0; k < _spawnAmount; k++)
            {
                GameObject go = Instantiate(_objectsToSpawn[j]._prefab);
                go.SetActive(false);
                _fatherList[j].Add(go);
            }
            j++;
        }
    }

    public GameObject GetObject(int id)
    {
        if (id >= 0)
        {
            for (int i = 0; i < _fatherList[id].Count; i++)
            {
                if (!_fatherList[id][i].activeInHierarchy)
                {
                    return _fatherList[id][i];
                }
            }
            GameObject newObj = Instantiate(_objectsToSpawn[id]._prefab);
            newObj.SetActive(false);
            return newObj;
        }
        return null;
    }

}
