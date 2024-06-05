using ImmersiveGames.ShopManagers.ShopProducts;
using UnityEngine;

namespace RiverAttack
{
    public class PlayerSkin : MonoBehaviour
    {
        private PlayerMasterOld _mPlayerMasterOld;

        #region UNITYMETHODS

        private void OnEnable()
        {
            SetInitialReferences();
            if (_mPlayerMasterOld != null ? _mPlayerMasterOld.getPlayerSettings : null)
            {
                ChangePlayerSkin(_mPlayerMasterOld.getPlayerSettings.playerSkin);
            }
        }

        private void Start()
        {
            ChangePlayerSkin(_mPlayerMasterOld.getPlayerSettings.playerSkin);
        }
  #endregion

  private void SetInitialReferences()
        {
            _mPlayerMasterOld = GetComponent<PlayerMasterOld>();
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
            
            /*var mySkin = Instantiate(skin.getSkin, transform);
            mySkin.transform.SetAsFirstSibling();*/
            
            playerSkinTrail.enabled = true;
            _mPlayerMasterOld.getPlayerSettings.playerSkin = skin;
            _mPlayerMasterOld.OnEventPlayerMasterUpdateSkin();
            GamePlayManager.instance.OnEventUpdateLives(_mPlayerMasterOld.getPlayerSettings.lives);
        }
    }
}
