using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Utils;
namespace RiverAttack
{
    public class GamePlayAudio : Singleton<GamePlayAudio>
    {
        public AudioMixerSnapshot[] audioMixerSnapshots;
        public enum LevelType { Grass = 4, Forest = 2, Antique = 0, Desert = 1, Ice = 3, Swamp = 7, Hub = 5, MainTheme = 6 }
        public LevelType levelType;
        [SerializeField]
        AudioSource audioSource;
        [SerializeField]
        List<AudioEventSample> listBGM;
        bool m_InFadeIn, m_InFadeOut;

        #region UNITYMETHODS
        void Update()
        {
            if (Time.timeScale <= 0)
            {
                PauseBGM();
            }
            else
            {
                UnPauseBGM();
            }
        }
        protected override void OnDestroy() { }
  #endregion

        void PauseBGM()
        {
            if (audioMixerSnapshots.Length > 1)
                audioMixerSnapshots[1].TransitionTo(0);
        }
        void UnPauseBGM()
        {
            if (audioMixerSnapshots.Length > 0)
                audioMixerSnapshots[0].TransitionTo(0);
        }
        public void PlayBGM(LevelType typeLevel)
        {
            int i = (int)typeLevel;
            listBGM[i].Play(audioSource);
        }

        public void ChangeBGM(LevelType typeLevel, float time)
        {
            int i = (int)typeLevel;
            //Debug.Log("Troca");
            StartCoroutine(PlayBGM(audioSource, listBGM[i], time));
        }

        public void ChangeBGM(AudioEventSample track, float time)
        {
            StartCoroutine(PlayBGM(audioSource, track, time));
        }

        IEnumerator PlayBGM(AudioSource source, AudioEventSample track, float time)
        {
            if (source.isPlaying)
                yield return StartCoroutine(FadeAudio(source, time, source.volume, 0));
            track.Play(source);
        }

        public void StopBGM()
        {
            if (audioSource)
                audioSource.Stop();
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
