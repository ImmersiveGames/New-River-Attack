using UnityEngine;
using Utils;
namespace RiverAttack
{
    public class EnemiesDropItem : MonoBehaviour
    {
        [SerializeField]
        ListDropItems itemsVariables;
        [SerializeField]
        float timeToAutoDestroy;
        [SerializeField, Tooltip("Se o minímo for diferente de 0 o valor é aleatório entre min e max."), Tools.MinMaxRangeAttribute(0, 1)]
        Tools.FloatRanged dropChance;

        EnemiesMaster m_EnemyMaster;
        GameObject m_ItemDrop;

        void OnEnable()
        {
            SetInitialReferences();
            m_EnemyMaster.EventDestroyEnemy += DropItem;
        }

        void SetInitialReferences()
        {
            m_EnemyMaster = GetComponent<EnemiesMaster>();
        }
        //TODO: implementar Dropar mais de 1 item e pois precisa alterar a poição;
        private void DropItem()
        {
            if (!(dropChance.maxValue > 0) || itemsVariables == null) return;
            float checkChance = (dropChance.minValue != 0) ? Random.Range(dropChance.minValue, dropChance.maxValue) : dropChance.maxValue;
            float sortRange = Random.value;
            //Debug.Log("Sorteio 1 - Chance: "+ checkChance + " Sorteio: " + sortRange);
            if (!(sortRange <= checkChance)) return;
            //Debug.Log("Vai Dropar um item");
            sortRange = Random.value;
            var dropItem = itemsVariables.TakeRandomItem(sortRange);
            if (dropItem.item == null) return;
            //Debug.Log("Dropou o item: " + dropItem.item.name);
            m_ItemDrop = Instantiate(dropItem.item, this.transform.position, Quaternion.identity);
            m_ItemDrop.SetActive(true);
            if (timeToAutoDestroy > 0)
                Invoke(nameof(DestroyDrop), timeToAutoDestroy);
        }
        private void DestroyDrop()
        {
            Destroy(m_ItemDrop);
        }

        private void OnDisable()
        {
            m_EnemyMaster.EventDestroyEnemy -= DropItem;
        }
    }
}
