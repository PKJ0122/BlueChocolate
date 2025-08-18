using UnityEngine;

public class Singleton<T> : MonoBehaviour
    where T : Singleton<T>
{
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
        else if (s_instance != this)
        {
            Destroy(gameObject);
        }
    }
}