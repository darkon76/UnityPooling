using System.Collections;
using UnityEngine;

namespace ObjectPool.Component
{
    public class AudioSourceAutoDisabler : MonoBehaviour
    {
        private AudioSource _audioSource;
        public bool UseAudioClipDuration = true;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        private void OnEnable()
        {
            var routine = UseAudioClipDuration ? WaitClipDuration() : WaitUntilFinishPlaying();
            StartCoroutine(routine);
        }

        IEnumerator WaitUntilFinishPlaying()
        {
            while (true)
            {
                if (!_audioSource.isPlaying)
                {
                    gameObject.SetActive(false);
                    break;
                }

                yield return null;
            }
        }

        IEnumerator WaitClipDuration()
        {
            var duration = _audioSource.clip.length;
            yield return new WaitForSeconds(duration);
            gameObject.SetActive(false);
        }
    }
}
