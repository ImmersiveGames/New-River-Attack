using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace RiverAttack
{
    [RequireComponent(typeof(PlayerMaster))]
    public class PlayerSkin : MonoBehaviour
    {
        [SerializeField]
        private ShopProductSkin defaultSkin;
        private GameObject mySkin;
        private PlayerMaster playerMaster;

        private void OnEnable()
        {
            SetInitialReferences();
            playerMaster.EventChangeSkin += SetPlayerSkin;
            playerMaster.EventPlayerDestroy += DisableSkin;
            playerMaster.EventPlayerReload += EnableSkin;
        }

        private void SetInitialReferences()
        {
            playerMaster = GetComponent<PlayerMaster>();
            ShopProductSkin skin = playerMaster.PlayersSettings().playerSkin ?? defaultSkin;
            SetPlayerSkin(skin);
        }

        private void SetPlayerSkin(ShopProductSkin skin)
        {
            if (transform.GetChild(0))
                DestroyImmediate(transform.GetChild(0).gameObject);
            mySkin = Instantiate(skin.GetSkin, transform);
            mySkin.transform.SetAsFirstSibling();
            playerMaster.PlayersSettings().playerSkin = skin;
            if (mySkin.GetComponent<Collider>())
            {
                Tools.CopyComponent(mySkin.GetComponentInChildren<Collider>(), gameObject);
            }
            //playerMaster.SetTagLayerChild();
        }

        private void DisableSkin()
        {
            mySkin.SetActive(false);
        }
        private void EnableSkin()
        {
            mySkin.SetActive(true);
        }

        private void OnDisable()
        {
            playerMaster.EventChangeSkin -= SetPlayerSkin;
            playerMaster.EventPlayerDestroy -= DisableSkin;
            playerMaster.EventPlayerReload -= EnableSkin;
        }
    }

}
