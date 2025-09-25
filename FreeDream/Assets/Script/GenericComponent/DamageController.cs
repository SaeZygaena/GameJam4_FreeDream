using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class DamageController : MonoBehaviour
{
    enum TypeEntity
    {
        Turret,
        Gunner,
        Swordman,
        Player
    }
    [SerializeField] private TypeEntity typeEntity;
    private bool isTakingDamage = false;
    private Animator anim;
    private Color color;
    private SpriteRenderer sprite;
    void Start()
    {
        if (typeEntity != TypeEntity.Player)
        {
            sprite = GetComponent<SpriteRenderer>();
            color = sprite.color;
        }
        anim = GetComponent<Animator>();
    }

    public void OnDeath()
    {
        switch (typeEntity)
        {
            case TypeEntity.Turret:
                TurretDeath();
                break;
            case TypeEntity.Gunner:
                break;
            case TypeEntity.Swordman:
                break;
            case TypeEntity.Player:
                PlayerDeath();
                break;
            default:
                break;
        }
    }

    public void OnDamage()
    {
        if (typeEntity == TypeEntity.Player)
        {
            PlayerDamage();
        }
        else if (typeEntity == TypeEntity.Turret || typeEntity == TypeEntity.Gunner || typeEntity == TypeEntity.Swordman)
        {
            EnemyDamage();
        }
    }



    #region /////////// PLAYER //////


    private void PlayerDamage()
    {
        anim.SetTrigger("hurt");
    }

    private void PlayerDeath()
    {
        if (!GetComponent<PlayerState>().GetIsDead())
            anim.SetTrigger("die");
        GetComponent<PlayerState>().SetIsDead(true);
    }

    public void PlayerIsDead()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    #endregion
    #region ////////// ENEMIES //////////
    private void EnemyDamage()
    {
        if (!isTakingDamage)
        {
            isTakingDamage = true;
            StartCoroutine(ColorFlash());
        }
    }

    IEnumerator ColorFlash()
    {
        sprite.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sprite.color = color;
        yield return new WaitForSeconds(0.1f);
        sprite.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sprite.color = color;
        isTakingDamage = false;
    }

    #region /////////// TURRET //////

    private void TurretDeath()
    {
        Destroy(gameObject);
    }

    #endregion

    #region /////////// SWORDMAN //////


    #endregion

    #region /////////// GUNNER //////


    #endregion

    #endregion
}
