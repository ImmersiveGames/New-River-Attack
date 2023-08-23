using UnityEngine;

namespace RiverAttack
{
    public abstract class ObstacleSkins : MonoBehaviour
    {
        [SerializeField]
        int indexStartSkin;
        public bool randomSkin;
        [SerializeField]
        protected GameObject[] skins;
        
        GameObject m_ObstacleSkin;

        void Awake()
        {
            LoadDefaultSkin();
        }

        void LoadDefaultSkin()
        {
            if (skins == null)  return;
            indexStartSkin = 0;
            if (randomSkin && skins.Length > 1)
                indexStartSkin = Random.Range(0, skins.Length);
            if (skins[indexStartSkin] == m_ObstacleSkin)  return;
            
            m_ObstacleSkin = skins[indexStartSkin];
            if (transform.GetChild(0))
                DestroyImmediate(transform.GetChild(0).gameObject);
            var go = Instantiate(m_ObstacleSkin, transform);
            go.transform.SetAsFirstSibling();
        }
    }
}
