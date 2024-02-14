using System.Collections.Generic;
using System.Threading.Tasks;
using RiverAttack;
using UnityEngine;
namespace Utils
{
    public class PoolObjectManager : MonoBehaviour
    {
        private static readonly Dictionary<IHasPool, PoolObject> ObjectPools = new Dictionary<IHasPool, PoolObject>();
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

        public static async Task<GameObject> GetObjectAsync(IHasPool objName)
        {
            var tcs = new TaskCompletionSource<GameObject>();
            tcs.SetResult(ObjectPools[objName].GetObject());
            return await tcs.Task;
        }
        public static GameObject GetObject(IHasPool objName)
        {
            //Find the right pool and ask it for an object.
            return ObjectPools[objName].GetObject();
        }
        public static GameObject GetObject<T>(IHasPool objName, ObjectMaster objMaster)
        {
            //Find the right pool and ask it for an object.
            var go = ObjectPools[objName].GetObject();
            var bullet = go.GetComponent<T>() as Bullets;
            if (bullet == null) return go;
            if (!bullet.haveAPool)
                bullet.SetMyPool(ObjectPools[objName].GetRoot());
            if (!bullet.ownerShoot)
                bullet.ownerShoot = objMaster;
            return go;
        }
        public static Transform GetPool(IHasPool objName)
        {
            return ObjectPools[objName].GetRoot();
        }
    }
}
