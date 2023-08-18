using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;
namespace RiverAttack
{
    public class PlayerBomb : MonoBehaviour, ICommand
    {
        [Header("Bomb Settings")]
        int m_BombQuantity;
        [SerializeField]
        Vector3 bombOffset;
        [SerializeField]
        GameObject prefabBomb;
        
        PlayerMaster m_PlayerMaster;
        GamePlayManager m_GamePlayManager;
        PlayerSettings m_PlayerSettings;
        PlayersInputActions m_PlayersInputActions;
        GamePlaySettings m_GamePlaySettings;

        #region UNITYMETHODS
        void Awake()
        {
            m_PlayersInputActions = new PlayersInputActions();
        }
        void OnEnable()
        {
            SetInitialReferences();
            m_GamePlayManager.EventCollectItem += CollectBombs;
            //TODO: Verificar a melhor forma de atualizar as bonbas.
        }
        void Start()
        {
            m_PlayersInputActions = m_PlayerMaster.playersInputActions;
            m_BombQuantity = GameSettings.instance.startBombs;
            m_PlayerSettings.bombs = m_BombQuantity;
            m_PlayersInputActions.Player.Bomb.performed += Execute;
        }
        void OnDisable()
        {
            m_GamePlayManager.EventCollectItem -= CollectBombs;
        }
  #endregion
        
        void SetInitialReferences()
        {
            m_GamePlayManager = GamePlayManager.instance;
            m_PlayerMaster = GetComponent<PlayerMaster>();
            m_PlayerSettings = m_PlayerMaster.getPlayerSettings;
            m_GamePlaySettings = m_GamePlayManager.gamePlaySettings;
        }
        void CollectBombs(CollectibleScriptable collectibles)
        {
            if (m_BombQuantity > collectibles.maxCollectible)
                return;
            m_BombQuantity += collectibles.amountCollectables;
            m_PlayerSettings.bombs = m_BombQuantity;
            m_GamePlayManager.AddResultList(m_GamePlaySettings.hitEnemiesResultsList, m_PlayerSettings, collectibles,collectibles.amountCollectables, CollisionType.Collected);
        }
        public void Execute(InputAction.CallbackContext callbackContext)
        {
            if (m_BombQuantity <= 0 || !m_GamePlayManager.shouldBePlayingGame || !m_PlayerMaster.shouldPlayerBeReady)
                return;
            m_BombQuantity -= 1;
            var bomb = Instantiate(prefabBomb);
            bomb.transform.localPosition = transform.localPosition + bombOffset;
            bomb.GetComponent<Bullets>().ownerShoot = m_PlayerMaster;
            bomb.SetActive(true);
            m_GamePlayManager.OnEventUpdateBombs(m_BombQuantity);
            LogGamePlay(1);
        }
        public void UnExecute(InputAction.CallbackContext callbackContext)
        {
            throw new NotImplementedException();
        }
        
        void LogGamePlay(int bomb)
        {
            m_GamePlaySettings.bombSpent += bomb;
            m_PlayerSettings.bombs -= bomb;
        }

        /*

        public void AddBomb(int ammoAmount)
        {
            bombQuantity += ammoAmount;
            m_PlayerMaster.CallEventPlayerBomb();
        }
        */
    }
}
