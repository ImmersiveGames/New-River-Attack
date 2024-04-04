using UnityEngine;

namespace RiverAttack
{
    public class MenuPlayerSkinManager : MonoBehaviour
    {
        [SerializeField] private PlayerSettings playerSettings;

        private GameObject m_PlayerSkin;
        private TrailRenderer[] m_TrailRenderer;

        // Start is called before the first frame update
        private void Start()
        {
            //m_PlayerSkin = playerSettings.playerSkin.getSkin;
            ChangePlayerSkin();
        }

        public void ChangePlayerSkin()
        {
            //m_PlayerSkin = playerSettings.playerSkin.getSkin;

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

        private void TurnOffTrails()
        {
            m_TrailRenderer = GetComponentsInChildren<TrailRenderer>();

            foreach(var trail in m_TrailRenderer)
            {
                trail.enabled = false;
            }
        }
    }
}

