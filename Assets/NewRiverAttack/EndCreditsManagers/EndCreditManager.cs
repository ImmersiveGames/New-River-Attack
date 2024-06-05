using System.Collections;
using ImmersiveGames.InputManager;
using NewRiverAttack.GameManagers;
using NewRiverAttack.StateManagers;
using UnityEngine;
using UnityEngine.Playables;

namespace NewRiverAttack.EndCreditsManagers
{
    public class EndCreditManager : MonoBehaviour
    {
        private PlayableDirector _playableDirector;

        #region Unity Methods

        private void OnEnable()
        {
            SetInitialReferences();
            InputGameManager.ActionManager.EventOnActionComplete += ExitButton;
        }
        

        private void Start()
        {
            StartCoroutine(WaitForInitialization());
        }

        private void OnDisable()
        {
            InputGameManager.ActionManager.EventOnActionComplete -= ExitButton;
        }

        #endregion

        private void SetInitialReferences()
        {
            _playableDirector = GetComponent<PlayableDirector>();
        }
        private IEnumerator WaitForInitialization()
        {
            while (!GameManager.StateManager.GetCurrentState().StateFinalization)
            {
                yield return null;
            }
            _playableDirector.Play();
        }
        private async void ExitButton()
        {
            _playableDirector.Pause();
            await GameManager.StateManager.ChangeStateAsync(StatesNames.GameStateMenuInitial.ToString()).ConfigureAwait(false);
        }
    }
}