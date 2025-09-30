using UnityEngine;

public class KeepObjectsOnLoad : MonoBehaviour
{
    KeepObjectsOnLoad Instance;
    [SerializeField] private GameObject[] objects;

    void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        foreach (var item in objects)
        {
            DontDestroyOnLoad(item);
        }
    }
}
