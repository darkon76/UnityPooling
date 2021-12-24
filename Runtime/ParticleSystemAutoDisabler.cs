using System.Collections;
using UnityEngine;

namespace ObjectPool
{
    public class ParticleSystemAutoDisabler : MonoBehaviour
    {
        private ParticleSystem _particles;
        public bool UseParticleSystemDuration = true;
        
        void Awake()
        {
            _particles = GetComponentInChildren<ParticleSystem>();
        }

        void OnEnable()
        {
            var coroutine = UseParticleSystemDuration ? WaitDuration() : WaitUntilIsDead();
            StartCoroutine(coroutine);
        }

        private IEnumerator WaitUntilIsDead()
        {
            while (true)
            {
                if (!_particles.IsAlive(true))
                {
                    gameObject.SetActive(false);
                    yield break;
                }

                yield return null;
            }
        }

        private IEnumerator WaitDuration()
        {
            var duration = _particles.main.duration;
            yield return new WaitForSeconds(duration);
            //WaitUntil every particle is dead.
            yield return WaitUntilIsDead();
            gameObject.SetActive(false);
        }
    }
}
