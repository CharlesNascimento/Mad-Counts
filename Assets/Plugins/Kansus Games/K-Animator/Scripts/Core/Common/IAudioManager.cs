using UnityEngine;

namespace KansusAnimator
{
    /// <summary>
    /// Defines an audio manager.
    /// </summary>
    public interface IAudioManager
    {
        /// <summary>
        /// Plays the given audio clip in one shot.
        /// </summary>
        /// <param name="audioClip">The audio clip.</param>
        void PlaySoundOneShot(AudioClip audioClip);

        /// <summary>
        /// Plays the given audio clip in loop.
        /// </summary>
        /// <param name="audioClip">The audio clip.</param>
        /// <returns>The audio source used to play the sound.</returns>
        AudioSource PlaySoundLoop(AudioClip audioClip);
    }
}
