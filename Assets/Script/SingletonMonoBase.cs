using UnityEngine;

public abstract class SingletonMonoBase<T> : MonoBehaviour
    where T : SingletonMonoBase<T>
{
    static bool s_isApplicationQuit = false;
    public static bool IsApplicationQuit => s_isApplicationQuit;


    static T s_instance;

    public static T Instance
    {
        get
        {
            if (s_instance == null)
            {
                s_instance = FindAnyObjectByType<T>();
                if (s_instance == null)
                {
                    s_instance = new GameObject(typeof(T).Name).AddComponent<T>();
                }
            }

            return s_instance;
        }
    }

    protected virtual void Awake()
    {
        if (s_instance == null)
        {
            s_instance = (T)this;
        }
        else if (s_instance != (T)this)
        {
            Destroy(gameObject);
        }
    }

    protected virtual void OnApplicationQuit()
    {
        s_isApplicationQuit = true;
    }
}
