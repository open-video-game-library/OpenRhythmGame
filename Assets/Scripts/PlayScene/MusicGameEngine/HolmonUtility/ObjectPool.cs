namespace HolmonUtility
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class ObjectPool : MonoBehaviour
    {
        [SerializeField] private GameObject _objectPrefab;
        [SerializeField] private Transform _parentTransform;
        [SerializeField] private int _poolSize = 10;

        public int _generatedObjectCount { get { return _parentTransform.childCount; } }
        private Queue<GameObject> _availableObjects = new Queue<GameObject>();

        private void Start()
        {
            for (int i = 0; i < _poolSize; i++)
            {
                try
                {
                    GameObject obj = Instantiate(_objectPrefab, _parentTransform);
                    obj.SetActive(false);
                    _availableObjects.Enqueue(obj);
                }
                catch
                {
                    Debug.LogError($"[{this.gameObject.transform.parent.name}/{this.gameObject.name}]プレファブがアタッチされていません");
                }
            }
        }

        public GameObject GetObject(bool active)
        {
            var gam = _availableObjects.Dequeue();
            gam.SetActive(active);

            if (_availableObjects.Count == 0)
            {
                GameObject obj = Instantiate(_objectPrefab, _parentTransform);
                obj.SetActive(false);
                _availableObjects.Enqueue(obj);
            }

            return gam;
        }

        public void ReturnObject(GameObject obj)
        {
            obj.SetActive(false);
            if (_generatedObjectCount > _poolSize)
            {
                Debug.LogWarning("[" + this.transform.parent.parent.gameObject.name + "/" + this.transform.parent.gameObject.name +"/"+ this.gameObject.name + "] Destroyed Object");
                Destroy(obj);
            }
            else _availableObjects.Enqueue(obj);
        }

        public GameObject GetPrefab()
        {
            //nullチェック
            if (_objectPrefab == null)
            {
                Debug.LogError("[" + this.transform.parent.parent.gameObject.name + "/" + this.transform.parent.gameObject.name + "/" + this.gameObject.name + "] プレファブがアタッチされていません");
                return null;
            }

            return _objectPrefab;
        }

        public void SetPrefab(GameObject gam)
        {
            _objectPrefab = gam;
        }

        public void ReGeneratePool()
        {
            //プールの再生成を行う
            foreach (Transform child in _parentTransform)
            {
                Destroy(child.gameObject);
            }
            _availableObjects = new Queue<GameObject>();

            for (int i = 0; i < _poolSize; i++)
            {
                try
                {
                    GameObject obj = Instantiate(_objectPrefab, _parentTransform);
                    obj.SetActive(false);
                    _availableObjects.Enqueue(obj);
                }
                catch
                {
                    Debug.LogError($"[{this.gameObject.transform.parent.name}/{this.gameObject.name}]プレファブがアタッチされていません");
                }
            }

        }
    }
}

