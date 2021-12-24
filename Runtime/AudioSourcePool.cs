using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace ObjectPool
{
    public class AudioSourcePool
    {
        public Transform Parent;

        private readonly Dictionary<AudioMixerGroup, PoolObjectContainer<AudioSource>> _pool = new Dictionary<AudioMixerGroup, PoolObjectContainer<AudioSource>>();
        private PoolObjectContainer<AudioSource> _nullPool;

        private PoolObjectContainer<AudioSource> CreatePool(AudioMixerGroup audioMixerGroup)
        {
            var template = new GameObject("AudioTemplate");
            template.SetActive(false);
            var audioSource = template.AddComponent<AudioSource>();
            audioSource.outputAudioMixerGroup = audioMixerGroup;
            var pool = new PoolObjectContainer<AudioSource>(audioSource, Parent, 1, false);
            if (audioMixerGroup == null)
            {
                _nullPool = pool;
            }
            else
            {
                _pool[audioMixerGroup] = pool;
            }
            return pool;
        }

        public AudioSource Get(AudioMixerGroup audioMixerGroup = null)
        {
            AudioSource audioSource;

            if (audioMixerGroup == null)
            {
                _nullPool ??= CreatePool(null);
                audioSource = _nullPool.Get();
            }
            else
            {
                if (!_pool.TryGetValue(audioMixerGroup, out var pool))
                {
                    pool = CreatePool(audioMixerGroup);
                }
                audioSource = pool.Get();
            }

            var audioSourceAutoDisabler = audioSource.GetComponent<AudioSourceAutoDisabler>();
            if (audioSourceAutoDisabler == null)
            {
                audioSource.gameObject.AddComponent<AudioSourceAutoDisabler>();
            }

            return audioSource;

        }

        public AudioSource Play( AudioClip audioClip, Vector3 position, Quaternion rotation, AudioMixerGroup audioMixerGroup = null )
        {
            var audioSource = Get(audioMixerGroup);
            audioSource.transform.SetPositionAndRotation(position,rotation);
            audioSource.clip = audioClip;
            audioSource.gameObject.SetActive(true);
            return audioSource;
        }

        public sealed class Nothing
        {
            public static readonly Nothing Value = new Nothing();
            private Nothing() { }
        }

    }
}
