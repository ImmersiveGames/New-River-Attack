using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;
namespace RiverAttack
{

    public class PlayerBomb : MonoBehaviour, ICommand
    {
        [SerializeField]
        private int bombQuantity;
        [SerializeField]
        private Vector3 bombOffset;
        [SerializeField]
        private LayerMask layerMask;
        [SerializeField]
        private GameObject prefabBomb;
        [SerializeField]
        private int idButtonMap;
        private PlayerMaster m_PlayerMaster;
        private GamePlayManager m_GamePlayManager;

        public int quantityBomb { get { return (int)bombQuantity; } }

        private void OnEnable()
        {
            SetInitialReferences();
            m_GamePlayManager.EventCollectItem += UpdateBombs;
        }

        private void UpdateBombs(CollectibleScriptable collectibles)
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

        private void SetInitialReferences()
        {
            m_PlayerMaster = GetComponent<PlayerMaster>();
            m_GamePlayManager = GamePlayManager.instance;
            prefabBomb.SetActive(false);
        }
        private void Update()
        {
            // executar a bomba no input novo
            /*if (m_PlayerMaster.playerSettings.controllerMap.ButtonDown(idButtonMap.Value))
            {
                this.Execute();
            }*/
        }

        public void Execute()
        {
            if (bombQuantity <= 0 || !m_GamePlayManager.shouldBePlayingGame || !m_PlayerMaster.hasPlayerReady)
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
        static void LogBomb(int bomb)
        {
            GamePlaySettings.instance.bombSpents += Mathf.Abs(bomb);

        }
        void OnDisable()
        {
            m_GamePlayManager.EventCollectItem -= UpdateBombs;
        }
    }
}
