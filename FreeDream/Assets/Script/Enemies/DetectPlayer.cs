using System.Threading.Tasks;
using UnityEngine;

public class DetectPlayer : MonoBehaviour
{
    [SerializeField] private EnemyController parent;
    [SerializeField] private bool isMeleeCheck;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //parent.PlayerEnter(collision);

            if (isMeleeCheck)
            {
                parent.LaunchMeleeAttack();
            }
            else
            {
                parent.PlayerEnter(collision);
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //parent.PlayerExit();

            if (isMeleeCheck)
            {
                parent.CancelMeleeAttack();
            }
            else
            {
                parent.PlayerExit();
            }

        }

        
    }
}
