using UnityEngine;

namespace RiverAttack
{
    public class ObstacleSkins : MonoBehaviour
    {
        [SerializeField]
        int indexStartSkin;
        public bool randomSkin;
        [SerializeField]
        protected GameObject[] skins;

        void OnEnable()
        {
            LoadDefaultSkin();
        }

        void LoadDefaultSkin()
        {
            if (skins == null) return;
            if (randomSkin && skins.Length > 1)
                indexStartSkin = Random.Range(0, skins.Length);
            if (transform.GetChild(0))
                DestroyImmediate(transform.GetChild(0).gameObject);
            var go = Instantiate(skins[indexStartSkin], transform);
            go.transform.SetAsFirstSibling();
        }
    }
}
