using UnityEngine;

namespace ObjectPool
{
    public class PoolObject : MonoBehaviour
    {
        public IPool OriginPool;

        //When the object is disabled return it to the pool.
        private void OnDisable()
        {
            OriginPool?.ReturnToPool(this);
        }
    }
}
