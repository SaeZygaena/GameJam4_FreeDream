using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UIElements;

public class BulletBehavior : MonoBehaviour
{
    private Rigidbody2D rBody;
    private Vector3 targetPosition;

    private Vector3 vectorBullet;
    public bool isPlayer = false;
    public float power = 1;
    [SerializeField] private float speedBullet;
    [SerializeField] private float deleteTimer;

    enum BulletType
    {
        Accelerating,
        ConstantSpeed,
        TargetPlayer,
        Static
    }

    [SerializeField] private BulletType bulletType;

    public int direction = 1;

    public void setDirection(int _direction) { direction = _direction; }
    void Start()
    {
        Destroy(gameObject, deleteTimer);
        rBody = GetComponent<Rigidbody2D>();
    }
    void FixedUpdate()
    {
        switch (bulletType)
        {
            case BulletType.Accelerating:
                AcceleratedBullet();
                break;
            case BulletType.ConstantSpeed:
                ConstantSpeedBullet();
                break;
            case BulletType.TargetPlayer:
                TargetPlayerBullet();
                break;
            case BulletType.Static:
                break;
            default:
                break;
        }
    }

    public void SetPositionTarget(Vector3 _target)
    {
        targetPosition = _target;
    }

    private void AcceleratedBullet()
    {
        rBody.linearVelocity += Vector2.right * Time.deltaTime * speedBullet * direction;
    }

    #region  /////////// TargetPlayerBullet //////

    private void ConstantSpeedBullet()
    {
        transform.position += new Vector3(Time.deltaTime * speedBullet * direction, 0f, 0f);
    }

    public void CalculateVectorTarget()
    {
        vectorBullet = (targetPosition - transform.position).normalized;
    }

    private void TargetPlayerBullet()
    {
        transform.position += vectorBullet * speedBullet * Time.deltaTime;
    }

    #endregion

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (isPlayer)
            TriggerPlayerBullet(collision);
        else
            TriggerEnemyBullet(collision);
    }

    void TriggerPlayerBullet(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (collision.GetComponent<HealthComponent>() != null)
            {
                collision.GetComponent<HealthComponent>().HealChange(-power);
            }
        }
    }

    void TriggerEnemyBullet(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<HealthComponent>().HealChange(-power);
        }
    }
}
