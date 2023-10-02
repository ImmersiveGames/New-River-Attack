using System.Collections.Generic;
using UnityEngine;
namespace Utils
{
    public class PoolObject
    {
        readonly List<GameObject> m_PooledObjects;
        readonly GameObject m_PooledObj;
        readonly Transform m_MyRoot;
        Transform m_Target;

        public PoolObject(GameObject prefab, int initialPoolSize, Transform myRoot, bool persistent = false)
        {
            m_PooledObjects = new List<GameObject>();
            m_MyRoot = new GameObject("Pool(" + myRoot.root.name + ")").transform;
            m_MyRoot.SetParent(myRoot);
            m_MyRoot.SetAsLastSibling();
            var transform = m_MyRoot.transform;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;

            for (int i = 0; i < initialPoolSize; i++)
            {
                CreateObject(prefab, m_MyRoot);
            }
            if (persistent)
                Object.DontDestroyOnLoad(myRoot);
            m_PooledObj = prefab;
        }

        GameObject CreateObject(GameObject prefab, Transform poolRoot)
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
                if (m_PooledObjects[i].activeSelf)
                    continue;
                ResetPosition(m_PooledObjects[i]);
                //set the object to active.
                m_PooledObjects[i].SetActive(true);
                //return the object we found.
                m_PooledObjects[i].transform.parent = null;
                return m_PooledObjects[i];
            }
            return CreateObject(m_PooledObj, m_MyRoot);
        }

        void ResetPosition(GameObject gameObject)
        {
            var transform = m_MyRoot.transform;
            var localPosition = transform.localPosition;
            gameObject.transform.localPosition = new Vector3(localPosition.x, localPosition.y, localPosition.z);
            var localRotation = transform.localRotation;
            gameObject.transform.localRotation = new Quaternion(localRotation.x, localRotation.y, localRotation.z, localRotation.w);
        }

        public Transform GetRoot()
        {
            return m_MyRoot.transform;
        }
    }
}
