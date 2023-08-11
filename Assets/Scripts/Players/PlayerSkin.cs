﻿using System;
using UnityEngine;
using Utils;

namespace RiverAttack
{
    [RequireComponent(typeof(PlayerMaster))]
    public class PlayerSkin : MonoBehaviour
    {
        /*[SerializeField]
        ShopProductSkin defaultSkin;
        GameObject m_MySkin;
        PlayerMaster m_PlayerMaster;
        PlayerSkinTrail m_PlayerSkinTrail;

        #region UNITY METHODS
        void Start()
        {
            SetInitialReferences();
        }
        void OnEnable()
        {
            SetInitialReferences();
            m_PlayerMaster.EventChangeSkin += SetPlayerSkin;
            m_PlayerMaster.EventPlayerMasterOnDestroy += DisableSkin;
            m_PlayerMaster.EventPlayerMasterReSpawn += EnableSkin;
        }
        void OnDisable()
        {
            m_PlayerMaster.EventChangeSkin -= SetPlayerSkin;
            m_PlayerMaster.EventPlayerMasterOnDestroy -= DisableSkin;
            m_PlayerMaster.EventPlayerMasterReSpawn -= EnableSkin;
        }
  #endregion

        void SetInitialReferences()
        {
            m_PlayerMaster = GetComponent<PlayerMaster>();
            m_PlayerSkinTrail = GetComponent<PlayerSkinTrail>();
            m_PlayerSkinTrail.RestTrail();
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
            
            //TODO: Rever a questão dos Trails fazendo reset no lugar errado
            m_PlayerSkinTrail.RestTrail();
            /*if (m_MySkin.GetComponent<Collider>())
            {
                Tools.CopyComponent(m_MySkin.GetComponentInChildren<Collider>(), gameObject);
            }#1#
        }
        void DisableSkin()
        {
            m_MySkin.SetActive(false);
        }
        void EnableSkin()
        {
            m_MySkin.SetActive(true);
        }*/
    }
}
