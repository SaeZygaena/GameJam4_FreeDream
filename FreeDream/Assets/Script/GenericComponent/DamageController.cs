using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

public class DamageController : MonoBehaviour
{
    public enum TypeEntity
    {
        Turret,
        Gunner,
        Swordman,
        Player
    }
    [SerializeField] private List<Image> listHeart;
    [SerializeField] private List<RectTransform> listHeartRect;
    [SerializeField] private TypeEntity typeEntity;
    [SerializeField] private GameObject dropItem;
    [SerializeField] private GameObject deathEffect;
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
                GunnerDeath();
                break;
            case TypeEntity.Swordman:
                SwordmanDeath();
                break;
            case TypeEntity.Player:
                PlayerDeath();
                break;
            default:
                break;
        }
    }

    public void OnDamage(float _change)
    {
        if (typeEntity == TypeEntity.Player)
        {
            PlayerDamage(_change);
        }
        else
        {
            EnemyDamage();
        }
    }



    #region /////////// PLAYER //////


    private void PlayerDamage(float _change)
    {
        if (!GetComponent<PlayerState>().GetIsDead() && _change < 0)
            anim.SetTrigger("hurt");

        if (_change < 0)
        {
            UnfillHeart(_change);
            StartCoroutine(FlashHeart());
        }
        else
        {
            FillHeart(_change);
            StartCoroutine(BumpHeart());
        }
    }

    private void UnfillHeart(float _change)
    {
        float currentNbChange = 0;

        foreach (var heart in listHeart)
        {
            while (currentNbChange != Mathf.Abs(_change))
            {
                if (heart.fillAmount != 0)
                {
                    heart.fillAmount -= 0.25f;
                    currentNbChange += 1;
                }

                if (heart.fillAmount == 0)
                    break;
            }

            if (currentNbChange == _change)
                return;
        }
    }

    private void FillHeart(float _change)
    {
        float currentNbChange = 0;

        foreach (var heart in listHeart.AsEnumerable().Reverse())
        {
            while (currentNbChange != Mathf.Abs(_change))
            {
                if (heart.fillAmount != 1)
                {
                    heart.fillAmount += 0.25f;
                    currentNbChange += 1;
                }

                if (heart.fillAmount == 1)
                    break;
            }

            if (currentNbChange == _change)
                return;
        }
    }




    IEnumerator FlashHeart()
    {
        Color tempo = Color.white;

        foreach (var heart in listHeart)
        {
            heart.color = Color.black;
        }

        yield return new WaitForSeconds(0.1f);

        foreach (var heart in listHeart)
        {
            heart.color = tempo;
        }
        yield return new WaitForSeconds(0.1f);

    }

    IEnumerator BumpHeart()
    {

        foreach (var heart in listHeartRect)
        {
            heart.localScale = new Vector3(1f, 1f, 1f);
        }

        yield return new WaitForSeconds(0.1f);

        foreach (var heart in listHeartRect)
        {
            heart.localScale = new Vector3(0.8f, 0.8f, 1f);
        }
        yield return new WaitForSeconds(0.1f);

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
        sprite.color = Color.black;
        sprite.enabled = false;
        yield return new WaitForSeconds(0.1f);
        sprite.color = color;
        sprite.enabled = true;
        yield return new WaitForSeconds(0.1f);
        sprite.color = Color.black;
        sprite.enabled = false;
        yield return new WaitForSeconds(0.1f);
        sprite.color = color;
        sprite.enabled = true;
        isTakingDamage = false;
    }

    #region /////////// TURRET //////

    private void TurretDeath()
    {
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    #endregion

    #region /////////// SWORDMAN //////

    private void SwordmanDeath()
    {
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    #endregion

    #region /////////// GUNNER //////

    private void GunnerDeath()
    {
        Instantiate(dropItem, transform.position, Quaternion.identity);
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    #endregion

    #endregion
}
