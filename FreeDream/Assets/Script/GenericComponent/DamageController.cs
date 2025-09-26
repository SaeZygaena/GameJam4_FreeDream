using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class DamageController : MonoBehaviour
{
    enum TypeEntity
    {
        Turret,
        Gunner,
        Swordman,
        Player
    }
    [SerializeField] private List<Image> listHeart;
    [SerializeField] private List<RectTransform> listHeartRect;
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
        

        Debug.Log(_change);

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
        
        foreach (var heart in listHeart)
        {
            if (heart.fillAmount != 0)
            {
                Debug.Log("Avant : " + heart.fillAmount);
                Debug.Log("Calcul : " + _change * 0.25f);
                heart.fillAmount += _change * 0.25f;
                Debug.Log("Here Unfill");
                Debug.Log(heart.fillAmount);
                return;
            }
        }
    }

    private void FillHeart(float _change)
    {
        
        foreach (var heart in listHeart.AsEnumerable().Reverse())
        {
            if (heart.fillAmount != 1)
            {
                Debug.Log("Avant : " + heart.fillAmount);
                Debug.Log("Calcul : " + _change * 0.25f);
                heart.fillAmount += _change * 0.25f;
                Debug.Log("Here Unfill");
                Debug.Log(heart.fillAmount);
                return;
            }
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
