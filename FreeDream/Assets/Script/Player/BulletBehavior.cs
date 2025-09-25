using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    private Rigidbody2D rBody;
    [SerializeField] private float speedBullet;

    public int direction = 1;
    void Start()
    {
        Destroy(gameObject, 3f);
        rBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rBody.linearVelocity += Vector2.right * Time.deltaTime * speedBullet * direction;
    }
}
