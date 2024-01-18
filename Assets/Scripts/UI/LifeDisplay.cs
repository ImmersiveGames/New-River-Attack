using UnityEngine;
using UnityEngine.UI;

namespace RiverAttack
{
    public class LifeDisplay : MonoBehaviour
    {
        [SerializeField]
        GameObject iconLives;

        int m_Lives;
        GamePlayManager m_GamePlayManager;
        PlayerSettings m_PlayerSettings;

        #region UNITYMETHODS
        void OnEnable()
        {
            SetInitialReferences();
            SetLivesUI(m_PlayerSettings.lives);
            m_GamePlayManager.EventUpdateLives += SetLivesUI;
        }
        void OnDisable()
        {
            m_GamePlayManager.EventUpdateLives -= SetLivesUI;
        }
  #endregion
        void SetInitialReferences()
        {
            m_GamePlayManager = GamePlayManager.instance;
            m_PlayerSettings = PlayerManager.instance.GetPlayerSettingsByIndex();
        }
        void SetLivesUI(int lives)
        {
            //Debug.Log("Vou tentar Criar os icones");
            int children = transform.childCount;

            if (children < lives)
            {
                CreateLiveIcon(transform, lives - children);
            }
            for (int x = 0; x < children; x++)
            {
                var child = transform.GetChild(x);
                child.gameObject.SetActive(x < lives);
                child.GetComponent<Image>().sprite = m_PlayerSettings.playerSkin.hubSprite;
            }
        }
        void CreateLiveIcon(Transform parent, int quantity)
        {
            for (int x = 0; x < quantity; x++)
            {
                var icon = Instantiate(iconLives, parent);
                icon.GetComponent<Image>().sprite = m_PlayerSettings.playerSkin.hubSprite;
                //Debug.Log("icone criado");
            }
        }

    }

}
