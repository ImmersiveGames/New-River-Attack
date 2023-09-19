using UnityEngine;

namespace RiverAttack
{
    public class PlayerSkin : MonoBehaviour
    {
        PlayerMaster m_PlayerMaster;

        #region UNITYMETHODS
        void OnEnable()
        {
            SetInitialReferences();
            if (m_PlayerMaster != null ? m_PlayerMaster.getPlayerSettings : null)
            {
                ChangePlayerSkin(m_PlayerMaster.getPlayerSettings.playerSkin);
            }
        }
        void Start()
        {
            ChangePlayerSkin(m_PlayerMaster.getPlayerSettings.playerSkin);
        }
  #endregion
        void SetInitialReferences()
        {
            m_PlayerMaster = GetComponent<PlayerMaster>();
        }

        void ChangePlayerSkin(ShopProductSkin skin)
        {
            var playerSkinTrail = GetComponent<PlayerSkinTrail>();
            playerSkinTrail.enabled = false;
            var children = GetComponentInChildren<PlayerSkinAttach>();
            if (children == true)
            {
                int siblingIndex = children.transform.GetSiblingIndex();
                DestroyImmediate(transform.GetChild(siblingIndex).gameObject);
            }
            var mySkin = Instantiate(skin.getSkin, transform);
            mySkin.transform.SetAsFirstSibling();
            playerSkinTrail.enabled = true;
            m_PlayerMaster.getPlayerSettings.playerSkin = skin;
            GamePlayManager.instance.OnEventUpdateLives(m_PlayerMaster.getPlayerSettings.lives);
        }
    }
}
