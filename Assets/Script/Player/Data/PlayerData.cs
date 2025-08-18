using UnityEngine;

public class PlayerData : Singleton<PlayerData>
{
    const string CONTAINER = "Container";

    DataContainer _container;
    public DataContainer Container
    {
        get
        {
            if (_container == null)
            {
                if (PlayerPrefs.HasKey(CONTAINER))
                {
                    string containerBase = PlayerPrefs.GetString(CONTAINER);
                    _container = JsonUtility.FromJson<DataContainer>(containerBase);
                }
                else
                {
                    _container = new DataContainer();
                }
            }

            return _container;
        }
    }
}