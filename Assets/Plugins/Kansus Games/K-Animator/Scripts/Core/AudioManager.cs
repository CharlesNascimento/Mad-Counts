using UnityEngine;

namespace KansusAnimator
{
    /// <summary>
    /// Implements the default audio manager.
    /// </summary>
    public class AudioManager : IAudioManager
    {
        #region Fields

        private static AudioManager instance;

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
                    instance = new AudioManager();

                return instance;
            }
        }

        #endregion

        #region Public

        /// <summary>
        /// Plays the given audio clip in one shot.
        /// </summary>
        /// <param name="audioClip">The audio clip.</param>
        public void PlaySoundOneShot(AudioClip audioClip)
        {
            AudioListener listener = Object.FindObjectOfType<AudioListener>();

            if (listener != null)
            {
                bool played = false;
                AudioSource[] sources = listener.gameObject.GetComponents<AudioSource>();

                if (sources.Length != 0)
                {
                    for (int i = 0; i < sources.Length; i++)
                    {
                        if (!sources[i].isPlaying)
                        {
                            played = true;
                            sources[i].PlayOneShot(audioClip);
                            break;
                        }
                    }
                }

                if (!played && sources.Length < 32)
                {
                    AudioSource source = listener.gameObject.AddComponent<AudioSource>();
                    source.rolloffMode = AudioRolloffMode.Linear;
                    source.playOnAwake = false;
                    source.PlayOneShot(audioClip);
                }
            }
        }

        /// <summary>
        /// Plays the given audio clip in loop.
        /// </summary>
        /// <param name="audioClip">The audio clip.</param>
        /// <returns>The audio source used to play the sound.</returns>
        public AudioSource PlaySoundLoop(AudioClip audioClip)
        {
            AudioSource source = null;
            AudioListener listener = Object.FindObjectOfType<AudioListener>();

            if (listener != null)
            {
                bool played = false;
                AudioSource[] sources = listener.gameObject.GetComponents<AudioSource>();

                if (sources.Length != 0)
                {
                    for (int i = 0; i < sources.Length; i++)
                    {
                        if (!sources[i].isPlaying)
                        {
                            played = true;
                            source = sources[i];
                            source.clip = audioClip;
                            source.loop = true;
                            source.Play();
                            break;
                        }
                    }
                }

                if (!played && sources.Length < 16)
                {
                    source = listener.gameObject.AddComponent<AudioSource>();
                    source.rolloffMode = AudioRolloffMode.Linear;
                    source.playOnAwake = false;
                    source.clip = audioClip;
                    source.loop = true;
                    source.Play();
                }
            }

            return source;
        }

        #endregion
    }
}
