using UnityEngine;
using UnityEngine.Audio;

namespace ObjectPool
{
    public class AudioSourcePoolHolder : MonoBehaviour
    {
        private readonly AudioSourcePool _pool = new AudioSourcePool();

        public void Awake()
        {
            _pool.Parent = transform;
        }
        public AudioSource Get(AudioMixerGroup audioMixerGroup)
        {
            return _pool.Get(audioMixerGroup);
        }

        public AudioSource Play(AudioClip audioClip, Vector3 position, Quaternion rotation, AudioMixerGroup audioMixerGroup = null)
        {
            return _pool.Play(audioClip, position, rotation, audioMixerGroup);
        }
    }
}
