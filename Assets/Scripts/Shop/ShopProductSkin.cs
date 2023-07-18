using UnityEngine;

namespace RiverAttack
{

    [CreateAssetMenu(fileName = "ShopSkin", menuName = "RiverAttack/Shopping/Skin", order = 1)]
    [System.Serializable]
    public class ShopProductSkin : ShopProduct
    {
        //[SerializeField]
        //private RuntimeAnimatorController animatorController;
        [SerializeField]
        private GameObject skinProduct;
        [SerializeField]
        public Sprite hubSprite;

        public GameObject GetSkin { get { return skinProduct; } }
        //public RuntimeAnimatorController GetAnimatorSkin { get { return animatorController; } }

        private void OnEnable()
        {
            isConsumable = false;
        }

        public override bool ShouldBeConsume(PlayerStats player)
        {
            if (player.playerSkin == this)
                return false;
            return true;
        }

        public override void ConsumeProduct(PlayerStats player)
        {
            player.playerSkin = this;
        }
    }
}
