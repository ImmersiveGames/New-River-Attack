using System.Collections;
using ImmersiveGames.StateManager.States;
using UnityEngine;

namespace ImmersiveGames.Test
{
    public class AudioManagerTest : MonoBehaviour
    {
        private AudioManager _audioManager;

        private void Start()
        {
            _audioManager = AudioManager.instance;
            StartCoroutine(TestMusicTransition());
        }

        private IEnumerator TestMusicTransition()
        {
            // Play initial state
            _audioManager.PlayBGM(new GameStateMenuInicial());

            // Wait for a few seconds (simulating gameplay or time passing)
            yield return new WaitForSeconds(5f);

            // Play a different state (simulating a change in game state)
            _audioManager.PlayBGM(new GameStatePlay());
        }

        // Replace YourInitialState and YourNextState with actual classes that implement IState

    }
}