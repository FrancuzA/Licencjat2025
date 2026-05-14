using UnityEngine;

public class SingletonMaker<T> : MonoBehaviour where T : Component
{
    public static T instance;

     public static T Instance
     {
        get
        {
           

            if (instance == null)
            {
                instance = FindFirstObjectByType<T>();

                if (instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(T).ToString());
                    instance = singletonObject.AddComponent<T>();
                }
            }
            return instance;
        }
    }

    public virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
        }
        else if (instance != this as T)
        {
            Destroy(gameObject);
        }
    }
}
