using NewRiverAttack.GameManagers;
using NewRiverAttack.GamePlayManagers;
using NewRiverAttack.StateManagers;
using UnityEngine;

namespace NewRiverAttack.HUBManagers
{
    public class LevelCompletionHandler : MonoBehaviour
    {
        private int _completeIndex = -1;
        private HubGameManager _hubGameManager;
        private GameManager _gameManager;
        private void Awake()
        {
            _gameManager = GameManager.instance;
            _hubGameManager = HubGameManager.Instance;
            _completeIndex = -1;
            CheckComplete();
        }

        private void Start()
        {
            if (_completeIndex < 0) return;
            //AQUI COMEÇA O DESBLOQUEIO
            Invoke(nameof(CompleteLevel), 2f);
        }

        private void CheckComplete()
        {
            if (_gameManager.CompleteIndex != _hubGameManager.SaveIndex || _gameManager.CompleteIndex < 0) return;
            _hubGameManager.ActiveHub = false;
            _completeIndex = _gameManager.CompleteIndex;
        }

        private void CompleteLevel()
        {
            _hubGameManager.OnEventExplodeBridge(_completeIndex);
            var nextIndex = _completeIndex + 1;
            if (nextIndex >= _hubGameManager.CachedHubOrderData.Count)
            {
                Invoke(nameof(SendToCompleteGame), 2f);
                return;
            }
            _hubGameManager.OnEventUpdateIndex(nextIndex);
            _hubGameManager.OnEventCursorMove(nextIndex);
            ClearActivities();
        }

        private async void SendToCompleteGame()
        {
            ClearActivities();
            await GameManager.StateManager.ChangeStateAsync(StatesNames.GameStateEndGame.ToString()).ConfigureAwait(false);
        }
        
        private void ClearActivities()
        {
            _gameManager.CompleteIndex = -1;
            _gameManager.ActiveIndex = -1;
            _hubGameManager.ActiveHub = true;
        }
    }
}
