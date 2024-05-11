using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            // Check if the instance is null
            if (_instance == null)
            {
                // Find the instance in the scene
                _instance = FindObjectOfType<T>();

                // If no instance is found, log an error
                if (_instance == null)
                {
                    Debug.LogError(typeof(T).Name + " instance not found in the scene.");
                }
            }

            // Return the instance
            return _instance;
        }
    }

    protected virtual void Awake()
    {
        // If this instance is not the singleton instance, destroy it
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Set the instance
        _instance = this as T;
        
    }
}