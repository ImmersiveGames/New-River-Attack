using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

namespace RiverAttack 
{
    public class AudioMenuOptions : MonoBehaviour
    {
        [Header("Audio Options References")]
        [SerializeField] AudioMixer mixerGroup;
        [SerializeField] Slider musicVolumeSlider;
        [SerializeField] Slider sfxVolumeSlider;

        [Header("Menu SFX")]
        [SerializeField] AudioClip clickSound;
        [SerializeField] AudioSource audioSource;

        // Start is called before the first frame update
        void Start()
        {
            SetMusicVolume();
            SetSFXVolume();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetMusicVolume() 
        {
            float volume = Mathf.Log10(musicVolumeSlider.value) * 20f;
            
            mixerGroup.SetFloat("MusicVolume", volume);

            //Debug.Log("Volume da Musica: " + volume.ToString());
        }

        public void SetSFXVolume()
        {
            float volume = Mathf.Log10(sfxVolumeSlider.value) * 20f;

            mixerGroup.SetFloat("SFXVolume", volume);

            Debug.Log("Volume de SFX: " + volume.ToString());
        }

        public void PlayClickSFX() 
        {
            audioSource.PlayOneShot(clickSound);
        }

    }
}


