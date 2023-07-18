using UnityEngine;
namespace RiverAttack
{

    [CreateAssetMenu(fileName = "ShopBomb", menuName = "RiverAttack/Shopping/Bombs", order = 3)]
    public class ShopProductBomb : ShopProduct
    {

        [SerializeField]
        private int quantity;

        private void OnEnable()
        {
            isConsumable = true;
        }

        public override bool ShouldBeConsume(PlayerStats player)
        {
            if (player.bombs + quantity > player.maxBombs)
                return false;
            return true;
        }

        public override void ConsumeProduct(PlayerStats player)
        {
            player.bombs += quantity;
        }
    }
}
