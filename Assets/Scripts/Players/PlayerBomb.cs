﻿using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;
namespace RiverAttack
{
    public class PlayerBomb : MonoBehaviour, ICommand
    {
        [Header("Bomb Settings")]
        [SerializeField]
        Vector3 bombOffset;
        [SerializeField]
        GameObject prefabBomb;

        [Header("Camera Shake"), SerializeField]
        float shakeIntensity;
        [SerializeField]
        float shakeTime;


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
        }
        void Start()
        {
            m_PlayersInputActions = m_PlayerMaster.playersInputActions;
            m_PlayerSettings.bombs = GameSettings.instance.startBombs;
            m_PlayersInputActions.Player.Bomb.performed += Execute;
        }
  #endregion

        void SetInitialReferences()
        {
            m_GamePlayManager = GamePlayManager.instance;
            m_PlayerMaster = GetComponent<PlayerMaster>();
            m_PlayerSettings = m_PlayerMaster.getPlayerSettings;
            m_GamePlaySettings = m_GamePlayManager.gamePlaySettings;
        }
        
        public void Execute(InputAction.CallbackContext callbackContext)
        {
           //Debug.Log($"Collect{m_PlayerSettings.bombs}");
            if (m_PlayerSettings.bombs <= 0 || !m_GamePlayManager.shouldBePlayingGame || !m_PlayerMaster.shouldPlayerBeReady)
                return;
            
            

            m_GamePlayManager.OnEventPlayerPushButtonBomb();
            m_PlayerSettings.bombs -= 1;
            var bomb = Instantiate(prefabBomb);
            bomb.transform.localPosition = transform.localPosition + bombOffset;
            bomb.GetComponent<Bullets>().ownerShoot = m_PlayerMaster;
            bomb.SetActive(true);
            m_GamePlayManager.OnEventUpdateBombs(m_PlayerSettings.bombs);
            LogGamePlay(1);
        }
        public void UnExecute(InputAction.CallbackContext callbackContext)
        {
            //throw new NotImplementedException();
        }

        void LogGamePlay(int bomb)
        {
            m_GamePlaySettings.bombSpent += bomb;
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
