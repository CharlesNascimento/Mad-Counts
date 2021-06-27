using UnityEngine;

namespace KansusGames.KansusAnimator.Core
{
    /// <summary>
    /// Implements the default audio manager.
    /// </summary>
    public class AudioManager : IAudioManager
    {
        #region Fields

        private const int AudioSourcePoolMaxSize = 32;

        private static AudioManager instance;

        private GameObject audioSourcePool;

        #endregion

        #region Properties

        /// <summary>
        /// Singleton instance of this class.
        /// </summary>
        public static AudioManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AudioManager();
                }

                return instance;
            }
        }

        #endregion

        #region Initialization

        private AudioManager()
        {
            audioSourcePool = Object.FindObjectOfType<AudioListener>().gameObject;
        }

        #endregion

        #region Public

        /// <summary>
        /// Plays the given audio clip.
        /// </summary>
        /// <param name="audioClip">The audio clip to be played.</param>
        public AudioSource PlaySound(AudioClip audioClip, bool loop = false)
        {
            var audioSources = audioSourcePool.GetComponents<AudioSource>();

            var audioSource = FindAvailableAudioSource(audioSources);

            if (audioSource != null)
            {
                PlaySound(audioClip, audioSource, loop);
            }
            else if (audioSources.Length < AudioSourcePoolMaxSize)
            {
                audioSource = IncreasePoolSize();
                PlaySound(audioClip, audioSource, loop);
            }

            return audioSource;
        }

        #endregion

        #region Private methods

        private AudioSource FindAvailableAudioSource(AudioSource[] audioSources)
        {
            if (audioSources.Length == 0)
            {
                return null;
            }

            foreach (AudioSource audioSource in audioSources)
            {
                if (!audioSource.isPlaying)
                {
                    return audioSource;
                }
            }

            return null;
        }

        private AudioSource IncreasePoolSize()
        {
            var newAudioSource = audioSourcePool.gameObject.AddComponent<AudioSource>();
            newAudioSource.rolloffMode = AudioRolloffMode.Linear;
            newAudioSource.playOnAwake = false;

            return newAudioSource;
        }

        private void PlaySound(AudioClip audioClip, AudioSource audioSource, bool loop)
        {
            if (loop)
            {
                audioSource.clip = audioClip;
                audioSource.loop = true;
                audioSource.Play();
            }
            else
            {
                audioSource.PlayOneShot(audioClip);
            }

        }

        #endregion
    }
}
