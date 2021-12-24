using System.Collections.Generic;
using UnityEngine;

namespace ObjectPool
{
    [System.Serializable]
    public class PoolObjectContainer<T>: IPool where T : UnityEngine.Component
    {
        [SerializeField] private T _template;
        [SerializeField] private Transform _parent;

        private Stack<T> _poolStack = new Stack<T>();
        
        public PoolObjectContainer(T component, Transform parent = null, int count = 1, bool useSpawnedTemplate = true)
        {
            _parent = parent;
            count = Mathf.Max(count, 1);
            _template = component;

            if (useSpawnedTemplate)
            {
                _template = CreateObject();
                _template.name = component.name;
            }

            for (var i = 0; i < count; ++i)
            {
                _poolStack.Push(CreateObject());
            }
        }

        public T Get(Vector3 position, Quaternion orientation, bool setActive = false)
        {
            var component = Get();
            component.transform.SetPositionAndRotation(position, orientation);
            if (setActive)
            {
                component.gameObject.SetActive(true);
            }
            return component;
        }
        
        public T Get() 
        {
            while (_poolStack.Count > 0)
            {
                var component = _poolStack.Pop();
                if (!component.gameObject.activeSelf)
                    return component;
            }
            return CreateObject();
        }

        private T CreateObject()
        {
            var component = Object.Instantiate(_template, _parent, false);
            component.gameObject.SetActive(false);
            var poolObject = component.GetComponent<PoolObject>();
            if (poolObject == null)
            {
                
                poolObject = component.gameObject.AddComponent<PoolObject>();
            }
            poolObject.OriginPool = this;

            return component;
        }

        public void ReturnToPool(PoolObject poolObject)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                return;
            }
#endif
            
            _poolStack.Push(poolObject.GetComponent<T>());
        }
    }
}

