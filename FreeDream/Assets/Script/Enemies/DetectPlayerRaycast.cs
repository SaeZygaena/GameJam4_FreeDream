using UnityEngine;

public class DetectPlayerRaycast : MonoBehaviour
{
    [SerializeField] private EnemyController parent;
    [SerializeField] private bool isMeleeCheck;
    [SerializeField] private float rayLength = 1f;

    private RaycastHit2D hitRayOne;
    public Transform posRayOne;

    [SerializeField] private LayerMask mask;

    void Start()
    {

    }

    void Update()
    {

        if (parent.transform.localScale.x == 1)
            hitRayOne = Physics2D.Raycast(posRayOne.position, Vector3.left, rayLength, mask);
        else
            hitRayOne = Physics2D.Raycast(posRayOne.position, Vector3.right, rayLength, mask);

        if (hitRayOne.collider != null)
        {
            if (isMeleeCheck)
            {
                parent.LaunchMeleeAttack();
            }
            else
            {
                parent.PlayerEnter(hitRayOne.collider);
            }
        }
        else
        {

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

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (parent.transform.localScale.x == 1)
            Gizmos.DrawLine(posRayOne.position, posRayOne.position + Vector3.left * rayLength);
        else
            Gizmos.DrawLine(posRayOne.position, posRayOne.position + Vector3.right * rayLength);
    }
}
