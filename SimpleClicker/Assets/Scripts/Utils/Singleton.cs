using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindFirstObjectByType<T>();
                
                if(instance == null)
                {
                    SetupInstance();
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private static void SetupInstance()
    {
        GameObject obj = new GameObject();
        obj.name = typeof(T).Name;
        instance = obj.AddComponent<T>();
        DontDestroyOnLoad(obj);
    }
}
