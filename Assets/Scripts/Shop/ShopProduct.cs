using UnityEngine;
using RiverAttack;
namespace Shopping
{
    [System.Serializable]
    public abstract class ShopProduct : ScriptableObject
    {

        public new string name;
        /*
        public LocalizationString translateName;*/
        public Sprite spriteItem;
        [Multiline]
        public string descriptionItem;
        /*
        public LocalizationString translateDesc;*/
        public string refPriceFirebase;
        public int priceItem;
        public bool isConsumable;

        /*public virtual string GetName()
        {
            if (translateName != null)
            {
                LocalizationTranslate translate = new LocalizationTranslate(LocalizationSettings.Instance.GetActualLanguage());
                return translate.Translate(translateName, LocalizationTranslate.StringFormat.FirstLetterUp);
            }
            return this.name;
        }
        public virtual string GetDescription()
        {
                if (translateDesc != null)
                {
                    LocalizationTranslate translate = new LocalizationTranslate(LocalizationSettings.Instance.GetActualLanguage());
                    return translate.Translate(translateDesc, LocalizationTranslate.StringFormat.FirstLetterUp);
                }
                return this.desciptionItem;
        }*/

        public virtual bool ShouldBeConsume(PlayerSettings player) { return false; }
        public virtual void ConsumeProduct(PlayerSettings player) { Debug.Log("Usou o Produto"); }
    }

}
