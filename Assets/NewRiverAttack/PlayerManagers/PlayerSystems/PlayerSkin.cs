using ImmersiveGames.ShopManagers.ShopProducts;
using NewRiverAttack.SaveManagers;
using RiverAttack;
using UnityEngine;

namespace NewRiverAttack.PlayerManagers.PlayerSystems
{
    public class PlayerSkin : MonoBehaviour
    {
        private PlayerMaster _playerMaster;
        
        private void OnEnable()
        {
            RemoveSkin();
            SetInitialReferences();
        }

        private void Start()
        {
            ChangePlayerSkin(_playerMaster.ActualSkin);
        }


        private void ChangePlayerSkin(ShopProductSkin productSkin)
        {
            if (productSkin == null || _playerMaster == null) return;
            var gameOptionsSave = GameOptionsSave.instance;
            gameOptionsSave.ChangeSkinToPlayer(_playerMaster.PlayerIndex, productSkin);
            //Remove A Skin Anterior
            RemoveSkin();
            CreateSkin(productSkin);
            _playerMaster.OnEventPlayerMasterChangeSkin(productSkin);
        }

        private void RemoveSkin()
        {
            var children = GetComponentInChildren<PlayerSkinAttach>();
            if (children != true) return;
            var siblingIndex = children.transform.GetSiblingIndex();
            DestroyImmediate(transform.GetChild(siblingIndex).gameObject);
        }

        private void CreateSkin(ShopProductSkin productSkin)
        {
            var mySkin = Instantiate(productSkin.prefabSkin, transform);
            mySkin.transform.SetAsFirstSibling();
        }

        private void SetInitialReferences()
        {
            _playerMaster = GetComponent<PlayerMaster>();
        }
    }
}