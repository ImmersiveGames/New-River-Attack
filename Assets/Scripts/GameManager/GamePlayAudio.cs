using System.Collections;
using UnityEngine;
using Utils;
namespace RiverAttack
{
    public class GamePlayAudio : Singleton<GamePlayAudio>
    {
        [SerializeField] AudioSource bgmAudioSource;
        [SerializeField] Tools.SerializableDictionary<LevelTypes, AudioEventSample> bgmLevels = new Tools.SerializableDictionary<LevelTypes, AudioEventSample>();
        [Header("Menu SFX")]
        [SerializeField] AudioClip clickSound;

        #region UNITYMETHODS
        void Awake()
        {
            bgmAudioSource = GetComponentInParent<AudioSource>();
        }
  #endregion
        public void PlayBGM(LevelTypes typeLevel)
        {
            bgmLevels.TryGetValue(typeLevel, out var audioSource);
            audioSource.Play(bgmAudioSource);
        }
        IEnumerator PlayBGM(AudioSource source, AudioEvent track, float time)
        {
            if (source.isPlaying)
                yield return StartCoroutine(FadeAudio(source, time, source.volume, 0));
            track.Play(source);
        }
        public void ChangeBGM(LevelTypes typeLevel, float time)
        {
            bgmLevels.TryGetValue(typeLevel, out var audioSource);
            StartCoroutine(PlayBGM(bgmAudioSource, audioSource, time));
        }
        static void PlayOneShot(AudioSource audioSource, AudioClip audioClip)
        {
            audioSource.PlayOneShot(audioClip);
        }
        public void PlayClickSfx(AudioSource audioSource)
        {
            PlayOneShot(audioSource, clickSound);
        }
        public void StopBGM()
        {
            if (bgmAudioSource != null)
                bgmAudioSource.Stop();
        }
        static IEnumerator FadeAudio(AudioSource source, float timer, float starts, float ends)
        {
            float i = 0.0F;
            float step = 1.0F / timer;
            while (i <= 1.0F)
            {
                i += step * Time.deltaTime;
                source.volume = Mathf.Lerp(starts, ends, i);
                yield return new WaitForSeconds(step * Time.deltaTime);
            }
            if (ends <= 0)
                source.Stop();
        }
    }
}
