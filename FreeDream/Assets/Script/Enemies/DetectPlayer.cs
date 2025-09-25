using UnityEngine;

public class DetectPlayer : MonoBehaviour
{
    [SerializeField] private EnemyController parent;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            parent.PlayerEnter(collision);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            parent.PlayerExit(collision);
        }
    }
}
