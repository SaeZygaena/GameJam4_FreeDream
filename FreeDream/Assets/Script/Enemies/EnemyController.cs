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

    [SerializeField][Range(0, 1)] private float SpeedAttack;
    [SerializeField] private TypeEnemy typeEnemy;
    [SerializeField] private GameObject bullet;
    [SerializeField] private float power;
    [SerializeField] private Transform startAttack;
    [SerializeField] private GameObject animationAttack;

    private Animator anim;

    private EnemyMovement moveController;
    private bool canAttack = false;
    private bool isAttacking = false;
    private Vector3 targetPosition;

    private GameObject playerPos;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();

        if (typeEnemy == TypeEnemy.Gunner || typeEnemy == TypeEnemy.Swordman)
        {
            moveController = GetComponent<EnemyMovement>();
        }
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
                    StartCoroutine(AttackDistance());
                    break;
                case TypeEnemy.Swordman:
                    SwordmanChase();
                    break;
                default:
                    break;
            }
        }

    }

    #region /////////////////// ATTACK PLAYER //////////////

    private void SwordmanChase()
    {
        moveController.SetBonusSpeed(5f);
    }

    public void LaunchMeleeAttack()
    {
        anim.SetBool("isAttack", true);
        moveController.SetChase(false);
        moveController.SetPatrol(false);
    }

    public void CancelMeleeAttack()
    {
        anim.SetBool("isAttack", false);
        animationAttack.SetActive(false);
        //moveController.SetPatrol(true);

        canAttack = false;
        isAttacking = false;
    }

    public void HitMeleeAttack()
    {
        animationAttack.SetActive(true);
        Instantiate(bullet, startAttack.position, Quaternion.identity);
    }

    public void EndMeleeAttack()
    {
        animationAttack.SetActive(false);
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

    IEnumerator AttackDistance()
    {
        GameObject tempo;

        yield return new WaitForSeconds(SpeedAttack);
        tempo = Instantiate(bullet, this.transform.position, Quaternion.identity);
        tempo.GetComponent<BulletBehavior>().setDirection(this.transform.localScale.x == 1 ? 1 : -1);
        isAttacking = false;
    }

    #endregion

    #region ////////////////// RECEIVE DETECTION PLAYER /////////////

    public void PlayerEnter(Collider2D collision)
    {
        if (typeEnemy == TypeEnemy.Turret)
        {
            playerPos = collision.gameObject.transform.Find("PosPlayer").gameObject;
            targetPosition = playerPos.transform.position;
            anim.SetBool("isAttack", true);
        }

        if (typeEnemy == TypeEnemy.Gunner)
        {
            anim.SetBool("isAttack", true);
            moveController.SetPatrol(false);
        }

        if (typeEnemy == TypeEnemy.Swordman)
        {
            anim.SetBool("isChase", true);
            moveController.SetPatrol(false);
            moveController.SetChase(true);
        }


        canAttack = true;
    }

    public void PlayerExit()
    {
        if (typeEnemy == TypeEnemy.Gunner || typeEnemy == TypeEnemy.Swordman)
        {
            moveController.SetPatrol(true);
        }

        if (typeEnemy == TypeEnemy.Swordman)
        {
            moveController.SetBonusSpeed(0);

            if (moveController.GetChase())
                moveController.FixFlip();
            moveController.SetChase(false);
            anim.SetBool("isChase", false);
        }
        else
        {

            anim.SetBool("isAttack", false);
            canAttack = false;
            isAttacking = false;
        }
        StopAllCoroutines();
    }
    
    #endregion
}
