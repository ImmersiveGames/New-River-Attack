using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Utils
{
    public class PoolObject
    {
        private readonly List<GameObject> m_PooledObjects;

        private readonly GameObject m_PooledObj;

        private int m_InitialPoolSize;

        private readonly Transform m_MyRoot;
        private Transform m_Target;

        public PoolObject(GameObject prefab, int initialPoolSize, Transform myRoot, bool persistent = false)
        {
            m_PooledObjects = new List<GameObject>();
            this.m_MyRoot = new GameObject("Pool(" + myRoot.root.name + ")").transform;
            this.m_MyRoot.SetParent(myRoot);
            this.m_MyRoot.SetAsLastSibling();
            var transform = this.m_MyRoot.transform;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            for (int i = 0; i < initialPoolSize; i++)
            {
                CreateObject(prefab, this.m_MyRoot);
            }
            if (persistent)
                Object.DontDestroyOnLoad(myRoot);
            this.m_PooledObj = prefab;
            this.m_InitialPoolSize = initialPoolSize;
        }

        private GameObject CreateObject(GameObject prefab, Transform poolRoot)
        {
            var transform = this.m_MyRoot.transform;
            var nObj = Object.Instantiate(prefab, transform.position, transform.rotation, poolRoot);
            nObj.SetActive(false);
            m_PooledObjects.Add(nObj);
            return nObj;
        }

        internal GameObject GetObject()
        {
            int lenght = m_PooledObjects.Count;
            for (int i = 0; i < lenght; i++)
            {
                //look for the first one that is inactive.
                if (m_PooledObjects[i].activeSelf != false)
                    continue;
                ResetPosition(m_PooledObjects[i]);
                //set the object to active.
                m_PooledObjects[i].SetActive(true);
                //return the object we found.
                m_PooledObjects[i].transform.parent = null;
                return m_PooledObjects[i];
            }
            return CreateObject(m_PooledObj, this.m_MyRoot);
        }

        private void ResetPosition(GameObject gameObject)
        {
            var transform = this.m_MyRoot.transform;
            var localPosition = transform.localPosition;
            gameObject.transform.localPosition = new Vector3(localPosition.x, localPosition.y, localPosition.z);
            var localRotation = transform.localRotation;
            gameObject.transform.localRotation = new Quaternion(localRotation.x, localRotation.y, localRotation.z, localRotation.w);
        }
    }
}
