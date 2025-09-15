using System;

public class Singleton<T>
    where T : Singleton<T> , new()
{
    static T s_instance;
    public static T Instance
    {
        get
        {
            if (s_instance == null)
            {
                s_instance = new T();
            }

            return s_instance;
        }
    }
}