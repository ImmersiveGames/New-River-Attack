using GD.MinMaxSlider;
using UnityEngine;
using UnityEngine.Serialization;
using Utils;
namespace RiverAttack
{
    public class EnemiesDropItem : MonoBehaviour
    {
        //ListDropItems itemsVariables;
        [SerializeField] private float dropHeight = 1;
        [SerializeField] private float timeToAutoDestroy;
        [SerializeField, Tooltip("Se o mínimo for diferente de 0 o valor é aleatório entre min e max."), MinMaxSlider(0, 1)]
        private Vector2 dropChance;

        private ListDropItems m_ItemsVariables;
        private EnemiesMaster m_EnemyMaster;
        private GameObject m_ItemDrop;

        #region UNITY METHODS

        private void OnEnable()
        {
            SetInitialReferences();
            m_EnemyMaster.EventObstacleMasterHit += DropItem;
        }

        private void OnDisable()
        {
            m_EnemyMaster.EventObstacleMasterHit -= DropItem;
        }
  #endregion

  private void SetInitialReferences()
        {
            m_EnemyMaster = GetComponent<EnemiesMaster>();
            m_ItemsVariables = m_EnemyMaster.enemy.listDropItems;
        }
        //TODO: implementar Dropar mais de 1 item e pois precisa alterar a posição;
        private void DropItem()
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
            var position = transform.position;
            var dropPosition = new Vector3(position.x, dropHeight, position.z);
            m_ItemDrop = Instantiate(dropItem.item, dropPosition, Quaternion.identity);
            m_ItemDrop.SetActive(true);
            if (timeToAutoDestroy > 0)
                Invoke(nameof(DestroyDrop), timeToAutoDestroy);
        }

        private void DestroyDrop()
        {
            Destroy(m_ItemDrop);
        }
    }
}
