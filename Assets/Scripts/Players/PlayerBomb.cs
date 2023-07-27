using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;
namespace RiverAttack
{
    public class PlayerBomb : MonoBehaviour, ICommand
    {
        [SerializeField]
        int bombQuantity;
        [SerializeField]
        Vector3 bombOffset;
        [SerializeField]
        LayerMask layerMask;
        [SerializeField]
        GameObject prefabBomb;
        
        PlayerMaster m_PlayerMaster;
        GamePlayManager m_GamePlayManager;
        PlayersInputActions m_PlayersInputActions;
        
        public int quantityBomb { get { return (int)bombQuantity; } }

        #region UNITY METHODS
        void Awake()
        {
            m_PlayersInputActions = new PlayersInputActions();
        }
        void Start()
        {
            m_PlayerMaster = GetComponent<PlayerMaster>();
            m_PlayersInputActions = m_PlayerMaster.playersInputActions;
            if (bombQuantity != 0) return;
            bombQuantity = m_PlayerMaster.GetPlayersSettings().startBombs;
            m_PlayerMaster.GetPlayersSettings().bombs = bombQuantity;
            m_PlayersInputActions.Player.Bomb.performed += Execute;
        }
        void OnEnable()
        {
            SetInitialReferences();
            m_GamePlayManager.EventCollectItem += UpdateBombs;
        }
        void OnDisable()
        {
            m_GamePlayManager.EventCollectItem -= UpdateBombs;
        }
  #endregion
        
        void UpdateBombs(CollectibleScriptable collectibles)
        {
            if (bombQuantity <= collectibles.maxCollectible)
            {
                bombQuantity += collectibles.amountCollectables;
            }
        }
        public void AddBomb(int ammoAmount)
        {
            bombQuantity += ammoAmount;
            m_PlayerMaster.CallEventPlayerBomb();
        }

        void SetInitialReferences()
        {
            m_PlayerMaster = GetComponent<PlayerMaster>();
            m_GamePlayManager = GamePlayManager.instance;
            prefabBomb.SetActive(false);
        }
        public void Execute()
        {
            throw new NotImplementedException();
        }
        public void Execute(InputAction.CallbackContext context)
        {
            if (bombQuantity <= 0 || !m_GamePlayManager.shouldBePlayingGame || !m_PlayerMaster.ShouldPlayerBeReady())
                return;
            bombQuantity -= 1;
            var bomb = Instantiate(prefabBomb);
            bomb.transform.localPosition = transform.localPosition + bombOffset;
            bomb.SetActive(true);
            m_PlayerMaster.CallEventPlayerBomb();
            LogBomb(1);
        }
        public void UnExecute()
        {
            throw new System.NotImplementedException();
        }
        public void UnExecute(InputAction.CallbackContext callbackContext)
        {
            throw new NotImplementedException();
        }
        void LogBomb(int bomb)
        {
            GamePlaySettings.instance.bombSpent += bomb;
            m_PlayerMaster.GetPlayersSettings().bombs -= bomb;
        }
    }
}
