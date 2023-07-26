using System;
using Unity.Mathematics;
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

        //Collider m_MyCollider;

        public int indexSkin { get { return indexStartSkin; } set { indexStartSkin = value; } }
        public GameObject[] enemiesSkins { get { return skins; } set { skins = value; } }

        protected void SetLayers(LayerMask layerMask)
        {
            foreach (var skin in enemiesSkins)
            {
                int novoLayer = Mathf.RoundToInt(Mathf.Log(layerMask.value, 2));
                skin.layer = novoLayer;
            }
        }
        private protected void LoadDefaultSkin()
        {
            //m_MyCollider = GetComponent<Collider>();
            if (skins == null)  return;
            indexStartSkin = 0;
            if (randomSkin && skins.Length > 1)
                indexStartSkin = UnityEngine.Random.Range(0, skins.Length);
            if (skins[indexStartSkin] == m_ObstacleSkin)  return;
            
            // precisa mudar
            m_ObstacleSkin = skins[indexStartSkin];
            if (transform.GetChild(0))
                DestroyImmediate(transform.GetChild(0).gameObject);
            var go = Instantiate(m_ObstacleSkin, transform);
            go.transform.SetAsFirstSibling();
            //go.transform.position = Vector3.zero;
            //go.transform.rotation = quaternion.identity;
            /*var goCollider = go.GetComponentInChildren<Collider>();
            if ((m_MyCollider && goCollider) && (m_MyCollider != goCollider))
            {
                Utils.Tools.CopyComponent<Collider>(goCollider, gameObject);
            }*/
        }
        
    }
}
