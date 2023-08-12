using UnityEngine;
using Shopping;
namespace RiverAttack
{
    [CreateAssetMenu(fileName = "ShopBomb", menuName = "RiverAttack/Shopping/Bombs", order = 3)]
    public class ShopProductBomb : ShopProduct
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
            return player.bombs + quantity <= GameSettings.instance.maxBombs;
        }

        public override void ConsumeProduct(PlayerSettings player)
        {
            player.bombs += quantity;
        }
    }
}
