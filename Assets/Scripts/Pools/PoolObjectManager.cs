using System.Collections.Generic;
using UnityEngine;
namespace Utils
{
    public class PoolObjectManager : Singleton<PoolObjectManager>
    {
        readonly Dictionary<IHasPool, PoolObject> m_ObjectPools = new Dictionary<IHasPool, PoolObject>();
        public bool isPersistent;

        public static bool CreatePool(IHasPool typePool, GameObject prefab, int initialPoolSize, Transform poolRoot, bool persistent = false)
        {
            //Check to see if the pool already exists.
            if (instance.m_ObjectPools.ContainsKey(typePool))
                return false;
            //create a new pool using the properties
            var nPool = new PoolObject(prefab, initialPoolSize, poolRoot, persistent);
            instance.m_ObjectPools.Add(typePool, nPool);
            return true;
        }
        public static GameObject GetObject(IHasPool objName)
        {
            //Find the right pool and ask it for an object.
            return instance.m_ObjectPools[objName].GetObject();
        }
    }
}
