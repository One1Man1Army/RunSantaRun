using System.Collections.Generic;
using UnityEngine;

namespace RSR.InternalLogic
{
    public sealed class ObjectsPool<T> where T : MonoBehaviour
    {
        private readonly Queue<T> _pool = new();
        private readonly List<T> _activeObjects = new();

        private readonly T _prefab;
        private readonly int _startSize;
        private readonly int _maxSize;

        private Transform _root;

        public ObjectsPool(T prefab, int startSize, int maxSize, string rootName)
        {
            _prefab = prefab;
            _startSize = startSize;
            _maxSize = maxSize;

            Initialize(rootName);
        }

        public T Get(Vector3 pos)
        {
            if (_pool.Count == 0)
                AddObjectToPool();

            var obj = _pool.Dequeue();
            obj.transform.position = pos;
            obj.gameObject.SetActive(true);
            _activeObjects.Add(obj);
            return obj;
        }

        public void Release(T obj)
        {
            if (_activeObjects.Contains(obj))
                _activeObjects.Remove(obj);

            if (_pool.Count >= _maxSize)
            {
                Debug.Log($"Pool is full! Max size is {_maxSize}. Destroying {obj.name}");
                Object.Destroy(obj.gameObject);
                return;
            }

            if (_pool.Contains(obj))
            {
                Debug.Log($"Pooling {obj.name} failed! Already pooled.");
                return;
            }

            _pool.Enqueue(obj);
            obj.gameObject.SetActive(false);
        }

        public void ReleaseAll()
        {
            foreach (var obj in _activeObjects)
            {
                _pool.Enqueue(obj);
                obj.gameObject.SetActive(false);
            }

            _activeObjects.Clear();
        }

        private void Initialize(string rootName)
        {
            _root = new GameObject(rootName).transform;

            for (int i = 0; i < _startSize; i++)
            {
                AddObjectToPool();
            }
        }

        private void AddObjectToPool()
        {
            Release(Object.Instantiate(_prefab, _root));
        }
    }
}
