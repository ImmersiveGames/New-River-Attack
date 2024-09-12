using UnityEngine;

namespace ImmersiveGames.Utils
{
    /// <summary>
    /// Utility class for audio-related functions.
    /// </summary>
    public static class AudioUtils
    {
        /// <summary>
        /// Sets the volume of an AudioSource.
        /// </summary>
        /// <param name="source">The AudioSource to set the volume for.</param>
        /// <param name="starts">The starting volume.</param>
        /// <param name="ends">The target volume.</param>
        /// <param name="t">The interpolation value (0 to 1).</param>
        public static void SetVolume(AudioSource source, float starts, float ends, float t)
        {
            source.volume = Mathf.Lerp(starts, ends, t);
        }

        /// <summary>
        /// Sets the pitch of an AudioSource.
        /// </summary>
        /// <param name="source">The AudioSource to set the pitch for.</param>
        /// <param name="starts">The starting pitch.</param>
        /// <param name="ends">The target pitch.</param>
        /// <param name="t">The interpolation value (0 to 1).</param>
        public static void SetPitch(AudioSource source, float starts, float ends, float t)
        {
            source.pitch = Mathf.Lerp(starts, ends, t);
        }

        /*
         * SoundBase10(float normalizeNumber)
         *  - transforma um numero base de 0.0 à 1.0 em um valor de metrica para sons (dB)
         */
        public static float SoundBase10(float normalizeNumber)
        {
            return Mathf.Log10(normalizeNumber) * 20f;
        }
    }
}
