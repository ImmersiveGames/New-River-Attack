using System;
using UnityEngine;
using UnityEngine.UI;

namespace RiverAttack
{
    public class LifeDisplay : MonoBehaviour
    {
        /*[SerializeField]
        int playerIndex;
        [SerializeField]
        GameObject iconLives;
        [SerializeField]
        Sprite defaultSprite;

        int m_Lives;
        GamePlayManager m_GamePlayManager;
        PlayerMaster m_PlayerMaster;

        
        void OnEnable()
        {
            SetInitialReferences();
            SetLivesUI();
            if (m_PlayerMaster == null) return;
            m_PlayerMaster.EventPlayerMasterReSpawn += UpdateUI;
            m_PlayerMaster.EventPlayerMasterOnDestroy += UpdateUI;
            m_PlayerMaster.EventPlayerAddLive += UpdateUI;
            m_PlayerMaster.EventChangeSkin += UpdateUI;
        }

        void SetInitialReferences()
        {
            m_GamePlayManager = GamePlayManager.instance;
            m_PlayerMaster = m_GamePlayManager.GetPlayerMasterByIndex(playerIndex);
            if (m_PlayerMaster == null) return;
            m_Lives = m_PlayerMaster.GetPlayersSettings().lives;         
        }

        void OnDisable()
        {
            if (m_PlayerMaster == null) return;
            m_PlayerMaster.EventPlayerMasterOnDestroy -= UpdateUI;
            m_PlayerMaster.EventPlayerMasterReSpawn -= UpdateUI;
            m_PlayerMaster.EventPlayerAddLive -= UpdateUI;
        }

        void UpdateUI()
        {
            m_Lives = m_PlayerMaster.GetPlayersSettings().lives;
            Invoke(nameof(SetLivesUI), .1f);
        }

        void UpdateUI(ShopProductSkin skin) 
        {
            ClearLiveIcon(transform);
            SetLivesUI();
        }

        void SetLivesUI()
        {           

            //Debug.Log("Vou tentar Criar os icones");
            int i = transform.childCount;

            if (i < m_Lives)
            {
                //Debug.Log("Chamando a criação dos icones");
                CreateLiveIcon(transform, m_Lives - i);
            }
            for (int x = 0; x < i; x++)
            {
                transform.GetChild(x).gameObject.SetActive(x < m_Lives);
            }
        }
        void CreateLiveIcon(Transform parent, int quantity)
        {
            for (int x = 0; x < quantity; x++)
            {
                var icon = Instantiate(iconLives, parent);
                icon.GetComponent<Image>().sprite = m_PlayerMaster.GetPlayersSettings().playerSkin.hubSprite;
                //Debug.Log("icone criado");
            }

        }

        static void ClearLiveIcon(Transform parent)
        {
            //Debug.Log("Clear Skin");
            for (int x = 0; x < parent.childCount; x++)
            {
                Destroy(parent.GetChild(x).gameObject);
                //Debug.Log("Skin Destroyed");
            }
        }*/
        
    }

}
