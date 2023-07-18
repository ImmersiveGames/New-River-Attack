using UnityEngine;
using RiverAttack;
[System.Serializable]
public abstract class ShopProduct : ScriptableObject
{

    [SerializeField]
    public new string name;
    /*[SerializeField]
    public LocalizationString translateName;*/
    [SerializeField]
    public Sprite spriteItem;
    [SerializeField]
    [Multiline]
    public string desciptionItem;
    /*[SerializeField]
    public LocalizationString translateDesc;*/
    [SerializeField]
    public string refPriceFirebase;
    [SerializeField]
    public int priceItem;
    [SerializeField]
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

    public virtual bool ShouldBeConsume(PlayerStats player) { return false; }
    public virtual void ConsumeProduct(PlayerStats player) { Debug.Log("Usou o Produto"); }
}
