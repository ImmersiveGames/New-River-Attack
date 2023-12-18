using UnityEngine;

namespace RiverAttack
{
    public class MenuPlayerSkinManager : MonoBehaviour
    {
        [SerializeField]
        PlayerSettings playerSettings;

        GameObject m_PlayerSkin;
        TrailRenderer[] m_TrailRenderer;

        // Start is called before the first frame update
        void Start()
        {
            m_PlayerSkin = playerSettings.playerSkin.getSkin;
            ChangePlayerSkin();
        }

        public void ChangePlayerSkin()
        {
            m_PlayerSkin = playerSettings.playerSkin.getSkin;

            var children = GetComponentInChildren<PlayerSkinAttach>();
            if (children == true)
            {
                int siblingIndex = children.transform.GetSiblingIndex();
                DestroyImmediate(transform.GetChild(siblingIndex).gameObject);
            }
            var mySkin = Instantiate(m_PlayerSkin, transform);
            mySkin.transform.SetAsFirstSibling();

            TurnOffTrails();
        }

        void TurnOffTrails()
        {
            m_TrailRenderer = GetComponentsInChildren<TrailRenderer>();

            foreach(var trail in m_TrailRenderer)
            {
                trail.enabled = false;
            }
        }
    }
}

