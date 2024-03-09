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
        
        public virtual string GetName(bool locale)
        {
            if(localizedName != null)
                return locale ? localizedName.GetLocalizedString() : name;
            return name;
        }

        public virtual string GetDescription(bool locale)
        {
            if(localizedName != null)
                return locale ? localizedDescription.GetLocalizedString() : descriptionItem;
            return descriptionItem;
        }

        public virtual int GetPrice()
        {
            return priceItem;
        }

        public virtual Sprite GetImage()
        {
            return spriteItem;
        }

        public virtual void Buy(PlayerSettings player)
        {
            throw new System.NotImplementedException();
        }
    }
}