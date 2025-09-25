using UnityEngine;
using System.Collections;
using UnityEngine.Timeline;
using UnityEngine.Rendering.Universal;

public class EnemyController : MonoBehaviour
{
    enum TypeEnemy
    {
        Turret,
        Gunner,
        Swordman
    }

    [SerializeField] [Range(0,1)] private float SpeedAttack;
    [SerializeField] private float DetectDistance;
    [SerializeField] private TypeEnemy typeEnemy;
    [SerializeField] private GameObject bullet;
    [SerializeField] private float power;

    private Animator anim;
    private bool canAttack = false;
    private bool isAttacking = false;
    private Vector3 targetPosition;

    private GameObject playerPos;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canAttack && !isAttacking)
        {
            isAttacking = true;
            switch (typeEnemy)
            {
                case TypeEnemy.Turret:
                    StartCoroutine(AttackTargetDistance());
                    break;
                case TypeEnemy.Gunner:
                    break;
                case TypeEnemy.Swordman:
                    break;
                default:
                    break;
            }
        }
            
    }

    IEnumerator AttackTargetDistance()
    {
        GameObject tempo;

        yield return new WaitForSeconds(SpeedAttack);
        tempo = Instantiate(bullet, this.transform.position, Quaternion.identity);
        tempo.GetComponent<BulletBehavior>().SetPositionTarget(targetPosition);
        tempo.GetComponent<BulletBehavior>().CalculateVectorTarget();
        isAttacking = false;

        targetPosition = playerPos.transform.position;
    }

    public void PlayerEnter(Collider2D collision)
    {
            Debug.Log("Player Detected!");
            anim.SetBool("isAttack", true);

            if (typeEnemy == TypeEnemy.Turret)
            {
                playerPos = collision.gameObject.transform.Find("PosPlayer").gameObject;
                targetPosition = playerPos.transform.position;
            }
            canAttack = true;
    }

    public void PlayerExit(Collider2D collision)
    {
            anim.SetBool("isAttack", false);
            Debug.Log("Player Quit");
            canAttack = false;
            isAttacking = false;
            StopAllCoroutines();
    }
}
