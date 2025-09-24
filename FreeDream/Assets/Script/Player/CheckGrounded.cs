using UnityEditor;
using UnityEngine;

public class CheckedGrounded : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private RaycastHit2D hitRayOne;
    private RaycastHit2D hitRayTwo;
    private PlayerState state;
    public Transform posRayOne;
    public Transform posRayTwo;
    [SerializeField] private float rayLength = 1f;
    [SerializeField] private LayerMask mask;

    void Start()
    {
        state = GetComponent<PlayerState>();
    }
    // Update is called once per frame
    void Update()
    {
        hitRayOne = Physics2D.Raycast(posRayOne.position, Vector3.down, rayLength, mask);
        hitRayTwo = Physics2D.Raycast(posRayTwo.position, Vector3.down, rayLength, mask);
        if (hitRayOne.collider != null || hitRayTwo.collider != null)
        {
            state.SetGrounded(true);
        }
        else
        {
            state.SetGrounded(false);
        }
    }

   void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(posRayOne.position, posRayOne.position + Vector3.down * rayLength);
        Gizmos.DrawLine(posRayTwo.position, posRayTwo.position + Vector3.down * rayLength);
    }
}
