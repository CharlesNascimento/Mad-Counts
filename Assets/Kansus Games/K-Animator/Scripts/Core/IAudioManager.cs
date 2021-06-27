using UnityEngine;

namespace KansusGames.KansusAnimator.Core
{
    /// <summary>
    /// Defines an audio manager.
    /// </summary>
    public interface IAudioManager
    {
        /// <summary>
        /// Plays the given audio clip in one shot.
        /// </summary>
        /// <param name="audioClip">The audio clip to be played.</param>
        /// <param name="loop">Whether the sound should loop.</param>
        AudioSource PlaySound(AudioClip audioClip, bool loop = false);
    }
}
