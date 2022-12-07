using UnityEngine;

/// <summary>
/// Be aware this will not prevent a non singleton constructor
///   such as `T myT = new T();`
/// To prevent that, add `protected T () {}` to your singleton class.
/// 
/// As a note, this is made as MonoBehaviour because we need Coroutines.
/// </summary>
/// 
namespace I2.Parallax
{
    public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T mSingleton;
        private static object _lock = new object();

        public static bool ApplicationIsQuitting;

        public static T singleton
        {
            get
            {
                lock (_lock)
                {
                    if (mSingleton == null)
                    {
                        mSingleton = (T)FindObjectOfType(typeof(T));

                        if (mSingleton == null)
                        {
                            GameObject singleton = new GameObject();
                            singleton.hideFlags = HideFlags.DontSave;
                            mSingleton = singleton.AddComponent<T>();
                            singleton.name = "[singleton] " + typeof(T).ToString();
                        }
                    }

                    return mSingleton;
                }
            }
        }

        public static bool HasSingletonInstance()
        {
            return mSingleton != null;
        }

        public static T TryGetSingleton()
        {
            if (mSingleton == null)
            {
                mSingleton = (T)FindObjectOfType(typeof(T));
            }
            return mSingleton;
        }


        public void OnDestroy()
        {
            if (mSingleton == this as T)
                mSingleton = null;
        }

        public virtual void Awake()
        {
            if (mSingleton == null)
            {
                mSingleton = this as T;
            }
            else
            if (mSingleton != this as T)
            {
                Destroy(this);
                return;
            }

            if (!CanBeDestroyedOnLevelLoad())
                DontDestroyOnLoad(gameObject);
        }

        public virtual bool CanBeDestroyedOnLevelLoad()
        {
            return false;
        }

        protected virtual void OnApplicationQuit()
        {
            ApplicationIsQuitting = true;
        }

    }
}