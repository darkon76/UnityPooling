using UnityEngine;

namespace ObjectPool.Component
{
    public class PoolDictionaryHolder : MonoBehaviour
    {
        private PoolDictionary _poolDictionary;

        public void Warm<T>(T component, int count, bool useTemplate = true) where T : UnityEngine.Component
        {
            _poolDictionary ??= new PoolDictionary(transform);
            _poolDictionary.Warm(component, count, useTemplate);
        }

        public T Get<T>(T component) where T : MonoBehaviour
        {
            _poolDictionary ??= new PoolDictionary(transform);
            return _poolDictionary.Get(component);
        }
    }

}
