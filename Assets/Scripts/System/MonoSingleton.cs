namespace ThisIsThePresident
{
    using UnityEngine;

    public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance;
        private static object lockObject = new object();

        public static T Instance
        {
            get
            {
                lock (lockObject)
                {
                    if (instance == null)
                    {
                        instance = (T)FindObjectOfType(typeof(T));
                        if (FindObjectsOfType(typeof(T)).Length > 1)
                        {
                            string objectNames = string.Empty;
                            foreach (var o in FindObjectsOfType(typeof(T)))
                                objectNames += o.name + ", ";
                            Debug.LogError($"[MonoSingleton] Something went really wrong - found more than 1 instance of singletone ({objectNames})!  Reopening the scene might fix it.");
                            return instance;
                        }
                        if (instance == null)
                        {
                            GameObject singleton = new GameObject();
                            instance = singleton.AddComponent<T>();
                            singleton.name = $"(singleton) {typeof(T)}";
                            Debug.Log($"[MonoSingleton] An instance of {typeof(T)} is needed in the scene so '{singleton}' was created with DontDestroyOnLoad.");
                        }
                    }
                    return instance;
                }
            }
        }
    }
}