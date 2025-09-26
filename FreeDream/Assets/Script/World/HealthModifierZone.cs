using UnityEngine;
using System.Collections;

public class HealthModifierZone : MonoBehaviour
{
    [SerializeField] private bool isDoT;
    [SerializeField] private bool isDestroyAfterUse;
    [SerializeField] private float power;

    private bool isDoingDoTDamage;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (isDoT)
                DoTDamage(collision);
            else
                OneInstanceDamage(collision);

            if (isDestroyAfterUse)
                Destroy(gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (isDoT && collision.CompareTag("Player"))
        {
            isDoingDoTDamage = false;
            StopAllCoroutines();
        }
    }

    private void DoTDamage(Collider2D collision)
    {
        isDoingDoTDamage = true;
        StartCoroutine(DoTAction(collision));
    }

    private void OneInstanceDamage(Collider2D collision)
    {
        collision.GetComponent<HealthComponent>().HealChange(-power);
    }

    IEnumerator DoTAction(Collider2D collision)
    {
        while (isDoingDoTDamage)
        {
            yield return new WaitForSeconds(1f);
            collision.GetComponent<HealthComponent>().HealChange(-power);
        }
    }

}
