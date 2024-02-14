using UnityEngine;

namespace RiverAttack
{
    public class PlayerSkin : MonoBehaviour
    {
        private PlayerMaster m_PlayerMaster;

        #region UNITYMETHODS

        private void OnEnable()
        {
            SetInitialReferences();
            if (m_PlayerMaster != null ? m_PlayerMaster.getPlayerSettings : null)
            {
                ChangePlayerSkin(m_PlayerMaster.getPlayerSettings.playerSkin);
            }
        }

        private void Start()
        {
            ChangePlayerSkin(m_PlayerMaster.getPlayerSettings.playerSkin);
        }
  #endregion

  private void SetInitialReferences()
        {
            m_PlayerMaster = GetComponent<PlayerMaster>();
        }

        private void ChangePlayerSkin(ShopProductSkin skin)
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
            m_PlayerMaster.OnEventPlayerMasterUpdateSkin();
            GamePlayManager.instance.OnEventUpdateLives(m_PlayerMaster.getPlayerSettings.lives);
        }
    }
}
