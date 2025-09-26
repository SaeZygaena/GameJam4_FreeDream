using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Runtime.ExceptionServices;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    [SerializeField] private float currentHealth;
    [SerializeField] private bool isPlayer = false;
    private bool isDead = false;
    private bool iFrame;

    private DamageController damageControl;
    void Start()
    {
        damageControl = GetComponent<DamageController>();
        FullHeal();
    }

    public bool GetIsDead() { return isDead; }
    public float GetCurrentHealth() { return currentHealth; }

    public void FullHeal()
    {
        currentHealth = maxHealth;
    }

    public void HealChange(float _change)
    {
        if (!iFrame || !isPlayer)
        {
            if (isPlayer && _change < 0)
                StartCoroutine(InternalCooldown());
            currentHealth += _change;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

            damageControl.OnDamage(_change);

            if (currentHealth == 0)
            {
                isDead = true;
                damageControl.OnDeath();
            }
            
        }
    }

    IEnumerator InternalCooldown()
    {
        iFrame = true;
        GameObject body = transform.Find("Skeletal").gameObject;

        for (int i = 0; i < 4; i++)
        {
            body.SetActive(false);
            yield return new WaitForSeconds(0.125f);
            body.SetActive(true);
            yield return new WaitForSeconds(0.125f);
        }
        iFrame = false;
    }
}
