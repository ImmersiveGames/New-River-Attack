using System.Collections.Generic;
using UnityEngine;
namespace Utils
{
    public class PoolObjectManager : MonoBehaviour
    {
        static readonly Dictionary<IHasPool, PoolObject> ObjectPools = new Dictionary<IHasPool, PoolObject>();
        public bool isPersistent;

        public static bool CreatePool(IHasPool typePool, GameObject prefab, int initialPoolSize, Transform poolRoot, bool persistent = false)
        {
            //Check to see if the pool already exists.
            if (ObjectPools.ContainsKey(typePool))
                return false;
            //create a new pool using the properties
            var nPool = new PoolObject(prefab, initialPoolSize, poolRoot, persistent);
            ObjectPools.Add(typePool, nPool);
            return true;
        }
        public static GameObject GetObject(IHasPool objName)
        {
            //Find the right pool and ask it for an object.
            return ObjectPools[objName].GetObject();
        }
        public static Transform GetPool(IHasPool objName)
        {
            return ObjectPools[objName].GetRoot();
        }
    }
}
