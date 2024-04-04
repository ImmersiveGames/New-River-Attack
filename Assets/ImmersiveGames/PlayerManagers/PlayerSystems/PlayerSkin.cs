using System;
using ImmersiveGames.SaveManagers;
using ImmersiveGames.ShopManagers.ShopProducts;
using RiverAttack;
using UnityEngine;

namespace ImmersiveGames.PlayerManagers.PlayerSystems
{
    public class PlayerSkin : MonoBehaviour
    {
        private PlayerMaster _playerMaster;

        private void OnEnable()
        {
            SetInitialReferences();
        }

        private void Start()
        {
            RemoveSkin();
            CreateSkin(_playerMaster.GetPlayerSkin());
        }


        private void ChangePlayerSkin(ShopProductSkin productSkin)
        {
            if (productSkin == null || _playerMaster == null) return;
            var gameOptionsSave = GameOptionsSave.instance;
            gameOptionsSave.SetSkinToPlayer(_playerMaster.PlayerIndex, productSkin);
            //Remove A Skin Anterior
            RemoveSkin();
            CreateSkin(productSkin.prefabSkin);
        }

        private void RemoveSkin()
        {
            var children = GetComponentInChildren<PlayerSkinAttach>();
            if (children != true) return;
            var siblingIndex = children.transform.GetSiblingIndex();
            DestroyImmediate(transform.GetChild(siblingIndex).gameObject);
        }

        private void CreateSkin(GameObject skin)
        {
            var mySkin = Instantiate(skin, transform);
            mySkin.transform.SetAsFirstSibling();
        }

        private void SetInitialReferences()
        {
            _playerMaster = GetComponent<PlayerMaster>();
        }
    }
}