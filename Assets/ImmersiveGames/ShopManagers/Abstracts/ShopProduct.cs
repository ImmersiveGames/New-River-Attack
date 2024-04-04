using ImmersiveGames.PlayerManagers.ScriptableObjects;
using ImmersiveGames.ShopManagers.Interfaces;
using UnityEngine;
using UnityEngine.Localization;

namespace ImmersiveGames.ShopManagers.Abstracts
{
    public abstract class ShopProduct: ScriptableObject, IShopProduct
    {
        public new string name;
        public LocalizedString localizedName;
        public Sprite spriteItem;
        [Multiline]
        public string descriptionItem;
        public LocalizedString localizedDescription;
        public int priceItem;
        
        public virtual string GetName()
        {
            return localizedName.IsEmpty ? name : localizedName.GetLocalizedString();
        }

        public virtual string GetDescription()
        {
            return localizedDescription.IsEmpty ? descriptionItem : localizedDescription.GetLocalizedString();
        }

        public virtual int GetPrice()
        {
            return priceItem;
        }

        public virtual Sprite GetImage()
        {
            return spriteItem;
        }
    }
}