using System.Collections;
using ImmersiveGames;
using NewRiverAttack.StateManagers.States;
using UnityEngine;

namespace NewRiverAttack.Test
{
    public class AudioManagerTest : MonoBehaviour
    {

        private void Start()
        {
            StartCoroutine(TestMusicTransition());
        }

        private IEnumerator TestMusicTransition()
        {
            // Play initial state
            AudioManager.PlayBGM(new GameStateMenuInitial());

            // Wait for a few seconds (simulating gameplay or time passing)
            yield return new WaitForSeconds(5f);

            // Play a different state (simulating a change in game state)
            AudioManager.PlayBGM(new GameStatePlay());
        }

        // Replace YourInitialState and YourNextState with actual classes that implement IState

    }
}