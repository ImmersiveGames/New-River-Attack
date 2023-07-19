using UnityEngine;
using Utils;

namespace RiverAttack
{
    [RequireComponent(typeof(PlayerMaster))]
    public class PlayerSkin : MonoBehaviour
    {
        [SerializeField]
        ShopProductSkin defaultSkin;
        GameObject m_MySkin;
        PlayerMaster m_PlayerMaster;

        #region UNITY METHODS
        void OnEnable()
        {
            SetInitialReferences();
            m_PlayerMaster.EventChangeSkin += SetPlayerSkin;
            m_PlayerMaster.EventPlayerDestroy += DisableSkin;
            m_PlayerMaster.EventPlayerReload += EnableSkin;
        }
        void OnDisable()
        {
            m_PlayerMaster.EventChangeSkin -= SetPlayerSkin;
            m_PlayerMaster.EventPlayerDestroy -= DisableSkin;
            m_PlayerMaster.EventPlayerReload -= EnableSkin;
        }
  #endregion

        void SetInitialReferences()
        {
            m_PlayerMaster = GetComponent<PlayerMaster>();
            var skin = m_PlayerMaster.GetPlayersSettings().playerSkin != null ? m_PlayerMaster.GetPlayersSettings().playerSkin : defaultSkin;
            SetPlayerSkin(skin);
        }

        void SetPlayerSkin(ShopProductSkin skin)
        {
            if (transform.GetChild(0))
                DestroyImmediate(transform.GetChild(0).gameObject);
            m_MySkin = Instantiate(skin.getSkin, transform);
            m_MySkin.transform.SetAsFirstSibling();
            m_PlayerMaster.GetPlayersSettings().playerSkin = skin;
            if (m_MySkin.GetComponent<Collider>())
            {
                Tools.CopyComponent(m_MySkin.GetComponentInChildren<Collider>(), gameObject);
            }
        }
        void DisableSkin()
        {
            m_MySkin.SetActive(false);
        }
        void EnableSkin()
        {
            m_MySkin.SetActive(true);
        }
    }
}
