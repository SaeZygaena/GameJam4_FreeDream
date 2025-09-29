using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    [SerializeField] private float timer;
    void Start()
    {
        Destroy(gameObject, timer);
    }
}
