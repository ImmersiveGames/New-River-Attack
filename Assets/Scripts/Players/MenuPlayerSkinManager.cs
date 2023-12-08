using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RiverAttack
{
    public class MenuPlayerSkinManager : MonoBehaviour
    {
        [SerializeField]
        PlayerSettings playerSettings;

        GameObject playerSkin;
        TrailRenderer[] m_TrailRenderer;

        // Start is called before the first frame update
        void Start()
        {
            playerSkin = playerSettings.playerSkin.getSkin;
            ChangePlayerSkin();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void ChangePlayerSkin()
        {
            playerSkin = playerSettings.playerSkin.getSkin;

            var children = GetComponentInChildren<PlayerSkinAttach>();
            if (children == true)
            {
                int siblingIndex = children.transform.GetSiblingIndex();
                DestroyImmediate(transform.GetChild(siblingIndex).gameObject);
            }
            var mySkin = Instantiate(playerSkin, transform);
            mySkin.transform.SetAsFirstSibling();

            TurnOffTrails();
        }

        void TurnOffTrails()
        {
            m_TrailRenderer = GetComponentsInChildren<TrailRenderer>();

            foreach(TrailRenderer trail in m_TrailRenderer)
            {
                trail.enabled = false;
            }
        }
    }
}

