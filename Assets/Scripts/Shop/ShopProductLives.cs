using UnityEngine;
namespace RiverAttack
{
    [CreateAssetMenu(fileName = "ShopLife", menuName = "RiverAttack/Shopping/Lives", order = 2)]
    public class ShopProductLives : ShopProduct
    {
        [SerializeField]
        private int quantity;

        private void OnEnable()
        {
            isConsumable = true;
        }
        public override bool ShouldBeConsume(PlayerStats player)
        {
            if (player.lives + quantity > player.maxLives)
                return false;
            return true;
        }

        public override void ConsumeProduct(PlayerStats player)
        {
            player.lives += quantity;
        }
    }
}
