using GD.MinMaxSlider;
using UnityEngine;
using Utils;
namespace RiverAttack
{
    public class EnemiesDropItem : MonoBehaviour
    {
        //ListDropItems itemsVariables;
        [SerializeField]
        float timeToAutoDestroy;
        [SerializeField, Tooltip("Se o mínimo for diferente de 0 o valor é aleatório entre min e max."), MinMaxSlider(0, 1)]
        Vector2 dropChance;

        ListDropItems m_ItemsVariables;
        EnemiesMaster m_EnemyMaster;
        GameObject m_ItemDrop;

        #region UNITY METHODS
        void OnEnable()
        {
            SetInitialReferences();
            m_EnemyMaster.EventObstacleMasterHit += DropItem;
        }
        void OnDisable()
        {
            m_EnemyMaster.EventObstacleMasterHit -= DropItem;
        }
  #endregion

        void SetInitialReferences()
        {
            m_EnemyMaster = GetComponent<EnemiesMaster>();
            m_ItemsVariables = m_EnemyMaster.enemy.listDropItems;
        }
        //TODO: implementar Dropar mais de 1 item e pois precisa alterar a posição;
        void DropItem()
        {
            if (dropChance.y <= 0 || m_ItemsVariables == null) return;
            float checkChance = (dropChance.x != 0) ? Random.Range(dropChance.x, dropChance.y) : dropChance.y;
            float sortRange = Random.value;
            //Debug.Log("Sorteio 1 - Chance: "+ checkChance + " Sorteio: " + sortRange);
            if (!(sortRange <= checkChance)) return;
            //Debug.Log("Vai Dropar um item");
            sortRange = Random.value;
            var dropItem = m_ItemsVariables.TakeRandomItem(sortRange);
            if (dropItem.item == null) return;
            //Debug.Log("Dropou o item: " + dropItem.item.name);
            m_ItemDrop = Instantiate(dropItem.item, this.transform.position, Quaternion.identity);
            m_ItemDrop.SetActive(true);
            if (timeToAutoDestroy > 0)
                Invoke(nameof(DestroyDrop), timeToAutoDestroy);
        }
        void DestroyDrop()
        {
            Destroy(m_ItemDrop);
        }
    }
}
