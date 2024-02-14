using UnityEngine;
using Shopping;
namespace RiverAttack
{
    [CreateAssetMenu(fileName = "ShopLife", menuName = "RiverAttack/Shopping/Lives", order = 2)]
    public class ShopProductLives : ShopProduct
    {
        [SerializeField] private int quantity;

        #region UNITY METHODS

        private void OnEnable()
        {
            isConsumable = true;
        }
  #endregion
        public override bool ShouldBeConsume(PlayerSettings player)
        {
            return player.lives + quantity <= GameSettings.instance.maxLives;
        }
        public override void ConsumeProduct(PlayerSettings player)
        {
            player.lives += quantity;
        }
    }
}
