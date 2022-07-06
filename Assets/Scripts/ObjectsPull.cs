using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsPull : MonoBehaviour
{
    public static ObjectsPull main;

    [System.Serializable]
    public class Pull
    {
        [SerializeField] private GameObject _objectPrefab;
        [SerializeField] private int _size;

        private Transform _parrent;

        private List<GameObject> _objects = new List<GameObject>();

        public void Inicializate(Transform parrent)
        {
            _objects.Clear();
            _parrent = parrent;

            for (int i = 0; i < _size; i++)
            {
                _objects.Add(Instantiate(_objectPrefab, _parrent));
                _objects[i].SetActive(false);
            }
        }

        public GameObject FindFreeObject()
        {
            for (int i = 0; i < _size; i++)
            {
                if (_objects[i].activeSelf == false)
                    return _objects[i];
            }

            _objects.Add(Instantiate(_objectPrefab, _parrent));
            _size++;
            _objects[_size - 1].SetActive(false);

            return _objects[_size - 1];
        }

        public GameObject ActivateObject(Vector2 position, float rotation)
        {
            GameObject obj = FindFreeObject();
            obj.transform.position = position;
            obj.transform.eulerAngles = new Vector3(0, 0, rotation);
            obj.SetActive(true);
            return obj;
        }

        public void DeactivateAllObject()
        {
            for (int i = 0; i < _size; i++)
            {
                _objects[i].SetActive(false);
            }
        }

        public bool CheckAllObjects()
        {
            for (int i = 0; i < _size; i++)
            {
                if (_objects[i].activeSelf)
                    return false;
            }

            return true;
        }
    }

    public Pull bullets;
    public List<Pull> asteroids = new List<Pull>();
    public Pull UFO;
    public Pull UFOBullets;

    private void Awake()
    {
        main = this;
    }

    private void Start()
    {
        bullets.Inicializate(transform);
        foreach(Pull pull in asteroids)
        {
            pull.Inicializate(transform);
        }
        UFO.Inicializate(transform);
        UFOBullets.Inicializate(transform);
    }

    public bool CheckAllAscteroids()
    {
        foreach (Pull pull in asteroids)
        {
            if (pull.CheckAllObjects() == false)
                return false;
        }
        return true;
    }
    public void DeactiveAll()
    {
        bullets.DeactivateAllObject();
        foreach (Pull pull in asteroids)
        {
            pull.DeactivateAllObject();
        }
        UFO.DeactivateAllObject();
        UFOBullets.DeactivateAllObject();
    }
}
