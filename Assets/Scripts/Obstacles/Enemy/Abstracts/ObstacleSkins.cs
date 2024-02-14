﻿using UnityEngine;

namespace RiverAttack
{
    public class ObstacleSkins : MonoBehaviour
    {
        [SerializeField] private int indexStartSkin;
        public bool randomSkin;
        [SerializeField]
        protected GameObject[] skins;

        private void Start()
        {
            LoadDefaultSkin();
        }

        private void LoadDefaultSkin()
        {
            if (skins == null) return;
            if (randomSkin && skins.Length > 1)
                indexStartSkin = Random.Range(0, skins.Length);
            if (transform.GetChild(0) != null)
                DestroyImmediate(transform.GetChild(0).gameObject);
            var go = Instantiate(skins[indexStartSkin], transform);
            go.transform.SetAsFirstSibling();
        }
    }
}
