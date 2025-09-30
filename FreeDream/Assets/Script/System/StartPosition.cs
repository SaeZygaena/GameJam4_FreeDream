using UnityEngine;

public class StartPosition : MonoBehaviour
{
    void Start()
    {
        GameObject.FindGameObjectWithTag("Player").transform.position = transform.position;
    }
}
