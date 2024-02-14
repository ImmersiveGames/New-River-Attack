using UnityEngine;
using Shopping;

namespace RiverAttack
{
    [CreateAssetMenu(fileName = "ShopSkin", menuName = "RiverAttack/Shopping/Skin", order = 1)]
    [System.Serializable]
    public class ShopProductSkin : ShopProduct
    {
        [SerializeField] private GameObject skinProduct;
        public Sprite hubSprite;

        public GameObject getSkin { get { return skinProduct; } }

        #region UNITY METHODS

        private void OnEnable()
        {
            isConsumable = false;
        }
  #endregion

        public override bool ShouldBeConsume(PlayerSettings player)
        {
            return player.playerSkin != this;
        }

        public override void ConsumeProduct(PlayerSettings player)
        {
            player.playerSkin = this;
        }
    }
}
