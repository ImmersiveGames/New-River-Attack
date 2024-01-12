using UnityEngine;
/*
 <summary>
** Be aware this will not prevent a non singleton constructor
**   such as `T myT = new T();`
** To prevent that, add `protected T () {}` to your singleton class.
** 
** As a note, this is made as MonoBehaviour because we need Coroutines.
** </summary>
*/
namespace Utils
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        static T _instance;

        static readonly object _lock = new object();

        public static T instance
        {
            get
            {
                if (_applicationIsQuitting)
                {
                    Debug.LogWarning("[Singleton] Instance '" + typeof(T) +
                        "' already destroyed on application quit." +
                        " Won't create again - returning null.");
                    return null;
                }
                lock (_lock)
                {
                    if (_instance)
                        return _instance;
                    _instance = (T)FindObjectOfType(typeof(T));

                    if (FindObjectsOfType(typeof(T)).Length > 1)
                    {
                        Debug.LogError("[Singleton] Something went really wrong " +
                            " - there should never be more than 1 singleton!" +
                            " Reopening the scene might fix it.");
                        return _instance;
                    }

                    if (_instance)
                        return _instance;
                    var singleton = new GameObject();
                    _instance = singleton.AddComponent<T>();
                    singleton.name = "(singleton) " + typeof(T);
                    
                   // DontDestroyOnLoad(singleton);

                    Debug.Log("[Singleton] An instance of " + typeof(T) +
                        " is needed in the scene, so '" + singleton +
                        "' was created with DontDestroyOnLoad.");
                    //Debug.Log("[Singleton] Using instance already created: " +
                    //	_instance.gameObject.name);

                    return _instance;
                }
            }
        }

        public void RemoveSingleton()
        {
            if (_instance)
            {
                Destroy(gameObject);
            }
        }
        // ReSharper disable once StaticMemberInGenericType
        static bool _applicationIsQuitting;
        /* <summary>
            When Unity quits, it destroys objects in a random order.
            In principle, a Singleton is only destroyed when application quits.
            If any script calls Instance after it have been destroyed,
            it will create a buggy ghost object that will stay on the Editor scene
            even after stopping playing the Application. Really bad!
            So, this was made to be sure we're not creating that buggy ghost object.
         </summary>
         */
        protected virtual void OnDestroy()
        {
            Debug.Log($"Destrui o singleton {typeof(T)}");
            _applicationIsQuitting = true;
        }

        public void SetApplicationIsQuitting()
        {
            _applicationIsQuitting = false;
        }
        
    }
}
