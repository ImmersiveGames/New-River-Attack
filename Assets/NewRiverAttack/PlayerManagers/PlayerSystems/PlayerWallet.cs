using ImmersiveGames.ObjectManagers.Interfaces;
using NewRiverAttack.GamePlayManagers;
using NewRiverAttack.ObstaclesSystems;
using NewRiverAttack.ObstaclesSystems.CollectibleSystems;
using NewRiverAttack.SaveManagers;
using UnityEngine;

namespace NewRiverAttack.PlayerManagers.PlayerSystems
{
    public class PlayerWallet : MonoBehaviour
    {
        [SerializeField] private int playerWallet;

        private PlayerMaster _playerMaster;
        private GamePlayManager _gamePlayManager;

        #region Unity Methods

        private void OnEnable()
        {
            SetInitialReferences();
            playerWallet = GameOptionsSave.instance.wallet;
            _playerMaster.EventPlayerMasterCollect += UpdateWallet;
        }

        private void OnDisable()
        {
            _playerMaster.EventPlayerMasterCollect -= UpdateWallet;
            GameOptionsSave.instance.wallet = playerWallet;
        }
        #endregion
        
        private void SetInitialReferences()
        {
            _playerMaster = GetComponent<PlayerMaster>();
            _gamePlayManager = GamePlayManager.instance;
        }
        
        private void UpdateWallet(ICollectable collectable)
        {
            var collected = collectable as CollectibleMaster;
            if (collected == null) return;
            var collectedData = collected.GetCollectibleSettings;
            if (collectedData.obstacleTypes != ObstacleTypes.Refugee) return;
            
            var walletValue = collectedData.collectValuable * collectedData.amountCollectables; 
            if( collectedData.maxCollectible != 0 && playerWallet >= collectedData.maxCollectible ) return;
            playerWallet += walletValue;
            _gamePlayManager.OnEventHudRefugiesUpdate(playerWallet, _playerMaster.PlayerIndex); 
        }
    }
}