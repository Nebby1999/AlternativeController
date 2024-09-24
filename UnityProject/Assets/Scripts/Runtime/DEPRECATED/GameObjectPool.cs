
using System;
using System.Collections.Generic;
using UnityEngine;

namespace AC
{

    [Obsolete]
    [CreateAssetMenu(menuName = "Pool")]
    public class GameObjectPool : ScriptableObject
    {
        [SerializeField] private GameObject _prefab;
        [SerializeField] private Transform _poolTransform;

        [SerializeField] private List<GameObject> _objectsInPool;
        [SerializeField] private List<GameObject> _objectsInUse;

        public GameObject SpawnObject(Vector3 position, Quaternion rotation)
        {
            GameObject currentObject;

            if(_objectsInPool.Count <= 0)
            {
                GameObject gameObject = Instantiate(_prefab);
                currentObject = gameObject;
            }
            else
            {
                currentObject = _objectsInPool[0];
                _objectsInPool.Remove(currentObject);
            }
            _objectsInUse.Add(currentObject);

            currentObject.SetActive(true);
            currentObject.transform.position = position;
            currentObject.transform.rotation = rotation;
            currentObject.transform.parent = GetParent();

            return currentObject;
        }
        private Transform GetParent()
        {
            if(!_poolTransform) 
            {
                Transform newPoolTransform = Instantiate(new GameObject()).transform;
                newPoolTransform.name = "Pool";
                _poolTransform = newPoolTransform;
                return newPoolTransform;
            }
            return _poolTransform;
        }
        public void ReturnToPool(GameObject objectToReturn)
        {
            if (_objectsInPool.Contains(objectToReturn)) return;
            if (_objectsInUse.Contains(objectToReturn))
            {
                objectToReturn.SetActive(false);
                _objectsInUse.Remove(objectToReturn);
                _objectsInPool.Add(objectToReturn);
                Debug.Log(objectToReturn + " has returned to his pool.");
            }
            else Debug.Log("Object not in Pool " + objectToReturn);
        }

        public void RemoveObject(GameObject objectToRemove)
        {
            _objectsInPool.Remove(objectToRemove);
            _objectsInUse.Remove(objectToRemove);
            Debug.Log(objectToRemove + " has been removed.");
        }
    }
}
