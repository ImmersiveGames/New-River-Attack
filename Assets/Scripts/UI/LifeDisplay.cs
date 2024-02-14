using UnityEngine;
using UnityEngine.UI;

namespace RiverAttack
{
    public class LifeDisplay : MonoBehaviour
    {
        [SerializeField] private GameObject iconLives;

        private int m_Lives;
        private GamePlayManager m_GamePlayManager;
        private PlayerSettings m_PlayerSettings;

        #region UNITYMETHODS

        private void OnEnable()
        {
            SetInitialReferences();
            SetLivesUI(m_PlayerSettings.lives);
            m_GamePlayManager.EventUpdateLives += SetLivesUI;
        }

        private void OnDisable()
        {
            m_GamePlayManager.EventUpdateLives -= SetLivesUI;
        }
  #endregion

  private void SetInitialReferences()
        {
            m_GamePlayManager = GamePlayManager.instance;
            m_PlayerSettings = PlayerManager.instance.GetPlayerSettingsByIndex();
        }

        private void SetLivesUI(int lives)
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

        private void CreateLiveIcon(Transform parent, int quantity)
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
