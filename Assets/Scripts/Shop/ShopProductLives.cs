using UnityEngine;
using Shopping;
namespace RiverAttack
{
    [CreateAssetMenu(fileName = "ShopLife", menuName = "RiverAttack/Shopping/Lives", order = 2)]
    public class ShopProductLives : ShopProduct
    {
        [SerializeField]
        int quantity;

        #region UNITY METHODS
        void OnEnable()
        {
            isConsumable = true;
        }
  #endregion
        public override bool ShouldBeConsume(PlayerSettings player)
        {
            return player.lives + quantity <= player.maxLives;
        }

        public override void ConsumeProduct(PlayerSettings player)
        {
            player.lives += quantity;
        }
    }
}
