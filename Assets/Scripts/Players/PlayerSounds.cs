using System.Collections;
using UnityEngine;

namespace RiverAttack
{
    [RequireComponent(typeof(PlayerMaster))]
[RequireComponent(typeof(AudioSource))]
public class PlayerSounds : MonoBehaviour
{
    #region Variable Private Inspector
    [SerializeField]
    private AudioEventSample playerEngineAudio;
    [SerializeField]
    private AudioEventSample playerStartAccelEngineAudio;
    [SerializeField]
    private AudioEventSample playerAceceEngineAudio;
    [SerializeField]
    private AudioEventSample playerDeaceceEngineAudio;
    [SerializeField]
    [Range(.1f, 3)]
    private float enginePitchDown = .5f;
    [SerializeField]
    private AudioEventSample playerExplosion;
    #endregion

    #region Variable Private References
    private PlayerMaster playerMaster;
    private AudioSource audioSource;
    private GamePlayManager gamePlay;
    #endregion

    private void OnEnable()
    {
        SetInitialReferences();
        playerMaster.EventControllerMovement += SoundEngine;
        playerMaster.EventPlayerDestroy += SoundExplosion;
        gamePlay.EventPausePlayGame += SoundStop;
        gamePlay.EventCompletePath += SoundStop;
    }

    private void SetInitialReferences()
    {
        playerMaster = GetComponent<PlayerMaster>();
        audioSource = GetComponent<AudioSource>();
        gamePlay = GamePlayManager.instance;
    }

    private void SoundEngine(Vector2 dir)
    {
        if (playerEngineAudio != null && playerMaster.shouldPlayerBeReady)
        {
            if (dir.y > 0 && playerMaster.playerStatus != PlayerMaster.Status.Accelerate)
            {
                playerMaster.playerStatus = PlayerMaster.Status.Accelerate;
                StartCoroutine(ChangeEngine(playerStartAccelEngineAudio, playerAceceEngineAudio));
            }
            if (dir.y < 0 && playerMaster.playerStatus != PlayerMaster.Status.Reduce)
            {
                playerMaster.playerStatus = PlayerMaster.Status.Reduce;
                AudioEventSample.UpdateChangePith(audioSource, playerEngineAudio.audioSample.pitch.maxValue, enginePitchDown);
            }
            else if (dir.y == 0 && playerMaster.playerStatus != PlayerMaster.Status.None)
            {
                if (playerMaster.playerStatus == PlayerMaster.Status.Accelerate)
                    StartCoroutine(ChangeEngine(playerDeaceceEngineAudio, playerEngineAudio));
                if (playerMaster.playerStatus == PlayerMaster.Status.Reduce)
                    AudioEventSample.UpdateChangePith(audioSource, audioSource.pitch, playerEngineAudio.audioSample.pitch.maxValue);
                playerMaster.playerStatus = PlayerMaster.Status.None;
            }
            else if (playerEngineAudio != null && playerMaster.shouldPlayerBeReady && !audioSource.isPlaying && playerMaster.playerStatus == PlayerMaster.Status.None)
            {
                //StopAllCoroutines();
                playerEngineAudio.Play(audioSource);
            }
        }
    }

    private IEnumerator ChangeEngine(AudioEventSample audioStart, AudioEventSample audiofix)
    {
        audioStart.Play(audioSource);
        while (audioSource.isPlaying)
        {
            yield return null;
        }
        audiofix.Play(audioSource);
    }

    private void SoundExplosion()
    {
        if (playerExplosion != null)
        {
            StopAllCoroutines();
            playerExplosion.Play(audioSource);
        }
    }

    private void SoundStop()
    {
        StopAllCoroutines();
        audioSource.Stop();
    }

    private void OnDisable()
    {
        playerMaster.EventControllerMovement -= SoundEngine;
        playerMaster.EventPlayerDestroy -= SoundExplosion;
        gamePlay.EventPausePlayGame -= SoundStop;
    }
}
}
