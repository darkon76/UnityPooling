using System.Collections.Generic;
using UnityEngine;

namespace ObjectPool
{
    public class PoolDictionary
    {
        private readonly Dictionary<object, dynamic> _containers = new Dictionary<object, dynamic>();
        private Transform _parentTransform;

        public PoolDictionary(Transform parentTransform)
        {
            _parentTransform = parentTransform;
        }

        public void Warm<T>(T component, int count, bool useSpawnedTemplate = true) where T: Component
        {
            if (_containers.ContainsKey(component))
            {
                return;
            }

            var poolContainer = new PoolObjectContainer<T>(component, _parentTransform, count, useSpawnedTemplate);
            _containers[component] = poolContainer;
        }

        public T Get<T>(T component) where T : Component
        {
            if (!_containers.TryGetValue(component, out var objSlot))
            {
                objSlot = new PoolObjectContainer<T>(component, _parentTransform, 1, true);
                _containers[component] = objSlot;
            }

            var poolObject = objSlot as PoolObjectContainer<T>;
            var pooledObject =  poolObject.Get();
            return pooledObject;
        }
        
    }

}
