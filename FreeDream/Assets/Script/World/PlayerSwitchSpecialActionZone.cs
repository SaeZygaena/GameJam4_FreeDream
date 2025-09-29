using UnityEngine;

public class PlayerSwitchSpecialActionZone : MonoBehaviour
{
    [SerializeField] private PlayerState.ActionType action;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerState>().SetActionType(action);
        }
    }
}
