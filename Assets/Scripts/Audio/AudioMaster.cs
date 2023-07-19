using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

namespace RiverAttack
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioMaster : MonoBehaviour
    {
        [SerializeField]
        public AudioEventClip startBGM;
        [SerializeField]
        public AudioEventClip loopingBGM;
        [SerializeField]
        public AudioEventClip endBGM;
        [SerializeField]
        public AudioMixerGroup audioMixerGroup;
        [SerializeField]
        public bool playOnEnable = false;

        AudioSource m_AudioSource;

        #region UnityMethods
        void OnEnable()
        {
            m_AudioSource = GetComponent<AudioSource>();
            if (audioMixerGroup != null) m_AudioSource.outputAudioMixerGroup = audioMixerGroup;
        }

        void Start()
        {
            if (playOnEnable)
                PlayBGM();
        }
        //private void OnDisable()
        //{ 
        //    if(playOnEnable)
        //    StopBGM();
        //}
  #endregion

        void PlayBGM()
        {
            if (m_AudioSource && loopingBGM != null)
            {
                StartCoroutine(StartPlay(m_AudioSource, startBGM, loopingBGM));
            }
        }

        public void StopBGM()
        {
            if (m_AudioSource && endBGM != null)
            {
                StartCoroutine(StopPlay(m_AudioSource, endBGM));
            }
        }

        static IEnumerator StartPlay(AudioSource source, AudioEventClip startEventClip, AudioEventClip loopEventClip)
        {
            var sourceClip = startEventClip.audioClip;
            if (sourceClip != null)
            {
                source.clip = sourceClip;
                source.volume = Random.Range(startEventClip.volume.x, startEventClip.volume.y);
                source.pitch = Random.Range(startEventClip.pitch.x, startEventClip.pitch.y);
                source.loop = false;
                source.Play();
                while (source.isPlaying)
                {
                    yield return null;
                }
            }
            source.clip = loopEventClip.audioClip;
            source.volume = Random.Range(loopEventClip.volume.x, loopEventClip.volume.y);
            source.pitch = Random.Range(loopEventClip.pitch.x, loopEventClip.pitch.y);
            source.loop = loopEventClip.loop;
            source.Play();
            yield return null;
        }

        static IEnumerator StopPlay(AudioSource source, AudioEventClip endEventClip)
        {
            source.clip = endEventClip.audioClip;
            source.volume = Random.Range(endEventClip.volume.x, endEventClip.volume.y);
            source.pitch = Random.Range(endEventClip.pitch.x, endEventClip.pitch.y);
            source.loop = false;
            source.Play();
            while (source.isPlaying)
            {
                yield return null;
            }
            source.Stop();
        }
    }

}
